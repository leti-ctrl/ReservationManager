namespace ReservationManager.Core.Dtos;

public class SessionInfo
{
    public string UserEmail { get; }    
    public SessionInfo(string userEmail)
    {
        UserEmail = userEmail;
    }
}