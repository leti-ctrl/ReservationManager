namespace ReservationManager.API.Request
{
    public class UserCreateRequest
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
}
