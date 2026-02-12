# TEST MATRIX - PAYE Calculator

## 1. Unit Tests

### 1.1 Backend Calculation Tests

#### PAYE Tax Calculation Tests
**Requirement Coverage: REQ-001 to REQ-006 (Tax Bracket Calculations)**

| Test ID | Test Case | Input | Expected Output | Requirement |
|---------|-----------|-------|-----------------|-------------|
| UT-PAYE-001 | Tax on income in first bracket only | $10,000 | $1,050 (10.5%) | REQ-001 |
| UT-PAYE-002 | Tax at first bracket boundary | $14,000 | $1,470 (10.5%) | REQ-001 |
| UT-PAYE-003 | Tax crossing first-second bracket | $20,000 | $1,470 + $1,050 = $2,520 | REQ-001, REQ-002 |
| UT-PAYE-004 | Tax at second bracket boundary | $48,000 | $1,470 + $5,950 = $7,420 | REQ-002 |
| UT-PAYE-005 | Tax crossing second-third bracket | $60,000 | $1,470 + $5,950 + $3,600 = $11,020 | REQ-002, REQ-003 |
| UT-PAYE-006 | Tax at third bracket boundary | $70,000 | $1,470 + $5,950 + $6,600 = $14,020 | REQ-003 |
| UT-PAYE-007 | Tax crossing third-fourth bracket | $100,000 | $1,470 + $5,950 + $6,600 + $9,900 = $23,920 | REQ-003, REQ-004 |
| UT-PAYE-008 | Tax at fourth bracket boundary | $180,000 | $1,470 + $5,950 + $6,600 + $36,300 = $50,320 | REQ-004 |
| UT-PAYE-009 | Tax crossing fourth-fifth bracket | $200,000 | $1,470 + $5,950 + $6,600 + $36,300 + $7,800 = $58,120 | REQ-004, REQ-005 |
| UT-PAYE-010 | Tax well above fifth bracket | $250,000 | $1,470 + $5,950 + $6,600 + $36,300 + $27,300 = $77,620 | REQ-005 |
| UT-PAYE-011 | Zero income | $0 | $0 | REQ-006 |
| UT-PAYE-012 | Boundary +$1 above first bracket | $14,001 | $1,470 + $0.175 = $1,470.18 | REQ-002 |
| UT-PAYE-013 | Boundary -$1 below second bracket | $47,999 | $1,470 + $5,949.83 | REQ-002 |

#### KiwiSaver Calculation Tests
**Requirement Coverage: REQ-007 to REQ-009 (KiwiSaver Contributions)**

| Test ID | Test Case | Input | Expected Output | Requirement |
|---------|-----------|-------|-----------------|-------------|
| UT-KS-001 | KiwiSaver on $60,000 | $60,000 | $1,800 (3%) | REQ-007 |
| UT-KS-002 | KiwiSaver on minimum wage | $35,000 | $1,050 (3%) | REQ-007 |
| UT-KS-003 | KiwiSaver on high income | $200,000 | $6,000 (3%) | REQ-007 |
| UT-KS-004 | KiwiSaver on zero income | $0 | $0 | REQ-008 |
| UT-KS-005 | KiwiSaver rounding | $33,333 | $999.99 | REQ-009 |
| UT-KS-006 | KiwiSaver on fractional income | $50,500.50 | $1,515.02 | REQ-009 |

#### ACC Levy Calculation Tests
**Requirement Coverage: REQ-010 to REQ-013 (ACC Levy)**

| Test ID | Test Case | Input | Expected Output | Requirement |
|---------|-----------|-------|-----------------|-------------|
| UT-ACC-001 | ACC on $60,000 | $60,000 | $918 (1.53%) | REQ-010 |
| UT-ACC-002 | ACC at maximum threshold | $139,384 | $2,132.57 | REQ-011 |
| UT-ACC-003 | ACC above maximum threshold | $150,000 | $2,132.57 (capped) | REQ-011 |
| UT-ACC-004 | ACC well above threshold | $200,000 | $2,132.57 (capped) | REQ-011 |
| UT-ACC-005 | ACC on zero income | $0 | $0 | REQ-012 |
| UT-ACC-006 | ACC below threshold | $100,000 | $1,530 (1.53%) | REQ-010 |
| UT-ACC-007 | ACC boundary -$1 | $139,383 | $2,132.56 | REQ-011 |
| UT-ACC-008 | ACC boundary +$1 | $139,385 | $2,132.57 (capped) | REQ-011 |
| UT-ACC-009 | ACC rounding | $65,432.15 | $1,001.11 | REQ-013 |

#### Monthly Conversion Tests
**Requirement Coverage: REQ-014 to REQ-016 (Monthly Calculations)**

| Test ID | Test Case | Input (Annual) | Expected Monthly Output | Requirement |
|---------|-----------|----------------|------------------------|-------------|
| UT-MONTH-001 | Monthly gross from $60,000 | $60,000 | $5,000.00 | REQ-014 |
| UT-MONTH-002 | Monthly PAYE from $60,000 | $11,020 annual tax | $918.33 | REQ-014 |
| UT-MONTH-003 | Monthly KiwiSaver from $60,000 | $1,800 annual | $150.00 | REQ-014 |
| UT-MONTH-004 | Monthly ACC from $60,000 | $918 annual | $76.50 | REQ-014 |
| UT-MONTH-005 | Complete monthly breakdown $60k | See REQ-016 | Gross: $5,000, PAYE: $918.33, KS: $150, ACC: $76.50, Take-home: $3,855.17 | REQ-016 |
| UT-MONTH-006 | Monthly rounding precision | $100,001 annual | All values to 2 decimal places | REQ-015 |
| UT-MONTH-007 | Indivisible by 12 | $100,001 annual | Gross: $8,333.42 | REQ-015 |

