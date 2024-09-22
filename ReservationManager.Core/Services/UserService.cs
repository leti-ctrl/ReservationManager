using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;

namespace ReservationManager.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTypeRepository _userTypeRepository;

        public UserService(IUserRepository userRepository, IUserTypeRepository userTypeRepository)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllEntitiesAsync();
            if (users == null)
                return Enumerable.Empty<UserDto>();

            return users.Select(x => x.Adapt<UserDto>());
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _userRepository.GetEntityByIdAsync(id)
                ?? throw new EntityNotFoundException($"User {id} not found.");

            return user.Adapt<UserDto>();
        }

        public async Task<UserDto> CreateUser(UpsertUserDto userDto)
        {
            var type = await _userTypeRepository.GetTypeByCode(userDto.Role)
                ?? throw new InvalidCodeTypeException($"User role {userDto.Role} not found");

            var userModel = userDto.Adapt<User>();
            userModel.Type = type;
            userModel.TypeId = type.Id;

            var user = await _userRepository.CreateEntityAsync(userModel);

            return user.Adapt<UserDto>();
        }

        public async Task<UserDto> UpdateUser(int id, UpsertUserDto userDto)
        {
            var type = await _userTypeRepository.GetTypeByCode(userDto.Role)
                ?? throw new InvalidCodeTypeException($"User role {userDto.Role} not found");

            var userModel = userDto.Adapt<User>();
            userModel.Id = id;
            userModel.Type = type;
            userModel.TypeId = type.Id;

            var updated = await _userRepository.UpdateEntityAsync(userModel);

            return updated.Adapt<UserDto>();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetEntityByIdAsync(id)
                ?? throw new EntityNotFoundException($"User {id} not found.");

            await _userRepository.DeleteEntityAsync(user);
        }
    }
}
