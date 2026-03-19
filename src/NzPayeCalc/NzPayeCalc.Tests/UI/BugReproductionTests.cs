namespace NzPayeCalc.Tests.UI;

/// <summary>
/// Bug Reproduction Tests
/// Tests to reproduce and verify bug fixes
/// </summary>
public class BugReproductionTests : PlaywrightTestBase
{
    /// <summary>
    /// Bug: Entering 200000 in Annual Salary field resets to 60000 after calculate
    /// Expected: Should calculate for 200000, not reset to 60000
    /// </summary>
    [Fact]
    public async Task Bug_200K_Salary_Resets_To_60K_After_Calculate()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act - Enter 200000 and calculate
        await CalculatorPage.EnterSalary(200000);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForCalculationComplete();
        
        // Take a screenshot before clicking calculate for debugging
        await Page!.ScreenshotAsync(new() { Path = "before-calculate-200k.png" });
        
        if (await CalculatorPage.AreResultsDisplayed())
        {
            await CalculatorPage.WaitForResults();
        }
        
        // Take a screenshot after calculation for debugging
        await Page.ScreenshotAsync(new() { Path = "after-calculate-200k.png" });

        // Get the actual salary value from the input field after calculation
        var actualInputValue = await Page.Locator("#annual-salary").InputValueAsync();
        
        // Assert - The input should still be 200000, not reset to 60000
        Assert.Equal("200000", actualInputValue);

        if (await CalculatorPage.AreResultsDisplayed())
        {
            var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
            var grossAmount = ParseCurrency(monthlyGross);
            Assert.NotEqual(5000m, grossAmount);
        }
    }

    /// <summary>
    /// Verification test: Manual entry and immediate verification of input value
    /// </summary>
    [Fact]
    public async Task Verify_Input_Value_Persists_During_Form_Interaction()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();

        // Act - Enter 200000
        await CalculatorPage!.EnterSalary(200000);
        await CalculatorPage.ClickCalculate();
        await CalculatorPage.WaitForCalculationComplete();
        
        // Get the value immediately after entry
        var valueAfterEntry = await Page.Locator("#annual-salary").InputValueAsync();
        
        // Assert - Value should be 200000
        Assert.Equal("200000", valueAfterEntry);
        
        // Act - Verify value remains after calculation
        await CalculatorPage.WaitForCalculationComplete();
        
        // Get the value after calculation
        var valueAfterCalculate = await Page.Locator("#annual-salary").InputValueAsync();
        
        // Assert - Value should STILL be 200000, not reset to 60000
        Assert.Equal("200000", valueAfterCalculate);
    }

    /// <summary>
    /// Test to check if the default value (60000) is interfering with user input
    /// </summary>
    [Fact]
    public async Task Test_Default_Value_Does_Not_Override_User_Input()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.WaitForAutoCalculatedResults();
        
        // Check the initial default value
        var initialValue = await Page!.Locator("#annual-salary").InputValueAsync();
        
        // The default might be 60000 based on the Calculator.razor code
        // This could be causing the issue
        
        // Act - Clear and enter a different value
        await CalculatorPage!.CalculateSalary(150000);
        
        var afterEntry = await Page.Locator("#annual-salary").InputValueAsync();
        Assert.Equal("150000", afterEntry);
        
        // Verify the value persisted
        var afterCalculate = await Page.Locator("#annual-salary").InputValueAsync();
        Assert.Equal("150000", afterCalculate);
        
        // Verify results are for 150000, not the default 60000
        var monthlyGross = await CalculatorPage.GetMonthlyGrossSalary();
        var grossAmount = ParseCurrency(monthlyGross);
        
        // 150000 / 12 = 12500
        AssertCurrencyEqual(12500m, grossAmount, 0.50m);
        Assert.NotEqual(5000m, grossAmount); // Should not be 60000 calculation
    }
}
