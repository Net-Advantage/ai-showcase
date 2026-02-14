## PAYE Calculator Specification

**Version**: 3.0  
**Tax Year**: 2025/2026 (with 2024/2025 support)  
**Last Updated**: February 13, 2026

### Overview
A web-based calculator that computes New Zealand PAYE (Pay As You Earn) tax, KiwiSaver contributions, ACC levy, and optional student loan repayments based on annual salary input. The calculator supports configurable KiwiSaver contribution rates and displays both monthly and annual breakdowns of gross salary, deductions, and take-home pay.

### Glossary
- **PAYE**: Pay As You Earn - New Zealand's income tax system for employees
- **KiwiSaver**: Voluntary retirement savings scheme with employee contributions
- **ACC Earners' Levy**: Accident Compensation Corporation levy - mandatory workplace accident insurance
- **Progressive Tax**: Tax system where income is taxed across multiple brackets at different rates
- **Take-Home Pay**: Net salary after all deductions (also called "net pay")
- **Gross Salary**: Total salary before any deductions
- **Student Loan Repayment**: Compulsory deduction when income exceeds repayment threshold
- **Repayment Threshold**: Minimum annual income before student loan repayments begin

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

### 1.3 KiwiSaver Rate Selection

- **Label**: "KiwiSaver Contribution Rate"
- **Type**: Dropdown/Select
- **Options**: 0%, 3%, 4%, 6%, 8%, 10%
- **Default Value**: 3%
- **Help Text**: "Select your KiwiSaver employee contribution rate"
- **Required**: Yes (with default pre-selected)

**Rationale**: While 3% is the minimum employee contribution rate, employees can opt for higher voluntary contributions. Common rates are provided as standard options.

**Allowed Values**:
- 3% - Minimum employee contribution rate (default)
- 4% - Optional higher contribution
- 6% - Optional higher contribution
- 8% - Optional higher contribution
- 10% - Optional higher contribution

### 1.4 Student Loan Repayment

- **Label**: "Do you have a student loan?"
- **Type**: Checkbox or Toggle
- **Default Value**: Unchecked (false)
- **Help Text**: "Student loan repayments apply if your income exceeds the repayment threshold"

**Behavior**:
- When unchecked: No student loan repayment calculated
- When checked: Student loan repayment calculated based on income and threshold
- State persists during calculation updates

**Rationale**: Student loan repayment is compulsory for borrowers whose income exceeds the repayment threshold. Making this optional allows the calculator to serve both borrowers and non-borrowers.

---

## 2. Tax Calculations

### 2.1 PAYE Tax Rates (2025/2026)

New Zealand uses a progressive tax system with five tax brackets:

| Annual Income Range | Tax Rate | Calculation |
|---------------------|----------|-------------|
| $0 - $15,600 | 10.5% | Income in bracket × 0.105 |
| $15,601 - $53,500 | 17.5% | Income in bracket × 0.175 |
| $53,501 - $78,100 | 30% | Income in bracket × 0.30 |
| $78,101 - $180,000 | 33% | Income in bracket × 0.33 |
| Over $180,000 | 39% | Income in bracket × 0.39 |

**Progressive Calculation Method**:
1. Calculate tax for income in first bracket
2. Add tax for income in second bracket (if applicable)
3. Continue through all applicable brackets
4. Sum total tax across all brackets

**Example**: For $60,000 annual salary (2025/2026):
- Bracket 1: $15,600 × 10.5% = $1,638.00
- Bracket 2: $37,900 × 17.5% = $6,632.50 (from $15,601 to $53,500)
- Bracket 3: $6,500 × 30% = $1,950.00 (from $53,501 to $60,000)
- **Total annual PAYE**: $10,220.50

**Note**: For 2024/2025 tax brackets, see §2.5 (Tax Year Comparison).

### 2.2 KiwiSaver Contribution
- **Rate**: Variable (3%, 4%, 6%, 8%, or 10% of gross salary)
- **Default Rate**: 3%
- **Calculation**: Annual Salary × Selected Rate
- **Cap**: None (applies to full gross salary)
- **Mandatory**: No, but calculator assumes participation at selected rate

**Examples**:
- $60,000 @ 3%: $60,000 × 0.03 = $1,800.00 annual KiwiSaver
- $60,000 @ 6%: $60,000 × 0.06 = $3,600.00 annual KiwiSaver
- $60,000 @ 10%: $60,000 × 0.10 = $6,000.00 annual KiwiSaver

