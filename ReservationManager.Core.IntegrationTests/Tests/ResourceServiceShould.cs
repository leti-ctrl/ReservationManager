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
public class ResourceServiceShould
{
    private readonly IResourceService _sut;
    private readonly IResourceRepository _resourceRepository;

    public ResourceServiceShould()
    {
        _resourceRepository = GetResourceRepository();
        _sut = GetResourceService();
    }
    
    [Test]
    public async Task ReturnSortedResources_WhenResourcesExist()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resourceToIns = new Resource(){ TypeId = resourceType.Id, Description = "Test Resoruce Description" };
        var resource = await GetResourceRepository().CreateEntityAsync(resourceToIns);
        
        var result = await _sut.GetAllResources();

        result.Count().Should().BeGreaterThanOrEqualTo(1);
        result.Should().ContainSingle(x => x.Id == resource.Id);
        result.Single(x => x.Id == resource.Id).Description.Should().Be(resource.Description);
        result.Single(x => x.Id == resource.Id).Type.Id.Should().Be(resourceType.Id);
    }
    
    [Test]
    public async Task DelegateToResourceFilterService_OnGetFilteredResource_WhenResourceFilterHasResourceId()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resourceToIns = new Resource(){ TypeId = resourceType.Id, Description = "Test Resoruce Description" };
        var resource = await GetResourceRepository().CreateEntityAsync(resourceToIns);
        var filterDto = new ResourceFilterDto() { ResourceId = resource.Id };
        
    
        var result = await _sut.GetFilteredResources(filterDto);
    
        result.Should().ContainSingle(x => x.Id == resource.Id);
    }
    
    [Test]
    public async Task DelegateToResourceFilterService_OnGetFilteredResource_WhenResourceFilterHasResourceTypeId()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resourceToIns = new Resource(){ TypeId = resourceType.Id, Description = "Test Resoruce Description" };
        var resource = await GetResourceRepository().CreateEntityAsync(resourceToIns);
        var filterDto = new ResourceFilterDto() { TypeId = resourceType.Id };
        
    
        var result = await _sut.GetFilteredResources(filterDto);
    
        result.Should().AllSatisfy(x => x.Type.Id.Should().Be(resourceType.Id));
    }
    
    [Test]
    public async Task ThrowInvalidCodeTypeException_OnCreate_WhenResourceTypeIsInvalid()
    {
        var resource = new UpsertResourceDto() { TypeId = 999, Description = "Test Resoruce Description" };
    
        var act = async () => await _sut.CreateResource(resource);
    
        await act.Should()
            .ThrowAsync<InvalidCodeTypeException>()
            .WithMessage($"Resource type {resource.TypeId} is not valid");
    }
    
    [Test]
    public async Task CreateResource_WhenValidDataIsProvided()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resource = new UpsertResourceDto() { TypeId = resourceType.Id, Description = "Test Resoruce Description" };
    
        var result = await _sut.CreateResource(resource);
    
        result.Should().NotBeNull();
        result.Description.Should().Be("Test Resoruce Description");
    }
    
    [Test]
    public async Task ReturnNull_OnUpdate_WhenResourceTypeIsInvalid()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resourceToIns = new Resource(){ TypeId = resourceType.Id, Description = "Test Resoruce Description" };
        var resource = await GetResourceRepository().CreateEntityAsync(resourceToIns);
        var toUpdate = new UpsertResourceDto() { TypeId = 9999, Description = "Updated Resource" };

    
        var result = await _sut.UpdateResource(resource.Id, toUpdate);
    
        result.Should().BeNull();
    }
    
    [Test]
    public async Task ReturnNull_OnUpdate_WhenResourceIdDoesNotExist()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var toUpdate = new UpsertResourceDto() { TypeId = resourceType.Id, Description = "Updated Resource" };

    
        var result = await _sut.UpdateResource(9999, toUpdate);
    
        result.Should().BeNull();
    }
    
    [Test]
    public async Task UpdateResource_WhenValidIdAndTypeAreProvided()
    {
        var resourceTypeToIns = new ResourceType() { Code = "TEST", Name = "Test Resoruce Type" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resourceToIns = new Resource(){ TypeId = resourceType.Id, Description = "Test Resoruce Description" };
        var resource = await GetResourceRepository().CreateEntityAsync(resourceToIns);
        var toUpdate = new UpsertResourceDto() { TypeId = resourceType.Id, Description = "Updated Resource" };
    
        var result = await _sut.UpdateResource(resource.Id, toUpdate);
        
        result.Should().NotBeNull();
        result.Id.Should().Be(resource.Id);
        result.Description.Should().Be("Updated Resource");
    }
    
    [Test]
    public async Task ThrowEntityNotFoundException_OnDelete_WhenResourceDoesNotExist()
    {
        var act = async () => await _sut.DeleteResource(9999);
    
        await act.Should()
            .ThrowAsync<EntityNotFoundException>()
            .WithMessage("Resource with id 9999 not found");
    }
    
    [Test]
    public async Task DeleteResource_WhenNoReservationsExist()
    {
        var resourceTypeToIns = new ResourceType() { Code = "RTD", Name = "Test Resoruce Type to delete" };
        var resourceType = await GetResourceTypeRepository().CreateTypeAsync(resourceTypeToIns);
        var resourceToIns = new Resource(){ TypeId = resourceType.Id, Description = "Test Resoruce to delete" };
        var resource = await GetResourceRepository().CreateEntityAsync(resourceToIns);
        
    
        await _sut.DeleteResource(resource.Id);
    
        var result = await _sut.GetAllResources();
        result.Should().NotContain(x => x.Id == resource.Id);
    }
}