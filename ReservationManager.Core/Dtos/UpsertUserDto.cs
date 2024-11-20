namespace ReservationManager.Core.Dtos
{
    public class UpsertUserDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
    }
}
