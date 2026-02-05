namespace Reception.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public string? Name { set; get; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal? DefaultPrice { get; set; }
        public Clinic? clinic { get; set; }
    }

}
