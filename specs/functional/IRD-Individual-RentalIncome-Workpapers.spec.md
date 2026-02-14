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

# 12. Domain Outputs and Compliance Mapping

This module separates rental preparation from compliance reporting.

* **Preparation** produces jurisdiction-neutral domain outputs.
* **Compliance** maps domain outputs to a jurisdiction-specific tax return (example: NZ IR3/IR3R).

This protects the preparation domain from external form terminology and future form changes.

---

# 12.1 RentalSummary Domain Output (Jurisdiction Neutral)

## Purpose

A `RentalSummary` is the canonical output of the RentalWorkpaperContext. It is the only input required by the TaxReturnContext for rental reporting.

## RentalSummary Schema

| Field                          | Type    | Required | Notes                                                                        |
| ------------------------------ | ------- | -------- | ---------------------------------------------------------------------------- |
| propertyId                     | GUID    | Yes      | Source property                                                              |
| taxYear                        | string  | Yes      | Example: 2025-2026                                                           |
| propertyType                   | enum    | Yes      | Residential, Commercial, MixedUse                                            |
| ownershipPercentage            | decimal | Yes      | 0.00 to 1.00, sourced from property                                          |
| grossIncome                    | decimal | Yes      | Rents received (and optionally other income if captured)                     |
| otherIncome                    | decimal | Yes      | Default 0                                                                    |
| totalIncome                    | decimal | Yes      | grossIncome + otherIncome                                                    |
| totalExpensesBeforeAdjustments | decimal | Yes      | Sum of expense lines (excluding capital) before ownership and apportionment  |
| capitalExcludedTotal           | decimal | Yes      | Sum of capital lines                                                         |
| interestIncurred               | decimal | Yes      | Total interest incurred                                                      |
| interestClaimed                | decimal | Yes      | Allowed interest after rules                                                 |
| deductibleExpenses             | decimal | Yes      | Total deductible expenses after ownership, apportionment, and interest rules |
| netIncome                      | decimal | Yes      | totalIncome - deductibleExpenses                                             |
| lossCarryForward               | decimal | Yes      | 0 if netIncome >= 0 else abs(netIncome)                                      |
| diagnostics                    | list    | Yes      | Warnings and blockers in jurisdiction-neutral language                       |

## RentalSummary Generation Rule

The RentalWorkpaperContext must expose a query/service that produces RentalSummary without referencing any tax form fields.

---

# 12.2 TaxReturnContext (New Bounded Context)

## Responsibility

The TaxReturnContext produces compliant tax return outputs for a selected jurisdiction (example: NZ IR3) using domain outputs from preparation contexts.

## Ubiquitous Language

* **TaxReturn**: A jurisdiction-specific filing representation for a tax year.
* **ReturnSection**: A grouping of return fields (example: Rental section).
* **ReturnField**: A single field identified by a jurisdiction-specific key.
* **Mapping**: Transformation from domain outputs to return fields.

## Aggregate: TaxReturn

### TaxReturn Fields

| Field        | Type     | Required | Notes                                  |
| ------------ | -------- | -------- | -------------------------------------- |
| taxReturnId  | GUID     | Yes      |                                        |
| taxpayerId   | GUID     | Yes      | External identity of taxpayer          |
| taxYear      | string   | Yes      |                                        |
| jurisdiction | enum     | Yes      | NZ-IRD, AU-ATO (future)                |
| status       | enum     | Yes      | Draft, ReadyToReview, Complete, Locked |
| sections     | list     | Yes      | Includes Rental section                |
| createdAt    | datetime | Yes      |                                        |
| updatedAt    | datetime | Yes      |                                        |

### TaxReturn Status Rules

* Draft: created, not validated
* ReadyToReview: validation passed, ready for review
* Complete: user confirmed completion
* Locked: immutable snapshot for filing or post-filing

---

# 12.3 Compliance Mapping Layer (Anti-Corruption Layer)

## Service: JurisdictionMappingService

### Purpose

Convert a set of domain outputs (example: RentalSummary[]) into a jurisdiction-specific return model.

### Inputs

* jurisdiction
* taxYear
* RentalSummary[]
* priorYearCarryForward inputs (if required)

### Outputs

* TaxReturnSection (Rental)
* Validation diagnostics (form-level)

### Hard Rule

No preparation aggregate or DTO may reference jurisdiction form keys or box numbers.

---

# 12.4 NZ IRD Mapping Specification (IR3 and IR3R)

This section is implemented inside the TaxReturnContext.

## 12.4.1 IR3 Rental Section Fields

### Residential Rental (IR3 Question 22)

| RentalSummary Source                    | IR3 Field Key | Description                                                                                 |
| --------------------------------------- | ------------- | ------------------------------------------------------------------------------------------- |
| sum(totalIncome) for Residential        | IR3.Q22.A     | Gross residential rental income (and related income totals as defined by your return model) |
| sum(otherIncome) for Residential        | IR3.Q22.C     | Other residential income                                                                    |
| sum(totalIncome) for Residential        | IR3.Q22.D     | Total residential income                                                                    |
| sum(deductibleExpenses) for Residential | IR3.Q22.E     | Residential rental deductions                                                               |
| priorYearResidentialLossUsed            | IR3.Q22.F     | Excess deductions brought forward                                                           |
| residentialDeductionsClaimedThisYear    | IR3.Q22.G     | Deductions claimed this year                                                                |
| residentialNetIncomeThisYear            | IR3.Q22.H     | Net residential income                                                                      |
| residentialLossCarryForward             | IR3.Q22.I     | Excess deductions to carry forward                                                          |

