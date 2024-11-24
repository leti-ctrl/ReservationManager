namespace ReservationManager.Core.Dtos;

public class UpsertReservationTypeDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}