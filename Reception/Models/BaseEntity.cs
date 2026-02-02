namespace Reception.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? EditId { get; set; }
        public DateTime? EditDate { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
