namespace ReservationManager.API.Request
{
    public class UserUpsertRequest
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
}
