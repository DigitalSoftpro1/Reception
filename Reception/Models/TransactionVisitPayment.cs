namespace Reception.Models
{
    public class TransactionVisitPayment : BaseEntity
    {
        public int TransactionVisitId { get; set; }
        public TransactionVisit TransactionVisit { get; set; } = null!;

        public DateTime PaidDate { get; set; }
        public string PaidType { get; set; } = null!; // Cash / Visa / etc
        public decimal PaidValue { get; set; }
    }

}
