namespace NzPayeCalc.Tests.UI;

/// <summary>
/// E2E Test Journey 3: Error Handling
/// Tests validation and error scenarios.
/// Requirement Coverage: REQ-039 (Error Scenarios)
/// </summary>
public class ErrorHandlingTests : PlaywrightTestBase
{
    /// <summary>
    /// E2E-J3-001: Negative input
    /// Expected: Error message, no calculation
    /// Requirements: REQ-020 (reject negative), REQ-039 (error handling)
    /// </summary>
    [Fact]
    public async Task E2E_J3_001_Negative_Input_Shows_Error()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.EnterSalary("-5000");
        await CalculatorPage.ClickCalculate();
        await Task.Delay(500); // Wait for validation

        // Assert
        var validationMsg = await CalculatorPage.GetValidationMessage();
        Assert.NotNull(validationMsg);
        Assert.Contains("between", validationMsg, StringComparison.OrdinalIgnoreCase);
        
        // Results should not be displayed
        Assert.False(await CalculatorPage.AreResultsDisplayed(), 
            "Results should not be displayed for invalid input");
    }

    /// <summary>
    /// E2E-J3-002: Non-numeric input
    /// Expected: Error message displayed
    /// Requirements: REQ-020 (reject non-numeric), REQ-039
    /// </summary>
    [Fact]
    public async Task E2E_J3_002_Non_Numeric_Input_Shows_Error()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.EnterSalary("abc");
        await CalculatorPage.ClickCalculate();
        await Task.Delay(500); // Wait for validation

        // Assert
        // The input field should either reject the text or show validation error
        // Note: InputNumber in Blazor may prevent non-numeric entry
        var isErrorDisplayed = await CalculatorPage.IsErrorDisplayed();
        var validationMsg = await CalculatorPage.GetValidationMessage();
        
        // Either validation message or error alert should be shown
        Assert.True(isErrorDisplayed || !string.IsNullOrEmpty(validationMsg),
            "Error or validation message should be displayed for non-numeric input");
    }

    /// <summary>
    /// E2E-J3-003: Empty submission
    /// Expected: Prompt to enter value
    /// Requirements: REQ-021 (handle empty), REQ-039
    /// </summary>
    [Fact]
    public async Task E2E_J3_003_Empty_Input_Shows_Validation_Error()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.EnterSalary(""); // Clear the default value
        await CalculatorPage.ClickCalculate();
        await Task.Delay(500); // Wait for validation

        // Assert
        var validationMsg = await CalculatorPage.GetValidationMessage();
        Assert.NotNull(validationMsg);
        Assert.Contains("required", validationMsg, StringComparison.OrdinalIgnoreCase);
        
        // Results should not be displayed
        Assert.False(await CalculatorPage.AreResultsDisplayed());
    }

    /// <summary>
    /// E2E-J3-004: API failure scenario (simulated with extremely large value)
    /// Expected: User-friendly error message
    /// Requirements: REQ-035 (error propagation), REQ-039
    /// </summary>
    [Fact]
    public async Task E2E_J3_004_Extreme_Value_Handled_Gracefully()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Try to submit a value beyond the allowed range
        await CalculatorPage.EnterSalary("99999999");
        await CalculatorPage.ClickCalculate();
        await Task.Delay(1000); // Wait for validation/error

        // Assert
        var isErrorDisplayed = await CalculatorPage.IsErrorDisplayed();
        var validationMsg = await CalculatorPage.GetValidationMessage();
        
        // Should show either validation or error message
        Assert.True(isErrorDisplayed || !string.IsNullOrEmpty(validationMsg),
            "Error should be displayed for extreme values");
    }

    /// <summary>
    /// E2E-J3-005: Recovery from error
    /// Expected: User can correct input and successfully calculate
    /// Requirement: REQ-039 (error recovery)
    /// </summary>
    [Fact]
    public async Task E2E_J3_005_Recovery_From_Error_State()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act 1 - Submit invalid input
        await CalculatorPage.EnterSalary("-1000");
        await CalculatorPage.ClickCalculate();
        await Task.Delay(500);

        // Verify error state
        var validationMsg = await CalculatorPage.GetValidationMessage();
        Assert.NotNull(validationMsg);

        // Act 2 - Correct the input
        await CalculatorPage.EnterSalary(60000);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForResults();

        // Assert - Should now show valid results
        Assert.True(await CalculatorPage.AreResultsDisplayed(),
            "Results should be displayed after correcting invalid input");
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        AssertCurrencyEquals("$5,000.00", monthlyGross);
    }

    /// <summary>
    /// Additional test: Very small positive value
    /// </summary>
    [Fact]
    public async Task Very_Small_Positive_Value_Accepted()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.CalculateSalary(1);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        // $1 annual / 12 = $0.08 monthly
        var gross = ParseCurrency(monthlyGross);
        Assert.True(gross >= 0 && gross < 1, "Monthly gross for $1 annual should be less than $1");
    }

    /// <summary>
    /// Additional test: Decimal input
    /// </summary>
    [Fact]
    public async Task Decimal_Salary_Input_Accepted()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.CalculateSalary(60000.50m);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        // $60,000.50 / 12 = $5,000.04
        AssertCurrencyEquals("$5,000.04", monthlyGross, 0.01m);
    }

    /// <summary>
    /// Additional test: Maximum allowed value
    /// </summary>
    [Fact]
    public async Task Maximum_Allowed_Value_Accepted()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Max is $1,000,000 based on validation in Calculator.razor
        await CalculatorPage.CalculateSalary(1000000);
        await CalculatorPage.WaitForResults();

        // Assert
        Assert.True(await CalculatorPage.AreResultsDisplayed());
        
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        // $1,000,000 / 12 = $83,333.33
        AssertCurrencyEquals("$83,333.33", monthlyGross, 0.50m);
    }

    /// <summary>
    /// Additional test: Value just over maximum
    /// </summary>
    [Fact]
    public async Task Value_Over_Maximum_Rejected()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.EnterSalary(1000001);
        await CalculatorPage.ClickCalculate();
        await Task.Delay(500);

        // Assert
        var validationMsg = await CalculatorPage.GetValidationMessage();
        Assert.NotNull(validationMsg);
        Assert.Contains("1,000,000", validationMsg, StringComparison.OrdinalIgnoreCase);
    }
}
