﻿namespace ReservationManager.Core.Dtos
{
    public class ResourceDto 
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public ResourceTypeDto Type { get; set; }
        public List<ResourceReservedDto>? ResourceReservedDtos { get; set; }
    }
}
