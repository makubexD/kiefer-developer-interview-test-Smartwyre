using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class AmountPerUomCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Incentive == IncentiveType.AmountPerUom &&
        product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) &&
        rebate.Amount > 0 &&
        request.Volume > 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Amount * request.Volume;
}
