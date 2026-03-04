using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Incentive == IncentiveType.FixedRateRebate &&
        product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) &&
        rebate.Percentage > 0 &&
        product.Price > 0 &&
        request.Volume > 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        product.Price * rebate.Percentage * request.Volume;
}
