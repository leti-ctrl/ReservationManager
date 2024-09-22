﻿namespace ReservationManager.Core.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required UserTypeDto Role { get; set; }
    }
}
