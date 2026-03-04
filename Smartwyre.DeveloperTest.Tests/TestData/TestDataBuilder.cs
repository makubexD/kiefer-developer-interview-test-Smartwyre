using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests.TestData;

internal static class TestDataBuilder
{
    internal static Rebate CreateFixedCashRebate(decimal amount = 100m) => new()
    {
        Identifier = "R1",
        Incentive = IncentiveType.FixedCashAmount,
        Amount = amount,
        Percentage = 0m
    };

    internal static Rebate CreateFixedRateRebate(decimal percentage = 0.10m) => new()
    {
        Identifier = "R2",
        Incentive = IncentiveType.FixedRateRebate,
        Amount = 0m,
        Percentage = percentage
    };

    internal static Rebate CreateAmountPerUomRebate(decimal amount = 5m) => new()
    {
        Identifier = "R3",
        Incentive = IncentiveType.AmountPerUom,
        Amount = amount,
        Percentage = 0m
    };

    internal static Product CreateProduct(
        SupportedIncentiveType supportedIncentives = SupportedIncentiveType.FixedCashAmount,
        decimal price = 50m) => new()
    {
        Identifier = "P1",
        Price = price,
        Uom = "kg",
        SupportedIncentives = supportedIncentives
    };

    internal static CalculateRebateRequest CreateRequest(
        string rebateId = "R1",
        string productId = "P1",
        decimal volume = 10m) => new()
    {
        RebateIdentifier = rebateId,
        ProductIdentifier = productId,
        Volume = volume
    };
}
