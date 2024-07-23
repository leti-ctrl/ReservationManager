using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Services
{
    public class UserService : IUserService
    {
        public UserDto CreateUser(UpsertUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserDto GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
