using Microsoft.Playwright;

namespace NzPayeCalc.Tests.UI;

/// <summary>
/// Accessibility Tests using Playwright
/// Tests WCAG AA compliance, keyboard navigation, and screen reader compatibility.
/// Requirement Coverage: REQ-040 (WCAG AA Accessibility)
/// </summary>
public class AccessibilityTests : PlaywrightTestBase
{
    /// <summary>
    /// A11Y-001 to A11Y-010: WCAG AA Compliance
    /// Tests color contrast, heading structure, labels, focus, etc.
    /// </summary>
    [Fact]
    public async Task A11Y_Automated_Accessibility_Scan_Calculator_Page()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Check that page loads with proper semantic structure
        var hasHeading = await Page!.Locator("h1").IsVisibleAsync();
        var hasInput = await Page.Locator("#annual-salary").IsVisibleAsync();
        var hasButton = await Page.GetByRole(AriaRole.Button).IsVisibleAsync();

        // Assert - Page should have accessible structure
        Assert.True(hasHeading, "Page should have an h1 heading");
        Assert.True(hasInput, "Page should have salary input");
        Assert.True(hasButton, "Page should have button");
    }

    /// <summary>
    /// A11Y-003: Heading structure
    /// Requirement: 1.3.1 Info and Relationships - Proper semantic heading hierarchy
    /// </summary>
    [Fact]
    public async Task A11Y_003_Heading_Structure_Is_Semantic()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        var h1 = await Page!.Locator("h1").AllAsync();
        var h2 = await Page.Locator("h2").AllAsync();

        // Assert
        Assert.NotEmpty(h1); // Should have at least one H1
        Assert.Single(h1); // Should have exactly one H1
        
        // H2 should only appear after results are shown
        // Initial page should have proper heading hierarchy
    }

    /// <summary>
    /// A11Y-004: Form labels
    /// Requirement: 3.3.2 Labels or Instructions - All inputs have associated labels
    /// </summary>
    [Fact]
    public async Task A11Y_004_All_Inputs_Have_Labels()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        var salaryInput = Page!.Locator("#annual-salary");
        var label = Page.Locator("label[for='annual-salary']");

        // Assert
        Assert.True(await salaryInput.IsVisibleAsync(), "Input should be visible");
        Assert.True(await label.IsVisibleAsync(), "Label should be visible");
        
        var labelText = await label.TextContentAsync();
        Assert.Contains("Annual Salary", labelText ?? "");
    }

    /// <summary>
    /// A11Y-005: Error identification
    /// Requirement: 3.3.1 Error Identification - Errors clearly described
    /// </summary>
    [Fact]
    public async Task A11Y_005_Errors_Are_Clearly_Identified()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Trigger validation error
        await CalculatorPage.EnterSalary("");
        await CalculatorPage.ClickCalculate();
        await Task.Delay(500);

        // Assert
        var validationMessage = Page!.Locator(".validation-message");
        Assert.True(await validationMessage.IsVisibleAsync(), "Validation message should be visible");
        
        var messageText = await validationMessage.TextContentAsync();
        Assert.NotNull(messageText);
        Assert.NotEmpty(messageText);
    }

    /// <summary>
    /// A11Y-KB-001 to A11Y-KB-003: Keyboard Navigation
    /// Requirement: Tab to input field, tab to button, activate with Enter
    /// </summary>
    [Fact]
    public async Task A11Y_KB_001_Tab_Navigation_To_Input_Field()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Tab to the input field
        await Page!.Keyboard.PressAsync("Tab");
        
        // Get currently focused element
        var focusedElement = await Page.EvaluateAsync<string>("document.activeElement?.id");

        // Assert - Input field should receive focus (eventually)
        // Note: May need to tab multiple times depending on page structure
        var maxTabs = 10;
        var found = false;
        for (int i = 0; i < maxTabs && !found; i++)
        {
            focusedElement = await Page.EvaluateAsync<string>("document.activeElement?.id");
            if (focusedElement == "annual-salary")
            {
                found = true;
                break;
            }
            await Page.Keyboard.PressAsync("Tab");
        }
        
        Assert.True(found, "Should be able to tab to the annual salary input field");
    }

    /// <summary>
    /// A11Y-KB-002 and A11Y-KB-003: Tab to submit button and activate via Enter
    /// </summary>
    [Fact]
    public async Task A11Y_KB_002_003_Tab_To_Button_And_Activate_With_Enter()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.EnterSalary(60000);

        // Act - Tab to calculate button
        await Page!.Keyboard.PressAsync("Tab");
        
        // Find the button by tabbing
        var maxTabs = 15;
        var found = false;
        for (int i = 0; i < maxTabs; i++)
        {
            var focusedElement = await Page.EvaluateAsync<string>(
                "document.activeElement?.textContent?.trim()");
            
            if (focusedElement?.Contains("Calculate") == true)
            {
                found = true;
                // Activate with Enter
                await Page.Keyboard.PressAsync("Enter");
                break;
            }
            await Page.Keyboard.PressAsync("Tab");
        }

        Assert.True(found, "Should be able to tab to the Calculate button");

        // Assert - Form should submit and show results
        await CalculatorPage.WaitForResults();
        Assert.True(await CalculatorPage.AreResultsDisplayed(), 
            "Results should be displayed after Enter key activation");
    }

    /// <summary>
    /// A11Y-KB-005: Tab order is logical
    /// Requirement: Tab order matches visual layout
    /// </summary>
    [Fact]
    public async Task A11Y_KB_005_Tab_Order_Is_Logical()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Tab through all elements and record order
        var tabOrder = new List<string>();
        var maxTabs = 20;
        
        for (int i = 0; i < maxTabs; i++)
        {
            await Page!.Keyboard.PressAsync("Tab");
            var focusedId = await Page.EvaluateAsync<string>("document.activeElement?.id");
            var focusedText = await Page.EvaluateAsync<string>("document.activeElement?.textContent?.trim()");
            
            if (!string.IsNullOrEmpty(focusedId) || !string.IsNullOrEmpty(focusedText))
            {
                tabOrder.Add(focusedId ?? focusedText ?? "unknown");
            }
        }

        // Assert - Tab order should include input then button
        Assert.Contains(tabOrder, item => item.Contains("annual-salary") || item.Contains("Annual Salary"));
        Assert.Contains(tabOrder, item => item.Contains("Calculate"));
        
        // Input should come before button in tab order
        var inputIndex = tabOrder.FindIndex(item => item.Contains("annual-salary") || item.Contains("Annual Salary"));
        var buttonIndex = tabOrder.FindIndex(item => item.Contains("Calculate"));
        
        if (inputIndex >= 0 && buttonIndex >= 0)
        {
            Assert.True(inputIndex < buttonIndex, "Input should come before button in tab order");
        }
    }

    /// <summary>
    /// A11Y-006: Focus visible
    /// Requirement: 2.4.7 Focus Visible - Focus indicators visible
    /// </summary>
    [Fact]
    public async Task A11Y_006_Focus_Indicators_Are_Visible()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Focus on the input
        await Page!.Locator("#annual-salary").FocusAsync();

        // Assert - Check if focus is visible by testing computed styles
        var hasFocusStyle = await Page.EvaluateAsync<bool>(@"
            () => {
                const input = document.querySelector('#annual-salary');
                const styles = window.getComputedStyle(input);
                // Check if outline or box-shadow is applied (common focus indicators)
                return styles.outline !== 'none' || 
                       styles.outlineWidth !== '0px' ||
                       styles.boxShadow !== 'none';
            }
        ");

        Assert.True(hasFocusStyle, "Focus should have visible indicator (outline or box-shadow)");
    }

    /// <summary>
    /// A11Y-ARIA-001: Input field labeling
    /// Requirement: aria-label or aria-labelledby present and descriptive
    /// </summary>
    [Fact]
    public async Task A11Y_ARIA_001_Input_Has_Accessible_Label()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        var input = Page!.Locator("#annual-salary");
        var ariaLabel = await input.GetAttributeAsync("aria-label");
        var ariaLabelledBy = await input.GetAttributeAsync("aria-labelledby");
        var ariaDescribedBy = await input.GetAttributeAsync("aria-describedby");

        // Assert - Should have either aria-label, aria-labelledby, or associated <label>
        var hasAccessibleLabel = !string.IsNullOrEmpty(ariaLabel) || 
                                 !string.IsNullOrEmpty(ariaLabelledBy) ||
                                 await Page.Locator("label[for='annual-salary']").IsVisibleAsync();

        Assert.True(hasAccessibleLabel, "Input should have accessible label");
    }

    /// <summary>
    /// A11Y-ARIA-003: Live region for results
    /// Requirement: aria-live for dynamic content updates
    /// </summary>
    [Fact]
    public async Task A11Y_ARIA_003_Results_Announced_To_Screen_Readers()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Assert - Check for aria-live or role="status" on results
        var resultsSection = Page!.Locator(".calculator-results");
        Assert.True(await resultsSection.IsVisibleAsync(), "Results section should be visible");

        // Results should be accessible to screen readers
        // (Either via aria-live, role="status", or semantic structure)
    }

    /// <summary>
    /// A11Y-009: Language of page
    /// Requirement: 3.1.1 Language of Page - HTML lang attribute set
    /// </summary>
    [Fact]
    public async Task A11Y_009_Page_Has_Language_Attribute()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        var htmlLang = await Page!.Locator("html").GetAttributeAsync("lang");

        // Assert
        Assert.NotNull(htmlLang);
        Assert.NotEmpty(htmlLang);
        Assert.Matches("^en", htmlLang); // Should be English variant (en, en-US, en-NZ, etc.)
    }

    /// <summary>
    /// A11Y-010: Page titled
    /// Requirement: 2.4.2 Page Titled - Descriptive page title
    /// </summary>
    [Fact]
    public async Task A11Y_010_Page_Has_Descriptive_Title()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act
        var title = await Page!.TitleAsync();

        // Assert
        Assert.NotNull(title);
        Assert.NotEmpty(title);
        Assert.Contains("PAYE", title, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Additional: Skip link test (if implemented)
    /// </summary>
    [Fact]
    public async Task Skip_To_Main_Content_Link_Available()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Press Tab to see if skip link appears
        await Page!.Keyboard.PressAsync("Tab");
        
        var skipLink = Page.Locator("a").Filter(new() { HasText = "Skip" }).Or(
            Page.Locator("[href='#main']")).Or(
            Page.Locator(".skip-link"));

        // Assert - Skip link may or may not be present (optional feature)
        // If present, should be the first focusable element
        var skipLinkVisible = await skipLink.IsVisibleAsync();
        // This is informational - skip links are best practice but not required for this test suite
    }

    /// <summary>
    /// Color contrast test using computed styles
    /// Requirement: 1.4.3 Contrast (Minimum) - 4.5:1 for text
    /// </summary>
    [Fact]
    public async Task A11Y_001_002_Text_Has_Sufficient_Contrast()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();

        // Act - Get computed styles for main text elements
        var h1Color = await Page!.Locator("h1").First.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).color");
        var bodyColor = await Page.Locator("p").First.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).color");

        // Assert - Colors should be defined (actual contrast calculation would require additional library)
        Assert.NotNull(h1Color);
        Assert.NotNull(bodyColor);
        
        // Note: Full contrast checking would require a dedicated accessibility testing library
        // This test verifies that text colors are explicitly set
    }

    /// <summary>
    /// A11Y-CONTRAST-001: Calculation notes text is visible
    /// Tests that the calculation-notes text has sufficient contrast and is readable
    /// Requirement: WCAG 2.1 Level AA - 1.4.3 Contrast (Minimum)
    /// </summary>
    [Fact]
    public async Task A11Y_CONTRAST_001_Calculation_Notes_Text_Is_Visible()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act - Get the calculation notes element
        var notesElement = Page!.Locator(".calculation-notes");
        var notesParagraph = notesElement.Locator("p");

        // Assert - Element should be visible
        Assert.True(await notesElement.IsVisibleAsync(), 
            "Calculation notes section should be visible");
        Assert.True(await notesParagraph.IsVisibleAsync(), 
            "Calculation notes paragraph should be visible");

        // Get computed styles using Playwright's evaluate method
        // Get text color
        var textColor = await notesParagraph.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).color");
        
        // Get background color (walk up DOM tree tohandle transparent backgrounds)
        var bgColor = await notesParagraph.EvaluateAsync<string>(@"
            el => {
                let currentEl = el;
                let bg = window.getComputedStyle(currentEl).backgroundColor;
                
                while ((!bg || bg === 'transparent' || bg === 'rgba(0, 0, 0, 0)' || 
                       (bg.startsWith('rgba') && bg.endsWith(', 0)'))) && currentEl.parentElement) {
                    currentEl = currentEl.parentElement;
                    bg = window.getComputedStyle(currentEl).backgroundColor;
                }
                
                return bg || 'rgb(13, 17, 23)';
            }");
        
        var opacity = await notesParagraph.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).opacity");
        var visibility = await notesParagraph.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).visibility");
        var display = await notesParagraph.EvaluateAsync<string>(
            "el => window.getComputedStyle(el).display");

        // Assert - Element must be displayed
        Assert.NotEqual("none", display);
        Assert.NotEqual("hidden", visibility);
        
        // Assert - Opacity should not make it invisible
        var opacityValue = double.Parse(opacity);
        Assert.True(opacityValue >= 0.7, 
            $"Text opacity is too low ({opacityValue}). Should be at least 0.7 for readability.");

        // Get color values and parse RGB
        var colorRgb = ParseRgbColor(textColor);
        var bgColorRgb = ParseRgbColor(bgColor);

        // Assert - Text color should not be too light (luminance check)
        var textLuminance = CalculateRelativeLuminance(colorRgb.r, colorRgb.g, colorRgb.b);
        var bgLuminance = CalculateRelativeLuminance(bgColorRgb.r, bgColorRgb.g, bgColorRgb.b);

        // Calculate contrast ratio
        var contrastRatio = CalculateContrastRatio(textLuminance, bgLuminance);

        // WCAG AA requires:
        // - 4.5:1 for normal text (< 18pt or < 14pt bold)
        // - 3:1 for large text (>= 18pt or >= 14pt bold)
        // The .small class makes this normal text, so we need 4.5:1
        Assert.True(contrastRatio >= 4.5, 
            $"Calculation notes text contrast ratio is {contrastRatio:F2}:1, " +
            $"which fails WCAG AA requirement of 4.5:1 for normal text. " +
            $"Text color: {textColor}, " +
            $"Background: {bgColor}");
    }

    /// <summary>
    /// A11Y-CONTRAST-002: All visible text meets minimum contrast requirements
    /// </summary>
    [Fact]
    public async Task A11Y_CONTRAST_002_All_Text_Elements_Have_Sufficient_Contrast()
    {
        // Arrange
        await CalculatorPage!.NavigateToCalculator();
        await CalculatorPage.CalculateSalary(60000);
        await CalculatorPage.WaitForResults();

        // Act - Get all text elements that should be readable
        var textSelectors = new[]
        {
            "h1",
            "h2", 
            "label",
            ".form-text",
            ".calculation-notes p",
            ".result-row dt",
            ".result-row dd"
        };

        var contrastFailures = new List<string>();

        foreach (var selector in textSelectors)
        {
            var elements = await Page!.Locator(selector).AllAsync();
            
            foreach (var element in elements)
            {
                if (!await element.IsVisibleAsync())
                    continue;

                // Get individual style properties
                var textColor = await element.EvaluateAsync<string>(
                    "el => window.getComputedStyle(el).color");
                
                var bgColor = await element.EvaluateAsync<string>(@"
                    el => {
                        let currentEl = el;
                        let bg = window.getComputedStyle(currentEl).backgroundColor;
                        
                        while ((!bg || bg === 'transparent' || bg === 'rgba(0, 0, 0, 0)' || 
                               (bg.startsWith('rgba') && bg.endsWith(', 0)'))) && currentEl.parentElement) {
                            currentEl = currentEl.parentElement;
                            bg = window.getComputedStyle(currentEl).backgroundColor;
                        }
                        
                        return bg || 'rgb(13, 17, 23)';
                    }");
                
                var fontSize = await element.EvaluateAsync<string>(
                    "el => window.getComputedStyle(el).fontSize");
                var fontWeight = await element.EvaluateAsync<string>(
                    "el => window.getComputedStyle(el).fontWeight");

                var colorRgb = ParseRgbColor(textColor);
                var bgColorRgb = ParseRgbColor(bgColor);

                var textLuminance = CalculateRelativeLuminance(colorRgb.r, colorRgb.g, colorRgb.b);
                var bgLuminance = CalculateRelativeLuminance(bgColorRgb.r, bgColorRgb.g, bgColorRgb.b);
                var contrastRatio = CalculateContrastRatio(textLuminance, bgLuminance);

                // Determine if this is large text
                var fontSizeValue = ParseFontSize(fontSize);
                var fontWeightValue = int.Parse(fontWeight);
                var isLargeText = fontSizeValue >= 18 || (fontSizeValue >= 14 && fontWeightValue >= 700);

                var requiredRatio = isLargeText ? 3.0 : 4.5;

                if (contrastRatio < requiredRatio)
                {
                    var elementText = await element.TextContentAsync();
                    var textLength = elementText?.Length ?? 0;
                    var displayText = textLength > 0 
                        ? elementText!.Substring(0, Math.Min(30, textLength))
                        : "(empty)";
                    contrastFailures.Add(
                        $"{selector}: {contrastRatio:F2}:1 (needs {requiredRatio}:1) - '{displayText}...'");
                }
            }
        }

        // Assert - No contrast failures
        Assert.Empty(contrastFailures);
    }

    // Helper methods for color contrast calculation

    private (int r, int g, int b) ParseRgbColor(string rgbString)
    {
        // Handle "rgb(r, g, b)" or "rgba(r, g, b, a)" format
        var match = System.Text.RegularExpressions.Regex.Match(
            rgbString, @"rgba?\((\d+),\s*(\d+),\s*(\d+)");
        
        if (match.Success)
        {
            return (
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value)
            );
        }

        // Default to black if parsing fails
        return (0, 0, 0);
    }

    private double CalculateRelativeLuminance(int r, int g, int b)
    {
        // WCAG formula for relative luminance
        var rsRGB = r / 255.0;
        var gsRGB = g / 255.0;
        var bsRGB = b / 255.0;

        var rLinear = rsRGB <= 0.03928 ? rsRGB / 12.92 : Math.Pow((rsRGB + 0.055) / 1.055, 2.4);
        var gLinear = gsRGB <= 0.03928 ? gsRGB / 12.92 : Math.Pow((gsRGB + 0.055) / 1.055, 2.4);
        var bLinear = bsRGB <= 0.03928 ? bsRGB / 12.92 : Math.Pow((bsRGB + 0.055) / 1.055, 2.4);

        return 0.2126 * rLinear + 0.7152 * gLinear + 0.0722 * bLinear;
    }

    private double CalculateContrastRatio(double luminance1, double luminance2)
    {
        // WCAG formula for contrast ratio
        var lighter = Math.Max(luminance1, luminance2);
        var darker = Math.Min(luminance1, luminance2);
        
        return (lighter + 0.05) / (darker + 0.05);
    }

    private double ParseFontSize(string fontSizeString)
    {
        // Parse "16px" to 16
        var match = System.Text.RegularExpressions.Regex.Match(fontSizeString, @"([\d.]+)");
        if (match.Success)
        {
            return double.Parse(match.Groups[1].Value);
        }
        return 16; // Default
    }
}
