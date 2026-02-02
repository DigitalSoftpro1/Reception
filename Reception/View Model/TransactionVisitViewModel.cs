namespace Reception.View_Model
{
    public class TransactionVisitViewModel
    {
        public int Id { get; set; }

        public string? VisitNo { get; set; }
        public DateTime? VisitDate { get; set; }

        public string? PatientName { get; set; }
        public string? ClinicName { get; set; }
        public string? VisitType { get; set; }

        public decimal? InvoiceTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? AmountToPay { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? Remain { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; }
    }
}
