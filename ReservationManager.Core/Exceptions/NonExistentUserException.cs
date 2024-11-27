namespace ReservationManager.Core.Exceptions;

public class NonExistentUserException : Exception
{
    public NonExistentUserException(string message) : base(message)
    {
    }
}