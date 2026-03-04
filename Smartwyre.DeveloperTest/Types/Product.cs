namespace Smartwyre.DeveloperTest.Types;

public class Product
{
    public int Id { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Uom { get; set; } = string.Empty;
    public SupportedIncentiveType SupportedIncentives { get; set; }
}
