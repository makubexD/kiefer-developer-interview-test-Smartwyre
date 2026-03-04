using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public Product? GetProduct(string productIdentifier) => productIdentifier switch
    {
        // Seed data — supports all three incentive types for console runner demos
        "product-01" => new Product
        {
            Identifier         = "product-01",
            Price              = 100m,
            Uom                = "kg",
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
                                | SupportedIncentiveType.FixedCashAmount
                                | SupportedIncentiveType.AmountPerUom
        },
        // Access database to retrieve product, code removed for brevity
        _ => new Product()
    };
}
