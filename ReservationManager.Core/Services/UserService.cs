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
    public class UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        : IUserService
    {
        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await userRepository.GetUserByIdWithRoleAsync(id);
            if (user == null)
                return null;
            return user.Adapt<UserDto>();
        }

        public async Task<UserDto?> GetUserByEmail(string email)
        {
            var user = await userRepository.GetUserByEmailAsync(email);
            return user == null ? null : user.Adapt<UserDto>();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await userRepository.GetAllEntitiesAsync();

            return users.Select(x => x.Adapt<UserDto>());
        }

        public async Task<UserDto> CreateUser(UpsertUserDto userDto)
        {
            var employeeRole = await roleRepository.GetTypeByCode(FixedUserRole.Employee);
            CheckEmail(userDto.Email);

            var userModel = userDto.Adapt<User>();
            userModel.Roles.Add(employeeRole!);

            var user = await userRepository.CreateEntityAsync(userModel);

            return user.Adapt<UserDto>();
        }

        public void CheckEmail(string email, int id = 0)
        {
            if (email == String.Empty || !email.Contains('@') || !email.Contains('.'))
                throw new ArgumentException("Invalid Email");
            var user = userRepository.GetUserByEmailAsync(email).Result;
            if (user != null && (id == 0 || user.Id != id))
                throw new ArgumentException("Email already exists");
        }

        public async Task<UserDto?> UpdateUser(int id, UpsertUserDto userDto)
        {
            //nella modifica non si può modificare il ruolo
            var user = await userRepository.GetEntityByIdAsync(id);
            if (user == null)
                return null;

            CheckEmail(userDto.Email, id);

            var userModel = userDto.Adapt<User>();
            userModel.Id = id;

            var updated = await userRepository.UpdateEntityAsync(userModel);

            return updated.Adapt<UserDto>();
        }

        public async Task<UserDto?> UpdateUserRoles(int userId, Role[] newRoles)
        {
            var user = await userRepository.GetEntityByIdAsync(userId);
            if (user == null)
                return null;

            if (newRoles.Length == 0)
                throw new ArgumentException("User roles must not be empty");

            var updated = await userRepository.UpdateUserRolesAsync(user, newRoles);
            return updated?.Adapt<UserDto>();
        }

        public async Task DeleteUser(int id)
        {
            var user = await userRepository.GetEntityByIdAsync(id)
                       ?? throw new EntityNotFoundException($"User {id} not found.");

            await userRepository.DeleteEntityAsync(user);
        }
    }
}