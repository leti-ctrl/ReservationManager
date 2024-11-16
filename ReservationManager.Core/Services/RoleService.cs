using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IUserTypeRepository userTypeRepository, IUserRepository userRepository)
        {
            _userTypeRepository = userTypeRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllUserTypes()
        {
            var userTypes = await _userTypeRepository.GetAllTypesAsync();
            if (userTypes == null)
                return Enumerable.Empty<RoleDto>();

            return userTypes.Select(ut => ut.Adapt<RoleDto>());
        }

        public async Task<RoleDto> CreateUserType(string code, string name)
        {
            var userType = await _userTypeRepository.CreateTypeAsync(new DomainModel.Meta.Role() { Code = code , Name = name});
            return userType.Adapt<RoleDto>();
        }

        public async Task<RoleDto> UpdateUserType(int id, string code, string name)
        {
            var oldUserType = await _userTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"User Type with id {id} not found.");
            oldUserType.Code = code;
            oldUserType.Name = name;

            var updated = await _userTypeRepository.UpdateTypeAsync(oldUserType);
            return updated.Adapt<RoleDto>();
        }

        public async Task DeleteUserType(int id)
        {
            var toDelete = await _userTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"User Type with id {id} not found.");
            
            await _userTypeRepository.DeleteTypeAsync(toDelete);
        }
    }
}
