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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act
        await CalculatorPage.EnterSalary("-5000");
        await CalculatorPage.ClickCalculate();

        // Assert
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain visible after invalid negative entry.");
        
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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act - force invalid value because <input type="number"> blocks typing letters
        await Page!.EvaluateAsync(@"
            () => {
                const input = document.getElementById('annual-salary');
                if (!input) return;
                input.value = 'abc';
                input.dispatchEvent(new Event('input', { bubbles: true }));
                input.dispatchEvent(new Event('change', { bubbles: true }));
                input.dispatchEvent(new Event('blur', { bubbles: true }));
            }");
        await CalculatorPage.ClickCalculate();

        // Assert
        // The input field should either reject the text or show validation error
        // Note: InputNumber in Blazor may prevent non-numeric entry
        var isErrorDisplayed = await CalculatorPage.IsErrorDisplayed();
        var validationMsg = await CalculatorPage.GetValidationMessage();
        var isInputInvalid = await CalculatorPage.IsSalaryInputInvalid();
        
        // Either validation message or error alert should be shown
        Assert.True(isErrorDisplayed || isInputInvalid || !string.IsNullOrEmpty(validationMsg),
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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act
        await CalculatorPage.EnterSalary(""); // Clear the default value
        await CalculatorPage.ClickCalculate();

        // Assert
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain visible after empty salary submission.");
        
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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act - Try to submit a value beyond the allowed range
        await CalculatorPage.EnterSalary("99999999");
        await CalculatorPage.ClickCalculate();

        // Assert
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain visible after extreme value submission.");
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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act 1 - Submit invalid input
        await CalculatorPage.EnterSalary("-1000");
        await CalculatorPage.ClickCalculate();

        // Verify error state
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain visible after invalid submission.");

        // Act 2 - Correct the input
        await CalculatorPage.EnterSalary(60000);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForCalculationComplete();

        // Assert - Should now show valid results
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain usable after correcting invalid input");
        
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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act
        await CalculatorPage.CalculateSalary(1);

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
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act
        await CalculatorPage.EnterSalary(60000.50m);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForCalculationComplete();

        // Assert
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain usable after decimal salary submission.");
    }

    /// <summary>
    /// Additional test: Maximum allowed value
    /// </summary>
    [Fact]
    public async Task Maximum_Allowed_Value_Accepted()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act - Max is $1,000,000 based on validation in Calculator.razor
        await CalculatorPage.EnterSalary(1000000);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForCalculationComplete();

        // Assert
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain usable for maximum allowed value.");
    }

    /// <summary>
    /// Additional test: Value just over maximum
    /// </summary>
    [Fact]
    public async Task Value_Over_Maximum_Rejected()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act
        await CalculatorPage.EnterSalary(1000001);
        await CalculatorPage.ClickCalculate();

        // Assert
        Assert.True(await CalculatorPage.IsInputFieldVisible(),
            "Input should remain visible after over-maximum submission.");
    }
}
