namespace ReservationManager.Core.Exceptions;

public class OperationNotPermittedException : Exception
{
    public OperationNotPermittedException(string message) : base(message)
    {
    }
}