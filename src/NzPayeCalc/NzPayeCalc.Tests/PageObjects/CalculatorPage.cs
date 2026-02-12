using Microsoft.Playwright;

namespace NzPayeCalc.Tests.PageObjects;

/// <summary>
/// Page Object Model for the NZ PAYE Calculator page.
/// Provides methods to interact with calculator UI elements.
/// </summary>
public class CalculatorPage
{
    private readonly IPage _page;

    public CalculatorPage(IPage page)
    {
        _page = page;
    }

    // Locators
    private ILocator SalaryInput => _page.Locator("#annual-salary");
    private ILocator CalculateButton => _page.GetByRole(AriaRole.Button, new() { Name = "Calculate" });
    private ILocator CalculatingButton => _page.GetByRole(AriaRole.Button, new() { Name = "Calculating..." });
    private ILocator ErrorAlert => _page.Locator(".alert-danger");
    private ILocator ResultsCard => _page.Locator(".calculator-results");
    private ILocator MonthlyGrossSalary => _page.Locator(".result-row", new() { HasText = "Monthly Gross Salary" }).Locator("dd");
    private ILocator MonthlyPayeTax => _page.Locator(".result-row", new() { HasText = "PAYE Tax" }).First.Locator("dd");
    private ILocator MonthlyKiwiSaver => _page.Locator(".result-row", new() { HasText = "KiwiSaver" }).First.Locator("dd");
    private ILocator MonthlyAccLevy => _page.Locator(".result-row", new() { HasText = "ACC Levy" }).First.Locator("dd");
    private ILocator MonthlyTakeHomePay => _page.Locator(".result-row.take-home", new() { HasText = "Take-Home Pay" }).First.Locator("dd");
    private ILocator ValidationMessage => _page.Locator(".validation-message");
    private ILocator PageHeading => _page.GetByRole(AriaRole.Heading, new() { Name = "NZ PAYE Calculator" });

    // Actions
    public async Task NavigateToCalculator()
    {
        await _page.GotoAsync("/calculator");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task EnterSalary(decimal salary)
    {
        await SalaryInput.ClearAsync();
        await SalaryInput.FillAsync(salary.ToString());
    }

    public async Task EnterSalary(string salaryText)
    {
        await SalaryInput.ClearAsync();
        await SalaryInput.FillAsync(salaryText);
    }

    public async Task ClickCalculate()
    {
        await CalculateButton.ClickAsync();
    }

    public async Task WaitForResults()
    {
        await ResultsCard.WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    public async Task WaitForCalculationComplete()
    {
        // Wait for loading state to disappear
        await CalculateButton.WaitForAsync(new() { State = WaitForSelectorState.Visible });
    }

    // Assertions helpers
    public async Task<bool> IsPageLoaded()
    {
        return await PageHeading.IsVisibleAsync();
    }

    public async Task<bool> IsInputFieldVisible()
    {
        return await SalaryInput.IsVisibleAsync();
    }

    public async Task<bool> IsCalculateButtonEnabled()
    {
        return await CalculateButton.IsEnabledAsync();
    }

    public async Task<bool> IsCalculating()
    {
        return await CalculatingButton.IsVisibleAsync();
    }

    public async Task<bool> AreResultsDisplayed()
    {
        return await ResultsCard.IsVisibleAsync();
    }

    public async Task<bool> IsErrorDisplayed()
    {
        return await ErrorAlert.IsVisibleAsync();
    }

    public async Task<string?> GetErrorMessage()
    {
        if (!await IsErrorDisplayed())
            return null;
        
        return await ErrorAlert.TextContentAsync();
    }

    public async Task<string?> GetValidationMessage()
    {
        if (!await ValidationMessage.IsVisibleAsync())
            return null;
        
        return await ValidationMessage.TextContentAsync();
    }

    // Get result values
    public async Task<string> GetMonthlyGrossSalary()
    {
        return (await MonthlyGrossSalary.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetMonthlyPayeTax()
    {
        return (await MonthlyPayeTax.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetMonthlyKiwiSaver()
    {
        return (await MonthlyKiwiSaver.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetMonthlyAccLevy()
    {
        return (await MonthlyAccLevy.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetMonthlyTakeHomePay()
    {
        return (await MonthlyTakeHomePay.TextContentAsync())?.Trim() ?? "";
    }

    // Combined workflow methods
    public async Task CalculateSalary(decimal salary)
    {
        await EnterSalary(salary);
        await ClickCalculate();
        await WaitForCalculationComplete();
    }

    public async Task CalculateSalary(string salaryText)
    {
        await EnterSalary(salaryText);
        await ClickCalculate();
        await WaitForCalculationComplete();
    }
}
