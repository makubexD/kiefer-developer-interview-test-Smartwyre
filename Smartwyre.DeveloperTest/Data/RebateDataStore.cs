using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    public Rebate? GetRebate(string rebateIdentifier) => rebateIdentifier switch
    {
        // Seed data — demonstrates each incentive type via the console runner
        "rebate-fixed-rate" => new Rebate { Identifier = "rebate-fixed-rate", Incentive = IncentiveType.FixedRateRebate, Percentage = 0.10m },
        "rebate-fixed-cash" => new Rebate { Identifier = "rebate-fixed-cash", Incentive = IncentiveType.FixedCashAmount, Amount = 50m },
        "rebate-per-uom"    => new Rebate { Identifier = "rebate-per-uom",    Incentive = IncentiveType.AmountPerUom,    Amount = 5m },
        // Access database to retrieve rebate, code removed for brevity
        _                   => new Rebate()
    };

    public void StoreCalculationResult(Rebate rebate, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
