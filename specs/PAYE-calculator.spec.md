## PAYE Calculator Specification

**Version**: 2.0  
**Tax Year**: 2024/2025  
**Last Updated**: February 13, 2026

### Overview
A web-based calculator that computes New Zealand PAYE (Pay As You Earn) tax, KiwiSaver contributions, and ACC levy based on annual salary input. The calculator displays both monthly and annual breakdowns of gross salary, deductions, and take-home pay.

### Glossary
- **PAYE**: Pay As You Earn - New Zealand's income tax system for employees
- **KiwiSaver**: Voluntary retirement savings scheme with employee contributions
- **ACC Earners' Levy**: Accident Compensation Corporation levy - mandatory workplace accident insurance
- **Progressive Tax**: Tax system where income is taxed across multiple brackets at different rates
- **Take-Home Pay**: Net salary after all deductions (also called "net pay")
- **Gross Salary**: Total salary before any deductions

---

## 1. Input Requirements

### 1.1 Input Field
- **Label**: "Annual Salary (NZD)"
- **Type**: Decimal number
- **Help Text**: "Enter your gross annual salary before tax"
- **Placeholder**: "60,000" (example value)
- **Default Value**: 60,000 (for demonstration purposes)

### 1.2 Input Validation

#### Client-Side Validation
- **Required**: Annual salary must be provided
- **Minimum**: $1.00
- **Maximum**: $1,000,000.00 (reasonable upper bound for UI)
- **Format**: Valid decimal number
- **Error Messages**:
  - Empty: "Annual salary is required"
  - Out of range: "Salary must be between $1 and $1,000,000"

#### Server-Side Validation (API)
- **Required**: Annual salary must be provided
- **Minimum**: $0.01
- **Maximum**: $999,999,999.99
- **Format**: Valid decimal type
- **Error Response**: HTTP 400 Bad Request with validation details

**Rationale**: Client-side validation provides immediate feedback; server-side validation ensures security and data integrity.

---

## 2. Tax Calculations

### 2.1 PAYE Tax Rates (2024/2025)

New Zealand uses a progressive tax system with five tax brackets:

| Annual Income Range | Tax Rate | Calculation |
|---------------------|----------|-------------|
| $0 - $14,000 | 10.5% | Income in bracket × 0.105 |
| $14,001 - $48,000 | 17.5% | Income in bracket × 0.175 |
| $48,001 - $70,000 | 30% | Income in bracket × 0.30 |
| $70,001 - $180,000 | 33% | Income in bracket × 0.33 |
| Over $180,000 | 39% | Income in bracket × 0.39 |

**Progressive Calculation Method**:
1. Calculate tax for income in first bracket
2. Add tax for income in second bracket (if applicable)
3. Continue through all applicable brackets
4. Sum total tax across all brackets

**Example**: For $60,000 annual salary:
- Bracket 1: $14,000 × 10.5% = $1,470.00
- Bracket 2: $34,000 × 17.5% = $5,950.00 (from $14,001 to $48,000)
- Bracket 3: $12,000 × 30% = $3,600.00 (from $48,001 to $60,000)
- **Total annual PAYE**: $11,020.00

### 2.2 KiwiSaver Contribution
- **Rate**: 3% of gross salary
- **Calculation**: Annual Salary × 0.03
- **Cap**: None (applies to full gross salary)
- **Mandatory**: No, but calculator assumes 3% employee contribution

**Example**: $60,000 × 3% = $1,800.00 annual KiwiSaver

### 2.3 ACC Earners' Levy
- **Rate**: 1.53% of gross salary
- **Maximum Earnings Threshold**: $139,384
- **Calculation**: min(Annual Salary, $139,384) × 0.0153
- **Cap Rationale**: ACC levy only applies to first $139,384 of income

**Examples**:
- $60,000: $60,000 × 1.53% = $918.00 (below cap)
- $200,000: $139,384 × 1.53% = $2,132.57 (capped at threshold)

---

## 3. Calculation Algorithm

