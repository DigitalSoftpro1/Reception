namespace Reception.Models
{
    public class VisitTreatment
    {
        public int Id { get; set; }



        public int TreatmentId { get; set; }
        public int VisitId { get; set; }
        public decimal PatientPrice { get; set; }

        public Treatment? Treatment { get; set; }
        public Visit? Visit { get; set; }
    }
}
