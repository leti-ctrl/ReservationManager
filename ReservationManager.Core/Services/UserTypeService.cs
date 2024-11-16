using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Persistence.Interfaces;

namespace ReservationManager.Core.Services
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IUserRepository _userRepository;

        public UserTypeService(IUserTypeRepository userTypeRepository, IUserRepository userRepository)
        {
            _userTypeRepository = userTypeRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserTypeDto>> GetAllUserTypes()
        {
            var userTypes = await _userTypeRepository.GetAllTypesAsync();
            if (userTypes == null)
                return Enumerable.Empty<UserTypeDto>();

            return userTypes.Select(ut => ut.Adapt<UserTypeDto>());
        }

        public async Task<UserTypeDto> CreateUserType(string code)
        {
            var userType = await _userTypeRepository.CreateTypeAsync(new DomainModel.Meta.UserType() { Code = code });
            return userType.Adapt<UserTypeDto>();
        }

        public async Task<UserTypeDto> UpdateUserType(int id, string userTypeDto)
        {
            var oldUserType = await _userTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"User Type with id {id} not found.");
            oldUserType.Code = userTypeDto;

            var updated = await _userTypeRepository.UpdateTypeAsync(oldUserType);
            return updated.Adapt<UserTypeDto>();
        }

        public async Task DeleteUserType(int id)
        {
            var toDelete = await _userTypeRepository.GetTypeById(id)
                ?? throw new EntityNotFoundException($"User Type with id {id} not found.");
            
            var exists = await _userRepository.GetByTypeAsync(toDelete.Code);
            if (exists.Any())
                throw new DeleteNotPermittedException($"Cannot delete {toDelete.Code} because exits user with this role");

            await _userTypeRepository.DeleteTypeAsync(toDelete);
        }
    }
}
