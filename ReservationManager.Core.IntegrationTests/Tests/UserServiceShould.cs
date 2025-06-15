using FluentAssertions;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.IntegrationTests.Tests;

[TestFixture]
[Category("Integration")]
public class UserServiceShould
{
    private IUserService _userService = null!;
    private IRoleRepository _roleService = null!;

    private int userToUpdated;
    private string userEmail;
    [SetUp]
    public void SetUp()
    {
        _userService = Setup.GetUserService();
        _roleService = Setup.GetRoleRepository();
        
        userEmail = "tmp@mail.it";
        var basicUser = new User { Email = userEmail, Name = "Basic", Surname = "Basic" };
        userToUpdated = Setup.GetUserRepository().CreateEntityAsync(basicUser).Result.Id;

    }
    

    [Test]
    public async Task ThrowEntityNotFoundException_WhenDeleteUserThatDoesNotExist()
    {
        var userId = 999999;

        var act = async () => await _userService.DeleteUser(userId);

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"User {userId} not found.");
    }

    [Test]
    public async Task ThrowArgumentException_WhenUpdatingUserRolesWithEmptyRoles()
    {
        var act = async () => await _userService.UpdateUserRoles(userToUpdated, Array.Empty<Role>());

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("User roles must not be empty");
    }

    [Test]
    public async Task UpdateUserRoles_WhenValidRolesProvided()
    {
        var roles = new[]
        {
            await _roleService.GetTypeByCode(FixedUserRole.Admin),
            await _roleService.GetTypeByCode(FixedUserRole.GeneralServices),
        };

        var result = await _userService.UpdateUserRoles(userToUpdated, roles);

        result.Should().NotBeNull();
        result.Roles.Should().HaveCount(2);
        result.Roles.Should().Contain(r => r.Code == FixedUserRole.Admin);
        result.Roles.Should().Contain(r => r.Code == FixedUserRole.GeneralServices);
    }

    [Test]
    public async Task UpdateUserDetails_WhenValidInputProvided()
    {
        var updateUserDto = new UpsertUserDto
        {
            Email = "esadsfegrht@mail.com",
            Name = "wertyu",
            Surname = "AWSFGH"
        };

        var result = await _userService.UpdateUser(userToUpdated, updateUserDto);

        result.Should().NotBeNull();
        result!.Id.Should().Be(userToUpdated);
        result.Email.Should().Be(updateUserDto.Email);
        result.Name.Should().Be(updateUserDto.Name);
        result.Surname.Should().Be(updateUserDto.Surname);
    }

    [Test]
    public async Task ReturnNull_WhenUpdatingNonExistentUser()
    {
        var updateUserDto = new UpsertUserDto
        {
            Email = "AWSERGHJasdfgh@mail.com",
            Name = "AQWSERTGYHJ",
            Surname = "awdfh"
        };

        var result = await _userService.UpdateUser(999999, updateUserDto);

        result.Should().BeNull();
    }
    

    [Test]
    public async Task ReturnUser_WhenUserExistsById()
    {
        var result = await _userService.GetUserById(userToUpdated);

        result.Should().NotBeNull();
        result!.Id.Should().Be(userToUpdated);
    }

    [Test]
    public async Task ReturnNull_WhenUserDoesNotExistById()
    {
        var result = await _userService.GetUserById(999999);

        result.Should().BeNull();
    }

    [Test]
    public async Task ReturnUser_WhenUserExistsByEmail()
    {
        var result = await _userService.GetUserByEmail(userEmail);

        result.Should().NotBeNull();
        result!.Email.Should().Be(userEmail);
    }

    [Test]
    public async Task ReturnNull_WhenUserDoesNotExistByEmail()
    {
        var result = await _userService.GetUserByEmail("nonexistent@email.com");

        result.Should().BeNull();
    }

    [Test]
    public async Task ReturnAllUsers_WhenUsersExist()
    {
        var result = await _userService.GetAllUsers();

        result.Should().NotBeNull();
        result.Count().Should().BeGreaterThanOrEqualTo(1);
    }
}
