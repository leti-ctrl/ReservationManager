namespace ReservationManager.Core.Exceptions
{
    public class InvalidCodeTypeException : Exception
    {
        public InvalidCodeTypeException(string? message) : base(message)
        {
        }
    }
}
