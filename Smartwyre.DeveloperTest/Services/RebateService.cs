using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService(
    IRebateDataStore rebateDataStore,
    IProductDataStore productDataStore,
    IEnumerable<IRebateCalculator> calculators) : IRebateService
{
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate is null || product is null)
            return new CalculateRebateResult { Success = false };

        var calculator = calculators.FirstOrDefault(c => c.CanCalculate(rebate, product, request));

        if (calculator is null)
            return new CalculateRebateResult { Success = false };

        var amount = calculator.Calculate(rebate, product, request);
        rebateDataStore.StoreCalculationResult(rebate, amount);

        return new CalculateRebateResult { Success = true };
    }
}
