using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            var userTypes = await _roleRepository.GetAllTypesAsync();
            if (userTypes == null)
                return Enumerable.Empty<RoleDto>();

            return userTypes.Select(ut => ut.Adapt<RoleDto>());
        }

        public async Task<RoleDto> CreateRole(string code, string name)
        {
            var userType = await _roleRepository.CreateTypeAsync(new DomainModel.Meta.Role() { Code = code , Name = name});
            return userType.Adapt<RoleDto>();
        }

        public async Task<RoleDto> UpdateRole(int id, string code, string name)
        {
            var oldUserType = await _roleRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"User Type with id {id} not found.");
            oldUserType.Code = code;
            oldUserType.Name = name;

            var updated = await _roleRepository.UpdateTypeAsync(oldUserType);
            return updated.Adapt<RoleDto>();
        }

        public async Task DeleteRole(int id)
        {
            var toDelete = await _roleRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"User Type with id {id} not found.");
            
            await _roleRepository.DeleteTypeAsync(toDelete);
        }
    }
}
