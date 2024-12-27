using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Services;
using Tests.EntityGenerators;
using NSubstitute.ExceptionExtensions;
using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;

namespace Tests.Services
{
    [Trait("Category", "Unit")]
    public class RoleServiceShould
    {
        private readonly IRoleRepository _mockRoleRepository;
        private readonly IRoleService _sut;

        public RoleServiceShould()
        {
            _mockRoleRepository = Substitute.For<IRoleRepository>();
            _sut = new RoleService(_mockRoleRepository);
        }

        [Fact]
        public async Task ReturnsAllRoles()
        {
            var roles = new RoleGenerator().GetAllRoles();
            _mockRoleRepository.GetAllTypesAsync().Returns(roles);

            var act = await _sut.GetAllRoles();

            act.Should().NotBeNull();
            act.Should().HaveCount(roles.Count);
        }

        [Fact]
        public async Task ReturnsEmptyCollectionWhenNoRoles()
        {
            _mockRoleRepository.GetAllTypesAsync().Returns((IEnumerable<Role>)null);

            var result = await _sut.GetAllRoles();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnsEmptyCollectionWhenRepositoryThrowsException()
        {
            _mockRoleRepository.GetAllTypesAsync().Throws(new Exception("Repository error"));

            Func<Task> act = async () => { await _sut.GetAllRoles(); };

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Repository error");
        }

        [Fact]
        public async Task CallsRepositoryGetAllTypesAsyncOnce()
        {
            var roles = new RoleGenerator().GetAllRoles();
            _mockRoleRepository.GetAllTypesAsync().Returns(roles);

            await _sut.GetAllRoles();

            await _mockRoleRepository.Received(1).GetAllTypesAsync();
        }

        [Fact]
        public async Task MapsRolesCorrectlyToRoleDto()
        {
            var roles = new RoleGenerator().GetAllRoles();
            _mockRoleRepository.GetAllTypesAsync().Returns(roles);

            var result = await _sut.GetAllRoles();

            result.Should().NotBeNull();
            result.Should().HaveCount(roles.Count);
            result.All(r => r.GetType() == typeof(RoleDto)).Should().BeTrue();
        }
    }
}
