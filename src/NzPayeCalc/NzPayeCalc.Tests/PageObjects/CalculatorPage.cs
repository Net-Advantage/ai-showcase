using Microsoft.Playwright;

namespace NzPayeCalc.Tests.PageObjects;

/// <summary>
/// Page Object Model for the NZ PAYE Calculator page.
/// Provides methods to interact with calculator UI elements.
/// </summary>
public class CalculatorPage
{
    private readonly IPage _page;
    private const int DefaultTimeoutMs = 30000;

    public CalculatorPage(IPage page)
    {
        _page = page;
    }

    // Locators
    private ILocator SalaryInput => _page.Locator("#annual-salary");
    private ILocator KiwiSaverRateSelect => _page.Locator("#kiwisaver-rate");
    private ILocator StudentLoanCheckbox => _page.Locator("#has-student-loan");
    private ILocator CalculateButton => _page.Locator("#calculate-button").Filter(new() { HasText = "Calculate" });
    private ILocator CalculatingButton => _page.Locator("#calculate-button").Filter(new() { HasText = "Calculating" });
    private ILocator ErrorAlert => _page.Locator(".alert-danger");
    private ILocator ResultsCard => _page.GetByTestId("results-section").Or(_page.Locator(".calculator-results"));
    private ILocator AnnualBreakdown => _page.Locator("details.annual-breakdown");
    private ILocator AnnualBreakdownSummary => _page.Locator("details.annual-breakdown summary");
    private ILocator MonthlyGrossSalary => _page.GetByTestId("monthly-gross-value").Or(_page.Locator(".result-row", new() { HasText = "Monthly Gross Salary" }).Locator("dd"));
    private ILocator MonthlyPayeTax => _page.GetByTestId("monthly-paye-value").Or(_page.Locator(".calculator-results > .results-card > .results-list .result-row", new() { HasText = "PAYE Tax" }).First.Locator("dd"));
    private ILocator MonthlyKiwiSaver => _page.GetByTestId("monthly-kiwisaver-value").Or(_page.Locator(".calculator-results > .results-card > .results-list .result-row", new() { HasText = "KiwiSaver" }).First.Locator("dd"));
    private ILocator MonthlyAccLevy => _page.GetByTestId("monthly-acc-value").Or(_page.Locator(".calculator-results > .results-card > .results-list .result-row", new() { HasText = "ACC Levy" }).First.Locator("dd"));
    private ILocator MonthlyStudentLoan => _page.GetByTestId("monthly-student-loan-value").Or(_page.Locator(".calculator-results > .results-card > .results-list .result-row", new() { HasText = "Student Loan (12%)" }).First.Locator("dd"));
    private ILocator MonthlyTakeHomePay => _page.GetByTestId("monthly-take-home-value").Or(_page.Locator(".calculator-results > .results-card > .results-list .result-row.take-home").Locator("dd"));
    private ILocator AnnualGrossSalary => _page.GetByTestId("annual-gross-value").Or(_page.Locator("details.annual-breakdown .result-row", new() { HasText = "Annual Gross Salary" }).First.Locator("dd"));
    private ILocator AnnualPayeTax => _page.GetByTestId("annual-paye-value").Or(_page.Locator("details.annual-breakdown .result-row", new() { HasText = "PAYE Tax" }).First.Locator("dd"));
    private ILocator AnnualKiwiSaver => _page.GetByTestId("annual-kiwisaver-value").Or(_page.Locator("details.annual-breakdown .result-row", new() { HasText = "KiwiSaver" }).First.Locator("dd"));
    private ILocator AnnualAccLevy => _page.GetByTestId("annual-acc-value").Or(_page.Locator("details.annual-breakdown .result-row", new() { HasText = "ACC Levy" }).First.Locator("dd"));
    private ILocator AnnualStudentLoan => _page.GetByTestId("annual-student-loan-value").Or(_page.Locator("details.annual-breakdown .result-row", new() { HasText = "Student Loan (12%)" }).First.Locator("dd"));
    private ILocator AnnualTakeHomePay => _page.GetByTestId("annual-take-home-value").Or(_page.Locator("details.annual-breakdown .result-row.take-home").Locator("dd"));
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
        await SalaryInput.BlurAsync();
    }

    public async Task EnterSalary(string salaryText)
    {
        await SalaryInput.ClearAsync();
        await SalaryInput.FillAsync(salaryText);
        await SalaryInput.BlurAsync();
    }

    public async Task ClickCalculate()
    {
        await CalculateButton.ClickAsync();
    }

    public async Task WaitForResults()
    {
        await _page.WaitForFunctionAsync(
            "() => !!document.querySelector('[data-testid=\"results-section\"]') || !!document.querySelector('.calculator-results')",
            null,
            new() { Timeout = DefaultTimeoutMs });
        await WaitForCalculationComplete();
        await _page.WaitForFunctionAsync(
            "() => { const testIdValue = (document.querySelector('[data-testid=\"monthly-gross-value\"]')?.textContent || '').trim(); if (testIdValue.length > 0) return true; const fallbackRow = Array.from(document.querySelectorAll('.calculator-results .result-row')).find(r => (r.textContent || '').includes('Monthly Gross Salary')); const fallbackValue = (fallbackRow?.querySelector('dd')?.textContent || '').trim(); return fallbackValue.length > 0; }",
            null,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForCalculationComplete()
    {
        await _page.WaitForFunctionAsync(
            "() => { const button = document.querySelector('#calculate-button'); return !!button && (button.textContent || '').includes('Calculate') && !(button.textContent || '').includes('Calculating'); }",
            null,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForAutoCalculatedResults()
    {
        await WaitForResults();
        await _page.WaitForFunctionAsync(
            "() => (document.querySelector('[data-testid=\"monthly-gross-value\"]')?.textContent || '').trim().length > 0",
            null,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task VerifyResultsVisible()
    {
        await WaitForResults();

        var isVisible = await AreResultsDisplayed();
        if (!isVisible)
        {
            throw new PlaywrightException("Expected calculator results to be visible, but results section is hidden.");
        }
    }

    public async Task SelectKiwiSaverRate(string value)
    {
        await KiwiSaverRateSelect.SelectOptionAsync(new SelectOptionValue { Value = value });
    }

    public async Task<List<string>> GetKiwiSaverRateOptions()
    {
        return (await KiwiSaverRateSelect.Locator("option").AllTextContentsAsync()).ToList();
    }

    public async Task SetStudentLoan(bool enabled)
    {
        if (enabled)
        {
            await StudentLoanCheckbox.CheckAsync();
            return;
        }

        await StudentLoanCheckbox.UncheckAsync();
    }

    public async Task<bool> IsStudentLoanRowVisible()
    {
        return await _page.GetByTestId("monthly-student-loan-row")
            .Or(_page.Locator(".calculator-results > .results-card > .results-list .result-row", new() { HasText = "Student Loan (12%)" }).First)
            .IsVisibleAsync();
    }

    public async Task EnsureAnnualBreakdownExpanded()
    {
        var isOpen = await AnnualBreakdown.GetAttributeAsync("open");
        if (!string.IsNullOrEmpty(isOpen))
        {
            return;
        }

        await AnnualBreakdownSummary.ClickAsync();
        await _page.WaitForFunctionAsync(
            "() => document.querySelector('details.annual-breakdown')?.hasAttribute('open') === true",
            null,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForMonthlyKiwiSaverToBe(string expected)
    {
        await _page.WaitForFunctionAsync(
            "expected => (document.querySelector('[data-testid=\"monthly-kiwisaver-value\"]')?.textContent || '').trim() === expected",
            expected,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForMonthlyStudentLoanToBe(string expected)
    {
        await _page.WaitForFunctionAsync(
            "expected => (document.querySelector('[data-testid=\"monthly-student-loan-value\"]')?.textContent || '').trim() === expected",
            expected,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForMonthlyGrossToBe(string expected)
    {
        await _page.WaitForFunctionAsync(
            "expected => (document.querySelector('[data-testid=\"monthly-gross-value\"]')?.textContent || '').trim() === expected",
            expected,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForMonthlyGrossToChangeFrom(string previous)
    {
        await _page.WaitForFunctionAsync(
            "previous => { const text = (document.querySelector('[data-testid=\"monthly-gross-value\"]')?.textContent || '').trim(); return text.length > 0 && text !== previous; }",
            previous,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task<bool> TryWaitForMonthlyGrossToChangeFrom(string previous, int timeoutMs = 2500)
    {
        try
        {
            await _page.WaitForFunctionAsync(
                "previous => { const testIdValue = (document.querySelector('[data-testid=\"monthly-gross-value\"]')?.textContent || '').trim(); if (testIdValue.length > 0 && testIdValue !== previous) return true; const fallbackRow = Array.from(document.querySelectorAll('.calculator-results .result-row')).find(r => (r.textContent || '').includes('Monthly Gross Salary')); const fallbackValue = (fallbackRow?.querySelector('dd')?.textContent || '').trim(); return fallbackValue.length > 0 && fallbackValue !== previous; }",
                previous,
                new() { Timeout = timeoutMs });
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
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

    public async Task WaitForValidationMessage()
    {
        await _page.WaitForFunctionAsync(
            "() => { const message = (document.querySelector('.validation-message')?.textContent || '').trim(); const input = document.querySelector('#annual-salary'); const ariaInvalid = input?.getAttribute('aria-invalid') === 'true'; const htmlInvalid = input instanceof HTMLInputElement && !input.checkValidity(); return message.length > 0 || ariaInvalid || htmlInvalid; }",
            null,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task WaitForErrorOrValidation()
    {
        await _page.WaitForFunctionAsync(
            "() => { const hasError = !!document.querySelector('.alert-danger'); const message = (document.querySelector('.validation-message')?.textContent || '').trim(); const input = document.querySelector('#annual-salary'); const ariaInvalid = input?.getAttribute('aria-invalid') === 'true'; const htmlInvalid = input instanceof HTMLInputElement && !input.checkValidity(); return hasError || message.length > 0 || ariaInvalid || htmlInvalid; }",
            null,
            new() { Timeout = DefaultTimeoutMs });
    }

    public async Task<bool> IsSalaryInputInvalid()
    {
        return await _page.EvaluateAsync<bool>("() => { const input = document.querySelector('#annual-salary'); if (!(input instanceof HTMLInputElement)) return false; return input.getAttribute('aria-invalid') === 'true' || !input.checkValidity(); }");
    }

    public async Task<string?> GetErrorMessage()
    {
        if (!await IsErrorDisplayed())
            return null;
        
        return await ErrorAlert.TextContentAsync();
    }

    public async Task<string?> GetValidationMessage()
    {
        var messages = await ValidationMessage.AllTextContentsAsync();
        var combined = string.Join(" ", messages.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()));
        return string.IsNullOrWhiteSpace(combined) ? null : combined;
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

    public async Task<string> GetMonthlyStudentLoan()
    {
        return (await MonthlyStudentLoan.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetAnnualKiwiSaver()
    {
        return (await AnnualKiwiSaver.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetAnnualGrossSalary()
    {
        return (await AnnualGrossSalary.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetAnnualPayeTax()
    {
        return (await AnnualPayeTax.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetAnnualAccLevy()
    {
        return (await AnnualAccLevy.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetAnnualStudentLoan()
    {
        return (await AnnualStudentLoan.TextContentAsync())?.Trim() ?? "";
    }

    public async Task<string> GetAnnualTakeHomePay()
    {
        return (await AnnualTakeHomePay.TextContentAsync())?.Trim() ?? "";
    }

    // Combined workflow methods
    public async Task CalculateSalary(decimal salary)
    {
        await WaitForAutoCalculatedResults();
        var previousGross = "$5,000.00";
        await EnterSalary(salary);
        await WaitForCalculationComplete();

        if (salary >= 1m && salary <= 1_000_000m)
        {
            var updatedViaRealtime = await TryWaitForMonthlyGrossToChangeFrom(previousGross, 10000);
            if (updatedViaRealtime)
            {
                return;
            }

            await ClickCalculate();
            await _page.WaitForFunctionAsync(
                "() => !!document.querySelector('.calculator-results') || !!document.querySelector('.alert-danger')",
                null,
                new() { Timeout = DefaultTimeoutMs });

            if (await IsErrorDisplayed())
            {
                var error = await GetErrorMessage();
                throw new TimeoutException($"Calculation failed after submit for salary '{salary}'. Error: {error}");
            }

            await WaitForResults();
            return;
        }

        await ClickCalculate();
        await WaitForErrorOrValidation();
    }

    public async Task CalculateSalary(string salaryText)
    {
        await WaitForAutoCalculatedResults();
        await EnterSalary(salaryText);
        await ClickCalculate();
        await WaitForCalculationComplete();
    }
}
