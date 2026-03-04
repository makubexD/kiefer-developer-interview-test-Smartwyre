using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

/// <summary>
/// Defines a self-contained unit of rebate calculation logic for a specific incentive type.
/// Implementors decide both whether they apply to a given request (CanCalculate)
/// and how to compute the rebate amount (Calculate).
/// </summary>
public interface IRebateCalculator
{
    /// <summary>
    /// Returns true if this calculator can handle the given rebate, product, and request.
    /// Checks incentive type match, product eligibility flags, and required non-zero values.
    /// </summary>
    bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request);

    /// <summary>
    /// Computes the rebate amount. Precondition: CanCalculate returned true.
    /// </summary>
    decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
}
