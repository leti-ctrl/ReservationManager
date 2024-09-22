using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.Persistence.Interfaces;

namespace ReservationManager.Core.Services
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _userTypeRepository;

        public UserTypeService(IUserTypeRepository userTypeRepository)
        {
            _userTypeRepository = userTypeRepository;
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
            var oldUserType = await _userTypeRepository.GetTypeById(id);
            oldUserType.Code = userTypeDto;

            var updated = await _userTypeRepository.UpdateTypeAsync(oldUserType);
            return updated.Adapt<UserTypeDto>();
        }

        public async Task DeleteUserType(int id)
        {
            await _userTypeRepository.DeleteTypeAsync(id);
        }
    }
}
