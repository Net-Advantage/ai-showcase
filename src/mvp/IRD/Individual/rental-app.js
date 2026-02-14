/* ================================================
   Rental Property Module — Shared JavaScript
   REQ-001, REQ-002, REQ-003, REQ-006, REQ-007,
   REQ-008, REQ-009, REQ-012
   ================================================ */

'use strict';

/* ------------------------------------------------
   Constants & Defaults
   ------------------------------------------------ */

// REQ-006: Default user stub
var DEFAULT_USER = Object.freeze({
    userId: 'default-user',
    displayName: 'Current User'
});

// REQ-007: Default settings
var DEFAULT_SETTINGS = Object.freeze({
    taxYear: '2025/2026',
    interestDeductibilityRate: 0.80
});

// REQ-002: Expense category list
var EXPENSE_CATEGORIES = Object.freeze([
    'Interest',
    'Rates',
    'Insurance',
    'PropertyManagement',
    'BodyCorporate',
    'RepairsMaintenance',
    'Cleaning',
    'Advertising',
    'LegalFees',
    'AccountingFees',
    'Utilities',
    'Travel',
    'Other'
]);

// Status lifecycle labels and order
var STATUS_ORDER = Object.freeze([
    'NotStarted',
    'InProgress',
    'ReadyToReview',
    'Complete',
    'Locked'
]);

/* ------------------------------------------------
   LocalStorage Keys
   ------------------------------------------------ */
var STORAGE_KEYS = Object.freeze({
    properties: 'rental_properties',
    workpapers: 'rental_workpapers',
    activities: 'rental_activities',
    evidence:   'rental_evidence',
    settings:   'rental_settings'
});

/* ------------------------------------------------
   Utility: Format currency (NZD)
   ------------------------------------------------ */
