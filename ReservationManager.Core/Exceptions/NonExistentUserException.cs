namespace ReservationManager.Core.Exceptions;

public class NonExistentUserException(string message) : Exception(message);