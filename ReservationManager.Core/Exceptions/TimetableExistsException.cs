namespace ReservationManager.Core.Exceptions
{
    public class TimetableExistsException : Exception
    {
        public TimetableExistsException(string message) : base(message)
        {}
    }
}
