namespace Reception.View_Model
{
    public class TransactionVisitPaymentViewModel
    {
        public int Id { get; set; }

        public int? TransactionVisitId { get; set; }

        public DateTime? PaidDate { get; set; }
        public string? PaidType { get; set; }
        public decimal? PaidValue { get; set; }

        public bool IsActive { get; set; }
    }
}
