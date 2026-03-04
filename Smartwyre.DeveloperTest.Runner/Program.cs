using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Types;

// Composition root: wire up all dependencies manually.
// No DI container is used — for a simple console runner, explicit wiring is clearer
// and easier to explain than adding a framework just to construct three objects.
var service = new RebateService(
    new RebateDataStore(),
    new ProductDataStore(),
    [
        new FixedCashAmountCalculator(),
        new FixedRateRebateCalculator(),
        new AmountPerUomCalculator()
    ]);

Console.WriteLine("=== Rebate Calculator ===");
Console.WriteLine();

Console.Write("Rebate identifier:  ");
var rebateId = Console.ReadLine() ?? string.Empty;

Console.Write("Product identifier: ");
var productId = Console.ReadLine() ?? string.Empty;

Console.Write("Volume:             ");
var volumeInput = Console.ReadLine();
if (!decimal.TryParse(volumeInput, out var volume))
{
    Console.WriteLine("Invalid volume. Must be a number.");
    return;
}

var result = service.Calculate(new CalculateRebateRequest
{
    RebateIdentifier = rebateId,
    ProductIdentifier = productId,
    Volume = volume
});

Console.WriteLine();
Console.WriteLine(result.Success
    ? "Result: Rebate calculation succeeded and was stored."
    : "Result: Rebate calculation failed. Check that the rebate and product are valid.");
