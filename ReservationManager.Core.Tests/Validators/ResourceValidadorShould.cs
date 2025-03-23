using NSubstitute;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class ResourceValidadorShould
{
    private readonly ResourceValidator _sut;
    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly IResourceRepository _resourceRepository;

    public ResourceValidadorShould()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();
        _resourceTypeRepository = Substitute.For<IResourceTypeRepository>();
        
        _sut = new ResourceValidator(_resourceTypeRepository, _resourceRepository);
    }

    [Fact]
    private async Task ReturnTrue_ValidateResourceType_WhenResourceTypeExists()
    {
        var resourceTypeId = 1;
        var resourceTypeCode = "ResourceTypeCode";
        var resourceType = new ResourceType
        {
            Id = resourceTypeId,
            Code = resourceTypeCode
        };
        _resourceTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns(resourceType);
        
        var result = await _sut.ValidateResourceType(resourceTypeId);
        
        Assert.True(result);
    }
    
    [Fact]
    private async Task ReturnFalse_ValidateResourceType_WhenResourceTypeNotExists()
    {
        var resourceTypeId = 1;
        _resourceTypeRepository.GetTypeById(Arg.Any<int>())
            .Returns((ResourceType)null!);
        
        var result = await _sut.ValidateResourceType(resourceTypeId);
        
        Assert.False(result);
    }

    [Fact]
    private async Task ReturnTrue_ExistingResouceId_WhenResourceIdExists()
    {
        var resourceId = 1;
        var resource = new Resource { Id = resourceId };
        _resourceRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns(resource);
        
        var result = await _sut.ExistingResouceId(resourceId);
        
        Assert.True(result);
    }
    
    [Fact]
    private async Task ReturnFalse_ExistingResouceId_WhenResourceIdNotExists()
    {
        var resourceId = 1;
        _resourceRepository.GetEntityByIdAsync(Arg.Any<int>())
            .Returns((Resource)null!);
        
        var result = await _sut.ExistingResouceId(resourceId);
        
        Assert.False(result);
    }
}