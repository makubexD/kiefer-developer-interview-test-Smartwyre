using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Tests.TestData;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services.Calculators;

public class AmountPerUomCalculatorTests
{
    private readonly AmountPerUomCalculator _sut = new();

    [Fact]
    public void CanCalculate_ShouldReturnTrue_WhenAllConditionsAreMet()
    {
        var rebate = TestDataBuilder.CreateAmountPerUomRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.AmountPerUom);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.True(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenIncentiveTypeDoesNotMatch()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.AmountPerUom);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenProductDoesNotSupportAmountPerUom()
    {
        var rebate = TestDataBuilder.CreateAmountPerUomRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedCashAmount);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenRebateAmountIsZero()
    {
        var rebate = TestDataBuilder.CreateAmountPerUomRebate(amount: 0m);
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.AmountPerUom);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenVolumeIsZero()
    {
        var rebate = TestDataBuilder.CreateAmountPerUomRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.AmountPerUom);
        var request = TestDataBuilder.CreateRequest(volume: 0m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void Calculate_ShouldReturnAmountTimesVolume()
    {
        var rebate = TestDataBuilder.CreateAmountPerUomRebate(amount: 5m);
        var product = TestDataBuilder.CreateProduct();
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.Calculate(rebate, product, request);

        Assert.Equal(50m, result);
    }
}
