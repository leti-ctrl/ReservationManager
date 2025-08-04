namespace ReservationManager.DomainModel.Base
{
    public class EditableType : BaseType
    {
        public DateTime? IsDeleted { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime? ModifiedOn { get; set; }
    }
}
