namespace Smartwyre.DeveloperTest.Types;

public class RebateCalculation
{
    public int Id { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public string RebateIdentifier { get; set; } = string.Empty;
    public IncentiveType IncentiveType { get; set; }
    public decimal Amount { get; set; }
}