### Interest Disclosure (IR3 Question 23)

| RentalSummary Source                  | IR3 Field Key | Description               |
| ------------------------------------- | ------------- | ------------------------- |
| sum(interestIncurred) for Residential | IR3.Q23.A     | Total interest incurred   |
| sum(interestClaimed) for Residential  | IR3.Q23.B     | Interest claimed          |
| interestReasonSelection               | IR3.Q23.C     | Reason for interest claim |

### Non-Residential Rental (IR3 Question 24)

| RentalSummary Source              | IR3 Field Key | Description |
| --------------------------------- | ------------- | ----------- |
| sum(netIncome) for NonResidential | IR3.Q24       | Net rents   |

## 12.4.2 IR3R Per Property Outputs

The system must be able to generate an IR3R-like per-property schedule from RentalSummary.

| RentalSummary Source    | IR3R Field Key | Description       |
| ----------------------- | -------------- | ----------------- |
| grossIncome             | IR3R.B1        | Rents received    |
| otherIncome             | IR3R.B2        | Other income      |
| totalIncome             | IR3R.B4        | Total income      |
| interestIncurred        | IR3R.B7A       | Interest incurred |
| interestClaimed         | IR3R.B7B       | Interest claimed  |
| interestReasonSelection | IR3R.B7C       | Interest reason   |
| deductibleExpenses      | IR3R.B14       | Total expenses    |
| netIncome               | IR3R.B15       | Net rents         |

## 12.4.3 Portfolio Aggregation Rules

| Rule                        | Description                                                                                        |
| --------------------------- | -------------------------------------------------------------------------------------------------- |
| Residential aggregation     | Only Residential properties contribute to IR3.Q22 and IR3.Q23                                      |
| Non-residential aggregation | Only non-residential properties contribute to IR3.Q24                                              |
| Ring-fencing                | Residential losses produce carry forward values; claimed amount is constrained by rules            |
| Interest separation         | interestIncurred and interestClaimed must be reported separately from expenses in the return model |

---

# 12.5 Export Contract

The TaxReturnContext must expose a stable export contract.

## 12.5.1 Return Export Payload

A jurisdiction-specific payload with:

* Return metadata (taxpayerId, year, jurisdiction, status)
* Rental section fields (IR3 keys)
* Per-property schedule records (IR3R keys)
* Validation results

## 12.5.2 Validation Requirements

Validation occurs at two levels:

* Preparation validation: workpaper-level diagnostics (already in RentalWorkpaperContext)
* Compliance validation: return-level checks, including completeness of required fields and consistency between rollups and schedules

---

# 12.6 API Additions

## 12.6.1 RentalSummary

GET `/api/rental/summaries?taxYear=2025-2026`

Returns a list of RentalSummary for the selected year.

## 12.6.2 Generate Tax Return

POST `/api/taxreturns/generate`

Request:

```json
{
  "taxYear": "2025-2026",
  "jurisdiction": "NZ-IRD",
  "includeSections": ["Rental"],
  "inputs": {
    "rental": {
      "priorYearResidentialLossUsed": 0,
      "interestReasonSelection": "TBD"
    }
  }
}
```

Response:

```json
{
  "taxReturnId": "guid",
  "status": "Draft",
  "jurisdiction": "NZ-IRD",
  "sections": {
    "rental": {
      "fields": {
        "IR3.Q22.A": 0,
        "IR3.Q22.C": 0,
        "IR3.Q22.D": 0,
        "IR3.Q22.E": 0,
        "IR3.Q22.F": 0,
        "IR3.Q22.G": 0,
        "IR3.Q22.H": 0,
        "IR3.Q22.I": 0,
        "IR3.Q23.A": 0,
        "IR3.Q23.B": 0,
        "IR3.Q23.C": "TBD",
        "IR3.Q24": 0
      },
      "schedule": [
        {
          "propertyId": "guid",
          "fields": {
            "IR3R.B1": 0,
            "IR3R.B2": 0,
            "IR3R.B4": 0,
            "IR3R.B7A": 0,
            "IR3R.B7B": 0,
            "IR3R.B7C": "TBD",
            "IR3R.B14": 0,
            "IR3R.B15": 0
          }
        }
      ]
    }
  },
  "validation": {
    "blocking": [],
    "warnings": []
  }
}
```

## 12.6.3 Validate Tax Return

POST `/api/taxreturns/{taxReturnId}/validate`

## 12.6.4 Lock Tax Return

POST `/api/taxreturns/{taxReturnId}/lock`

---

# Document Control

Version 1.4 - Added separation between preparation outputs and compliance mapping, plus TaxReturnContext and export contract

Status: Draft

Version 1.4 - Added IR3 mapping and export contract

Status: Draft
