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
        IEnumerable<UserTypeDto> GetAllUserTypes();
        UserTypeDto CreateUserType(string code);
        UserTypeDto UpdateUserType(int id, string userTypeDto);
        void DeleteUserType(int id);
    }
}
