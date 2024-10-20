using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Exceptions
{
    public class NotEditableTypeException : Exception
    {
        public NotEditableTypeException(string message) : base(message)
        {
        }
    }
}
