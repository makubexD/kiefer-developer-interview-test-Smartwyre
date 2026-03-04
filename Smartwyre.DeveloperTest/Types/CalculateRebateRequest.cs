namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateRequest
{
    public string RebateIdentifier { get; set; } = string.Empty;
    public string ProductIdentifier { get; set; } = string.Empty;
    public decimal Volume { get; set; }
}
