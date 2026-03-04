using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Tests.TestData;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services.Calculators;

public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator _sut = new();

    [Fact]
    public void CanCalculate_ShouldReturnTrue_WhenAllConditionsAreMet()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 50m);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.True(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenIncentiveTypeDoesNotMatch()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 50m);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenProductDoesNotSupportFixedRateRebate()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedCashAmount, price: 50m);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenPercentageIsZero()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate(percentage: 0m);
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 50m);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenProductPriceIsZero()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 0m);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void CanCalculate_ShouldReturnFalse_WhenVolumeIsZero()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate();
        var product = TestDataBuilder.CreateProduct(SupportedIncentiveType.FixedRateRebate, price: 50m);
        var request = TestDataBuilder.CreateRequest(volume: 0m);

        var result = _sut.CanCalculate(rebate, product, request);

        Assert.False(result);
    }

    [Fact]
    public void Calculate_ShouldReturnPriceTimesPercentageTimesVolume()
    {
        var rebate = TestDataBuilder.CreateFixedRateRebate(percentage: 0.10m);
        var product = TestDataBuilder.CreateProduct(price: 50m);
        var request = TestDataBuilder.CreateRequest(volume: 10m);

        var result = _sut.Calculate(rebate, product, request);

        Assert.Equal(50m, result);
    }
}