#### Take-Home Pay Calculation Tests
**Requirement Coverage: REQ-017 to REQ-018 (Take-Home Calculation)**

| Test ID | Test Case | Input | Expected Output | Requirement |
|---------|-----------|-------|-----------------|-------------|
| UT-THP-001 | Take-home for $60,000 | $60,000 | Annual: $46,262, Monthly: $3,855.17 | REQ-017 |
| UT-THP-002 | Take-home for minimum income | $20,000 | Gross - PAYE - KS - ACC | REQ-017 |
| UT-THP-003 | Take-home for high income | $200,000 | Gross - PAYE - KS - ACC | REQ-017 |
| UT-THP-004 | Take-home cannot be negative | Edge case | >= $0 | REQ-018 |
| UT-THP-005 | Take-home calculation order | $60,000 | Verify: Annual first, then ÷ 12 | REQ-017 |

### 1.2 Component Tests (Frontend)

#### Input Component Tests
**Requirement Coverage: REQ-019 to REQ-024 (Input Validation)**

| Test ID | Test Case | Input | Expected Behavior | Requirement |
|---------|-----------|-------|-------------------|-------------|
| UT-COMP-001 | Valid integer input | "60000" | Accepted, formatted | REQ-019 |
| UT-COMP-002 | Valid decimal input | "60000.50" | Accepted, formatted | REQ-019 |
| UT-COMP-003 | Negative number rejection | "-5000" | Rejected, error shown | REQ-020 |
| UT-COMP-004 | Non-numeric rejection | "abc" | Rejected, error shown | REQ-020 |
| UT-COMP-005 | Empty input | "" | Error or prompt | REQ-021 |
| UT-COMP-006 | Zero input | "0" | Accepted, all outputs $0 | REQ-021 |
| UT-COMP-007 | Maximum reasonable value | "10000000" | Accepted | REQ-022 |
| UT-COMP-008 | Formatting with commas | Input: 60000 | Display: "60,000" | REQ-023 |
| UT-COMP-009 | Currency symbol display | Any valid | Shows $ prefix | REQ-024 |
| UT-COMP-010 | Decimal precision handling | "60000.123" | Rounded to 2 decimals | REQ-019 |

#### Display Component Tests
**Requirement Coverage: REQ-025 to REQ-029 (Output Display)**

| Test ID | Test Case | Input | Expected Display | Requirement |
|---------|-----------|-------|------------------|-------------|
| UT-DISP-001 | All output fields present | $60,000 | Shows Gross, PAYE, KS, ACC, Take-home | REQ-025 |
| UT-DISP-002 | Currency formatting | $5,000.00 | Displays "$5,000.00" | REQ-026 |
| UT-DISP-003 | Two decimal places | Any amount | Always .XX format | REQ-027 |
| UT-DISP-004 | Labels clearly shown | All fields | "Monthly Gross Salary", etc. | REQ-028 |
| UT-DISP-005 | Take-home highlighted | Any valid | Visually distinct | REQ-029 |
| UT-DISP-006 | Thousands separator | $10,000 | "10,000" not "10000" | REQ-026 |
| UT-DISP-007 | Alignment of amounts | All fields | Right-aligned numbers | REQ-028 |

### 1.3 Utility Function Tests

#### Number Formatting Tests
**Requirement Coverage: REQ-023, REQ-026, REQ-027**

| Test ID | Test Case | Input | Expected Output | Requirement |
|---------|-----------|-------|-----------------|-------------|
| UT-UTIL-001 | Format currency | 5000 | "$5,000.00" | REQ-026 |
| UT-UTIL-002 | Format with decimals | 5000.5 | "$5,000.50" | REQ-027 |
| UT-UTIL-003 | Format large number | 1234567.89 | "$1,234,567.89" | REQ-026 |
| UT-UTIL-004 | Format zero | 0 | "$0.00" | REQ-027 |
| UT-UTIL-005 | Round to 2 decimals | 5000.126 | "$5,000.13" | REQ-027 |

#### Validation Functions Tests
**Requirement Coverage: REQ-020 to REQ-022**

| Test ID | Test Case | Input | Expected Result | Requirement |
|---------|-----------|-------|-----------------|-------------|
| UT-VAL-001 | Validate positive number | "5000" | true | REQ-019 |
| UT-VAL-002 | Reject negative | "-5000" | false | REQ-020 |
| UT-VAL-003 | Reject non-numeric | "abc" | false | REQ-020 |
| UT-VAL-004 | Accept zero | "0" | true | REQ-021 |
| UT-VAL-005 | Validate decimal | "5000.50" | true | REQ-019 |

---

## 2. Integration Tests

### 2.1 API Integration Tests
**Requirement Coverage: REQ-030 to REQ-032 (API Communication)**

| Test ID | Test Case | Method | Endpoint | Expected | Requirement |
|---------|-----------|--------|----------|----------|-------------|
| IT-API-001 | Calculate endpoint exists | POST | /api/calculate | 200 OK | REQ-030 |
| IT-API-002 | Request with valid salary | POST | /api/calculate | JSON response with breakdown | REQ-030 |
| IT-API-003 | Response structure | POST | /api/calculate | Contains: gross, paye, kiwisaver, acc, takeHome | REQ-031 |
| IT-API-004 | Invalid input handling | POST | /api/calculate | 400 Bad Request | REQ-032 |
| IT-API-005 | Missing input handling | POST | /api/calculate | 400 Bad Request | REQ-032 |
| IT-API-006 | Response Content-Type | POST | /api/calculate | application/json | REQ-031 |
| IT-API-007 | CORS headers | OPTIONS | /api/calculate | Appropriate CORS headers | REQ-033 |
| IT-API-008 | Large number handling | POST | /api/calculate (10M) | Valid response | REQ-032 |

