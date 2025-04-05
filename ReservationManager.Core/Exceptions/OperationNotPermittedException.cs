namespace ReservationManager.Core.Exceptions;

public class OperationNotPermittedException(string message) : Exception(message);