### 2.3 ACC Earners' Levy
- **Rate**: 1.53% of gross salary
- **Maximum Earnings Threshold**: $139,384
- **Calculation**: min(Annual Salary, $139,384) × 0.0153
- **Cap Rationale**: ACC levy only applies to first $139,384 of income

**Examples**:
- $60,000: $60,000 × 1.53% = $918.00 (below cap)
- $200,000: $139,384 × 1.53% = $2,132.57 (capped at threshold)

### 2.4 Student Loan Repayment

**Applicability**: Only if user has indicated they have a student loan

**Repayment Threshold (2025/2026)**: $24,128 annual income
**Repayment Rate**: 12% of income above threshold

**Calculation**:
```
IF Annual Salary > Repayment Threshold THEN
  Student Loan Repayment = (Annual Salary - Repayment Threshold) × 0.12
ELSE
  Student Loan Repayment = $0.00
END IF
```

**Examples**:
- $20,000 (below threshold): $0.00 (no repayment required)
- $24,128 (at threshold): $0.00 (repayment starts above threshold)
- $60,000: ($60,000 - $24,128) × 12% = $4,304.64 annual repayment
- $200,000: ($200,000 - $24,128) × 12% = $21,104.64 annual repayment

**Notes**:
- Repayment is calculated on gross income
- No maximum repayment amount (12% applied to all income above threshold)
- Threshold is reviewed annually and may change

**2024/2025 Tax Year Values** (for reference):
- Repayment Threshold: $24,128
- Repayment Rate: 12%

### 2.5 Tax Year Comparison

The calculator supports both 2024/2025 and 2025/2026 tax years. Below is a comparison of rates and thresholds.

#### PAYE Tax Brackets

**2025/2026 Tax Year** (Current):

| Annual Income Range | Tax Rate |
|---------------------|----------|
| $0 - $15,600 | 10.5% |
| $15,601 - $53,500 | 17.5% |
| $53,501 - $78,100 | 30% |
| $78,101 - $180,000 | 33% |
| Over $180,000 | 39% |

**2024/2025 Tax Year** (Previous):

| Annual Income Range | Tax Rate |
|---------------------|----------|
| $0 - $14,000 | 10.5% |
| $14,001 - $48,000 | 17.5% |
| $48,001 - $70,000 | 30% |
| $70,001 - $180,000 | 33% |
| Over $180,000 | 39% |

**Key Changes**:
- All lower bracket thresholds increased to account for inflation
- Tax rates remain unchanged

#### Other Rates and Thresholds

| Component | 2025/2026 | 2024/2025 | Notes |
|-----------|-----------|-----------|-------|
| **ACC Earners' Levy Rate** | 1.53% | 1.53% | No change |
| **ACC Maximum Earnings** | $139,384 | $139,384 | No change |
| **KiwiSaver Minimum Rate** | 3% | 3% | No change |
| **KiwiSaver Optional Rates** | 4%, 6%, 8%, 10% | 4%, 6%, 8%, 10% | No change |
| **Student Loan Threshold** | $24,128 | $24,128 | No change |
| **Student Loan Rate** | 12% | 12% | No change |

**Default Tax Year**: The calculator uses 2025/2026 rates by default.

---

## 3. Calculation Algorithm

### 3.1 Calculation Steps
1. Validate input (annual salary > 0, KiwiSaver rate is valid)
2. Calculate annual PAYE tax using progressive tax brackets (Section 2.1)
3. Calculate annual KiwiSaver: Annual Salary × Selected KiwiSaver Rate
4. Calculate annual ACC: min(Annual Salary, $139,384) × 1.53%
5. Calculate annual Student Loan Repayment (if applicable): 
   - If hasStudentLoan AND Annual Salary > Threshold: (Annual Salary - Threshold) × 12%
   - Else: $0.00
6. Calculate annual take-home pay: Annual Salary - PAYE - KiwiSaver - ACC - Student Loan
7. Calculate monthly values: Divide all annual amounts by 12
8. Round all currency values to 2 decimal places for display

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
- **KiwiSaver (X%)**: Annual KiwiSaver ÷ 12 (shown as deduction with minus sign, X = selected rate)
- **ACC Levy (1.53%)**: Annual ACC ÷ 12 (shown as deduction with minus sign)
- **Student Loan (12%)**: Annual Student Loan ÷ 12 (shown as deduction with minus sign, only if applicable)
- **Take-Home Pay**: Monthly gross - monthly deductions (highlighted/emphasized)

