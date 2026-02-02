namespace Reception.Models
{
    public class TransactionVisitTreatment : BaseEntity
    {
        public int TransactionVisitId { get; set; }
        public TransactionVisit TransactionVisit { get; set; } = null!;

        public string TreatmentName { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public decimal PatientPrice { get; set; }

        public bool IsRefused { get; set; }
    }

}
