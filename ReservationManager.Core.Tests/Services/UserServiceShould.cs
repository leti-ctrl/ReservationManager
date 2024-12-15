using FluentAssertions;
using Mapster;
using NSubstitute;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
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
    public async Task SetHolderRoleAsDefault_WhenAddUser()
    {
        var createUserReq = new UserGenerator().GetBasicUser();
        _mockUserRepository.AddUserAsync(Arg.Any<User>()).Returns(createUserReq);
        _mockRoleRepository.GetTypeByCode(Arg.Any<string>())
            .Returns(new Role { Name = "Employee", Code = FixedUserRole.Employee });

        var result = await _sut.CreateUser(createUserReq.Adapt<UpsertUserDto>());

        result.Should().NotBeNull();
        result.Roles.Should().NotBeEmpty();
        result.Roles.First().Code.Should().Be(FixedUserRole.Employee);
    }
}