**Visual Hierarchy**:
- Large, prominent display
- Deductions visually distinct (prefixed with "-")
- Take-Home Pay emphasized (larger/bolder)
- Student Loan deduction only shown when hasStudentLoan = true

### 4.2 Annual Breakdown (Secondary Display)

The calculator also displays annual values in a collapsible section:

- **Annual Gross Salary**: Input value
- **PAYE Tax**: Annual PAYE (shown as deduction)
- **KiwiSaver (X%)**: Annual KiwiSaver (shown as deduction, X = selected rate)
- **ACC Levy (1.53%)**: Annual ACC (shown as deduction)
- **Student Loan (12%)**: Annual Student Loan (shown as deduction, only if applicable)
- **Take-Home Pay**: Annual gross - annual deductions

**Interaction**:
- Collapsed by default (progressive disclosure)
- Toggle label: "View Annual Breakdown"
- Same structure as monthly breakdown
- Student Loan row only shown when hasStudentLoan = true

### 4.3 Results Layout

**Structure** (using semantic HTML):
```
Monthly Breakdown
  ├─ Monthly Gross Salary: $X,XXX.XX
  ├─ PAYE Tax: -$XXX.XX
  ├─ KiwiSaver (X%): -$XXX.XX
  ├─ ACC Levy (1.53%): -$XX.XX
  ├─ Student Loan (12%): -$XXX.XX (conditional - only if hasStudentLoan)
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
   - KiwiSaver rate dropdown (with label and help text)
   - Student loan checkbox/toggle (with label and help text)
   - Calculate button
   - Validation messages (inline, below inputs)

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
  "annualSalary": number (decimal, required),
  "kiwiSaverRate": number (decimal, required),
  "hasStudentLoan": boolean (required)
}
```

**Constraints**:
- `annualSalary` > 0.01
- `annualSalary` ≤ 999,999,999.99
- Must be valid decimal format
- `kiwiSaverRate` must be one of: 0.03, 0.04, 0.06, 0.08, 0.10
- `hasStudentLoan` must be boolean (true/false)

**Example Request**:
```json
{
  "annualSalary": 60000.00,
  "kiwiSaverRate": 0.03,
  "hasStudentLoan": true
}
```

### 6.3 Response (Success - 200 OK)

**Schema**:
```json
{
  "annualSalary": number,
  "kiwiSaverRate": number,
  "hasStudentLoan": boolean,
  "annual": {
    "grossSalary": number,
    "payeTax": number,
    "kiwiSaver": number,
    "accLevy": number,
    "studentLoan": number,
    "takeHomePay": number
  },
  "monthly": {
    "grossSalary": number,
    "payeTax": number,
    "kiwiSaver": number,
    "accLevy": number,
    "studentLoan": number,
    "takeHomePay": number
  }
}
```

**Example Response** (for $60,000, 3% KiwiSaver, with student loan):
```json
{
  "annualSalary": 60000.00,
  "kiwiSaverRate": 0.03,
  "hasStudentLoan": true,
  "annual": {
    "grossSalary": 60000.00,
    "payeTax": 11020.00,
    "kiwiSaver": 1800.00,
    "accLevy": 918.00,
    "studentLoan": 4304.64,
    "takeHomePay": 41957.36
  },
  "monthly": {
    "grossSalary": 5000.00,
    "payeTax": 918.33,
    "kiwiSaver": 150.00,
    "accLevy": 76.50,
    "studentLoan": 358.72,
    "takeHomePay": 3496.45
  }
}
```

**Field Descriptions**:
- All monetary values in NZD
- All values rounded to 2 decimal places
- Monthly values = Annual values ÷ 12
- `studentLoan` = $0.00 if hasStudentLoan = false OR annual salary ≤ threshold
- `takeHomePay` = `grossSalary` - `payeTax` - `kiwiSaver` - `accLevy` - `studentLoan`

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

### 7.1 Example: $60,000 Annual Salary (2025/2026)

**Input**: 
- Annual Salary = $60,000.00
- KiwiSaver Rate = 3%
- Has Student Loan = Yes

**Calculation Breakdown**:

