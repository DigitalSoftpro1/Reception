namespace Reception.Models
{
    public class addTreatmentDto
    {
        public int ClinicId { get; set; }
        public string? Name { set; get; }
        public decimal? DefaultPrice { get; set; }
    }

    public class AddVisitTreatmentDto
    {
        public int TreatmentId { get; set; }
        public int VisitId { get; set; }
        public decimal PatientPrice { get; set; }
    }
    public class AddVisitDto
    {

        public DateTime VisitDate { get; set; }

        public string? PatientName { set; get; }
        public string? PhoneNumber { get; set; }

        public int ClinicId { get; set; }
        public string? ClinicName { get; set; }

        public string? Status { get; set; }
        public string? Notes { get; set; }
       
            public List<Visit> Visits { get; set; } = new List<Visit>();

            public List<Clinic> Clinics { get; set; } = new List<Clinic>();

    }

}