using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;
using Mapster;
using ReservationManager.Core.Services;
using Tests.EntityGenerators;

namespace Tests.Services
{
    [Trait("Category", "Unit")]
    public class ResourceTypeServiceShould
    {
        private readonly IResourceTypeRepository _mockResourceTypeRepository;
        private readonly IResourceService _mockResourceService;
        private readonly IResourceTypeService _sut;

        public ResourceTypeServiceShould()
        {
            _mockResourceTypeRepository = Substitute.For<IResourceTypeRepository>();
            _mockResourceService = Substitute.For<IResourceService>();
            _sut = new ResourceTypeService(_mockResourceTypeRepository, _mockResourceService);
        }

        [Fact]
        public async Task GetAllResourceTypes_ReturnsCorrectDtos()
        {
            var resourceTypes = new ResourceTypeGenerator().GenerateList();
            _mockResourceTypeRepository.GetAllTypesAsync().Returns(resourceTypes);

            var result = await _sut.GetAllResourceTypes();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Code.Should().Be("RT1");
            result.First().Name.Should().Be("Resource Type 1");
        }

        [Fact]
        public async Task UpdateResourceType_ReturnsUpdatedDto_WhenResourceTypeExists()
        {
            var existingResourceType = new ResourceTypeGenerator().GenerateSingle();
            var updatedDto = new UpsertResourceTypeDto { Code = "RT1", Name = "Updated Name" };
            _mockResourceTypeRepository.GetTypeById(1).Returns(existingResourceType);
            _mockResourceTypeRepository.UpdateTypeAsync(existingResourceType).Returns(existingResourceType);

            var result = await _sut.UpdateResourceType(1, updatedDto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Updated Name");
            existingResourceType.Name.Should().Be("Updated Name");
        }

        [Fact]
        public async Task UpdateResourceType_ReturnsNull_WhenResourceTypeDoesNotExist()
        {
            var updatedDto = new UpsertResourceTypeDto { Code = "RT1", Name = "Updated Name" };
            _mockResourceTypeRepository.GetTypeById(1).Returns((ResourceType)null);

            var result = await _sut.UpdateResourceType(1, updatedDto);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateResourceType_ReturnsCreatedDto()
        {
            var newResourceType = new ResourceTypeGenerator().GenerateSingle();
            var newResourceDto = newResourceType.Adapt<UpsertResourceTypeDto>();
            _mockResourceTypeRepository.CreateTypeAsync(Arg.Any<ResourceType>()).Returns(newResourceType);

            var result = await _sut.CreateResourceType(newResourceDto);

            result.Should().NotBeNull();
            result.Code.Should().Be("RT1");
            result.Name.Should().Be("Name");
        }

        [Fact]
        public async Task DeleteResourceType_ThrowsException_WhenResourceTypeDoesNotExist()
        {
            _mockResourceTypeRepository.GetTypeById(1).Returns((ResourceType)null);

            var act = async () => await _sut.DeleteResourceType(1);

            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage("Resource Type with id 1 not found");
        }

        [Fact]
        public async Task DeleteResourceType_ThrowsException_WhenResourcesExist()
        {
            var resourceType = new ResourceTypeGenerator().GenerateSingle();
            _mockResourceTypeRepository.GetTypeById(1).Returns(resourceType);
            _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
                .Returns(new List<ResourceDto> { new ResourceDto
                    {
                        Description = ""
                    }
                });

            var act = async () => await _sut.DeleteResourceType(1);

            await act.Should().ThrowAsync<DeleteNotPermittedException>()
                .WithMessage("Cannot delete RT1 because exits resources with this type");
        }

        [Fact]
        public async Task DeleteResourceType_DeletesResource_WhenNoResourcesExist()
        {
            var resourceType = new ResourceTypeGenerator().GenerateSingle();
            _mockResourceTypeRepository.GetTypeById(1).Returns(resourceType);
            _mockResourceService.GetFilteredResources(Arg.Any<ResourceFilterDto>())
                .Returns(Enumerable.Empty<ResourceDto>());

            await _sut.DeleteResourceType(1);

            await _mockResourceTypeRepository.Received(1).DeleteTypeAsync(resourceType);
        }
    }
}
