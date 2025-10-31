using Moq;
using YarpEe.Application.Ports;
using YarpEe.Application.UseCases.Clusters;
using YarpEe.Domain.Entities;

namespace YarpEe.Application.Tests.UseCases.Clusters;

public class CreateClusterTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidData_CreatesCluster()
    {
        // Arrange
        var mockClusterRepo = new Mock<IClusterRepository>();
        var mockProxyReloader = new Mock<IProxyConfigReloader>();
        
        mockClusterRepo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cluster?)null);
        
        var useCase = new CreateCluster(mockClusterRepo.Object, mockProxyReloader.Object);
        var request = new CreateCluster.Request("TestCluster", "RoundRobin");

        // Act
        var result = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestCluster", result.Name);
        Assert.Equal("RoundRobin", result.LoadBalancingPolicy);
        
        mockClusterRepo.Verify(r => r.AddAsync(It.IsAny<Cluster>(), It.IsAny<CancellationToken>()), Times.Once);
        mockProxyReloader.Verify(r => r.ReloadAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithDuplicateName_ThrowsException()
    {
        // Arrange
        var existingCluster = new Cluster(Guid.NewGuid(), "TestCluster");
        var mockClusterRepo = new Mock<IClusterRepository>();
        var mockProxyReloader = new Mock<IProxyConfigReloader>();
        
        mockClusterRepo.Setup(r => r.GetByNameAsync("TestCluster", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCluster);
        
        var useCase = new CreateCluster(mockClusterRepo.Object, mockProxyReloader.Object);
        var request = new CreateCluster.Request("TestCluster");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => useCase.ExecuteAsync(request));
        
        mockClusterRepo.Verify(r => r.AddAsync(It.IsAny<Cluster>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithDestinations_CreatesClusterWithDestinations()
    {
        // Arrange
        var mockClusterRepo = new Mock<IClusterRepository>();
        var mockProxyReloader = new Mock<IProxyConfigReloader>();
        
        mockClusterRepo.Setup(r => r.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cluster?)null);
        
        var useCase = new CreateCluster(mockClusterRepo.Object, mockProxyReloader.Object);
        var destinations = new[]
        {
            new CreateCluster.DestinationRequest("http://localhost:5001", "/health"),
            new CreateCluster.DestinationRequest("http://localhost:5002")
        };
        var request = new CreateCluster.Request("TestCluster", "RoundRobin", destinations);

        // Act
        var result = await useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Destinations.Count);
        
        mockClusterRepo.Verify(r => r.AddAsync(It.Is<Cluster>(c => c.Destinations.Count == 2), It.IsAny<CancellationToken>()), Times.Once);
    }
}
