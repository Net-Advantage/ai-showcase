# IRD Individual Tax Roadmap

In order to work up a real world test for AI development. We propose the following roadmap.

Due to it's size, we need to figure out how to coordinate this roadmap.

The Inland Revenue site is organised by income types, expenses, and credits, not by tax-return preparation steps.

Most salary and wage earners have tax deducted at source (PAYE), and many receive an automatic assessment, meaning fewer adjustment workpapers than in countries like Australia or the US.

This roadmap seeks to deal with individuals who have specific scenarios not covered by the automatic assessment.

This roadmap has been compiled using only the IRD's web site and ChatGPT. No other documentation has been consulted. The future plan will be to include the necessary legislation when specific use cases are being addressed.

## PAYE Calculator

A web-based calculator that computes New Zealand PAYE (Pay As You Earn) tax, KiwiSaver contributions, ACC levy, and optional student loan repayments based on annual salary input. The calculator supports configurable KiwiSaver contribution rates and displays both monthly and annual breakdowns of gross salary, deductions, and take-home pay.

The use-case for this calculator is to help the majority of salaried people determine their tax obligation. It is often used to help people who get offers of employment or increases to check their take home pay.

A future version of this calculator should have a comparison and a household feature.

## Core Identity & Filing Position

| Workpaper         | Description                          | IRD Link                                                                                                                       |
| ----------------- | ------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------ |
| Taxpayer Details  | IRD number, residency, filing type   | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals](https://www.ird.govt.nz/income-tax/income-tax-for-individuals) |
| Filing Obligation | Automatic assessment vs IR3 required | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals](https://www.ird.govt.nz/income-tax/income-tax-for-individuals) |
| Tax Year Summary  | Income year (1 April to 31 March)    | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals](https://www.ird.govt.nz/income-tax/income-tax-for-individuals) |


## Income Workpapers

IRD structures income by type. Each type becomes a workpaper.

| Workpaper              | Description                    | IRD Link                                                                                                                                                                             |
| ---------------------- | ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Salary & Wages         | PAYE income, benefits          | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| Schedular Payments     | Contractor income              | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| Self-Employment Income | Business or sole trader income | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| Rental Income          | Property income                | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| Investment Income      | Interest, dividends            | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| Overseas Income        | Foreign income and pensions    | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| PIE Income             | Portfolio investment entities  | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |
| Other Income           | Trust distributions, estates   | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-income) |

## Withholding & Tax Already Paid

Tax is often deducted at source, reducing filing complexity. This part will normally involve integration into IRD in order to retrieve withheld tax reported to IRD.

| Workpaper                      | Description                     | IRD Link                                                                                                                         |
| ------------------------------ | ------------------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| PAYE Tax Deducted              | Tax withheld by employer        | [https://www.ird.govt.nz/employing-staff/deductions-from-income](https://www.ird.govt.nz/employing-staff/deductions-from-income) |
| Resident Withholding Tax (RWT) | On interest/dividends           | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax)                                                         |
| Schedular Tax                  | Contractor withholding          | [https://www.ird.govt.nz/employing-staff/deductions-from-income](https://www.ird.govt.nz/employing-staff/deductions-from-income) |
| Provisional Tax                | Prepayments for business income | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax)                                                         |

## Expense & Deduction Workpapers

This is where most "workpaper logic" lives.

### Non-Business Expenses

These are the main deductions for PAYE earners.

| Workpaper                   | Description           | IRD Link                                                                                                                                                                                                                             |
| --------------------------- | --------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Tax Agent Fees              | Accounting costs      | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-expenses/non-business-expenses](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/types-of-individual-expenses/non-business-expenses) |
| Income Protection Insurance | If payout is taxable  | same as above                                                                                                                                                                                                                        |
| Investment Expenses         | Interest, commissions | same as above                                                                                                                                                                                                                        |

### Business / Self-Employment Expenses

Expenses must be incurred in earning income.