### 2.2 Frontend-Backend Integration Tests
**Requirement Coverage: REQ-034 to REQ-036 (Data Flow)**

| Test ID | Test Case | Scenario | Expected Behavior | Requirement |
|---------|-----------|----------|-------------------|-------------|
| IT-FE-001 | Submit salary, receive results | User enters $60,000, clicks calculate | Display shows correct breakdown | REQ-034 |
| IT-FE-002 | Error message propagation | API returns 400 | Error displayed to user | REQ-035 |
| IT-FE-003 | Loading state during API call | Submit calculation | Loading indicator shown | REQ-036 |
| IT-FE-004 | Results update on new input | Change salary, recalculate | Previous results replaced | REQ-034 |
| IT-FE-005 | Network error handling | API unreachable | User-friendly error shown | REQ-035 |
| IT-FE-006 | Response parsing | Valid API response | All fields populated correctly | REQ-034 |

### 2.3 Data Flow Tests
**Requirement Coverage: REQ-017, REQ-034**

| Test ID | Test Case | Flow | Verification | Requirement |
|---------|-----------|------|--------------|-------------|
| IT-FLOW-001 | Complete calculation flow | Input → API → Calculation → Response → Display | End-to-end data integrity | REQ-017, REQ-034 |
| IT-FLOW-002 | Multiple sequential calculations | Calculate 3 different salaries | Each correct, no cross-contamination | REQ-034 |
| IT-FLOW-003 | Calculation consistency | Same input multiple times | Same output every time | REQ-034 |

---

## 3. End-to-End Tests

### 3.1 Complete User Journeys

#### Journey 1: Basic Calculation
**Requirement Coverage: REQ-037 (Primary User Flow)**

| Test ID | Step | Action | Expected Result | Requirement |
|---------|------|--------|-----------------|-------------|
| E2E-J1-001 | 1 | Navigate to calculator | Page loads, input field visible | REQ-037 |
| E2E-J1-002 | 2 | Enter $60,000 | Input accepted and formatted | REQ-019, REQ-023 |
| E2E-J1-003 | 3 | Click Calculate / Submit | Results display | REQ-034 |
| E2E-J1-004 | 4 | Verify monthly gross | Shows $5,000.00 | REQ-014, REQ-016 |
| E2E-J1-005 | 5 | Verify monthly PAYE | Shows ~$918.33 | REQ-014, REQ-016 |
| E2E-J1-006 | 6 | Verify monthly KiwiSaver | Shows $150.00 | REQ-014, REQ-016 |
| E2E-J1-007 | 7 | Verify monthly ACC | Shows $76.50 | REQ-014, REQ-016 |
| E2E-J1-008 | 8 | Verify monthly take-home | Shows ~$3,855.17 | REQ-014, REQ-016, REQ-017 |

#### Journey 2: Edge Case Testing
**Requirement Coverage: REQ-038 (Edge Cases)**

| Test ID | Scenario | Input | Expected Outcome | Requirement |
|---------|----------|-------|------------------|-------------|
| E2E-J2-001 | Minimum wage worker | $30,000 | All values calculated correctly | REQ-038 |
| E2E-J2-002 | High earner | $200,000 | ACC capped, other values correct | REQ-011, REQ-038 |
| E2E-J2-003 | Boundary: ACC cap | $139,384 | ACC at maximum | REQ-011 |
| E2E-J2-004 | Just above ACC cap | $139,385 | ACC capped | REQ-011 |
| E2E-J2-005 | Tax bracket boundary | $48,000 | Tax calculated correctly | REQ-002 |
| E2E-J2-006 | Zero salary | $0 | All outputs $0 | REQ-021 |

#### Journey 3: Error Handling
**Requirement Coverage: REQ-039 (Error Scenarios)**

| Test ID | Error Scenario | User Action | Expected Behavior | Requirement |
|---------|---------------|-------------|-------------------|-------------|
| E2E-J3-001 | Negative input | Enter -5000 | Error message, no calculation | REQ-020, REQ-039 |
| E2E-J3-002 | Non-numeric input | Enter "ABC" | Error message displayed | REQ-020, REQ-039 |
| E2E-J3-003 | Empty submission | Submit with empty field | Prompt to enter value | REQ-021, REQ-039 |
| E2E-J3-004 | API failure | Backend unavailable | User-friendly error, retry option | REQ-035, REQ-039 |
| E2E-J3-005 | Malformed API response | Corrupted data | Graceful degradation | REQ-039 |

### 3.2 Multi-Scenario Testing

| Test ID | Test Scenarios | Requirement |
|---------|---------------|-------------|
| E2E-MS-001 | Test 10 different salary values in sequence | REQ-034 |
| E2E-MS-002 | Test all tax bracket boundaries | REQ-001 to REQ-005 |
| E2E-MS-003 | Test values around ACC cap (±$1000) | REQ-011 |

---

## 4. Accessibility Tests

### 4.1 WCAG AA Compliance Tests
**Requirement Coverage: REQ-040 (Accessibility)**

