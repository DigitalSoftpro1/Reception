namespace Reception.Models
{
    public class Visit 
    {
        public int Id { get; set; }
    
        public DateTime VisitDate { get; set; }

        public string? PatientName { set; get; }

        public int? ClinicId { get; set; }
        public string? ClinicName { get; set; }

        public string? Status { get; set; }
        public string? Notes { get; set; }
        public Clinic? Clinic { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