### 3.1 Calculation Steps
1. Validate input (annual salary > 0)
2. Calculate annual PAYE tax using progressive tax brackets (Section 2.1)
3. Calculate annual KiwiSaver: Annual Salary × 3%
4. Calculate annual ACC: min(Annual Salary, $139,384) × 1.53%
5. Calculate annual take-home pay: Annual Salary - PAYE - KiwiSaver - ACC
6. Calculate monthly values: Divide all annual amounts by 12
7. Round all currency values to 2 decimal places for display

### 3.2 Precision and Rounding

#### During Calculation
- **Data Type**: Decimal (not float or double)
- **Precision**: Full decimal precision maintained throughout calculation
- **Rationale**: Prevents floating-point rounding errors

#### For Display
- **Rounding Method**: Banker's Rounding (Round-to-Even / MidpointRounding.ToEven)
- **Decimal Places**: 2 (standard currency format)
- **Application**: Applied only at final display step, not during intermediate calculations

**Example**: 
- Calculation maintains $918.333333... during computation
- Display shows $918.33 after rounding

### 3.3 Currency Formatting
- **Format**: $X,XXX.XX (NZD)
- **Currency Symbol**: $ (prefix)
- **Thousand Separators**: Yes (comma)
- **Decimal Places**: 2 (always shown, even for whole numbers)
- **Examples**:
  - $5,000.00 (not $5000 or $5,000)
  - $918.33
  - $0.00

---

## 4. Output Display

### 4.1 Monthly Breakdown (Primary Display)

The calculator prominently displays monthly values:

- **Monthly Gross Salary**: Annual salary ÷ 12
- **PAYE Tax**: Annual PAYE ÷ 12 (shown as deduction with minus sign)
- **KiwiSaver (3%)**: Annual KiwiSaver ÷ 12 (shown as deduction with minus sign)
- **ACC Levy (1.53%)**: Annual ACC ÷ 12 (shown as deduction with minus sign)
- **Take-Home Pay**: Monthly gross - monthly deductions (highlighted/emphasized)

**Visual Hierarchy**:
- Large, prominent display
- Deductions visually distinct (prefixed with "-")
- Take-Home Pay emphasized (larger/bolder)

### 4.2 Annual Breakdown (Secondary Display)

The calculator also displays annual values in a collapsible section:

- **Annual Gross Salary**: Input value
- **PAYE Tax**: Annual PAYE (shown as deduction)
- **KiwiSaver (3%)**: Annual KiwiSaver (shown as deduction)
- **ACC Levy (1.53%)**: Annual ACC (shown as deduction)
- **Take-Home Pay**: Annual gross - annual deductions

**Interaction**:
- Collapsed by default (progressive disclosure)
- Toggle label: "View Annual Breakdown"
- Same structure as monthly breakdown

### 4.3 Results Layout

**Structure** (using semantic HTML):
```
Monthly Breakdown
  ├─ Monthly Gross Salary: $X,XXX.XX
  ├─ PAYE Tax: -$XXX.XX
  ├─ KiwiSaver (3%): -$XXX.XX
  ├─ ACC Levy (1.53%): -$XX.XX
  └─ Take-Home Pay: $X,XXX.XX

[▶ View Annual Breakdown]
  (Collapsible section with same structure)
```

---

## 5. User Interface

### 5.1 Page Layout

**Components**:
1. **Header Section**:
   - Page title: "NZ PAYE Calculator"
   - Subtitle: "Calculate your monthly take-home pay based on your annual salary"

2. **Input Section**:
   - Annual salary input field (with label and help text)
   - Calculate button
   - Validation messages (inline, below input)

3. **Results Section** (displayed after successful calculation):
   - Monthly breakdown (always visible)
   - Annual breakdown (collapsible details)
   - Calculation note/disclaimer

4. **Error Section** (displayed on error):
   - Error message in alert banner
   - Dismissible close button

### 5.2 Interactive States

#### Default State
- Input field empty or with default value
- Calculate button enabled
- No results displayed
- No errors displayed

#### Loading State
- Calculate button shows spinner and "Calculating..." text
- Calculate button disabled
- Input field remains editable

