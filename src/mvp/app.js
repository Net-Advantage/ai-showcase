/* ================================================
   DOM References
   ================================================ */
// REQ-MVP-001 through REQ-MVP-004: Input elements
var form = document.getElementById('paye-form');
var salaryInput = document.getElementById('annual-salary');
var kiwisaverSelect = document.getElementById('kiwisaver-rate');
var studentLoanCheckbox = document.getElementById('student-loan');
var salaryError = document.getElementById('salary-error');
var resultsSection = document.getElementById('results');

/* ================================================
   Constants
   ================================================ */
// REQ-MVP-005: PAYE Tax brackets — 2025/2026 progressive rates
var TAX_BRACKETS = [
    { min: 0,      max: 15600,    rate: 0.105 },
    { min: 15600,  max: 53500,    rate: 0.175 },
    { min: 53500,  max: 78100,    rate: 0.30  },
    { min: 78100,  max: 180000,   rate: 0.33  },
    { min: 180000, max: Infinity,  rate: 0.39  }
];

// REQ-MVP-007: ACC Levy
var ACC_RATE = 0.0153;
var ACC_MAX_EARNINGS = 139384;

// REQ-MVP-008: Student Loan
var STUDENT_LOAN_RATE = 0.12;
var STUDENT_LOAN_THRESHOLD = 24128;

/* ================================================
   Formatting  — REQ-MVP-010
   $X,XXX.XX — dollar sign, comma separators, 2 decimals
   ================================================ */
function formatCurrency(amount) {
    var abs = Math.abs(amount);
    var str = abs.toFixed(2);
    var parts = str.split('.');
    var intPart = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    return '$' + intPart + '.' + parts[1];
}

function formatDeduction(amount) {
    return '-' + formatCurrency(amount);
}

/* ================================================
   Parsing — REQ-MVP-002
   Strip $, commas, spaces before evaluating
   ================================================ */
function parseSalary(value) {
    var cleaned = value.replace(/[$,\s]/g, '');
    if (cleaned === '') {
        return { value: NaN, empty: true };
    }
    var num = Number(cleaned);
    return { value: num, empty: false };
}

/* ================================================
   Validation — REQ-MVP-002, REQ-MVP-028
   ================================================ */
function validateSalary(input) {
    var parsed = parseSalary(input);

    if (parsed.empty) {
        return { valid: false, error: 'Annual salary is required' };
    }
    if (isNaN(parsed.value) || !isFinite(parsed.value)) {
        return { valid: false, error: 'Salary must be between $1 and $1,000,000' };
    }
    if (parsed.value < 1 || parsed.value > 1000000) {
        return { valid: false, error: 'Salary must be between $1 and $1,000,000' };
    }

    return { valid: true, value: parsed.value };
}

/* ================================================
   Calculation Functions
   ================================================ */
// REQ-MVP-005: Progressive PAYE tax
function calculatePAYE(salary) {
    var tax = 0;
    for (var i = 0; i < TAX_BRACKETS.length; i++) {
        var bracket = TAX_BRACKETS[i];
        if (salary <= bracket.min) break;
        var taxable = Math.min(salary, bracket.max) - bracket.min;
        tax += taxable * bracket.rate;
    }
    return tax;
}

// REQ-MVP-006: KiwiSaver contribution (no cap)
function calculateKiwiSaver(salary, rate) {
    return salary * rate;
}

// REQ-MVP-007: ACC Levy
// Uses integer-path multiplication (earnings * 153 / 100) to avoid
// floating-point precision issues with the 0.0153 rate multiplier.
function calculateACC(salary) {
    var earnings = Math.min(salary, ACC_MAX_EARNINGS);
    return Math.floor(earnings * 153 / 100) / 100;
}

// REQ-MVP-008: Student Loan repayment
function calculateStudentLoan(salary, hasLoan) {
    if (!hasLoan || salary <= STUDENT_LOAN_THRESHOLD) return 0;
    return (salary - STUDENT_LOAN_THRESHOLD) * STUDENT_LOAN_RATE;
}

