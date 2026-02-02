namespace Reception.Models
{
    public class TransactionVisit : BaseEntity
    {
        public string VisitNo { get; set; } = null!;
        public DateTime VisitDate { get; set; }

        public string PatientName { get; set; } = null!;
        public string ClinicName { get; set; } = null!;
        public string VisitType { get; set; } = null!; // Cash / Insurance

        public decimal InvoiceTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountToPay { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Remain { get; set; }

        public string? Notes { get; set; }

        // Relations
        public ICollection<TransactionVisitTreatment> Treatments { get; set; } = new List<TransactionVisitTreatment>();
        public ICollection<TransactionVisitPayment> Payments { get; set; } = new List<TransactionVisitPayment>();
    }
}
