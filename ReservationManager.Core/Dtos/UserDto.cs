using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class UserDto : UpsertUserDto
    {
        public int Id { get; set; }
    }
}
