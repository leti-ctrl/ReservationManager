using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IUserTypeService
    {
        Task<IEnumerable<UserTypeDto>> GetAllUserTypes();
        Task<UserTypeDto> CreateUserType(string code);
        Task<UserTypeDto> UpdateUserType(int id, string userTypeDto);
        Task DeleteUserType(int id);
    }
}
