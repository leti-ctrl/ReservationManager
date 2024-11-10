namespace ReservationManager.Core.Exceptions;

public class UpdateNotPermittedException : Exception
{
    public UpdateNotPermittedException(string message) : base(message)
    { }
}