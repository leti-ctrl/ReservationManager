using Mapster;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await _userRepository.GetEntityByIdAsync(id);
            if (user == null)
                return null;
            return user.Adapt<UserDto>();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllEntitiesAsync();
            
            return users.Select(x => x.Adapt<UserDto>());
        }

        public async Task<UserDto> CreateUser(UpsertUserDto userDto)
        {
            var employeeRole = await _roleRepository.GetTypeByCode(FixedUserRole.Employee);
            var userModel = userDto.Adapt<User>();
            userModel.Roles.Add(employeeRole!);

            var user = await _userRepository.AddUserAsync(userModel);

            return user.Adapt<UserDto>();
        }

        public async Task<UserDto?> UpdateUser(int id, UpsertUserDto userDto)
        {
            //nella modifica non si può modificare il ruolo
            var user = await _userRepository.GetEntityByIdAsync(id);
            if (user == null)
                return null;
            
            var userModel = userDto.Adapt<User>();
            userModel.Id = id;

            var updated = await _userRepository.UpdateEntityAsync(userModel);

            return updated.Adapt<UserDto>();
        }

        public async Task<UserDto?> UpdateUserRoles(int userId, Role[] newRoles)
        {
            var user = await _userRepository.GetEntityByIdAsync(userId);
            if (user == null) return null;

            if (newRoles.Length == 0) throw new ArgumentException("User roles must not be empty");

            var updated = await _userRepository.UpdateUserRolesAsync(user, newRoles);
            return updated?.Adapt<UserDto>();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetEntityByIdAsync(id)
                ?? throw new EntityNotFoundException($"User {id} not found.");

            await _userRepository.DeleteEntityAsync(user);
        }
    }
}