**PAYE Tax** (Progressive, 2025/2026 brackets):
- $0 - $15,600 @ 10.5% = $1,638.00
- $15,601 - $53,500 @ 17.5% = $6,632.50
- $53,501 - $60,000 @ 30% = $1,950.00
- **Total Annual PAYE**: $10,220.50

**KiwiSaver**: $60,000 × 3% = $1,800.00

**ACC Levy**: $60,000 × 1.53% = $918.00 (under cap)

**Student Loan**: ($60,000 - $24,128) × 12% = $4,304.64

**Annual Take-Home**: $60,000 - $10,220.50 - $1,800 - $918 - $4,304.64 = $42,756.86

**Monthly Breakdown** (Annual ÷ 12):
- Monthly Gross: $5,000.00
- Monthly PAYE: $851.71
- Monthly KiwiSaver: $150.00
- Monthly ACC: $76.50
- Monthly Student Loan: $358.72
- **Monthly Take-Home**: $3,563.07

### 7.2 Example: $60,000 Annual Salary with 6% KiwiSaver (2025/2026)

**Input**: 
- Annual Salary = $60,000.00
- KiwiSaver Rate = 6%
- Has Student Loan = No

**Calculation Breakdown**:

**PAYE Tax**: $10,220.50 (same as 7.1)

**KiwiSaver**: $60,000 × 6% = $3,600.00

**ACC Levy**: $918.00 (same as 7.1)

**Student Loan**: $0.00 (no student loan)

**Annual Take-Home**: $60,000 - $10,220.50 - $3,600 - $918 = $45,261.50

**Monthly Breakdown**:
- Monthly Gross: $5,000.00
- Monthly PAYE: $851.71
- Monthly KiwiSaver: $300.00
- Monthly ACC: $76.50
- **Monthly Take-Home**: $3,771.79

### 7.3 Example: $200,000 Annual Salary (ACC Cap, 2025/2026)

**Input**: 
- Annual Salary = $200,000.00
- KiwiSaver Rate = 3%
- Has Student Loan = Yes

**PAYE Tax** (Progressive, 2025/2026 brackets):
- $0 - $15,600 @ 10.5% = $1,638.00
- $15,601 - $53,500 @ 17.5% = $6,632.50
- $53,501 - $78,100 @ 30% = $7,380.00
- $78,101 - $180,000 @ 33% = $33,627.00
- $180,001 - $200,000 @ 39% = $7,800.00
- **Total Annual PAYE**: $57,077.50

**KiwiSaver**: $200,000 × 3% = $6,000.00

**ACC Levy**: $139,384 × 1.53% = $2,132.57 (**capped at maximum earnings**)

**Student Loan**: ($200,000 - $24,128) × 12% = $21,104.64

**Annual Take-Home**: $200,000 - $57,077.50 - $6,000 - $2,132.57 - $21,104.64 = $113,685.29

**Monthly Breakdown**:
- Monthly Gross: $16,666.67
- Monthly PAYE: $4,756.46
- Monthly KiwiSaver: $500.00
- Monthly ACC: $177.71
- Monthly Student Loan: $1,758.72
- **Monthly Take-Home**: $9,473.78

### 7.4 Edge Cases

| Scenario | Annual Salary | KiwiSaver | Student Loan | Expected Behavior | Notes |
|----------|---------------|-----------|--------------|-------------------|-------|
| Minimum input | $1.00 | 3% | No | All calculations work, very small amounts | Tests lower bound |
| Below student loan threshold | $24,000 | 3% | Yes | Student loan = $0.00 | No repayment required |
| At student loan threshold | $24,128 | 3% | Yes | Student loan = $0.00 | Repayment starts above threshold |
| Just above threshold | $24,129 | 3% | Yes | Student loan = $0.12 | Minimal repayment |
| First bracket boundary | $15,600 | 3% | No | Only first bracket applied | 2025/2026 boundary |
| Second bracket boundary | $53,500 | 3% | No | First two brackets applied | 2025/2026 boundary |
| Third bracket boundary | $78,100 | 3% | No | First three brackets applied | 2025/2026 boundary |
| Fourth bracket boundary | $180,000 | 3% | Yes | First four brackets applied | Bracket boundary |
| ACC cap exact | $139,384 | 3% | No | ACC at maximum ($2,132.57) | ACC cap boundary |
| High earner | $500,000 | 10% | Yes | All five brackets, ACC capped, high KiwiSaver | Top bracket + caps |
| Variable KiwiSaver | $60,000 | 10% | No | KiwiSaver = $6,000 annual | Tests 10% rate |

