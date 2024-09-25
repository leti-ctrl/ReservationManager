using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Exceptions
{
    public class DeleteNotPermittedException : Exception
    {
        public DeleteNotPermittedException(string? message) : base(message)
        {
        }
    }
}
