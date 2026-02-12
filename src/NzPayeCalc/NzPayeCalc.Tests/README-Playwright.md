# Playwright UI Tests for NZ PAYE Calculator

## Overview
This test suite implements end-to-end UI testing for the NZ PAYE Calculator using Microsoft Playwright. The tests verify the complete user journey, edge cases, error handling, and accessibility compliance.

## Test Coverage

### Test Suites

1. **BasicCalculationJourneyTests** (`UI/BasicCalculationJourneyTests.cs`)
   - Tests E2E-J1-001 through E2E-J1-008
   - Validates the primary user flow from input to results display
   - Verifies correct calculation outputs for $60,000 annual salary
   - **Requirements**: REQ-037 (Primary User Flow), REQ-014, REQ-016, REQ-017

2. **EdgeCaseTests** (`UI/EdgeCaseTests.cs`)
   - Tests E2E-J2-001 through E2E-J2-006
   - Validates boundary conditions and special values
   - Tests minimum wage, high earners, ACC cap, tax bracket boundaries, zero salary
   - **Requirements**: REQ-038 (Edge Cases), REQ-011 (ACC cap), REQ-001-REQ-005 (Tax brackets)

3. **ErrorHandlingTests** (`UI/ErrorHandlingTests.cs`)
   - Tests E2E-J3-001 through E2E-J3-005
   - Validates input validation and error messaging
   - Tests negative, non-numeric, empty inputs, and error recovery
   - **Requirements**: REQ-039 (Error Scenarios), REQ-020, REQ-021, REQ-035

4. **AccessibilityTests** (`UI/AccessibilityTests.cs`)
   - Tests A11Y-001 through A11Y-ARIA-003
   - Validates WCAG AA compliance
   - Tests keyboard navigation, ARIA labels, focus indicators, semantic structure
   - **Requirements**: REQ-040 (WCAG AA Accessibility)

## Test Architecture

### Page Object Model
- **CalculatorPage** (`PageObjects/CalculatorPage.cs`)
  - Encapsulates all UI interactions with the calculator page
  - Provides reusable methods for common actions (navigate, enter salary, click calculate, etc.)
  - Centralizes element locators for maintainability

### Base Test Class
- **PlaywrightTestBase** (`PlaywrightTestBase.cs`)
  - Implements `IAsyncLifetime` for test fixture setup/teardown
  - Manages Aspire application hosting (starts web frontend)
  - Initializes Playwright browser and context
  - Provides utility methods for currency parsing and assertions

## Prerequisites

### Required Software
1. **.NET 10.0 SDK** - Already installed with the project
2. **Playwright Browsers** - Installed via `playwright install`
3. **Microsoft.Playwright** NuGet package - Already added to test project

### Browser Installation
The browsers are already installed globally on this machine at:
- Chromium: `C:\Users\dwsch\AppData\Local\ms-playwright\chromium-1208`
- Firefox: `C:\Users\dwsch\AppData\Local\ms-playwright\firefox-1509`
- WebKit: `C:\Users\dwsch\AppData\Local\ms-playwright\webkit-2248`

To reinstall or update browsers:
```powershell
playwright install
```

## Running the Tests

### Run All Playwright Tests
```powershell
cd src/NzPayeCalc/NzPayeCalc.Tests
dotnet test --filter "FullyQualifiedName~UI"
```

### Run Specific Test Suite
```powershell
# Basic calculation journey
dotnet test --filter "FullyQualifiedName~BasicCalculationJourneyTests"

# Edge cases
dotnet test --filter "FullyQualifiedName~EdgeCaseTests"

# Error handling
dotnet test --filter "FullyQualifiedName~ErrorHandlingTests"

# Accessibility
dotnet test --filter "FullyQualifiedName~AccessibilityTests"
```

### Run Single Test
```powershell
dotnet test --filter "FullyQualifiedName~E2E_J1_008_Monthly_TakeHome_Pay_Is_Correct"
```

### Run in Headed Mode (see browser)
```powershell
$env:HEADED = "1"
dotnet test --filter "FullyQualifiedName~UI"
$env:HEADED = $null # Reset
```

### Run with Different Browser
By default, tests run in Chromium. To use a different browser, modify `PlaywrightTestBase.cs`:
```csharp
// Change from:
Browser = await Playwright.Chromium.LaunchAsync(...)
// To:
Browser = await Playwright.Firefox.LaunchAsync(...)
// Or:
Browser = await Playwright.Webkit.LaunchAsync(...)
```

## Test Execution Flow

1. **Setup** (`InitializeAsync`):
   - Aspire application starts (AppHost, WebFrontend)
   - Waits for application health check
   - Launches Playwright browser
   - Creates page and calculator page object

2. **Test Execution**:
   - Navigate to calculator page
   - Perform test actions (enter salary, click calculate)
   - Assert expected outcomes

3. **Teardown** (`DisposeAsync`):
   - Close Playwright page and browser
   - Dispose Aspire application

## Requirements Traceability

