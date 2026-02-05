namespace Reception.Models
{
    public class AddPaymentDto
    {
        public DateTime PaidDate { get; set; }
        public string? PaidType { get; set; } 
        public decimal? PaidValue { get; set; }
        public int IdInvoice { get; set; }
    }
    public class AddInvoiceDto
    {
        public decimal? InvoiceTotal { get; set; }

        public decimal? AmmountToPay { get; set; }
        public decimal? TotalPaid { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Remain { get; set; }

        public int PatientId { get; set; }

        public int VisitId { get; set; }
    }
}