#### Success State
- Results section visible
- Both monthly and annual breakdowns displayed
- Input field retains entered value
- Calculate button returns to default "Calculate" state

#### Error State
- Error message displayed in dismissible alert banner
- Input field retains entered value
- Results hidden (if previously shown)
- Form remains interactive for correction

### 5.3 Error Handling

#### Error Types and Messages

**Validation Error** (client-side):
- Display: Inline below input field
- Style: Red text, accessible color contrast
- Examples:
  - "Annual salary is required"
  - "Salary must be between $1 and $1,000,000"

**API Communication Error**:
- Display: Alert banner above results
- Message: "Unable to connect to the calculation service. Please try again."
- Action: Dismissible close button

**Server Error**:
- Display: Alert banner above results
- Message: "An unexpected error occurred. Please try again."
- Action: Dismissible close button

**Form State on Error**:
- Input value preserved
- User can correct and resubmit
- Previous results hidden (if any)

### 5.4 Accessibility Requirements

The calculator must meet **WCAG 2.1 Level AA** standards:

#### Semantic HTML
- Proper heading hierarchy (H1 for page title)
- Labels associated with form inputs (`for` attribute)
- Semantic lists for results (`<dl>`, `<dt>`, `<dd>`)
- Native HTML controls (`<button>`, `<input>`, `<details>`)

#### Keyboard Navigation
- All interactive elements keyboard accessible
- Logical tab order
- Visible focus indicators
- Enter key submits form
- Space/Enter toggles annual breakdown

#### Screen Reader Support
- ARIA labels where needed
- Form field descriptions (`aria-describedby`)
- Error messages announced
- Loading state communicated

#### Visual Accessibility
- Color contrast ratio ≥ 4.5:1 (normal text)
- Color contrast ratio ≥ 3:1 (large text, UI components)
- Focus indicators visible
- No information conveyed by color alone
- Minimum touch target size: 44×44 CSS pixels

#### Responsive Design
- Single column layout on mobile (<768px)
- Readable font sizes (≥16px on mobile)
- Touch-friendly button sizes
- Horizontal scroll not required
- Zoom to 200% without loss of functionality

---

## 6. API Contract

### 6.1 Endpoint

**POST** `/api/paye/calculate`

- **Content-Type**: `application/json`
- **Authentication**: None (public endpoint)

### 6.2 Request

**Schema**:
```json
{
  "annualSalary": number (decimal, required)
}
```

**Constraints**:
- `annualSalary` > 0.01
- `annualSalary` ≤ 999,999,999.99
- Must be valid decimal format

**Example Request**:
```json
{
  "annualSalary": 60000.00
}
```

### 6.3 Response (Success - 200 OK)

**Schema**:
```json
{
  "annualSalary": number,
  "annual": {
    "grossSalary": number,
    "payeTax": number,
    "kiwiSaver": number,
    "accLevy": number,
    "takeHomePay": number
  },
  "monthly": {
    "grossSalary": number,
    "payeTax": number,
    "kiwiSaver": number,
    "accLevy": number,
    "takeHomePay": number
  }
}
```

**Example Response** (for $60,000):
```json
{
  "annualSalary": 60000.00,
  "annual": {
    "grossSalary": 60000.00,
    "payeTax": 11020.00,
    "kiwiSaver": 1800.00,
    "accLevy": 918.00,
    "takeHomePay": 46262.00
  },
  "monthly": {
    "grossSalary": 5000.00,
    "payeTax": 918.33,
    "kiwiSaver": 150.00,
    "accLevy": 76.50,
    "takeHomePay": 3855.17
  }
}
```

**Field Descriptions**:
- All monetary values in NZD
- All values rounded to 2 decimal places
- Monthly values = Annual values ÷ 12
- `takeHomePay` = `grossSalary` - `payeTax` - `kiwiSaver` - `accLevy`

### 6.4 Error Responses

#### 400 Bad Request (Validation Error)