### Test Matrix Coverage
Based on `/docs/test-matrix.md`:

| Test ID | Test Case | Status | Test Class |
|---------|-----------|--------|------------|
| E2E-J1-001 | Navigate to calculator | ✅ | BasicCalculationJourneyTests |
| E2E-J1-002 | Enter valid salary | ✅ | BasicCalculationJourneyTests |
| E2E-J1-003 | Click calculate | ✅ | BasicCalculationJourneyTests |
| E2E-J1-004 | Verify monthly gross | ✅ | BasicCalculationJourneyTests |
| E2E-J1-005 | Verify monthly PAYE | ✅ | BasicCalculationJourneyTests |
| E2E-J1-006 | Verify monthly KiwiSaver | ✅ | BasicCalculationJourneyTests |
| E2E-J1-007 | Verify monthly ACC | ✅ | BasicCalculationJourneyTests |
| E2E-J1-008 | Verify monthly take-home | ✅ | BasicCalculationJourneyTests |
| E2E-J2-001 | Minimum wage ($30,000) | ✅ | EdgeCaseTests |
| E2E-J2-002 | High earner ($200,000) | ✅ | EdgeCaseTests |
| E2E-J2-003 | ACC cap boundary ($139,384) | ✅ | EdgeCaseTests |
| E2E-J2-004 | Just above ACC cap | ✅ | EdgeCaseTests |
| E2E-J2-005 | Tax bracket boundary ($48,000) | ✅ | EdgeCaseTests |
| E2E-J2-006 | Zero salary | ✅ | EdgeCaseTests |
| E2E-J3-001 | Negative input error | ✅ | ErrorHandlingTests |
| E2E-J3-002 | Non-numeric input error | ✅ | ErrorHandlingTests |
| E2E-J3-003 | Empty input validation | ✅ | ErrorHandlingTests |
| E2E-J3-004 | Extreme value handling | ✅ | ErrorHandlingTests |
| E2E-J3-005 | Error recovery | ✅ | ErrorHandlingTests |
| A11Y-001 to A11Y-010 | WCAG AA compliance | ✅ | AccessibilityTests |
| A11Y-KB-001 to A11Y-KB-005 | Keyboard navigation | ✅ | AccessibilityTests |
| A11Y-ARIA-001 to A11Y-ARIA-003 | ARIA implementation | ✅ | AccessibilityTests |

**Total Tests Implemented**: 30+ individual test methods covering 24 test matrix scenarios

### Requirements Coverage
- **REQ-037**: Primary User Flow ✅
- **REQ-038**: Edge Cases ✅
- **REQ-039**: Error Scenarios ✅
- **REQ-040**: WCAG AA Accessibility ✅
- **REQ-001 to REQ-021**: PAYE calculations, input validation, output display (indirectly verified through E2E tests) ✅

## Troubleshooting

### Tests Fail to Start Application
- Ensure no other instance of the application is running on the expected ports
- Check that Aspire AppHost builds successfully: `dotnet build`

### Browser Not Found Errors
- Run `playwright install` to install browser binaries
- Check `C:\Users\dwsch\AppData\Local\ms-playwright\` for installed browsers

### Tests Timeout
- Default timeout is 30 seconds. For slower machines, increase `DefaultTimeout` in `PlaywrightTestBase.cs`
- Check application logs for startup issues

### Flaky Tests
- Playwright tests may occasionally fail due to timing issues
- Review wait strategies in `CalculatorPage.cs`
- Use `WaitForResults()` and `WaitForCalculationComplete()` instead of arbitrary delays

### Headless Mode Debugging
- Run tests in headed mode with `$env:HEADED = "1"`
- Add `SlowMo` parameter to `LaunchAsync` to slow down actions
- Use `await Page.PauseAsync()` to pause execution for debugging

## CI/CD Integration

### GitHub Actions / Azure DevOps
```yaml
- name: Install Playwright browsers
  run: playwright install --with-deps

- name: Run Playwright tests
  run: dotnet test --filter "FullyQualifiedName~UI" --logger "trx"
  
- name: Upload test results
  if: always()
  uses: actions/upload-artifact@v3
  with:
    name: playwright-test-results
    path: TestResults/
```

## Future Enhancements

1. **Visual Regression Testing**
   - Implement screenshot comparison tests
   - Use Playwright's screenshot capabilities

2. **Performance Testing**
   - Measure page load times
   - Track calculation response times

3. **Cross-Browser Testing**
   - Run tests against Firefox and WebKit
   - Implement browser-specific test suites

4. **Responsive Testing**
   - Test mobile viewports (REQ-041)
   - Validate responsive layouts

5. **Advanced Accessibility Testing**
   - Integrate axe-core for automated WCAG scanning
   - Test with real screen readers programmatically

## References

- [Playwright for .NET Documentation](https://playwright.dev/dotnet/)
- [Test Matrix](/docs/test-matrix.md)
- [PAYE Calculator Specification](/specs/PAYE-calculator.spec.md)
- [UX Design Specification](/docs/ux-design-specification.md)
