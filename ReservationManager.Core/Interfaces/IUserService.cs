using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetUser(int id);
        UserDto CreateUser(UpsertUserDto userDto);
        UserDto UpdateUser(UserDto userDto);
        void DeleteUser(int id);
    }
}
