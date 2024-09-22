namespace ReservationManager.DomainModel.Base
{
    public class BaseType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? IsDeleted { get; set; }
    }
}
