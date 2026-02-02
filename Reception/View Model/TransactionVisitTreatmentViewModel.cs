namespace Reception.View_Model
{
    public class TransactionVisitTreatmentViewModel
    {
        public int Id { get; set; }

        public int? TransactionVisitId { get; set; }

        public string? TreatmentName { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? PatientPrice { get; set; }

        public bool? IsRefused { get; set; }

        public bool IsActive { get; set; }
    }
}