| Test ID | Test Case | WCAG Criterion | Test Method | Requirement |
|---------|-----------|----------------|-------------|-------------|
| A11Y-001 | Color contrast: text on background | 1.4.3 Contrast (Minimum) | Contrast ratio ≥ 4.5:1 | REQ-040 |
| A11Y-002 | Color contrast: interactive elements | 1.4.3 Contrast (Minimum) | Contrast ratio ≥ 4.5:1 | REQ-040 |
| A11Y-003 | Heading structure | 1.3.1 Info and Relationships | Proper semantic heading hierarchy | REQ-040 |
| A11Y-004 | Form labels | 3.3.2 Labels or Instructions | All inputs have associated labels | REQ-040 |
| A11Y-005 | Error identification | 3.3.1 Error Identification | Errors clearly described | REQ-040 |
| A11Y-006 | Focus visible | 2.4.7 Focus Visible | Focus indicators visible | REQ-040 |
| A11Y-007 | Resize text to 200% | 1.4.4 Resize Text | Content readable at 200% zoom | REQ-040 |
| A11Y-008 | Link purpose | 2.4.4 Link Purpose | Links have descriptive text | REQ-040 |
| A11Y-009 | Language of page | 3.1.1 Language of Page | HTML lang attribute set | REQ-040 |
| A11Y-010 | Page titled | 2.4.2 Page Titled | Descriptive page title | REQ-040 |

### 4.2 Keyboard Navigation Tests
**Requirement Coverage: REQ-040 (Keyboard Access)**

| Test ID | Test Case | Action | Expected Result | Requirement |
|---------|-----------|--------|-----------------|-------------|
| A11Y-KB-001 | Tab to input field | Press Tab | Input field receives focus | REQ-040 |
| A11Y-KB-002 | Tab to submit button | Press Tab from input | Button receives focus | REQ-040 |
| A11Y-KB-003 | Activate button via Enter | Focus on button, press Enter | Form submits | REQ-040 |
| A11Y-KB-004 | Activate button via Space | Focus on button, press Space | Form submits | REQ-040 |
| A11Y-KB-005 | Tab order logical | Tab through all elements | Order matches visual layout | REQ-040 |
| A11Y-KB-006 | No keyboard trap | Tab through entire page | Can navigate forward and back | REQ-040 |
| A11Y-KB-007 | Skip to main content | Use skip link | Focus moves to main content | REQ-040 |

### 4.3 Screen Reader Compatibility Tests
**Requirement Coverage: REQ-040 (Screen Reader Support)**

| Test ID | Screen Reader | Test Case | Expected Announcement | Requirement |
|---------|---------------|-----------|----------------------|-------------|
| A11Y-SR-001 | NVDA | Land on input field | "Annual salary, edit, text" | REQ-040 |
| A11Y-SR-002 | NVDA | Input validation error | Error message read aloud | REQ-040 |
| A11Y-SR-003 | NVDA | Results displayed | Each result value and label announced | REQ-040 |
| A11Y-SR-004 | JAWS | Navigate results | Logical reading order | REQ-040 |
| A11Y-SR-005 | JAWS | Button activation | Button purpose announced | REQ-040 |
| A11Y-SR-006 | VoiceOver (macOS) | Full page navigation | All content accessible | REQ-040 |
| A11Y-SR-007 | VoiceOver (iOS) | Mobile interaction | Touch gestures work | REQ-040 |

### 4.4 ARIA Implementation Tests
**Requirement Coverage: REQ-040 (ARIA)**

| Test ID | Test Case | ARIA Attribute | Verification | Requirement |
|---------|-----------|----------------|--------------|-------------|
| A11Y-ARIA-001 | Input field labeling | aria-label or aria-labelledby | Present and descriptive | REQ-040 |
| A11Y-ARIA-002 | Error messages | aria-describedby | Links input to error | REQ-040 |
| A11Y-ARIA-003 | Live region for results | aria-live="polite" | Results announced when updated | REQ-040 |
| A11Y-ARIA-004 | Required fields | aria-required="true" | Marked appropriately | REQ-040 |
| A11Y-ARIA-005 | Invalid state | aria-invalid="true" | Set when validation fails | REQ-040 |

---

## 5. Responsive/Cross-Browser Tests

### 5.1 Breakpoint Testing
**Requirement Coverage: REQ-041 (Responsive Design)**

| Test ID | Breakpoint | Viewport Size | Test Focus | Expected Behavior | Requirement |
|---------|-----------|---------------|------------|-------------------|-------------|
| RESP-001 | Mobile Small | 320px × 568px | Layout stacks vertically | All content visible, scrollable | REQ-041 |
| RESP-002 | Mobile Medium | 375px × 667px | Touch targets | Min 44px × 44px touch areas | REQ-041 |
| RESP-003 | Mobile Large | 414px × 896px | Input field width | Full width or comfortable size | REQ-041 |
| RESP-004 | Tablet Portrait | 768px × 1024px | Layout adaptation | Optimal use of space | REQ-041 |
| RESP-005 | Tablet Landscape | 1024px × 768px | Horizontal layout | Effective use of width | REQ-041 |
| RESP-006 | Desktop Small | 1280px × 720px | Standard desktop view | Readable, well-spaced | REQ-041 |
| RESP-007 | Desktop Large | 1920px × 1080px | Wide screen | Content doesn't stretch excessively | REQ-041 |
| RESP-008 | Ultra-wide | 2560px × 1440px | Maximum width constraint | Content has max-width | REQ-041 |

### 5.2 Browser Compatibility Tests
**Requirement Coverage: REQ-041 (Cross-Browser)**

