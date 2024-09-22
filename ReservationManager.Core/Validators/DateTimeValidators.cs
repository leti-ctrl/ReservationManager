using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Validators
{
    public static class DateTimeValidators
    {
        public static TimeOnly? TimeOnlyValidator(this string time)
        {
            if(TimeOnly.TryParse(time, out var timeOnly))
                return timeOnly;
            else 
                return null;
        } 
    }
}
