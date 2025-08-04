using FluentAssertions;
using NUnit.Framework;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Services;

    
namespace ReservationManager.Core.IntegrationTests.Tests;

[TestFixture]
[Category("Integration")]
public class RoleServiceShould
{
    private IRoleService _roleService = null!;

    [SetUp]
    public void SetUp()
    {
        _roleService = Setup.GetRoleService();
    }

    [Test]
    public async Task ReturnAllRoles_WhenRolesExist()
    {
        // Act
        var result = await _roleService.GetAllRoles();

        // Assert
        result.Should().NotBeNull();
        result.Should().AllBeOfType<RoleDto>();
        result.Should().NotBeEmpty();
    }
    
    [Test]
    public async Task MapAllRolesToDtoCorrectly()
    {
        var result = await _roleService.GetAllRoles();

        result.Should().NotBeNull();
        result.Should().AllBeOfType<RoleDto>();

        foreach (var roleDto in result)
        {
            roleDto.Id.Should().BeGreaterThan(0);
            roleDto.Code.Should().NotBeNullOrWhiteSpace();
            roleDto.Name.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public async Task ReturnRoleDtoWithCorrectData_WhenRolesExist()
    {
        var roleRepo = Setup.GetRoleRepository();
        var domainRoles = await roleRepo.GetAllTypesAsync();
        var dtos = await _roleService.GetAllRoles();

        domainRoles.Should().NotBeEmpty();
        dtos.Should().HaveCount(domainRoles.Count());

        foreach (var domainRole in domainRoles)
        {
            var dto = dtos.FirstOrDefault(r => r.Id == domainRole.Id);
            dto.Should().NotBeNull();
            dto!.Code.Should().Be(domainRole.Code);
            dto.Name.Should().Be(domainRole.Name);
        }
    }
}
