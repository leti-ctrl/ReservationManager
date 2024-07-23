using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class UpsertUserDto 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public UserTypeDto Type { get; set; }
    }
}
