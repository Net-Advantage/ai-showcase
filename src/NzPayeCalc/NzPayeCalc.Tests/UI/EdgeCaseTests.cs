namespace NzPayeCalc.Tests.UI;

/// <summary>
/// E2E Test Journey 2: Edge Case Testing
/// Tests boundary conditions and special salary values.
/// Requirement Coverage: REQ-038 (Edge Cases)
/// </summary>
public class EdgeCaseTests : PlaywrightTestBase
{
    /// <summary>
    /// E2E-J2-001: Minimum wage worker ($30,000)
    /// Expected: All values calculated correctly
    /// Requirement: REQ-038
    /// </summary>
    [Fact]
    public async Task E2E_J2_001_Minimum_Wage_Calculation()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 30000;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var monthlyTakeHome = await CalculatorPage.GetMonthlyTakeHomePay();

        // Monthly gross should be $2,500
        AssertCurrencyEquals("$2,500.00", monthlyGross);
        
        // Take-home should be positive
        var takeHome = ParseCurrency(monthlyTakeHome);
        Assert.True(takeHome > 0, "Take-home pay should be positive");
        Assert.True(takeHome < 2500, "Take-home should be less than gross");
    }

    /// <summary>
    /// E2E-J2-002: High earner ($200,000)
    /// Expected: ACC capped, other values correct
    /// Requirements: REQ-011 (ACC cap), REQ-038
    /// </summary>
    [Fact]
    public async Task E2E_J2_002_High_Earner_ACC_Capped()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 200000;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var monthlyAcc = await CalculatorPage.GetMonthlyAccLevy();

        // Monthly gross should be $16,666.67
        var gross = ParseCurrency(monthlyGross);
        AssertCurrencyEqual(16666.67m, gross, 0.50m);
        
        // ACC should be capped at $2,132.57 annually / 12 = $177.71 monthly
        // (1.53% of $139,384 = $2,132.57)
        var acc = ParseCurrency(monthlyAcc);
        AssertCurrencyEqual(177.71m, acc, 1.00m); // Allow tolerance for rounding
    }

    /// <summary>
    /// E2E-J2-003: Boundary - ACC cap at $139,384
    /// Expected: ACC at maximum
    /// Requirement: REQ-011 (ACC cap boundary)
    /// </summary>
    [Fact]
    public async Task E2E_J2_003_ACC_Cap_Boundary_Exact()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 139384;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        var monthlyAcc = await CalculatorPage.GetMonthlyAccLevy();
        
        // ACC should be exactly at cap: 1.53% of $139,384 = $2,132.57 annual / 12 = $177.71
        var acc = ParseCurrency(monthlyAcc);
        AssertCurrencyEqual(177.71m, acc, 1.00m);
    }

    /// <summary>
    /// E2E-J2-004: Just above ACC cap ($139,385)
    /// Expected: ACC capped at same value as E2E-J2-003
    /// Requirement: REQ-011 (ACC cap verification)
    /// </summary>
    [Fact]
    public async Task E2E_J2_004_Just_Above_ACC_Cap()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 139385;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        var monthlyAcc = await CalculatorPage.GetMonthlyAccLevy();
        
        // ACC should still be capped at $177.71 monthly
        var acc = ParseCurrency(monthlyAcc);
        AssertCurrencyEqual(177.71m, acc, 1.00m);
    }

    /// <summary>
    /// E2E-J2-005: Tax bracket boundary ($48,000)
    /// Expected: Tax calculated correctly at second bracket boundary
    /// Requirement: REQ-002 (second tax bracket)
    /// </summary>
    [Fact]
    public async Task E2E_J2_005_Tax_Bracket_Boundary_48K()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 48000;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var monthlyPaye = await CalculatorPage.GetMonthlyPayeTax();

        // Monthly gross should be $4,000
        AssertCurrencyEquals("$4,000.00", monthlyGross);
        
        // PAYE: $14,000 @ 10.5% = $1,470; $34,000 @ 17.5% = $5,950
        // Total annual PAYE = $7,420 / 12 = $618.33 monthly
        var paye = ParseCurrency(monthlyPaye);
        AssertCurrencyEqual(618.33m, paye, 1.00m);
    }

    /// <summary>
    /// E2E-J2-006: Zero salary
    /// Expected: All outputs $0.00
    /// Requirement: REQ-021 (zero handling)
    /// </summary>
    [Fact]
    public async Task E2E_J2_006_Zero_Salary_Returns_Zero_Results()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.CalculateSalary(0);
        await CalculatorPage.WaitForResults();

        // Assert
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var monthlyPaye = await CalculatorPage.GetMonthlyPayeTax();
        var monthlyKiwiSaver = await CalculatorPage.GetMonthlyKiwiSaver();
        var monthlyAcc = await CalculatorPage.GetMonthlyAccLevy();
        var monthlyTakeHome = await CalculatorPage.GetMonthlyTakeHomePay();

        // All values should be $0.00
        AssertCurrencyEquals("$0.00", monthlyGross);
        AssertCurrencyEquals("$0.00", monthlyPaye);
        AssertCurrencyEquals("$0.00", monthlyKiwiSaver);
        AssertCurrencyEquals("$0.00", monthlyAcc);
        AssertCurrencyEquals("$0.00", monthlyTakeHome);
    }

    /// <summary>
    /// Additional edge case: Tax bracket 3 boundary ($70,000)
    /// </summary>
    [Fact]
    public async Task Tax_Bracket_Boundary_70K()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 70000;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        AssertCurrencyEquals("$5,833.33", monthlyGross, 0.01m);
    }

    /// <summary>
    /// Additional edge case: First tax bracket boundary ($14,000)
    /// </summary>
    [Fact]
    public async Task Tax_Bracket_Boundary_14K()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 14000;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var monthlyPaye = await CalculatorPage.GetMonthlyPayeTax();

        // Monthly gross: $14,000 / 12 = $1,166.67
        AssertCurrencyEquals("$1,166.67", monthlyGross, 0.01m);
        
        // PAYE: $14,000 @ 10.5% = $1,470 annual / 12 = $122.50 monthly
        var paye = ParseCurrency(monthlyPaye);
        AssertCurrencyEqual(122.50m, paye, 1.00m);
    }

    /// <summary>
    /// Additional edge case: High income tax bracket ($180,000)
    /// </summary>
    [Fact]
    public async Task Tax_Bracket_Boundary_180K()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        const decimal salary = 180000;

        // Act
        await CalculatorPage.CalculateSalary(salary);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var monthlyTakeHome = await CalculatorPage.GetMonthlyTakeHomePay();

        // Monthly gross: $180,000 / 12 = $15,000
        AssertCurrencyEquals("$15,000.00", monthlyGross);
        
        // Take-home should be positive and less than gross
        var takeHome = ParseCurrency(monthlyTakeHome);
        Assert.True(takeHome > 0);
        Assert.True(takeHome < 15000);
    }
}