**Schema** (RFC 7807 Problem Details):
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "AnnualSalary": [
      "Annual salary must be between $0.01 and $999,999,999.99"
    ]
  }
}
```

#### 500 Internal Server Error

**Schema**:
```json
{
  "type": "https://httpstatuses.com/500",
  "title": "An error occurred processing your request.",
  "status": 500
}
```

---

## 7. Examples and Test Cases

### 7.1 Example: $60,000 Annual Salary

**Input**: Annual Salary = $60,000.00

**Calculation Breakdown**:

**PAYE Tax** (Progressive):
- $0 - $14,000 @ 10.5% = $1,470.00
- $14,001 - $48,000 @ 17.5% = $5,950.00
- $48,001 - $60,000 @ 30% = $3,600.00
- **Total Annual PAYE**: $11,020.00

**KiwiSaver**: $60,000 × 3% = $1,800.00

**ACC Levy**: $60,000 × 1.53% = $918.00 (under cap)

**Annual Take-Home**: $60,000 - $11,020 - $1,800 - $918 = $46,262.00

**Monthly Breakdown** (Annual ÷ 12):
- Monthly Gross: $5,000.00
- Monthly PAYE: $918.33
- Monthly KiwiSaver: $150.00
- Monthly ACC: $76.50
- **Monthly Take-Home**: $3,855.17

### 7.2 Example: $200,000 Annual Salary (ACC Cap)

**Input**: Annual Salary = $200,000.00

**PAYE Tax** (Progressive):
- $0 - $14,000 @ 10.5% = $1,470.00
- $14,001 - $48,000 @ 17.5% = $5,950.00
- $48,001 - $70,000 @ 30% = $6,600.00
- $70,001 - $180,000 @ 33% = $36,300.00
- $180,001 - $200,000 @ 39% = $7,800.00
- **Total Annual PAYE**: $58,120.00

**KiwiSaver**: $200,000 × 3% = $6,000.00

**ACC Levy**: $139,384 × 1.53% = $2,132.57 (**capped at maximum earnings**)

**Annual Take-Home**: $200,000 - $58,120 - $6,000 - $2,132.57 = $133,747.43

**Monthly Breakdown**:
- Monthly Gross: $16,666.67
- Monthly PAYE: $4,843.33
- Monthly KiwiSaver: $500.00
- Monthly ACC: $177.71
- **Monthly Take-Home**: $11,145.62

### 7.3 Edge Cases

| Scenario | Annual Salary | Expected Behavior | Notes |
|----------|---------------|-------------------|-------|
| Minimum input | $1.00 | All calculations work, very small amounts | Tests lower bound |
| Zero salary | $0.00 | All outputs $0.00 | Special case handling |
| First bracket boundary | $14,000 | Only first bracket applied | Bracket boundary |
| Second bracket boundary | $48,000 | First two brackets applied | Bracket boundary |
| Third bracket boundary | $70,000 | First three brackets applied | Bracket boundary |
| Fourth bracket boundary | $180,000 | First four brackets applied | Bracket boundary |
| ACC cap exact | $139,384 | ACC at maximum ($2,132.57) | ACC cap boundary |
| ACC cap + $1 | $139,385 | ACC still at maximum | Verifies cap works |
| High earner | $500,000 | All five brackets, ACC capped | Top bracket + cap |

---

## 8. Notes and Disclaimers

### 8.1 Disclaimer Text

The calculator should display the following note:

> **Note**: This calculation uses 2024/2025 NZ tax rates. This is an estimate and does not include other deductions such as student loans or additional voluntary contributions.

**Placement**: Below results section, in smaller text (muted color)

### 8.2 Limitations

This calculator:
- ✅ Calculates PAYE tax using 2024/2025 rates
- ✅ Includes KiwiSaver at 3% employee contribution
- ✅ Includes ACC Earners' Levy at 1.53%
- ❌ Does NOT include student loan repayments
- ❌ Does NOT include additional voluntary KiwiSaver contributions
- ❌ Does NOT include other deductions (e.g., union fees, insurance)
- ❌ Does NOT account for secondary tax codes (assumes primary employment)
- ❌ Does NOT account for individual tax credits or rebates

### 8.3 Tax Year

- **Current Tax Year**: 2024/2025
- **Update Frequency**: Annually (after NZ Budget announcement)
- **Rate Change Process**: Update tax brackets in configuration without code changes

---

## 9. Technical Implementation Notes

### 9.1 Technology Stack
- **Backend**: ASP.NET Core 10.0 (Minimal API)
- **Frontend**: Blazor Server (.NET 10.0)
- **Orchestration**: .NET Aspire
- **Testing**: xUnit (unit), Playwright (E2E)

### 9.2 Data Types
- All currency calculations use `decimal` type (not `double` or `float`)
- Prevents floating-point precision errors
- Maintains accuracy to the cent

### 9.3 Configuration
- Tax rates stored in configuration (not hardcoded)
- Enables updates without code changes
- Supports different tax years if needed

---

## 10. Requirement Traceability

### 10.1 Core Requirements

| Requirement ID | Description | Section Reference | Status |
|----------------|-------------|-------------------|--------|
| REQ-001 | Annual salary input | §1.1, §1.2 | ✅ |
| REQ-002 | Tax bracket 1 (0-14K @ 10.5%) | §2.1 | ✅ |
| REQ-003 | Tax bracket 2 (14-48K @ 17.5%) | §2.1 | ✅ |
| REQ-004 | Tax bracket 3 (48-70K @ 30%) | §2.1 | ✅ |
| REQ-005 | Tax bracket 4 (70-180K @ 33%) | §2.1 | ✅ |
| REQ-006 | Tax bracket 5 (>180K @ 39%) | §2.1 | ✅ |
| REQ-007 | KiwiSaver 3% | §2.2 | ✅ |
| REQ-008 | ACC 1.53% | §2.3 | ✅ |
| REQ-009 | ACC cap $139,384 | §2.3 | ✅ |
| REQ-010 | Progressive tax calculation | §2.1, §3.1 | ✅ |
| REQ-011 | Take-home calculation | §3.1 | ✅ |
| REQ-012 | Monthly conversion | §3.1, §4.1 | ✅ |
| REQ-013 | Display monthly gross | §4.1 | ✅ |
| REQ-014 | Display monthly PAYE | §4.1 | ✅ |
| REQ-015 | Display monthly KiwiSaver | §4.1 | ✅ |
| REQ-016 | Display monthly ACC | §4.1 | ✅ |
| REQ-017 | Display monthly take-home | §4.1 | ✅ |
| REQ-018 | Example validation | §7.1 | ✅ Corrected |

### 10.2 Extended Requirements (Implementation Enhancements)

| Feature | Description | Section Reference | Status |
|---------|-------------|-------------------|--------|
| Annual breakdown | Display annual values | §4.2 | ✅ |
| Input validation | Client and server validation | §1.2 | ✅ |
| Currency formatting | Standardized display format | §3.3 | ✅ |
| Rounding method | Banker's rounding specification | §3.2 | ✅ |
| Error handling | Error states and messages | §5.3 | ✅ |
| UI/UX specification | Complete interface design | §5 | ✅ |
| API contract | Backend API definition | §6 | ✅ |
| Accessibility | WCAG AA compliance | §5.4 | ✅ |
| Responsive design | Mobile-friendly layout | §5.4 | ✅ |
| Loading states | User feedback during calculation | §5.2 | ✅ |

---

## Document Control

**Version History**:

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | [Original] | [Original Author] | Initial specification |
| 2.0 | 2026-02-13 | Product Owner (AI) | Comprehensive update to reflect implementation:<br>• Corrected example calculation ($60K PAYE)<br>• Added annual breakdown specification<br>• Added input validation requirements<br>• Added currency formatting and rounding rules<br>• Added UI/UX requirements<br>• Added API contract specification<br>• Added accessibility requirements (WCAG AA)<br>• Added error handling specification<br>• Added examples and edge cases<br>• Added requirement traceability |

**Review Status**: ✅ Ready for stakeholder review

**Approvals**: [Pending]