| Test ID | Browser | Version | OS | Test Scope | Requirement |
|---------|---------|---------|-----|------------|-------------|
| BROW-001 | Chrome | Latest | Windows 11 | Full functionality | REQ-041 |
| BROW-002 | Chrome | Latest | macOS | Full functionality | REQ-041 |
| BROW-003 | Firefox | Latest | Windows 11 | Full functionality | REQ-041 |
| BROW-004 | Firefox | Latest | macOS | Full functionality | REQ-041 |
| BROW-005 | Safari | Latest | macOS | Full functionality | REQ-041 |
| BROW-006 | Safari | Latest | iOS | Touch interactions | REQ-041 |
| BROW-007 | Edge | Latest | Windows 11 | Full functionality | REQ-041 |
| BROW-008 | Chrome | Latest | Android | Mobile experience | REQ-041 |
| BROW-009 | Samsung Internet | Latest | Android | Mobile experience | REQ-041 |
| BROW-010 | Chrome | Latest -2 versions | Windows | Backward compatibility | REQ-041 |

### 5.3 Device Testing
**Requirement Coverage: REQ-041 (Device Compatibility)**

| Test ID | Device Type | Specific Device | Test Focus | Requirement |
|---------|-------------|-----------------|------------|-------------|
| DEV-001 | iPhone | iPhone 13/14/15 | Touch, layout, Safari | REQ-041 |
| DEV-002 | iPhone | iPhone SE | Small screen handling | REQ-041 |
| DEV-003 | Android Phone | Samsung Galaxy S21+ | Chrome on Android | REQ-041 |
| DEV-004 | Android Phone | Google Pixel 6 | Stock Android | REQ-041 |
| DEV-005 | iPad | iPad Air | Tablet experience | REQ-041 |
| DEV-006 | Android Tablet | Samsung Galaxy Tab | Large touch interface | REQ-041 |
| DEV-007 | Desktop | Windows PC | Standard desktop | REQ-041 |
| DEV-008 | Desktop | Mac | macOS environment | REQ-041 |

---

## 6. Test Data and Scenarios

### 6.1 Standard Test Data Sets

#### Dataset 1: Tax Bracket Coverage
```json
{
  "purpose": "Cover all tax brackets",
  "testCases": [
    {"salary": 10000, "bracket": "First only"},
    {"salary": 14000, "bracket": "First boundary"},
    {"salary": 30000, "bracket": "First + Second"},
    {"salary": 48000, "bracket": "Second boundary"},
    {"salary": 60000, "bracket": "Second + Third (reference case)"},
    {"salary": 70000, "bracket": "Third boundary"},
    {"salary": 100000, "bracket": "Third + Fourth"},
    {"salary": 180000, "bracket": "Fourth boundary"},
    {"salary": 200000, "bracket": "Fourth + Fifth"},
    {"salary": 250000, "bracket": "Well into fifth"}
  ]
}
```

#### Dataset 2: ACC Cap Coverage
```json
{
  "purpose": "Test ACC levy capping",
  "testCases": [
    {"salary": 100000, "note": "Below ACC cap"},
    {"salary": 139383, "note": "Just below cap"},
    {"salary": 139384, "note": "At cap"},
    {"salary": 139385, "note": "Just above cap"},
    {"salary": 150000, "note": "Above cap"},
    {"salary": 200000, "note": "Well above cap"}
  ]
}
```

#### Dataset 3: Real-World Salaries
```json
{
  "purpose": "Common NZ salary ranges",
  "testCases": [
    {"salary": 35000, "description": "Minimum wage full-time (approx)"},
    {"salary": 45000, "description": "Entry-level professional"},
    {"salary": 60000, "description": "Mid-level professional"},
    {"salary": 75000, "description": "Senior professional"},
    {"salary": 90000, "description": "Management level"},
    {"salary": 120000, "description": "Senior management"},
    {"salary": 150000, "description": "Executive level"}
  ]
}
```

#### Dataset 4: Precision and Rounding
```json
{
  "purpose": "Test decimal handling",
  "testCases": [
    {"salary": 60000.00, "note": "Clean number"},
    {"salary": 60000.01, "note": "Minimal fraction"},
    {"salary": 60000.50, "note": "Half dollar"},
    {"salary": 60000.99, "note": "Near rounding"},
    {"salary": 33333.33, "note": "Repeating decimal"},
    {"salary": 99999.99, "note": "Complex rounding"}
  ]
}
```

### 6.2 Edge Cases

| Edge Case ID | Category | Value | Expected Behavior | Requirement |
|--------------|----------|-------|-------------------|-------------|
| EDGE-001 | Minimum | $0 | All outputs $0.00 | REQ-021 |
| EDGE-002 | Very small | $1 | Minimal calculations | REQ-019 |
| EDGE-003 | Boundary | $14,000 | At first bracket edge | REQ-001 |
| EDGE-004 | Boundary | $14,001 | Just into second bracket | REQ-002 |
| EDGE-005 | Boundary | $48,000 | At second bracket edge | REQ-002 |
| EDGE-006 | Boundary | $48,001 | Just into third bracket | REQ-003 |
| EDGE-007 | Boundary | $70,000 | At third bracket edge | REQ-003 |
| EDGE-008 | Boundary | $70,001 | Just into fourth bracket | REQ-004 |
| EDGE-009 | Boundary | $180,000 | At fourth bracket edge | REQ-004 |
| EDGE-010 | Boundary | $180,001 | Just into fifth bracket | REQ-005 |
| EDGE-011 | ACC Cap | $139,384 | Exactly at ACC cap | REQ-011 |
| EDGE-012 | ACC Cap | $139,385 | First value to trigger cap | REQ-011 |
| EDGE-013 | Large | $1,000,000 | Very high income | REQ-022 |
| EDGE-014 | Large | $10,000,000 | Extreme income | REQ-022 |
| EDGE-015 | Precision | $0.01 | Minimum non-zero | REQ-027 |

