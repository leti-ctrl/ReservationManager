namespace ReservationManager.Core.Exceptions
{
    public class TimeOnlyException : Exception
    {
        public TimeOnlyException(string? message) : base(message)
        {
        }
    }
}
