using FluentAssertions;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.IntegrationTests.Tests;
using static Setup;

[TestFixture]
[Category("Integration")]
public class ResourceTypeServiceShould
{
    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly IResourceTypeService _sut;

    public ResourceTypeServiceShould()
    {
        _resourceTypeRepository = GetResourceTypeRepository();
        _sut = GetResourceTypeService();
    }

    [Test]
    public async Task ReturnsCorrectDtos_OnGetAll()
    {
        var resourceTypeToIns = new ResourceType() {Code = "RM", Name = "Room"};
        var resourceType = await _resourceTypeRepository.CreateTypeAsync(resourceTypeToIns);

        var result = await _sut.GetAllResourceTypes();

        result.Should().NotBeNull();
        result.Count().Should().BeGreaterThan(0);
        result.Should().ContainSingle(x => x.Id == resourceType.Id);
        result.Should().ContainSingle(x => x.Id == resourceType.Id && x.Code == "RM");
        result.Should().ContainSingle(x => x.Id == resourceType.Id && x.Name == "Room");
    }

    [Test]
    public async Task ReturnsUpdatedDto_OnUpdate_WhenResourceTypeExists()
    {
        var resourceTypeToIns = new ResourceType() {Code = "RM", Name = "Room"};
        var resourceType = await _resourceTypeRepository.CreateTypeAsync(resourceTypeToIns);
        var toUpdate = new UpsertResourceTypeDto() {Code = "RT1", Name = "Updated Name"};
       

        var result = await _sut.UpdateResourceType(resourceType.Id, toUpdate);

        result.Should().NotBeNull();
        result. Name.Should().Be("Updated Name");
        result.Code.Should().Be("RT1");
        result.Id.Should().Be(resourceType.Id);
    }

    [Test]
    public async Task ReturnsNull_OnUpdate_WhenResourceTypeDoesNotExist()
    {
        var resourceTypeToIns = new ResourceType() {Code = "RM", Name = "Room"};
        var resourceType = await _resourceTypeRepository.CreateTypeAsync(resourceTypeToIns);
        var toUpdate = new UpsertResourceTypeDto() {Code = "RT1", Name = "Updated Name"};
        
        var result = await _sut.UpdateResourceType(resourceType.Id + 999, toUpdate);
    
        result.Should().BeNull();
    }
    
    [Test]
    public async Task ReturnsCreatedDto_OnCreate_WhenValidDataIsPassed()
    {
        var resourceTypeToIns = new UpsertResourceTypeDto() {Code = "RM", Name = "Room"};
        
        var result = await _sut.CreateResourceType(resourceTypeToIns);
    
        result.Should().NotBeNull();
        result.Code.Should().Be("RM");
        result.Name.Should().Be("Room");
        result.Id.Should().BeGreaterThan(0);
    }
    
    [Test]
    public async Task ThrowsException_OnDelete_WhenResourceTypeDoesNotExist()
    {
        var act = async () => await _sut.DeleteResourceType(999);
    
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage("Resource Type with id 999 not found");
    }
    
    [Test]
    public async Task ThrowsException_OnDelete_WhenResourcesExist()
    {
        var resourceTypeToIns = new ResourceType() {Code = "RM", Name = "Room"};
        var resourceType = await _resourceTypeRepository.CreateTypeAsync(resourceTypeToIns);
        var resource = new Resource()
        {
            Description = "Meeting room",
            TypeId = resourceType.Id,
        };
        await GetResourceRepository().CreateEntityAsync(resource);
    
        var act = async () => await _sut.DeleteResourceType(resourceType.Id);
    
        await act.Should().ThrowAsync<DeleteNotPermittedException>()
            .WithMessage($"Cannot delete {resourceType.Code} because exits resources with this type");
    }
    
    [Test]
    public async Task DeletesResourceType_OnDelete_WhenNoResourcesExist()
    {
        var resourceTypeToIns = new ResourceType() {Code = "RM", Name = "Room"};
        var resourceType = await _resourceTypeRepository.CreateTypeAsync(resourceTypeToIns);
    
        await _sut.DeleteResourceType(resourceTypeToIns.Id);
    
        var deleted = await _resourceTypeRepository.GetTypeById(resourceType.Id);
        deleted.Should().BeNull();
    }
}