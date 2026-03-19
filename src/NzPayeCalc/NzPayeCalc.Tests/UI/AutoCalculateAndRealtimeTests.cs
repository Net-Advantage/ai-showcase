namespace NzPayeCalc.Tests.UI;

/// <summary>
/// E2E tests for REQ-PAYE-001 and UX auto/realtime recalculation behavior.
/// </summary>
public class AutoCalculateAndRealtimeTests : PlaywrightTestBase
{
    /// <summary>
    /// REQ-PAYE-001: 0% KiwiSaver option exists and calculates to zero.
    /// </summary>
    [Fact]
    public async Task E2E_KS_001_Zero_Percent_KiwiSaver_Option_Exists()
    {
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        var options = await CalculatorPage.GetKiwiSaverRateOptions();
        Assert.Contains("0%", options.Select(x => x.Trim()));

        await CalculatorPage.SelectKiwiSaverRate("0");
        await CalculatorPage.WaitForMonthlyKiwiSaverToBe("-$0.00");

        var monthlyKiwiSaver = await CalculatorPage.GetMonthlyKiwiSaver();
        Assert.True(monthlyKiwiSaver is "-$0.00" or "$0.00");

        await CalculatorPage.EnsureAnnualBreakdownExpanded();
        var annualKiwiSaver = await CalculatorPage.GetAnnualKiwiSaver();
        Assert.True(annualKiwiSaver is "-$0.00" or "$0.00");
    }

    /// <summary>
    /// REQ-PAYE-UX-002: Initial page load automatically calculates with defaults.
    /// </summary>
    [Fact]
    public async Task E2E_UX_001_Auto_Calculate_On_Page_Load()
    {
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        Assert.True(await CalculatorPage.AreResultsDisplayed(), "Results should be visible without clicking Calculate");

        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        Assert.Equal("$5,000.00", monthlyGross);
    }

    /// <summary>
    /// REQ-PAYE-UX-001: KiwiSaver rate changes trigger automatic recalculation when results are visible.
    /// </summary>
    [Fact]
    public async Task E2E_UX_002_Recalculate_On_KiwiSaver_Change()
    {
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        var initialMonthlyKiwiSaver = await CalculatorPage.GetMonthlyKiwiSaver();
        Assert.Contains("150.00", initialMonthlyKiwiSaver);

        await CalculatorPage.SelectKiwiSaverRate("0.06");
        await CalculatorPage.WaitForMonthlyKiwiSaverToBe("-$300.00");

        var updatedMonthlyKiwiSaver = await CalculatorPage.GetMonthlyKiwiSaver();
        Assert.Equal("-$300.00", updatedMonthlyKiwiSaver);
    }

    /// <summary>
    /// REQ-PAYE-UX-001: Student loan changes trigger automatic recalculation when results are visible.
    /// </summary>
    [Fact]
    public async Task E2E_UX_003_Recalculate_On_Student_Loan_Change()
    {
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        Assert.False(await CalculatorPage.IsStudentLoanRowVisible(), "Student loan row should not be visible before enabling checkbox");

        await CalculatorPage.SetStudentLoan(true);
        await CalculatorPage.WaitForMonthlyStudentLoanToBe("-$358.72");

        Assert.True(await CalculatorPage.IsStudentLoanRowVisible(), "Student loan row should appear after enabling checkbox");

        var monthlyStudentLoan = await CalculatorPage.GetMonthlyStudentLoan();
        Assert.Equal("-$358.72", monthlyStudentLoan);
    }
}
