namespace ReservationManager.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public RoleDto[] Roles { get; set; } = null!;    
        
        public string RolesWithSeparator => String.Join(",", Roles.Select(x => x.Code));

    }
}