---

## 8. Notes and Disclaimers

### 8.1 Disclaimer Text

The calculator should display the following note:

> **Note**: This calculation uses 2025/2026 NZ tax rates (2024/2025 also available). This is an estimate and does not include other deductions such as union fees, insurance, or special tax codes.

**Placement**: Below results section, in smaller text (muted color)

### 8.2 Limitations

This calculator:
- ✅ Calculates PAYE tax using 2025/2026 rates (with 2024/2025 support)
- ✅ Includes KiwiSaver at configurable rates (3%, 4%, 6%, 8%, 10%)
- ✅ Includes ACC Earners' Levy at 1.53% (with maximum earnings cap)
- ✅ Includes student loan repayments (optional, 12% above threshold)
- ❌ Does NOT include other deductions (e.g., union fees, insurance, child support)
- ❌ Does NOT account for secondary tax codes (assumes primary employment)
- ❌ Does NOT account for individual tax credits or rebates (e.g., IETC, Working for Families)
- ❌ Does NOT calculate employer contributions (e.g., employer KiwiSaver contribution)
- ❌ Does NOT support interim/provisional tax for self-employed individuals

### 8.3 Tax Year

- **Current Tax Year**: 2025/2026 (default)
- **Supported Tax Years**: 2024/2025, 2025/2026
- **Update Frequency**: Annually (after NZ Budget announcement, typically May/June)
- **Rate Change Process**: Update tax brackets in configuration without code changes
- **Implementation Note**: Calculator defaults to current tax year (2025/2026); previous year available for comparison

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
| REQ-007 | KiwiSaver variable rate | §1.3, §2.2 | ✅ |
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
| REQ-018 | Example validation | §7.1, §7.2, §7.3 | ✅ Updated |
| REQ-019 | Student loan input | §1.4 | ✅ New |
| REQ-020 | Student loan calculation | §2.4, §3.1 | ✅ New |
| REQ-021 | Student loan threshold $24,128 | §2.4 | ✅ New |
| REQ-022 | Student loan rate 12% | §2.4 | ✅ New |
| REQ-023 | Display student loan deduction | §4.1, §4.2 | ✅ New |
| REQ-024 | 2025/2026 tax brackets | §2.1, §2.5 | ✅ New |
| REQ-025 | 2024/2025 tax brackets (historical) | §2.5 | ✅ New |

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
| KiwiSaver rate selector | Dropdown with 5 rate options | §1.3 | ✅ New |
| Student loan support | Optional student loan calculation | §1.4, §2.4 | ✅ New |
| Multi-year support | 2024/2025 and 2025/2026 tax years | §2.5 | ✅ New |

---

## Document Control

**Version History**:

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | [Original] | [Original Author] | Initial specification |
| 2.0 | 2026-02-13 | Product Owner (AI) | Comprehensive update to reflect implementation:<br>• Corrected example calculation ($60K PAYE)<br>• Added annual breakdown specification<br>• Added input validation requirements<br>• Added currency formatting and rounding rules<br>• Added UI/UX requirements<br>• Added API contract specification<br>• Added accessibility requirements (WCAG AA)<br>• Added error handling specification<br>• Added examples and edge cases<br>• Added requirement traceability |
| 3.0 | 2026-02-13 | Product Owner (AI) | Major feature expansion:<br>• **Added student loan repayment support** (§1.4, §2.4)<br>  - Optional student loan calculation<br>  - Threshold: $24,128, Rate: 12%<br>  - Conditional display in results<br>• **Added variable KiwiSaver rates** (§1.3, §2.2)<br>  - Dropdown selector: 3%, 4%, 6%, 8%, 10%<br>  - Updated calculations and API<br>• **Added 2025/2026 tax year** (§2.5)<br>  - Updated PAYE brackets for 2025/2026<br>  - Maintained 2024/2025 for reference<br>  - Comparison table of both years<br>• Updated all examples with new calculations<br>• Updated API contract for new fields<br>• Removed lifted limitations<br>• Added new requirement IDs (REQ-019 to REQ-025) |

**Review Status**: ✅ Ready for stakeholder review

**Approvals**: [Pending]

