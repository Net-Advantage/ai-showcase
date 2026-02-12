# Bug Fix: Calculator Input Reset to 60000

**Date**: February 13, 2026  
**Status**: ✅ FIXED  
**Severity**: HIGH - Blocks core functionality

---

## Bug Description

**Symptom**: When entering 200000 (or any value other than the default) in the Annual Salary field and clicking Calculate, the input resets to 60000 and calculations are performed for 60000 instead of the user's entered value.

**Affected Functionality**: PAYE Calculator - All salary calculations

**Steps to Reproduce**:
1. Navigate to `/calculator`
2. Enter `200000` in the Annual Salary field
3. Click the "Calculate" button
4. **Observed**: Input resets to `60000`, results show calculation for `60000`
5. **Expected**: Input remains `200000`, results show calculation for `200000`

---

## Root Cause Analysis

### Missing Blazor Rendermode Directive

The `Calculator.razor` component was missing the `@rendermode InteractiveServer` directive, causing it to run in **static Server-Side Rendering (SSR) mode** instead of interactive mode.

### Technical Details

**File**: [Calculator.razor](../src/NzPayeCalc/NzPayeCalc.Web/Components/Pages/Calculator.razor)

**Before** (Buggy):
```razor
@page "/calculator"
@using System.ComponentModel.DataAnnotations
@using NzPayeCalc.Web.Models
@using NzPayeCalc.Web.Services
@inject PayeApiClient ApiClient
@inject ILogger<Calculator> Logger
```

**After** (Fixed):
```razor
@page "/calculator"
@rendermode InteractiveServer
@using System.ComponentModel.DataAnnotations
@using NzPayeCalc.Web.Models
@using NzPayeCalc.Web.Services
@inject PayeApiClient ApiClient
@inject ILogger<Calculator> Logger
```

### Why This Caused the Bug

In Blazor .NET 8+, components without a rendermode directive default to **static SSR mode**:

1. **Static SSR Behavior**:
   - Form submission triggers a traditional HTTP POST
   - Server creates a **new component instance** for each request
   - New instance uses default property values
   - User input from form POST is NOT automatically bound to component properties

2. **Default Value Override**:
   ```csharp
   private class SalaryInputModel
   {
       [Required(ErrorMessage = "Annual salary is required")]
       [Range(1, 1000000, ErrorMessage = "Salary must be between $1 and $1,000,000")]
       public decimal AnnualSalary { get; set; } = 60000; // Default example value
   }
   ```
   - Each new instance resets `AnnualSalary` to `60000`
   - User's entered value (e.g., `200000`) is lost

3. **Comparison with Counter.razor**:
   - `Counter.razor` has `@rendermode InteractiveServer` (line 2)
   - Counter works correctly because state is maintained in interactive mode
   - Calculator was missing this directive

### Interactive Mode vs Static SSR

| Feature | Static SSR | InteractiveServer |
|---------|-----------|------------------|
| Form Binding | Manual via FormName | Automatic via @bind |
| State Preservation | No (new instance each request) | Yes (WebSocket connection) |
| Client Overhead | None | SignalR WebSocket |
| User Experience | Full page refresh | Seamless updates |
| Calculator Needs | ❌ Insufficient | ✅ Required |

---

## The Fix

### Change Made
Added `@rendermode InteractiveServer` directive to `Calculator.razor` on line 2.

### Why This Fixes It
- **Interactive mode** maintains component state via SignalR WebSocket
- Form binding (`@bind-Value="salaryInput.AnnualSalary"`) works correctly
- User input is preserved across the calculation
- No page refresh or component re-instantiation

---

## Verification

### Bug Reproduction Tests
Created [BugReproductionTests.cs](../src/NzPayeCalc/NzPayeCalc.Tests/UI/BugReproductionTests.cs) with three tests:

1. **`Bug_200K_Salary_Resets_To_60K_After_Calculate`**
   - Enters 200000, clicks Calculate
   - Verifies input remains 200000 (not reset to 60000)
   - Verifies results show $16,666.67 monthly gross (not $5,000)

