namespace NzPayeCalc.Tests.UI;

/// <summary>
/// E2E Test Journey 1: Basic Calculation Flow
/// Tests the primary user journey through the calculator.
/// Requirement Coverage: REQ-037 (Primary User Flow)
/// </summary>
public class BasicCalculationJourneyTests : PlaywrightTestBase
{
    /// <summary>
    /// E2E-J1-001: Navigate to calculator
    /// Expected: Page loads, input field visible
    /// </summary>
    [Fact]
    public async Task E2E_J1_001_Navigate_To_Calculator_Shows_Input_Field()
    {
        // Act
        await CalculatorPage!.NavigateToCalculator();

        // Assert
        Assert.True(await CalculatorPage.IsPageLoaded(), "Page should be loaded");
        Assert.True(await CalculatorPage.IsInputFieldVisible(), "Input field should be visible");
    }

    /// <summary>
    /// E2E-J1-002: Enter $60,000
    /// Expected: Input accepted and formatted
    /// Requirements: REQ-019 (input validation), REQ-023 (formatting)
    /// </summary>
    [Fact]
    public async Task E2E_J1_002_Enter_Valid_Salary_Is_Accepted()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.EnterSalary(60000);

        // Assert
        Assert.True(await CalculatorPage.IsCalculateButtonEnabled(), "Calculate button should be enabled");
        Assert.False(await CalculatorPage.IsErrorDisplayed(), "No error should be displayed");
    }

    /// <summary>
    /// E2E-J1-003: Click Calculate
    /// Expected: Results display
    /// Requirement: REQ-034 (frontend-backend flow)
    /// </summary>
    [Fact]
    public async Task E2E_J1_003_Click_Calculate_Shows_Results()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.EnterSalary(60000);

        // Act
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed(), "Results should be displayed");
    }

    /// <summary>
    /// E2E-J1-004: Verify monthly gross
    /// Expected: Shows $5,000.00
    /// Requirements: REQ-014 (monthly conversion), REQ-016 (example validation)
    /// </summary>
    [Fact]
    public async Task E2E_J1_004_Monthly_Gross_Salary_Is_Correct()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();

        // Assert
        Assert.Equal("$5,000.00", monthlyGross);
    }

    /// <summary>
    /// E2E-J1-005: Verify monthly PAYE
    /// Expected: Shows ~$918.33
    /// Requirements: REQ-014 (monthly conversion), REQ-016 (example validation)
    /// Note: Calculation = $11,020 annual / 12 = $918.33
    /// </summary>
    [Fact]
    public async Task E2E_J1_005_Monthly_PAYE_Tax_Is_Correct()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act
        var monthlyPaye = await CalculatorPage.GetMonthlyPayeTax();

        // Assert - Allow for rounding differences
        var actual = ParseCurrency(monthlyPaye);
        AssertCurrencyEqual(918.33m, actual, 1.00m);
    }

    /// <summary>
    /// E2E-J1-006: Verify monthly KiwiSaver
    /// Expected: Shows $150.00
    /// Requirements: REQ-014, REQ-016
    /// Note: 3% of $60,000 = $1,800 annual / 12 = $150.00 monthly
    /// </summary>
    [Fact]
    public async Task E2E_J1_006_Monthly_KiwiSaver_Is_Correct()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act
        var monthlyKiwiSaver = await CalculatorPage.GetMonthlyKiwiSaver();

        // Assert
        AssertCurrencyEquals("$150.00", monthlyKiwiSaver);
    }

    /// <summary>
    /// E2E-J1-007: Verify monthly ACC
    /// Expected: Shows $76.50
    /// Requirements: REQ-014, REQ-016
    /// Note: 1.53% of $60,000 = $918 annual / 12 = $76.50 monthly
    /// </summary>
    [Fact]
    public async Task E2E_J1_007_Monthly_ACC_Levy_Is_Correct()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act
        var monthlyAcc = await CalculatorPage.GetMonthlyAccLevy();

        // Assert
        AssertCurrencyEquals("$76.50", monthlyAcc);
    }

    /// <summary>
    /// E2E-J1-008: Verify monthly take-home
    /// Expected: Shows ~$3,855.17
    /// Requirements: REQ-014, REQ-016, REQ-017 (take-home calculation)
    /// Note: $5,000 - $918.33 - $150.00 - $76.50 = ~$3,855.17
    /// </summary>
    [Fact]
    public async Task E2E_J1_008_Monthly_TakeHome_Pay_Is_Correct()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act
        var monthlyTakeHome = await CalculatorPage.GetMonthlyTakeHomePay();

        // Assert - Allow some tolerance for calculation differences
        var actual = ParseCurrency(monthlyTakeHome);
        AssertCurrencyEqual(3855.17m, actual, 2.00m);
    }

    /// <summary>
    /// Complete flow test: Enter, calculate, verify all results
    /// Integration of E2E-J1-001 through E2E-J1-008
    /// </summary>
    [Fact]
    public async Task Complete_Calculation_Journey_60K_Salary()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Step by step through user journey
        Assert.True(await CalculatorPage.IsPageLoaded());
        Assert.True(await CalculatorPage.IsInputFieldVisible());
        
        await CalculatorPage.EnterSalary(60000);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForResults();

        // Assert - All results displayed correctly
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var gross = await CalculatorPage.GetMonthlyGrossSalary();
        var paye = await CalculatorPage.GetMonthlyPayeTax();
        var kiwiSaver = await CalculatorPage.GetMonthlyKiwiSaver();
        var acc = await CalculatorPage.GetMonthlyAccLevy();
        var takeHome = await CalculatorPage.GetMonthlyTakeHomePay();

        Assert.Equal("$5,000.00", gross);
        AssertCurrencyEquals("$150.00", kiwiSaver);
        AssertCurrencyEquals("$76.50", acc);
        
        // PAYE and take-home allow slight variation
        AssertCurrencyEqual(918.33m, ParseCurrency(paye), 1.00m);
        AssertCurrencyEqual(3855.17m, ParseCurrency(takeHome), 2.00m);
    }
}
