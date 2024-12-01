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

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllTypesAsync();
            if (roles == null)
                return Enumerable.Empty<RoleDto>();

            return roles.Select(ut => ut.Adapt<RoleDto>());
        }
    }
}
