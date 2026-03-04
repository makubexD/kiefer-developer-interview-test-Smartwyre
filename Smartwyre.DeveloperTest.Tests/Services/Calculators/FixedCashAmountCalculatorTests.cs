using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Tests.TestData;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services.Calculators;

public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _sut = new();

    [Fact]
    public void CanCalculate_ShouldReturnTrue_WhenAllConditionsAreMet()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedCashAmount);
        var request = TestDataBuilder.CreateRequest();

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.True(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenIncentiveTypeDoesNotMatch()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedCashAmount);
        var request = TestDataBuilder.CreateRequest();

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenProductDoesNotSupportFixedCashAmount()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedRateRebate);
        var request = TestDataBuilder.CreateRequest();

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenRebateAmountIsZero()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate(amount: 0m);
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedCashAmount);
        var request = TestDataBuilder.CreateRequest();

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void Calculate_ShouldReturnRebateAmount()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate(amount: 250m);
        var product = TestDataBuilder.CreateProduct();
        var request = TestDataBuilder.CreateRequest();

        var result = _sut.Calculate(rebate, product, request);

        Assert.Equal(250m, result);
    }
}