function formatCurrency(value) {
    if (value == null || isNaN(value)) return '$0.00';
    var abs = Math.abs(value);
    var formatted = '$' + abs.toLocaleString('en-NZ', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
    return value < 0 ? '-' + formatted : formatted;
}

/* ------------------------------------------------
   Utility: Format readable category names
   ------------------------------------------------ */
function formatCategory(cat) {
    if (!cat) return '';
    return cat.replace(/([A-Z])/g, ' $1').trim();
}

/* ------------------------------------------------
   Utility: Format date for display
   ------------------------------------------------ */
function formatDate(isoString) {
    if (!isoString) return '';
    var d = new Date(isoString);
    return d.toLocaleDateString('en-NZ', {
        year: 'numeric', month: 'short', day: 'numeric',
        hour: '2-digit', minute: '2-digit'
    });
}

/* ------------------------------------------------
   Utility: Parse query string params
   ------------------------------------------------ */
function getQueryParam(name) {
    var params = new URLSearchParams(window.location.search);
    return params.get(name);
}

/* ================================================
   Data Layer — Generic localStorage CRUD
   ================================================ */

function _load(key) {
    try {
        var raw = localStorage.getItem(key);
        return raw ? JSON.parse(raw) : [];
    } catch (e) {
        console.error('Failed to load ' + key, e);
        return [];
    }
}

function _save(key, data) {
    try {
        localStorage.setItem(key, JSON.stringify(data));
    } catch (e) {
        console.error('Failed to save ' + key, e);
    }
}

/* ------------------------------------------------
   Settings (REQ-007)
   ------------------------------------------------ */
function loadSettings() {
    try {
        var raw = localStorage.getItem(STORAGE_KEYS.settings);
        if (raw) {
            var parsed = JSON.parse(raw);
            return Object.assign({}, DEFAULT_SETTINGS, parsed);
        }
    } catch (e) { /* ignore */ }
    return Object.assign({}, DEFAULT_SETTINGS);
}

function saveSettings(settings) {
    _save(STORAGE_KEYS.settings, settings);
}

/* ------------------------------------------------
   Properties CRUD (REQ-001)
   ------------------------------------------------ */
function loadProperties() {
    return _load(STORAGE_KEYS.properties);
}

function saveProperties(properties) {
    _save(STORAGE_KEYS.properties, properties);
}

function getPropertyById(id) {
    var props = loadProperties();
    return props.find(function (p) { return p.propertyId === id; }) || null;
}

function createProperty(data) {
    var props = loadProperties();
    var property = {
        propertyId: crypto.randomUUID(),
        displayName: data.displayName || '',
        addressLine1: data.addressLine1 || '',
        city: data.city || '',
        propertyType: data.propertyType || 'House',
        ownershipPercentage: parseFloat(data.ownershipPercentage) || 1.0,
        acquisitionDate: data.acquisitionDate || null,
        disposalDate: data.disposalDate || null,
        isMainHome: !!data.isMainHome,
        isNewBuild: !!data.isNewBuild,
        isActive: data.isActive !== false
    };
    props.push(property);
    saveProperties(props);

    // REQ-001: Also create a workpaper for this property
    createWorkpaper(property.propertyId);

    return property;
}

function updateProperty(id, data) {
    var props = loadProperties();
    var idx = props.findIndex(function (p) { return p.propertyId === id; });
    if (idx === -1) return null;
    Object.assign(props[idx], data);
    saveProperties(props);
    return props[idx];
}

function deleteProperty(id) {
    var props = loadProperties().filter(function (p) { return p.propertyId !== id; });
    saveProperties(props);
    // Also remove associated workpapers, activities, evidence
    var wps = loadWorkpapers().filter(function (w) { return w.propertyId !== id; });
    saveWorkpapers(wps);
}

/* ------------------------------------------------
   Workpapers CRUD (REQ-002, REQ-003, REQ-007)
   ------------------------------------------------ */
function loadWorkpapers() {
    return _load(STORAGE_KEYS.workpapers);
}

function saveWorkpapers(workpapers) {
    _save(STORAGE_KEYS.workpapers, workpapers);
}

function getWorkpaperByPropertyId(propertyId) {
    var settings = loadSettings();
    var wps = loadWorkpapers();
    return wps.find(function (w) {
        return w.propertyId === propertyId && w.taxYear === settings.taxYear;
    }) || null;
}

function getWorkpaperById(id) {
    var wps = loadWorkpapers();
    return wps.find(function (w) { return w.workpaperId === id; }) || null;
}

function createWorkpaper(propertyId) {
    var settings = loadSettings();
    var wps = loadWorkpapers();
    // Don't duplicate
    var existing = wps.find(function (w) {
        return w.propertyId === propertyId && w.taxYear === settings.taxYear;
    });
    if (existing) return existing;

    var wp = {
        workpaperId: crypto.randomUUID(),
        propertyId: propertyId,
        taxYear: settings.taxYear,
        status: 'NotStarted',
        grossRentalIncome: 0,
        daysRented: 0,
        daysAvailable: 0,
        daysPrivate: 0,
        mixedUse: false,
        expenseLines: [],
        createdBy: DEFAULT_USER.userId,
        lastModifiedBy: DEFAULT_USER.userId,
        currentOwnerUserId: DEFAULT_USER.userId,
        // Calculated fields
        totalExpenses: 0,
        capitalExcludedTotal: 0,
        deductibleExpenseBase: 0,
        ownedExpenses: 0,
        apportionedExpenses: 0,
        interestTotal: 0,
        deductibleInterest: 0,
        adjustedDeductibleExpenses: 0,
        adjustedIncome: 0,
        netRentalIncome: 0,
        lossCarryForward: 0
    };
    wps.push(wp);
    saveWorkpapers(wps);
    logActivity(wp.workpaperId, 'Created', 'status', null, 'NotStarted');
    return wp;
}

function updateWorkpaper(id, data) {
    var wps = loadWorkpapers();
    var idx = wps.findIndex(function (w) { return w.workpaperId === id; });
    if (idx === -1) return null;
    Object.assign(wps[idx], data);
    wps[idx].lastModifiedBy = DEFAULT_USER.userId;
    saveWorkpapers(wps);
    return wps[idx];
}

/* ------------------------------------------------
   Expense Lines CRUD (REQ-002)
   ------------------------------------------------ */
function addExpenseLine(workpaperId, lineData) {
    var wps = loadWorkpapers();
    var idx = wps.findIndex(function (w) { return w.workpaperId === workpaperId; });
    if (idx === -1) return null;

    var line = {
        lineId: crypto.randomUUID(),
        category: lineData.category || 'Other',
        description: lineData.description || '',
        amount: parseFloat(lineData.amount) || 0,
        isCapital: !!lineData.isCapital,
        isApportionable: lineData.isApportionable !== false,
        evidenceIds: [],
        notes: lineData.notes || ''
    };
    wps[idx].expenseLines.push(line);
    saveWorkpapers(wps);
    logActivity(workpaperId, 'AddedExpense', 'expenseLine', null,
        line.category + ': ' + formatCurrency(line.amount));
    return line;
}

function updateExpenseLine(workpaperId, lineId, lineData) {
    var wps = loadWorkpapers();
    var wpIdx = wps.findIndex(function (w) { return w.workpaperId === workpaperId; });
    if (wpIdx === -1) return null;
    var lineIdx = wps[wpIdx].expenseLines.findIndex(function (l) { return l.lineId === lineId; });
    if (lineIdx === -1) return null;
    var old = Object.assign({}, wps[wpIdx].expenseLines[lineIdx]);
    Object.assign(wps[wpIdx].expenseLines[lineIdx], lineData);
    saveWorkpapers(wps);
    logActivity(workpaperId, 'UpdatedExpense', 'expenseLine',
        old.category + ': ' + formatCurrency(old.amount),
        (lineData.category || old.category) + ': ' + formatCurrency(lineData.amount || old.amount));
    return wps[wpIdx].expenseLines[lineIdx];
}

function removeExpenseLine(workpaperId, lineId) {
    var wps = loadWorkpapers();
    var wpIdx = wps.findIndex(function (w) { return w.workpaperId === workpaperId; });
    if (wpIdx === -1) return;
    var line = wps[wpIdx].expenseLines.find(function (l) { return l.lineId === lineId; });
    wps[wpIdx].expenseLines = wps[wpIdx].expenseLines.filter(function (l) { return l.lineId !== lineId; });
    saveWorkpapers(wps);
    if (line) {
        logActivity(workpaperId, 'RemovedExpense', 'expenseLine',
            line.category + ': ' + formatCurrency(line.amount), null);
    }
}

/* ------------------------------------------------
   Evidence CRUD (REQ-009)
   ------------------------------------------------ */
function loadEvidence() {
    return _load(STORAGE_KEYS.evidence);
}

function saveEvidence(evidence) {
    _save(STORAGE_KEYS.evidence, evidence);
}

function getEvidenceForWorkpaper(workpaperId) {
    var wp = getWorkpaperById(workpaperId);
    if (!wp) return [];
    var allLineEvidenceIds = [];
    wp.expenseLines.forEach(function (line) {
        if (line.evidenceIds) {
            allLineEvidenceIds = allLineEvidenceIds.concat(line.evidenceIds);
        }
    });
    var allEvidence = loadEvidence();
    return allEvidence.filter(function (e) {
        return e.workpaperId === workpaperId || allLineEvidenceIds.indexOf(e.evidenceId) !== -1;
    });
}

function addEvidence(workpaperId, data) {
    var evidenceList = loadEvidence();
    var evidence = {
        evidenceId: crypto.randomUUID(),
        workpaperId: workpaperId,
        fileName: data.fileName || 'untitled',
        contentType: data.contentType || 'application/octet-stream',
        sizeBytes: parseInt(data.sizeBytes, 10) || 0,
        uploadedAt: new Date().toISOString(),
        uploadedBy: DEFAULT_USER.userId
    };
    evidenceList.push(evidence);
    saveEvidence(evidenceList);
    logActivity(workpaperId, 'AddedEvidence', 'evidence', null, evidence.fileName);
    return evidence;
}

function removeEvidence(evidenceId) {
    var evidenceList = loadEvidence();
    var item = evidenceList.find(function (e) { return e.evidenceId === evidenceId; });
    evidenceList = evidenceList.filter(function (e) { return e.evidenceId !== evidenceId; });
    saveEvidence(evidenceList);
    if (item) {
        logActivity(item.workpaperId, 'RemovedEvidence', 'evidence', item.fileName, null);
    }
}

/* ------------------------------------------------
   Activity Logging (REQ-006)
   ------------------------------------------------ */
function loadActivities() {
    return _load(STORAGE_KEYS.activities);
}

function saveActivities(activities) {
    _save(STORAGE_KEYS.activities, activities);
}

function logActivity(workpaperId, actionType, fieldName, oldValue, newValue) {
    var activities = loadActivities();
    var entry = {
        activityId: crypto.randomUUID(),
        workpaperId: workpaperId,
        userId: DEFAULT_USER.userId,
        actionType: actionType,
        fieldName: fieldName || null,
        oldValue: oldValue != null ? String(oldValue) : null,
        newValue: newValue != null ? String(newValue) : null,
        timestamp: new Date().toISOString()
    };
    activities.push(entry);
    saveActivities(activities);
    return entry;
}

function getActivitiesForWorkpaper(workpaperId) {
    return loadActivities().filter(function (a) {
        return a.workpaperId === workpaperId;
    }).sort(function (a, b) {
        return new Date(b.timestamp) - new Date(a.timestamp);
    });
}

/* ================================================
   Calculation Pipeline (REQ-007)
   ================================================ */
function calculateWorkpaper(workpaperId) {
    var wp = getWorkpaperById(workpaperId);
    if (!wp) return null;

    var property = getPropertyById(wp.propertyId);
    var settings = loadSettings();
    var ownershipPct = property ? property.ownershipPercentage : 1.0;

    var expenseLines = wp.expenseLines || [];

    // TotalExpenses = sum(expenseLines.amount)
    var totalExpenses = expenseLines.reduce(function (sum, l) {
        return sum + (parseFloat(l.amount) || 0);
    }, 0);

    // CapitalExcludedTotal = sum(amount where isCapital)
    var capitalExcludedTotal = expenseLines.reduce(function (sum, l) {
        return sum + (l.isCapital ? (parseFloat(l.amount) || 0) : 0);
    }, 0);

    // DeductibleExpenseBase = TotalExpenses - CapitalExcludedTotal
    var deductibleExpenseBase = totalExpenses - capitalExcludedTotal;

    // OwnedExpenses = DeductibleExpenseBase * ownershipPercentage
    var ownedExpenses = deductibleExpenseBase * ownershipPct;

    // Apportionment
    var apportionedExpenses = ownedExpenses;
    if (wp.mixedUse) {
        var daysRented = parseInt(wp.daysRented, 10) || 0;
        var daysPrivate = parseInt(wp.daysPrivate, 10) || 0;
        var totalDays = daysRented + daysPrivate;
        if (totalDays > 0) {
            var ratio = daysRented / totalDays;
            apportionedExpenses = ownedExpenses * ratio;
        }
    }

    // InterestTotal = sum(amount where category == 'Interest')
    var interestTotal = expenseLines.reduce(function (sum, l) {
        return sum + (l.category === 'Interest' && !l.isCapital ? (parseFloat(l.amount) || 0) : 0);
    }, 0);

    // DeductibleInterest = InterestTotal * interestDeductibilityRate
    var deductibleInterest = interestTotal * settings.interestDeductibilityRate;

    // AdjustedExpenses = (ApportionedExpenses - InterestTotal) + DeductibleInterest
    var adjustedDeductibleExpenses = (apportionedExpenses - interestTotal) + deductibleInterest;

    // AdjustedIncome = grossRentalIncome * ownershipPercentage
    var adjustedIncome = (parseFloat(wp.grossRentalIncome) || 0) * ownershipPct;

    // NetRentalIncome = AdjustedIncome - AdjustedExpenses
    var netRentalIncome = adjustedIncome - adjustedDeductibleExpenses;

    // LossCarryForward
    var lossCarryForward = netRentalIncome < 0 ? Math.abs(netRentalIncome) : 0;

    var calculated = {
        totalExpenses: totalExpenses,
        capitalExcludedTotal: capitalExcludedTotal,
        deductibleExpenseBase: deductibleExpenseBase,
        ownedExpenses: ownedExpenses,
        apportionedExpenses: apportionedExpenses,
        interestTotal: interestTotal,
        deductibleInterest: deductibleInterest,
        adjustedDeductibleExpenses: adjustedDeductibleExpenses,
        adjustedIncome: adjustedIncome,
        netRentalIncome: netRentalIncome,
        lossCarryForward: lossCarryForward
    };

    updateWorkpaper(workpaperId, calculated);

    // Auto-transition to InProgress if still NotStarted
    var wpUpdated = getWorkpaperById(workpaperId);
    if (wpUpdated && wpUpdated.status === 'NotStarted') {
        updateWorkpaper(workpaperId, { status: 'InProgress' });
        logActivity(workpaperId, 'StatusChange', 'status', 'NotStarted', 'InProgress');
    }

    return Object.assign({}, wpUpdated, calculated);
}

/* ================================================
   Diagnostics (REQ-012)
   ================================================ */
function runDiagnostics(workpaperId) {
    var wp = getWorkpaperById(workpaperId);
    if (!wp) return [];

    var diagnostics = [];

    // Blocking diagnostics
    if (!wp.grossRentalIncome || parseFloat(wp.grossRentalIncome) <= 0) {
        diagnostics.push({
            level: 'blocking',
            message: 'Gross rental income is missing or zero.'
        });
    }

    var daysRented = parseInt(wp.daysRented, 10) || 0;
    if (daysRented <= 0) {
        diagnostics.push({
            level: 'blocking',
            message: 'Days rented is missing or zero.'
        });
    }

    if (daysRented > 365) {
        diagnostics.push({
            level: 'blocking',
            message: 'Days rented exceeds 365.'
        });
    }

    if (wp.mixedUse) {
        var daysPrivate = parseInt(wp.daysPrivate, 10) || 0;
        if (daysPrivate <= 0) {
            diagnostics.push({
                level: 'blocking',
                message: 'Mixed use is enabled but days private is missing or zero.'
            });
        }
        if (daysPrivate > 365) {
            diagnostics.push({
                level: 'blocking',
                message: 'Days private exceeds 365.'
            });
        }
        if (daysRented + daysPrivate > 365) {
            diagnostics.push({
                level: 'blocking',
                message: 'Total days (rented + private) exceeds 365.'
            });
        }
    }

    // Warning diagnostics
    if (wp.mixedUse) {
        diagnostics.push({
            level: 'warning',
            message: 'Mixed-use apportionment is active.'
        });
    }

    var hasCapital = (wp.expenseLines || []).some(function (l) { return l.isCapital; });
    if (hasCapital) {
        diagnostics.push({
            level: 'warning',
            message: 'Capital expenses are present and excluded from deductions.'
        });
    }

    var evidence = getEvidenceForWorkpaper(workpaperId);
    var evidenceIds = evidence.map(function (e) { return e.evidenceId; });
    var linesWithoutEvidence = (wp.expenseLines || []).filter(function (l) {
        return !l.evidenceIds || l.evidenceIds.length === 0 ||
            !l.evidenceIds.some(function (eid) { return evidenceIds.indexOf(eid) !== -1; });
    });
    if (linesWithoutEvidence.length > 0) {
        diagnostics.push({
            level: 'warning',
            message: linesWithoutEvidence.length + ' expense line(s) have no linked evidence.'
        });
    }

    // Info diagnostics
    if (wp.status === 'NotStarted') {
        diagnostics.push({
            level: 'info',
            message: 'Workpaper has not yet been calculated.'
        });
    }

    return diagnostics;
}

/* ================================================
   Portfolio Totals (REQ-008)
   ================================================ */
function calculatePortfolioTotals() {
    var settings = loadSettings();
    var properties = loadProperties().filter(function (p) { return p.isActive; });
    var workpapers = loadWorkpapers().filter(function (w) { return w.taxYear === settings.taxYear; });

    var totals = {
        totalIncome: 0,
        totalExpenses: 0,
        netPosition: 0,
        lossCarryForward: 0,
        completedCount: 0,
        warningCount: 0,
        propertyCount: properties.length
    };

    properties.forEach(function (prop) {
        var wp = workpapers.find(function (w) { return w.propertyId === prop.propertyId; });
        if (!wp) return;

        totals.totalIncome += (parseFloat(wp.adjustedIncome) || 0);
        totals.totalExpenses += (parseFloat(wp.adjustedDeductibleExpenses) || 0);
        totals.netPosition += (parseFloat(wp.netRentalIncome) || 0);
        totals.lossCarryForward += (parseFloat(wp.lossCarryForward) || 0);

        if (wp.status === 'Complete' || wp.status === 'Locked') {
            totals.completedCount++;
        }

        var diags = runDiagnostics(wp.workpaperId);
        var warnings = diags.filter(function (d) {
            return d.level === 'warning' || d.level === 'blocking';
        });
        if (warnings.length > 0) {
            totals.warningCount++;
        }
    });

    return totals;
}

/* ------------------------------------------------
   Status transition helpers
   ------------------------------------------------ */
function transitionStatus(workpaperId, newStatus) {
    var wp = getWorkpaperById(workpaperId);
    if (!wp) return null;

    var currentIdx = STATUS_ORDER.indexOf(wp.status);
    var newIdx = STATUS_ORDER.indexOf(newStatus);

    // Allow forward transitions and back to InProgress from ReadyToReview
    if (newIdx === -1) return null;
    if (newIdx <= currentIdx && !(newStatus === 'InProgress' && wp.status === 'ReadyToReview')) {
        return null;
    }

    var oldStatus = wp.status;
    updateWorkpaper(workpaperId, { status: newStatus });
    logActivity(workpaperId, 'StatusChange', 'status', oldStatus, newStatus);
    return getWorkpaperById(workpaperId);
}

/* ------------------------------------------------
   Portfolio summary per property (for list page)
   ------------------------------------------------ */
function getPropertySummary(propertyId) {
    var property = getPropertyById(propertyId);
    if (!property) return null;
    var wp = getWorkpaperByPropertyId(propertyId);
    var diags = wp ? runDiagnostics(wp.workpaperId) : [];
    return {
        property: property,
        workpaper: wp,
        diagnostics: diags,
        hasBlocking: diags.some(function (d) { return d.level === 'blocking'; }),
        hasWarning: diags.some(function (d) { return d.level === 'warning'; }),
        netRentalIncome: wp ? wp.netRentalIncome : 0,
        status: wp ? wp.status : 'NotStarted'
    };
}