### 6.3 Boundary Conditions

| Boundary ID | Type | Lower | Exact | Upper | Purpose |
|-------------|------|-------|-------|-------|---------|
| BOUND-001 | Tax Bracket 1 | $13,999 | $14,000 | $14,001 | First bracket edge |
| BOUND-002 | Tax Bracket 2 | $47,999 | $48,000 | $48,001 | Second bracket edge |
| BOUND-003 | Tax Bracket 3 | $69,999 | $70,000 | $70,001 | Third bracket edge |
| BOUND-004 | Tax Bracket 4 | $179,999 | $180,000 | $180,001 | Fourth bracket edge |
| BOUND-005 | ACC Cap | $139,383 | $139,384 | $139,385 | ACC levy cap |
| BOUND-006 | Zero | null | $0 | $1 | Minimum values |
| BOUND-007 | Decimals | $100.00 | $100.50 | $100.99 | Rounding behavior |

### 6.4 Invalid Input Test Data

| Invalid ID | Input Value | Input Type | Expected Error | Requirement |
|------------|-------------|------------|----------------|-------------|
| INVALID-001 | "-5000" | Negative | "Please enter a positive value" | REQ-020 |
| INVALID-002 | "abc" | Non-numeric | "Please enter a valid number" | REQ-020 |
| INVALID-003 | "12.34.56" | Malformed | "Please enter a valid number" | REQ-020 |
| INVALID-004 | "" | Empty | "Please enter a salary" | REQ-021 |
| INVALID-005 | "   " | Whitespace only | "Please enter a salary" | REQ-021 |
| INVALID-006 | "$60000" | With symbol | Stripped or rejected | REQ-020 |
| INVALID-007 | "60,000" | With comma | Accept if stripped, or reject | REQ-020 |
| INVALID-008 | "1e10" | Scientific notation | Accept as 10 billion or reject | REQ-020 |
| INVALID-009 | "Infinity" | Special value | Rejected | REQ-020 |
| INVALID-010 | "NaN" | Special value | Rejected | REQ-020 |

---

## 7. Requirements Coverage Matrix

| Requirement ID | Description | Unit Tests | Integration Tests | E2E Tests | Accessibility Tests | Total Test Coverage |
|----------------|-------------|------------|-------------------|-----------|---------------------|---------------------|
| **CALCULATIONS** |
| REQ-001 | PAYE: First bracket (10.5%) | UT-PAYE-001, 002, 012 | - | E2E-MS-002 | - | 4 tests |
| REQ-002 | PAYE: Second bracket (17.5%) | UT-PAYE-003, 004, 013 | - | E2E-MS-002, E2E-J2-005 | - | 5 tests |
| REQ-003 | PAYE: Third bracket (30%) | UT-PAYE-005, 006 | - | E2E-MS-002 | - | 3 tests |
| REQ-004 | PAYE: Fourth bracket (33%) | UT-PAYE-007, 008 | - | E2E-MS-002 | - | 3 tests |
| REQ-005 | PAYE: Fifth bracket (39%) | UT-PAYE-009, 010 | - | E2E-MS-002 | - | 3 tests |
| REQ-006 | PAYE: Zero handling | UT-PAYE-011 | - | E2E-J2-006 | - | 2 tests |
| REQ-007 | KiwiSaver: 3% calculation | UT-KS-001, 002, 003 | - | E2E-J1-006 | - | 4 tests |
| REQ-008 | KiwiSaver: Zero handling | UT-KS-004 | - | - | - | 1 test |
| REQ-009 | KiwiSaver: Rounding | UT-KS-005, 006 | - | - | - | 2 tests |
| REQ-010 | ACC: 1.53% calculation | UT-ACC-001, 006 | - | - | - | 2 tests |
| REQ-011 | ACC: Maximum threshold cap | UT-ACC-002, 003, 004, 007, 008 | - | E2E-J2-002, 003, 004, E2E-MS-003 | - | 9 tests |
| REQ-012 | ACC: Zero handling | UT-ACC-005 | - | E2E-J2-006 | - | 2 tests |
| REQ-013 | ACC: Rounding | UT-ACC-009 | - | - | - | 1 test |
| REQ-014 | Monthly conversion (÷12) | UT-MONTH-001 to 004 | - | E2E-J1-004 to 008 | - | 9 tests |
| REQ-015 | Monthly rounding precision | UT-MONTH-006, 007 | - | - | - | 2 tests |
| REQ-016 | Example validation ($60k) | UT-MONTH-005 | - | E2E-J1-004 to 008 | - | 6 tests |
| REQ-017 | Take-home calculation | UT-THP-001 to 005 | IT-FLOW-001 | E2E-J1-008 | - | 7 tests |
| REQ-018 | Take-home non-negative | UT-THP-004 | - | - | - | 1 test |
| **INPUT VALIDATION** |
| REQ-019 | Accept valid numeric input | UT-COMP-001, 002, 010, UT-VAL-001, 005 | - | E2E-J1-002 | - | 6 tests |
| REQ-020 | Reject invalid inputs | UT-COMP-003, 004, UT-VAL-002, 003 | - | E2E-J3-001, 002 | - | 6 tests |
| REQ-021 | Handle empty/zero | UT-COMP-005, 006, UT-VAL-004 | - | E2E-J3-003, E2E-J2-006 | - | 5 tests |
| REQ-022 | Handle large values | UT-COMP-007 | IT-API-008 | E2E-J2-002 | - | 3 tests |
| REQ-023 | Input formatting | UT-COMP-008, UT-UTIL-001 to 005 | - | E2E-J1-002 | - | 7 tests |
| REQ-024 | Currency symbol | UT-COMP-009 | - | - | - | 1 test |
| **OUTPUT DISPLAY** |
| REQ-025 | All fields present | UT-DISP-001 | IT-API-003 | E2E-J1-004 to 008 | - | 7 tests |
| REQ-026 | Currency formatting | UT-DISP-002, 006, UT-UTIL-001, 003 | - | - | - | 4 tests |
| REQ-027 | Two decimal precision | UT-DISP-003, UT-UTIL-002, 005 | - | - | - | 3 tests |
| REQ-028 | Clear labeling | UT-DISP-004, 007 | - | - | A11Y-004 | 3 tests |
| REQ-029 | Take-home highlighted | UT-DISP-005 | - | - | - | 1 test |
| **API & INTEGRATION** |
| REQ-030 | API endpoint exists | - | IT-API-001, 002 | - | - | 2 tests |
| REQ-031 | API response structure | - | IT-API-003, 006 | - | - | 2 tests |
| REQ-032 | API error handling | - | IT-API-004, 005, 008 | - | - | 3 tests |
| REQ-033 | CORS configuration | - | IT-API-007 | - | - | 1 test |
| REQ-034 | Frontend-backend flow | - | IT-FE-001, 004, 006, IT-FLOW-001 to 003 | E2E-J1-003, E2E-MS-001 | - | 9 tests |
| REQ-035 | Error propagation | - | IT-FE-002, 005 | E2E-J3-004 | - | 3 tests |
| REQ-036 | Loading states | - | IT-FE-003 | - | - | 1 test |
| **USER EXPERIENCE** |
| REQ-037 | Primary user flow | - | - | E2E-J1-001 to 008 | - | 8 tests |
| REQ-038 | Edge case handling | - | - | E2E-J2-001 to 006 | - | 6 tests |
| REQ-039 | Error scenarios | - | - | E2E-J3-001 to 005 | A11Y-005 | 6 tests |
| **ACCESSIBILITY** |
| REQ-040 | WCAG AA compliance | - | - | - | A11Y-001 to A11Y-ARIA-005 | 27 tests |
| **RESPONSIVE DESIGN** |
| REQ-041 | Responsive & cross-browser | - | - | - | RESP-001 to DEV-008 | 24 tests |

