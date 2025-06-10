namespace Ordering.Core.Common
{
    public abstract class EntityBase
    {
        // Protected set us made to use in the derived classes
        public int Id { get; protected set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifyBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