/* ================================================
   Error Display
   ================================================ */
function showError(message) {
    salaryError.textContent = message;
    salaryError.hidden = false;
    salaryInput.classList.add('has-error');
    resultsSection.hidden = true;
}

function clearError() {
    salaryError.textContent = '';
    salaryError.hidden = true;
    salaryInput.classList.remove('has-error');
}

/* ================================================
   Main Calculation — REQ-MVP-009
   ================================================ */
function calculate() {
    var validation = validateSalary(salaryInput.value);

    if (!validation.valid) {
        showError(validation.error);
        return;
    }

    clearError();

    var salary = validation.value;
    var kiwiSaverRate = parseFloat(kiwisaverSelect.value);
    var hasStudentLoan = studentLoanCheckbox.checked;

    // REQ-MVP-011: Full precision during intermediate calcs
    var paye = calculatePAYE(salary);
    var kiwiSaver = calculateKiwiSaver(salary, kiwiSaverRate);
    var acc = calculateACC(salary);
    var studentLoan = calculateStudentLoan(salary, hasStudentLoan);
    var takeHome = salary - paye - kiwiSaver - acc - studentLoan;

    var kiwiSaverPercent = Math.round(kiwiSaverRate * 100);

    // ── Monthly Breakdown ──────────────────────────
    document.getElementById('monthly-gross').textContent = formatCurrency(salary / 12);
    document.getElementById('monthly-paye').textContent = formatDeduction(paye / 12);
    document.getElementById('monthly-kiwisaver').textContent = formatDeduction(kiwiSaver / 12);
    document.getElementById('monthly-kiwisaver-label').textContent = 'KiwiSaver (' + kiwiSaverPercent + '%)';
    document.getElementById('monthly-acc').textContent = formatDeduction(acc / 12);
    document.getElementById('monthly-take-home').textContent = formatCurrency(takeHome / 12);

    // REQ-MVP-013: Student loan row — only shown when checked
    var monthlyStudentLoanRow = document.getElementById('monthly-student-loan-row');
    if (hasStudentLoan) {
        monthlyStudentLoanRow.hidden = false;
        document.getElementById('monthly-student-loan').textContent = formatDeduction(studentLoan / 12);
    } else {
        monthlyStudentLoanRow.hidden = true;
    }

    // ── Annual Breakdown ───────────────────────────
    document.getElementById('annual-gross').textContent = formatCurrency(salary);
    document.getElementById('annual-paye').textContent = formatDeduction(paye);
    document.getElementById('annual-kiwisaver').textContent = formatDeduction(kiwiSaver);
    document.getElementById('annual-kiwisaver-label').textContent = 'KiwiSaver (' + kiwiSaverPercent + '%)';
    document.getElementById('annual-acc').textContent = formatDeduction(acc);
    document.getElementById('annual-take-home').textContent = formatCurrency(takeHome);

    var annualStudentLoanRow = document.getElementById('annual-student-loan-row');
    if (hasStudentLoan) {
        annualStudentLoanRow.hidden = false;
        document.getElementById('annual-student-loan').textContent = formatDeduction(studentLoan);
    } else {
        annualStudentLoanRow.hidden = true;
    }

    // Show results with animation
    resultsSection.hidden = false;
}

/* ================================================
   Event Listeners
   ================================================ */
form.addEventListener('submit', function (e) {
    e.preventDefault();
    calculate();
});

// Recalculate on dropdown/checkbox changes
kiwisaverSelect.addEventListener('change', calculate);
studentLoanCheckbox.addEventListener('change', calculate);

// Recalculate on salary input if results already visible
salaryInput.addEventListener('input', function () {
    if (!resultsSection.hidden) {
        calculate();
    }
});

// Auto-calculate on page load with default values (PO Decision)
document.addEventListener('DOMContentLoaded', calculate);