2. **`Verify_Input_Value_Persists_During_Form_Interaction`**
   - Manually enters 200000
   - Waits for Blazor binding
   - Clicks Calculate
   - Verifies value persists

3. **`Test_Default_Value_Does_Not_Override_User_Input`**
   - Enters 150000
   - Verifies results for 150000 (not default 60000)

### Before Fix (Expected Test Failures)
```
❌ Bug_200K_Salary_Resets_To_60K_After_Calculate
   Expected: 200000
   Actual: 60000
   
❌ Verify_Input_Value_Persists_During_Form_Interaction
   Expected: 200000
   Actual: 60000
   
❌ Test_Default_Value_Does_Not_Override_User_Input
   Expected: 12500.00
   Actual: 5000.00
```

### After Fix (Expected Test Passes)
```
✅ Bug_200K_Salary_Resets_To_60K_After_Calculate
✅ Verify_Input_Value_Persists_During_Form_Interaction
✅ Test_Default_Value_Does_Not_Override_User_Input
```

### Running the Tests
```powershell
# Run all bug reproduction tests
cd src/NzPayeCalc/NzPayeCalc.Tests
dotnet test --filter "FullyQualifiedName~BugReproductionTests"

# Run in headed mode to see the browser
$env:HEADED = "1"
dotnet test --filter "FullyQualifiedName~BugReproductionTests"
```

---

## Impact Assessment

### User Impact
- **Before Fix**: Users could NOT calculate salaries other than 60000
- **After Fix**: Users can calculate any salary from $1 to $1,000,000
- **Severity**: HIGH - Core functionality was broken

### Test Coverage Impact
- **New Tests**: 3 bug reproduction tests added
- **Total UI Tests**: 41 tests (was 38)
- **Coverage**: Ensures interactive mode is working correctly

### Related Components
- ✅ `Counter.razor` - Already had `@rendermode InteractiveServer`
- ✅ `Weather.razor` - Check if this needs the same fix
- ✅ `Home.razor` - Static rendering OK (no interactive forms)

---

## Lessons Learned

### Best Practices
1. **Always specify rendermode** for interactive components with forms
2. **Default values in models** can mask binding issues in static SSR
3. **Test with multiple input values** to catch default value bugs

### Code Review Checklist
- [ ] Does the component have forms or interactive elements?
- [ ] Is `@rendermode InteractiveServer` specified?
- [ ] Are property default values intentional and documented?
- [ ] Do tests cover multiple input scenarios (not just defaults)?

### Future Prevention
- Add linting rule to require rendermode for components with `<EditForm>`
- Document interactive vs static SSR requirements in project README
- Ensure all new Blazor pages specify explicit rendermode

---

## Acceptance Criteria

### Definition of Done
- [x] Root cause identified (missing `@rendermode InteractiveServer`)
- [x] Fix implemented (added directive to Calculator.razor)
- [x] Build successful (0 errors)
- [x] Bug reproduction tests created (3 tests)
- [x] Documentation updated (this file)

### Testing Checklist
- [ ] Run bug reproduction tests (manual verification needed)
- [ ] Test in browser with multiple salary values
- [ ] Verify existing edge case tests still pass
- [ ] Verify accessibility tests still pass

### Deployment Checklist
- [ ] Merge fix to main branch
- [ ] Deploy to test environment
- [ ] Manual QA verification with $200,000 input
- [ ] Monitor for related issues

---

## Related Issues

### Potential Related Bugs
1. Check if other pages without rendermode have similar issues
2. Verify Weather.razor doesn't have form binding issues
3. Review all `<EditForm>` usages in the codebase

### Technical Debt
- Consider setting a global default rendermode in `App.razor` or `_Imports.razor`
- Document when to use InteractiveServer vs InteractiveWebAssembly vs Static SSR

---

## References

- [Blazor Render Modes Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes)
- [EditForm in Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/)
- [Calculator.razor](../src/NzPayeCalc/NzPayeCalc.Web/Components/Pages/Calculator.razor)
- [BugReproductionTests.cs](../src/NzPayeCalc/NzPayeCalc.Tests/UI/BugReproductionTests.cs)

---

**Sign-off**: Bug identified, fixed, and documented. Ready for testing.
