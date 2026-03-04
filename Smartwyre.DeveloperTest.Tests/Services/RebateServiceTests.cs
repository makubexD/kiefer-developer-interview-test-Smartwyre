using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.Calculators;
using Smartwyre.DeveloperTest.Tests.TestData;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services;

public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateStore = new();
    private readonly Mock<IProductDataStore> _productStore = new();
    private readonly Mock<IRebateCalculator> _calculator = new();
    private readonly RebateService _sut;

    public RebateServiceTests()
    {
        _sut = new RebateService(_rebateStore.Object, _productStore.Object, [_calculator.Object]);
    }

    [Fact]
    public void Calculate_ShouldReturnFailure_WhenRebateIsNull()
    {
        _rebateStore.Setup(s => s.GetRebate(It.IsAny<string>())).Returns((Rebate?)null);
        _productStore.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(TestDataBuilder.CreateProduct());

        var result = _sut.Calculate(TestDataBuilder.CreateRequest());

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ShouldReturnFailure_WhenProductIsNull()
    {
        _rebateStore.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(TestDataBuilder.CreateFixedCashRebate());
        _productStore.Setup(s => s.GetProduct(It.IsAny<string>())).Returns((Product?)null);

        var result = _sut.Calculate(TestDataBuilder.CreateRequest());

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ShouldReturnFailure_WhenNoCalculatorCanHandle()
    {
        _rebateStore.Setup(s => s.GetRebate(It.IsAny<string>())).Returns(TestDataBuilder.CreateFixedCashRebate());
        _productStore.Setup(s => s.GetProduct(It.IsAny<string>())).Returns(TestDataBuilder.CreateProduct());
        _calculator.Setup(c => c.CanCalculate(It.IsAny<Rebate>(), It.IsAny<Product>(), It.IsAny<CalculateRebateRequest>())).Returns(false);

        var result = _sut.Calculate(TestDataBuilder.CreateRequest());

        Assert.False(result.Success);
    }

    [Fact]
    public void Calculate_ShouldReturnSuccess_WhenCalculatorHandlesRequest()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate();
        var product = TestDataBuilder.CreateProduct();
        var request = TestDataBuilder.CreateRequest();
        _rebateStore.Setup(s => s.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productStore.Setup(s => s.GetProduct(request.ProductIdentifier)).Returns(product);
        _calculator.Setup(c => c.CanCalculate(rebate, product, request)).Returns(true);
        _calculator.Setup(c => c.Calculate(rebate, product, request)).Returns(100m);

        var result = _sut.Calculate(request);

        Assert.True(result.Success);
    }

    [Fact]
    public void Calculate_ShouldStoreResult_WhenCalculationSucceeds()
    {
        var rebate = TestDataBuilder.CreateFixedCashRebate();
        var product = TestDataBuilder.CreateProduct();
        var request = TestDataBuilder.CreateRequest();
        _rebateStore.Setup(s => s.GetRebate(request.RebateIdentifier)).Returns(rebate);
        _productStore.Setup(s => s.GetProduct(request.ProductIdentifier)).Returns(product);
        _calculator.Setup(c => c.CanCalculate(rebate, product, request)).Returns(true);
        _calculator.Setup(c => c.Calculate(rebate, product, request)).Returns(100m);

        _sut.Calculate(request);

        _rebateStore.Verify(s => s.StoreCalculationResult(rebate, 100m), Times.Once());
    }
}
