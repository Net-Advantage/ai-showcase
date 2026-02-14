# Rental Property Module Specification

**Version**: 1.3
**Tax Year**: 2025/2026 (configurable)
**Last Updated**: February 15, 2026

---

# Overview

This module enables an individual taxpayer to manage rental properties, prepare rental workpapers, and produce outputs for tax calculation.

The module supports:

* Management of a rental property portfolio
* One workpaper per property per tax year
* Multi-user preparation with audit tracking
* Evidence attachment and validation
* Calculation of deductible expenses and net rental position
* Portfolio-level summaries and diagnostics

---

# 1. Key Concepts

## 1.1 Workpaper Structure

Each property has a single workpaper per tax year containing:

* Inputs
* Expense lines
* Adjustments
* Calculations
* Evidence
* Contributors
* Activity log

## 1.2 Status Lifecycle

* NotStarted
* InProgress
* ReadyToReview
* Complete
* Locked

---

# 2. Data Model

## 2.1 User

| Field       | Type                             |
| ----------- | -------------------------------- |
| userId      | GUID                             |
| displayName | string                           |
| email       | string                           |
| role        | enum (Preparer, Reviewer, Admin) |

---

## 2.2 RentalProperty

| Field               | Type    | Required |
| ------------------- | ------- | -------- |
| propertyId          | GUID    | Yes      |
| displayName         | string  | Yes      |
| addressLine1        | string  | Yes      |
| city                | string  | Yes      |
| propertyType        | enum    | Yes      |
| ownershipPercentage | decimal | Yes      |
| acquisitionDate     | date    | No       |
| disposalDate        | date    | No       |
| isMainHome          | bool    | Yes      |
| isNewBuild          | bool    | Yes      |
| isActive            | bool    | Yes      |

---

## 2.3 RentalExpenseLine

| Field           | Type    | Required |
| --------------- | ------- | -------- |
| lineId          | GUID    | Yes      |
| category        | enum    | Yes      |
| description     | string  | Yes      |
| amount          | decimal | Yes      |
| isCapital       | bool    | Yes      |
| isApportionable | bool    | Yes      |
| evidenceIds     | GUID[]  | No       |
| notes           | string  | No       |

### Categories

* Interest
* Rates
* Insurance
* PropertyManagement
* BodyCorporate
* RepairsMaintenance
* Cleaning
* Advertising
* LegalFees
* AccountingFees
* Utilities
* Travel
* Other

---

## 2.4 RentalWorkpaper

| Field                   | Type    |
| ----------------------- | ------- |
| workpaperId             | GUID    |
| propertyId              | GUID    |
| taxYear                 | string  |
| status                  | enum    |
| grossRentalIncome       | decimal |
| daysRented              | int     |
| daysAvailable           | int     |
| daysPrivate             | int     |
| mixedUse                | bool    |
| hasCapitalExpenditure   | bool    |
| capitalExpenditureTotal | decimal |
| expenseLines            | list    |
| createdBy               | GUID    |
| lastModifiedBy          | GUID    |
| currentOwnerUserId      | GUID    |

### Calculated Fields

* totalExpenses
* capitalExcludedTotal
* deductibleInterest
* adjustedDeductibleExpenses
* netRentalIncome
* lossCarryForward

---

## 2.5 WorkpaperContributor

| Field           | Type     |
| --------------- | -------- |
| contributorId   | GUID     |
| workpaperId     | GUID     |
| userId          | GUID     |
| role            | enum     |
| firstActivityAt | datetime |
| lastActivityAt  | datetime |
| isCurrentOwner  | bool     |
| isActive        | bool     |

---

## 2.6 WorkpaperActivity

| Field       | Type     |
| ----------- | -------- |
| activityId  | GUID     |
| workpaperId | GUID     |
| userId      | GUID     |
| actionType  | enum     |
| fieldName   | string   |
| oldValue    | string   |
| newValue    | string   |
| timestamp   | datetime |

---

## 2.7 Evidence

| Field       | Type     |
| ----------- | -------- |
| evidenceId  | GUID     |
| fileName    | string   |
| contentType | string   |
| sizeBytes   | long     |
| uploadedAt  | datetime |

---

# 3. Screens

## 3.1 Rental Properties Portfolio

### Features

* List all properties
* Display ownership percentage
* Display status
* Display prepared by
* Show warnings and net position

### Totals Panel

* Total Income
* Total Expenses
* Net Position
* Loss Carry Forward
* Completed Count
* Warning Count

---

## 3.2 Property Detail

* Edit property metadata
* Ownership percentage
* Property flags

---

## 3.3 Workpaper Detail

### Tabs

* Income & Days
* Expenses
* Adjustments
* Results
* Evidence
* Contributors
* Activity

---

## 3.4 Contributors Panel

| Name | Role | Owner |
| ---- | ---- | ----- |

Actions:

* Assign Owner
* Change Role

---

## 3.5 Activity Log

Chronological list of actions.

---

# 4. Calculations

## 4.1 Total Expenses

TotalExpenses = sum(expenseLines)

## 4.2 Capital Exclusion

CapitalExcludedTotal = sum(capital lines)

## 4.3 Ownership Adjustment

OwnedExpenses = TotalExpenses * ownershipPercentage

## 4.4 Apportionment

Ratio = DaysRented / (DaysRented + DaysPrivate)

ApportionedExpenses = OwnedExpenses * Ratio

## 4.5 Interest Deductibility

DeductibleInterest = Interest * Rate

## 4.6 Final Expenses

AdjustedExpenses = (ApportionedExpenses - Interest) + DeductibleInterest

## 4.7 Net Income

NetRentalIncome = Income - AdjustedExpenses

## 4.8 Loss

If Net < 0 then LossCarryForward = abs(Net)

---

# 5. Prepared By and Contributors

## 5.1 Rules

* Workpaper has multiple contributors
* One current owner
* Prepared By = current owner

## 5.2 Automatic Tracking

* Add contributor on activity
* Update lastModifiedBy

## 5.3 Ownership Assignment

* Only one current owner
* Can be reassigned

---

# 6. Diagnostics

## Blocking

* Missing inputs
* Invalid days

## Warning

* Mixed use
* Capital expenses
* Missing evidence

## Info

* Not calculated

---

# 7. API

## Portfolio

GET /api/rental/portfolio

## Workpaper

GET /api/rental/workpapers/{id}
POST /api/rental/workpapers/{id}/calculate
PUT /api/rental/workpapers/{id}
POST /api/rental/workpapers/{id}/complete

## Assign Owner

POST /api/rental/workpapers/{id}/assign

---

# 8. Security

* Scoped to taxpayer
* Role-based permissions

---

# 9. Performance

* Portfolio < 500ms
* Calculation < 200ms

---

# 10. Accessibility

* WCAG 2.1 AA
* Keyboard navigation
* Screen reader support

---

# 11. Requirements

| ID      | Description             |
| ------- | ----------------------- |
| REQ-001 | Manage properties       |
| REQ-002 | Capture expenses        |
| REQ-003 | Ownership handling      |
| REQ-004 | Multi-user contributors |
| REQ-005 | Prepared By             |
| REQ-006 | Audit log               |
| REQ-007 | Calculations            |
| REQ-008 | Portfolio totals        |
| REQ-009 | Evidence tracking       |
| REQ-010 | API support             |

---

# Document Control

Version 1.3 - Added multi-user support and Prepared By functionality

Status: Draft