### Coverage Summary

| Test Category | Total Test Cases | Requirements Covered |
|---------------|------------------|----------------------|
| Unit Tests | 75 | REQ-001 to REQ-029 |
| Integration Tests | 17 | REQ-030 to REQ-036 |
| End-to-End Tests | 24 | REQ-037 to REQ-039 |
| Accessibility Tests | 27 | REQ-040 |
| Responsive/Browser Tests | 24 | REQ-041 |
| **TOTAL** | **167 test cases** | **41 requirements** |

### Untestable Requirements

**NONE** - All 41 requirements are testable and have mapped test coverage.

### High-Risk Areas Requiring Extra Testing

1. **Progressive Tax Calculations** (REQ-001 to REQ-005)
   - Complex multi-bracket logic
   - Boundary conditions critical
   - **Test Priority: HIGH**

2. **ACC Levy Capping** (REQ-011)
   - Edge case at $139,384 threshold
   - **Test Priority: HIGH**

3. **Decimal Precision** (REQ-009, REQ-013, REQ-015, REQ-027)
   - Rounding errors can compound
   - **Test Priority: MEDIUM**

4. **Cross-Browser Compatibility** (REQ-041)
   - Different rendering engines
   - **Test Priority: MEDIUM**

---

## 8. Test Environment Needs

### 8.1 Testing Tools Required

#### Unit Testing
- **Backend (.NET)**
  - xUnit or NUnit test framework
  - Moq for mocking (if needed)
  - FluentAssertions for readable assertions
  
- **Frontend (Blazor/Razor)**
  - bUnit for Blazor component testing
  - xUnit for test runner
  - AngleSharp for DOM assertions

#### Integration Testing
- **API Testing**
  - Microsoft.AspNetCore.Mvc.Testing (WebApplicationFactory)
  - HTTP client testing utilities
  - Test server in-memory hosting

#### End-to-End Testing
- **E2E Framework**
  - Playwright (recommended) or Selenium
  - Page Object Model pattern
  - Cross-browser test execution

#### Accessibility Testing
- **Automated Tools**
  - axe-core (via Playwright or browser extension)
  - WAVE browser extension
  - Lighthouse CI

- **Manual Testing**
  - NVDA screen reader (Windows)
  - JAWS screen reader (Windows)
  - VoiceOver (macOS/iOS)
  - Chrome DevTools Accessibility

#### Responsive Testing
- **Browser DevTools** for viewport simulation
- **BrowserStack** or **Sauce Labs** for real device testing
- **Playwright** for automated responsive testing

### 8.2 Test Data Management

#### Static Test Data
- **Location**: `/tests/TestData/`
- **Files**:
  - `salary-test-cases.json` - Test salary values
  - `expected-results.json` - Pre-calculated expected values
  - `invalid-inputs.json` - Invalid input test cases

#### Dynamic Test Data
- Calculated on-the-fly in test setup
- Use factory patterns for test data generation

#### Test Database
- Not required (calculator is stateless)
- If persistence added: Use in-memory database or test containers