| Workpaper                 | Description          | IRD Link                                                                                                                                                                                                               |
| ------------------------- | -------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| General Business Expenses | Operating costs      | [https://www.ird.govt.nz/income-tax/income-tax-for-businesses-and-organisations/types-of-business-expenses](https://www.ird.govt.nz/income-tax/income-tax-for-businesses-and-organisations/types-of-business-expenses) |
| Depreciation              | Asset write-off      | same                                                                                                                                                                                                                   |
| Vehicle Expenses          | Business use of car  | same                                                                                                                                                                                                                   |
| Home Office Expenses      | Work from home       | same                                                                                                                                                                                                                   |
| Entertainment Expenses    | Partially deductible | same                                                                                                                                                                                                                   |
| Mixed-Use Assets          | Holiday homes, boats | same                                                                                                                                                                                                                   |

### Rental Property Expenses

| Workpaper              | Description        | IRD Link                                                                 |
| ---------------------- | ------------------ | ------------------------------------------------------------------------ |
| Interest Deductibility | Mortgage interest  | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Property Expenses      | Rates, insurance   | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Depreciation & Repairs | Capital vs revenue | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |

### Gig Economy / Side Income Expenses

| Workpaper            | Description          | IRD Link                                                                           |
| -------------------- | -------------------- | ---------------------------------------------------------------------------------- |
| Platform Fees        | Uber, Airbnb fees    | [https://www.ird.govt.nz/sharing-economy](https://www.ird.govt.nz/sharing-economy) |
| Asset Use            | Vehicle, equipment   | same                                                                               |
| Mixed Use Allocation | Business vs personal | same                                                                               |


## Tax Credit Workpapers

Tax credits directly reduce tax payable.

| Workpaper                     | Description          | IRD Link                                                                                                                                                                     |
| ----------------------------- | -------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Independent Earner Tax Credit | Income-based credit  | [https://www.ird.govt.nz/income-tax/income-tax-for-individuals/individual-tax-credits](https://www.ird.govt.nz/income-tax/income-tax-for-individuals/individual-tax-credits) |
| Donations Tax Credit          | Charitable donations | same                                                                                                                                                                         |
| Foreign Tax Credit            | Tax paid overseas    | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax)                                                                                                     |

## Adjustments & Special Items

| Workpaper               | Description                | IRD Link                                                                 |
| ----------------------- | -------------------------- | ------------------------------------------------------------------------ |
| Losses Carried Forward  | Business or rental losses  | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Use of Money Interest   | IRD interest               | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Student Loan Repayments | Income-based deductions    | [https://www.ird.govt.nz](https://www.ird.govt.nz)                       |
| KiwiSaver Contributions | Retirement savings impacts | [https://www.ird.govt.nz](https://www.ird.govt.nz)                       |

## Final Calculation Workpapers

NZ uses progressive tax rates from 10.5% to 39%

| Workpaper                  | Description             | IRD Link                                                                 |
| -------------------------- | ----------------------- | ------------------------------------------------------------------------ |
| Taxable Income Calculation | Income minus deductions | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Tax Calculation            | Apply tax brackets      | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Credits & Offsets          | Reduce tax payable      | [https://www.ird.govt.nz/income-tax](https://www.ird.govt.nz/income-tax) |
| Refund / Balance Due       | Final position          | [https://www.ird.govt.nz](https://www.ird.govt.nz)                       |

## Filing & Compliance

| Workpaper                   | Description           | IRD Link                                           |
| --------------------------- | --------------------- | -------------------------------------------------- |
| IR3 Return Preparation      | Manual return filing  | [https://www.ird.govt.nz](https://www.ird.govt.nz) |
| Automatic Assessment Review | Check IRD calculation | [https://www.ird.govt.nz](https://www.ird.govt.nz) |
| Supporting Documents        | Receipts, invoices    | [https://www.ird.govt.nz](https://www.ird.govt.nz) |
| myIR Submission             | Filing workflow       | [https://www.ird.govt.nz](https://www.ird.govt.nz) |
