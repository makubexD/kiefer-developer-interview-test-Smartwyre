using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Calculators;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Incentive == IncentiveType.FixedCashAmount &&
        product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) &&
        rebate.Amount > 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequest request) =>
        rebate.Amount;
}
