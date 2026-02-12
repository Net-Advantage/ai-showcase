using Microsoft.Playwright;
using Microsoft.Extensions.Logging;
using NzPayeCalc.Tests.PageObjects;

namespace NzPayeCalc.Tests;

/// <summary>
/// Base class for Playwright UI tests.
/// Handles browser setup, application hosting, and page object initialization.
/// </summary>
public class PlaywrightTestBase : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    
    protected IPlaywright? Playwright { get; private set; }
    protected IBrowser? Browser { get; private set; }
    protected IBrowserContext? Context { get; private set; }
    protected IPage? Page { get; private set; }
    protected CalculatorPage? CalculatorPage { get; private set; }
    protected string BaseUrl { get; private set; } = "";

    private IAsyncDisposable? _app;

    public async ValueTask InitializeAsync()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        // Set up Aspire application
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.NzPayeCalc_AppHost>(cancellationToken);
        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Information);
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Information);
            logging.AddFilter("Aspire.", LogLevel.Warning);
        });
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        var app = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
        _app = app;
        await app.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        // Wait for the web frontend to be healthy
        await app.ResourceNotifications.WaitForResourceHealthyAsync("webfrontend", cancellationToken)
            .WaitAsync(DefaultTimeout, cancellationToken);

        // Get the base URL for the web frontend
        var httpClient = app.CreateHttpClient("webfrontend");
        BaseUrl = httpClient.BaseAddress?.ToString().TrimEnd('/') ?? "http://localhost";

        // Initialize Playwright
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        // Launch browser (headless by default, can be overridden with env variable)
        var headless = Environment.GetEnvironmentVariable("HEADED") != "1";
        Browser = await Playwright.Chromium.LaunchAsync(new()
        {
            Headless = headless,
            SlowMo = headless ? 0 : 100 // Slow down when headed for debugging
        });

        // Create browser context
        Context = await Browser.NewContextAsync(new()
        {
            BaseURL = BaseUrl,
            ViewportSize = new() { Width = 1280, Height = 720 },
            Locale = "en-NZ",
            TimezoneId = "Pacific/Auckland"
        });

        // Create page
        Page = await Context.NewPageAsync();

        // Initialize page objects
        CalculatorPage = new CalculatorPage(Page);
    }

    public async ValueTask DisposeAsync()
    {
        if (Page != null)
            await Page.CloseAsync();

        if (Context != null)
            await Context.CloseAsync();

        if (Browser != null)
            await Browser.CloseAsync();

        Playwright?.Dispose();

        if (_app != null)
            await _app.DisposeAsync();
    }

    /// <summary>
    /// Parse currency string (e.g., "$5,000.00" or "-$150.00") to decimal value.
    /// </summary>
    protected static decimal ParseCurrency(string currencyString)
    {
        var cleaned = currencyString.Replace("$", "").Replace(",", "").Trim();
        return decimal.Parse(cleaned);
    }

    /// <summary>
    /// Assert that two currency values are equal within a tolerance (for rounding).
    /// </summary>
    protected static void AssertCurrencyEqual(decimal expected, decimal actual, decimal tolerance = 0.01m)
    {
        Assert.InRange(actual, expected - tolerance, expected + tolerance);
    }

    /// <summary>
    /// Assert that a currency string matches the expected value within tolerance.
    /// </summary>
    protected static void AssertCurrencyEquals(string expectedCurrencyString, string actualCurrencyString, decimal tolerance = 0.01m)
    {
        var expected = ParseCurrency(expectedCurrencyString);
        var actual = ParseCurrency(actualCurrencyString);
        AssertCurrencyEqual(expected, actual, tolerance);
    }
}
