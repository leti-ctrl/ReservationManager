using FluentAssertions;
using Mapster;
using NSubstitute;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Services;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using Tests.EntityGenerators;

namespace Tests.Services;

[Trait("Category", "Unit")]
public class UserServiceShould
{
    private readonly IUserService _sut;
    private readonly IUserRepository _mockUserRepository;
    private readonly IRoleRepository _mockRoleRepository;

    public UserServiceShould()
    {
        _mockUserRepository = Substitute.For<IUserRepository>();
        _mockRoleRepository = Substitute.For<IRoleRepository>();
        
        _sut = new UserService(_mockUserRepository, _mockRoleRepository);
    }
    
    [Fact]
    public async Task AssignEmployeeRole_WhenCreatingUser()
    {
        var userGen = new UserGenerator();
        var basicUser = userGen.GetBasicUser();

        _mockRoleRepository.GetTypeByCode(FixedUserRole.Employee)
            .Returns(new Role { Name = "Employee", Code = FixedUserRole.Employee });
        _mockUserRepository.AddUserAsync(Arg.Any<User>()).Returns(ci =>
        {
            var user = ci.Arg<User>();
            user.Id = 1; 
            return user;
        });

        var result = await _sut.CreateUser(basicUser.Adapt<UpsertUserDto>());

        result.Should().NotBeNull();
        result.Roles.Should().ContainSingle(r => r.Code == FixedUserRole.Employee);
    }


    [Fact]
    public async Task ThrowEntityNotFoundException_WhenDeleteUserThatDoesNotExist()
    {
        var userId = 999; 
        _mockUserRepository.GetEntityByIdAsync(userId).Returns((User?)null);

        var act = async () => await _sut.DeleteUser(userId);

        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"User {userId} not found.");
    }

    [Fact]
    public async Task ThrowArgumentException_WhenUpdatingUserRolesWithEmptyRoles()
    {
        var userId = 1;
        var emptyRoles = Array.Empty<Role>();
        var user = new UserGenerator().GetBasicUser();

        _mockUserRepository.GetEntityByIdAsync(userId).Returns(user);

        var act = async () => await _sut.UpdateUserRoles(userId, emptyRoles);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("User roles must not be empty");
    }

    [Fact]
    public async Task UpdateUserRoles_WhenValidRolesProvided()
    {
        var userId = 1;
        var roleGen = new RoleGenerator();
        var newRoles = new[]
        {
            roleGen.GetAdminRole(),
            roleGen.GetEmployeeRole()
        };
        var existingUser = new UserGenerator().GetBasicUser();
        existingUser.Roles = newRoles;

        _mockUserRepository.GetEntityByIdAsync(userId).Returns(existingUser);
        _mockUserRepository.UpdateUserRolesAsync(existingUser, newRoles).Returns(existingUser);

        var result = await _sut.UpdateUserRoles(userId, newRoles);

        result.Should().NotBeNull();
        result!.Roles.Should().HaveCount(2);
        result.Roles.Should().Contain(r => r.Code == FixedUserRole.Admin);
        result.Roles.Should().Contain(r => r.Code == FixedUserRole.Employee);
    }

    [Fact]
    public async Task ReturnNull_WhenUpdatingNonExistentUser()
    {
        var userId = 999;
        var updateUserDto = new UpsertUserDto
        {
            Email = "nonexistent@mail.com",
            Name = "NonExistent",
            Surname = "User"
        };

        _mockUserRepository.GetEntityByIdAsync(userId).Returns((User?)null);

        var result = await _sut.UpdateUser(userId, updateUserDto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteUser_WhenUserExists()
    {
        var userId = 1;
        var user = new UserGenerator().GetBasicUser();

        _mockUserRepository.GetEntityByIdAsync(userId).Returns(user);

        var act = async () => await _sut.DeleteUser(userId);

        await act.Should().NotThrowAsync();
        await _mockUserRepository.Received(1).DeleteEntityAsync(user);
    }

}