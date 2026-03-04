# Smartwyre Developer Test Instructions

You have been selected to complete our candidate coding exercise. Please follow the directions in this readme.

Clone, **DO NOT FORK**, this repository to your account on the online Git resource of your choosing (GitHub, BitBucket, GitLab, etc.). Your solution should retain previous commit history and you should utilize best practices for committing your changes to the repository.

You are welcome to use whatever tools you normally would when coding — including documentation, libraries, frameworks, or AI tools (such as ChatGPT or Copilot).

However, it is important that you fully understand your solution. As part of the interview process, we will review your code with you in detail. You should be able to:

- Explain the design choices you made.
- Walk us through how your solution works.
- Make modifications or extensions to your code during the review.

Please note: if your submission appears to have been generated entirely by an AI agent or another third party, without your own understanding or contribution, it will not meet our evaluation criteria.

# The Exercise

In the 'RebateService.cs' file you will find a method for calculating a rebate. At a high level the steps for calculating a rebate are:

 1. Lookup the rebate that the request is being made against.
 2. Lookup the product that the request is being made against.
 2. Check that the rebate and request are valid to calculate the incentive type rebate.
 3. Store the rebate calculation.

What we'd like you to do is refactor the code with the following things in mind:

 - Adherence to SOLID principles
 - Testability
 - Readability
 - Currently there are 3 known incentive types. In the future the business will want to add many more incentive types. Your solution should make it easy for developers to add new incentive types in the future.

We’d also like you to 
 - Add some unit tests to the Smartwyre.DeveloperTest.Tests project to show how you would test the code that you’ve produced 
 - Run the RebateService from the Smartwyre.DeveloperTest.Runner console application accepting inputs (either via command line arguments or via prompts is fine)

The only specific "rules" are:

- The solution must build
- All tests must pass

You are free to use any frameworks/NuGet packages that you see fit. You should plan to spend around 1 hour completing the exercise.

Feel free to use code comments to describe your changes. You are also welcome to update this readme with any important details for us to consider.

Once you have completed the exercise either ensure your repository is available publicly or contact the hiring manager to set up a private share.

---

## Solution Summary

The refactoring follows SOLID principles across 5 staged feature branches:

| Stage | Change |
|-------|--------|
| 1 — Data interfaces | Extracted `IRebateDataStore` / `IProductDataStore`; enabled nullable reference types |
| 2 — Strategy pattern | `IRebateCalculator` with `CanCalculate()` + `Calculate()` — one class per incentive type |
| 3 — Refactored service | Constructor DI (C# 12); replaced switch with `FirstOrDefault`; guard clauses |
| 4 — Unit tests | 23 xUnit tests across all calculators and service edge cases |
| 5 — Console runner | Interactive prompts with manual composition root in `Program.cs` |

Adding a new incentive type requires only a new `IRebateCalculator` class — `RebateService` never changes.

---

## Usage

```bash
dotnet build
dotnet test
dotnet run --project Smartwyre.DeveloperTest.Runner
```

The runner prompts for three inputs interactively:

```
=== Rebate Calculator ===

Rebate identifier:  R001
Product identifier: P001
Volume:             100

Result: Rebate calculation failed. Check that the rebate and product are valid.
```

```
=== Rebate Calculator ===

Rebate identifier:  R001
Product identifier: P001
Volume:             abc

Invalid volume. Must be a number.
```

```
=== Rebate Calculator ===

Rebate identifier:  R001
Product identifier: P001
Volume:             0

Result: Rebate calculation failed. Check that the rebate and product are valid.
```

```
=== Rebate Calculator ===

Rebate identifier:  R001
Product identifier: P001
Volume:             -50

Result: Rebate calculation failed. Check that the rebate and product are valid.
```

```
=== Rebate Calculator ===

Rebate identifier:
Product identifier:
Volume:             100

Result: Rebate calculation failed. Check that the rebate and product are valid.
```

> **Note:** The data stores (`RebateDataStore`, `ProductDataStore`) are intentional stubs — they
> return empty objects regardless of the identifier. All valid-input paths currently result in
> failure because the stub rebate/product have zero values that fail the `CanCalculate()` guards
> (`Volume > 0`, `Price > 0`, `Percentage > 0`, `SupportedIncentives` flag). A real implementation
> would query a database and succeed when a valid rebate/product pair is found.

### Happy path — seed data

The data stores include hardcoded seed identifiers to demonstrate each incentive type end-to-end.
Use `product-01` as the product identifier for all three:

**FixedRateRebate** — `Price(100) × Percentage(0.10) × Volume = result`

```
=== Rebate Calculator ===

Rebate identifier:  rebate-fixed-rate
Product identifier: product-01
Volume:             10

Result: Rebate calculation succeeded and was stored.
```

**FixedCashAmount** — flat `Amount = 50` regardless of volume

```
=== Rebate Calculator ===

Rebate identifier:  rebate-fixed-cash
Product identifier: product-01
Volume:             10

Result: Rebate calculation succeeded and was stored.
```

**AmountPerUom** — `Amount(5) × Volume = result`

```
=== Rebate Calculator ===

Rebate identifier:  rebate-per-uom
Product identifier: product-01
Volume:             20

Result: Rebate calculation succeeded and was stored.
```

---

## Coverage

```powershell
.\Scripts\ExecCoverage.ps1                      # terminal table
.\Scripts\ExecCoverage.ps1 -Quiet               # summary only
.\Scripts\ExecCoverage.ps1 -GenerateHtmlReport  # HTML report → TestResults\HtmlReport\index.html
```
