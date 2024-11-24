namespace ReservationManager.Core.Exceptions
{
    public class CreateNotPermittedException : Exception
    {
        public CreateNotPermittedException(string message) : base(message) { }
    }
}
