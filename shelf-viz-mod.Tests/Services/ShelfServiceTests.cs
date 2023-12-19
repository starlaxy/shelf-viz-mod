using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using shelf_viz_mod.Data.Models;
using shelf_viz_mod.Data.Services;
using System.Collections.Generic;
using System.Linq;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Net.Http.Json;

[TestClass]
public class ShelfServiceTests
{
    private Mock<IHttpClientFactory>? _httpClientFactoryMock;
    private Mock<ILocalStorageService>? _localStorageMock;
    private Mock<ILogger<ShelfService>>? _loggerMock;
    private Mock<IScopedServiceFactory>? _scopedServiceFactoryMock;
    private ShelfService? _shelfService;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _localStorageMock = new Mock<ILocalStorageService>();
        _loggerMock = new Mock<ILogger<ShelfService>>();
        _scopedServiceFactoryMock = new Mock<IScopedServiceFactory>();

        _scopedServiceFactoryMock.Setup(f => f.GetScopedService<ILocalStorageService>()).Returns(_localStorageMock.Object);

        _shelfService = new ShelfService(_httpClientFactoryMock.Object, _loggerMock.Object, _scopedServiceFactoryMock.Object);
    }

    [TestMethod]
    public async Task InitializeAsync_LoadsDataSuccessfully()
    {
        // Arrange
        var sampleData = new List<Cabinet> { new Cabinet { Number = 1 } };
        _localStorageMock.Setup(s => s.GetItemAsync<List<Cabinet>>("shelfLayout", default)).ReturnsAsync(sampleData);

        // Act
        await _shelfService.InitializeAsync();
        var actualCabinets = await _shelfService.GetAllCabinetsAsync().FirstAsync();

        // Assert
        Assert.AreEqual(sampleData.Count, actualCabinets.Count(), "Cabinets count should match.");
    }

    [TestMethod]
    public async Task GetAllCabinetsAsync_ReturnsCabinets()
    {
        // Arrange
        var expectedCabinets = new List<Cabinet> { new Cabinet { /* Initialize with test data */ }, new Cabinet { /* Initialize with test data */ } };
        _localStorageMock.Setup(s => s.GetItemAsync<List<Cabinet>>("shelfLayout", default)).ReturnsAsync(expectedCabinets);

        // Act
        var cabinetsObservable = _shelfService.GetAllCabinetsAsync();
        var actualCabinets = await cabinetsObservable.FirstAsync();

        // Assert
        Assert.IsNotNull(actualCabinets, "Cabinets should not be null.");
        Assert.AreEqual(expectedCabinets.Count, actualCabinets.Count(), "The count of cabinets should match the expected count.");
        CollectionAssert.AreEquivalent(expectedCabinets, actualCabinets.ToList(), "The actual cabinets should match the expected cabinets.");
    }

    // Additional test methods for other functionalities like SwapLanesAsync, DeleteCabinetAsync, etc.

    // Example: Testing SwapLanesAsync
    [TestMethod]
    public async Task SwapLanesAsync_SwapsLanesCorrectly()
    {
        // Arrange
        var cabinets = new List<Cabinet> { /* Initialize with test data including lanes */ };
        _localStorageMock.Setup(s => s.GetItemAsync<List<Cabinet>>("shelfLayout", default)).ReturnsAsync(cabinets);


    }

    // ... More test methods for other methods like DeleteCabinetAsync, UpdateRowAsync, etc.

}
