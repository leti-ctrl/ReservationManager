using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using ReservationManager.Core.Services;
using Tests.EntityGenerators;
using Xunit;

namespace Tests.Services
{
    [Trait("Category", "Unit")]
    public class ResourceServiceShould
    {
        private readonly IResourceRepository _mockResourceRepository;
        private readonly IResourceValidator _mockResourceValidator;
        private readonly IReservationRepository _mockReservationRepository;
        private readonly IResourceFilterService _mockResourceFilterService;
        private readonly IResourceService _sut;


        public ResourceServiceShould()
        {
            _mockResourceRepository = Substitute.For<IResourceRepository>();
            _mockResourceValidator = Substitute.For<IResourceValidator>();
            _mockReservationRepository = Substitute.For<IReservationRepository>();
            _mockResourceFilterService = Substitute.For<IResourceFilterService>();

            _sut = new ResourceService(
                _mockResourceRepository,
                _mockResourceValidator,
                _mockReservationRepository,
                _mockResourceFilterService
            );
        }

        [Fact]
        public async Task ReturnEmptyList_WhenNoResourcesExist()
        {
            _mockResourceRepository.GetAllEntitiesAsync()
                .Returns(Enumerable.Empty<Resource>());

            var result = await _sut.GetAllResources();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnSortedResources_WhenResourcesExist()
        {
            var resources = new ResourceGenerator().CreateResourceList(2);
            _mockResourceRepository.GetAllEntitiesAsync()
                .Returns(resources);

            var result = await _sut.GetAllResources();

            result.Should().HaveCount(2);
            result.First().Type.Code.Should().Be("T1");
            result.Last().Type.Code.Should().Be("T2");
        }

        [Fact]
        public async Task DelegateToResourceFilterService_WhenGetFilteredResourcesIsCalled()
        {
            var filterDto = new ResourceFilterDtoGenerator().CreateValidFilter();
            var resources = new List<ResourceDto>
            {
                new ResourceDto { Id = 1, Description = "Resource 1", Type = new ResourceTypeDto { Code = "T1" } }
            };
            _mockResourceFilterService.GetFilteredResources(filterDto)
                .Returns(resources);

            var result = await _sut.GetFilteredResources(filterDto);

            result.Should().BeEquivalentTo(resources);
            await _mockResourceFilterService.Received(1).GetFilteredResources(filterDto);
        }

        [Fact]
        public async Task ThrowInvalidCodeTypeException_WhenResourceTypeIsInvalid()
        {
            var resourceDto = new UpsertResourceDtoGenerator().Generate(999, "");
            _mockResourceValidator.ValidateResourceType(999)
                .Returns(false);

            var act = async () => await _sut.CreateResource(resourceDto);

            await act.Should()
                .ThrowAsync<InvalidCodeTypeException>()
                .WithMessage("Resource type 999 is not valid");
        }

        [Fact]
        public async Task CreateResource_WhenValidDataIsProvided()
        {
            var resourceDto = new UpsertResourceDtoGenerator().Generate(1, "New Resource");
            _mockResourceValidator.ValidateResourceType(1)
                .Returns(true);
            _mockResourceRepository.CreateEntityAsync(Arg.Any<Resource>())
                .Returns(resourceDto.Adapt<Resource>());

            var result = await _sut.CreateResource(resourceDto);

            result.Should().NotBeNull();
            result.Description.Should().Be("New Resource");
        }

        [Fact]
        public async Task ReturnNull_WhenResourceTypeIsInvalidOrIdDoesNotExist()
        {
            var resourceDto = new UpsertResourceDtoGenerator().Generate(1,"Updated Resource");
            _mockResourceValidator.ValidateResourceType(1)
                .Returns(true);
            _mockResourceValidator.ExistingResouceId(1)
                .Returns(false); 

            var result = await _sut.UpdateResource(1, resourceDto);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateResource_WhenValidIdAndTypeAreProvided()
        {
            var resourceDto = new UpsertResourceDtoGenerator().Generate(1, "Updated Resource");
            var resource = resourceDto.Adapt<Resource>();
            resource.Id = 1;
            _mockResourceValidator.ValidateResourceType(1)
                .Returns(true);
            _mockResourceValidator.ExistingResouceId(resource.Id)
                .Returns(true); // Valid ID
            _mockResourceRepository.UpdateEntityAsync(Arg.Any<Resource>())
                .Returns(resource);

            var result = await _sut.UpdateResource(resource.Id, resourceDto);

            result.Should().NotBeNull();
            result.Id.Should().Be(resource.Id);
            result.Description.Should().Be("Updated Resource");
        }

        [Fact]
        public async Task ThrowEntityNotFoundException_WhenResourceDoesNotExist()
        {
            _mockResourceRepository.GetEntityByIdAsync(1).Returns((Resource)null);

            var act = async () => await _sut.DeleteResource(1);

            await act.Should()
                .ThrowAsync<EntityNotFoundException>()
                .WithMessage("Resource with id 1 not found");
        }

        [Fact]
        public async Task ThrowDeleteNotPermittedException_WhenResourceHasReservations()
        {
            var resource = new ResourceGenerator().CreateResource(1, 1);
            _mockResourceRepository.GetEntityByIdAsync(1)
                .Returns(resource);
            _mockReservationRepository.GetReservationByResourceIdAfterTodayAsync(1)
                .Returns(new List<Reservation> { new Reservation() });

            var act = async () => await _sut.DeleteResource(1);

            await act.Should()
                .ThrowAsync<DeleteNotPermittedException>()
                .WithMessage("Resource cannot be deleted because of existing reservation");
        }

        [Fact]
        public async Task DeleteResource_WhenNoReservationsExist()
        {
            var resource = new ResourceGenerator().CreateResource(1, 1);
            _mockResourceRepository.GetEntityByIdAsync(1)
                .Returns(resource);
            _mockReservationRepository.GetReservationByResourceIdAfterTodayAsync(1)
                .Returns(new List<Reservation>());
            _mockResourceRepository.DeleteEntityAsync(Arg.Any<Resource>())
                .Returns(Task.CompletedTask);

            await _sut.DeleteResource(1);

            await _mockResourceRepository.Received(1).DeleteEntityAsync(resource);
        }
    }
}