### 8.3 Test Environments

| Environment | Purpose | Configuration | Data |
|-------------|---------|---------------|------|
| **Local Dev** | Unit tests during development | Visual Studio / VS Code test runner | Static test data |
| **CI Pipeline** | Automated test execution | GitHub Actions / Azure DevOps | Static test data |
| **Integration** | Integration & E2E tests | Docker containers for services | Test data sets |
| **Accessibility** | Manual accessibility testing | Local with assistive tech | Sample salaries |
| **Browser Lab** | Cross-browser testing | BrowserStack or local VMs | Standard test cases |

### 8.4 Required Test Infrastructure

#### Continuous Integration
```yaml
# Example CI configuration needs
- .NET SDK 10.0
- Node.js (if frontend build required)
- Playwright browsers
- Chrome, Firefox, Edge browsers
```

#### Local Development
- Visual Studio 2022 or VS Code
- .NET 10.0 SDK
- Playwright CLI (for E2E tests)
- Screen reader software (for accessibility)

#### Cloud Testing Services (Optional)
- **BrowserStack**: Real device testing
- **Sauce Labs**: Cross-browser automation
- **LambdaTest**: Responsive testing

### 8.5 Test Execution Strategy

#### Pre-Commit
- Fast unit tests only
- Linting and code style checks

#### Pull Request
- All unit tests
- Integration tests
- E2E smoke tests
- Accessibility automated checks

#### Nightly Build
- Full test suite
- All E2E scenarios
- Cross-browser testing
- Performance tests

#### Release Candidate
- Full regression suite
- Manual accessibility testing
- Real device testing
- User acceptance testing

### 8.6 Test Reporting

#### Metrics to Track
- Test pass/fail rate
- Code coverage (target: >80%)
- Test execution time
- Accessibility violations count
- Browser compatibility matrix

#### Reporting Tools
- xUnit HTML reports
- Playwright HTML reporter
- axe-core accessibility reports
- Coverage reports (Coverlet)
- Allure Reports (optional, comprehensive)

### 8.7 Special Testing Considerations

#### Decimal Precision Testing
- Use decimal type in .NET (not float/double)
- Verify rounding at multiple stages
- Test accumulation of rounding errors

#### Tax Calculation Accuracy
- Independent verification required
- Consider using IRD official calculator for validation
- Document calculation methodology

#### Performance Testing (Future)
- Response time < 200ms for calculation
- Page load time < 2 seconds
- Accessibility audit under 1 second

---

## Appendix A: Requirement Definitions

Based on specification analysis, the following requirements are derived:

### Calculations (REQ-001 to REQ-018)
- **REQ-001**: Calculate PAYE for first tax bracket (0-$14,000 @ 10.5%)
- **REQ-002**: Calculate PAYE for second tax bracket ($14,001-$48,000 @ 17.5%)
- **REQ-003**: Calculate PAYE for third tax bracket ($48,001-$70,000 @ 30%)
- **REQ-004**: Calculate PAYE for fourth tax bracket ($70,001-$180,000 @ 33%)
- **REQ-005**: Calculate PAYE for fifth tax bracket ($180,001+ @ 39%)
- **REQ-006**: Handle zero income correctly
- **REQ-007**: Calculate KiwiSaver at 3% of gross salary
- **REQ-008**: Handle zero KiwiSaver on zero income
- **REQ-009**: Round KiwiSaver to 2 decimal places
- **REQ-010**: Calculate ACC levy at 1.53% of gross salary
- **REQ-011**: Cap ACC levy at maximum earnings threshold ($139,384)
- **REQ-012**: Handle zero ACC on zero income
- **REQ-013**: Round ACC levy to 2 decimal places
- **REQ-014**: Convert annual amounts to monthly (÷ 12)
- **REQ-015**: Round monthly amounts to 2 decimal places
- **REQ-016**: Validate example calculation ($60k annual)
- **REQ-017**: Calculate take-home pay (gross - PAYE - KS - ACC)
- **REQ-018**: Ensure take-home is never negative

### Input Validation (REQ-019 to REQ-024)
- **REQ-019**: Accept valid numeric input (integers and decimals)
- **REQ-020**: Reject negative numbers and non-numeric input
- **REQ-021**: Handle empty input and zero appropriately
- **REQ-022**: Support large salary values
- **REQ-023**: Format input with thousands separators
- **REQ-024**: Display currency symbol ($)

### Output Display (REQ-025 to REQ-029)
- **REQ-025**: Display all five output fields
- **REQ-026**: Format output as currency with commas
- **REQ-027**: Display amounts to 2 decimal places
- **REQ-028**: Show clear labels for each field
- **REQ-029**: Visually emphasize take-home pay

### API & Integration (REQ-030 to REQ-036)
- **REQ-030**: Provide calculation API endpoint
- **REQ-031**: Return structured JSON response
- **REQ-032**: Handle API errors gracefully
- **REQ-033**: Configure CORS appropriately
- **REQ-034**: Enable frontend-backend data flow
- **REQ-035**: Propagate errors to user interface
- **REQ-036**: Show loading state during calculations

### User Experience (REQ-037 to REQ-039)
- **REQ-037**: Support primary user journey (enter, calculate, view)
- **REQ-038**: Handle edge cases (very low/high salaries)
- **REQ-039**: Provide clear error messages

### Non-Functional (REQ-040 to REQ-041)
- **REQ-040**: Meet WCAG AA accessibility standards
- **REQ-041**: Support responsive design and cross-browser compatibility

---

**END OF TEST MATRIX**
