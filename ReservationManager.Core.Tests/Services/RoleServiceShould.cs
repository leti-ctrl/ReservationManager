using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Services;
using Tests.EntityGenerators;

namespace Tests.Services;

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
}