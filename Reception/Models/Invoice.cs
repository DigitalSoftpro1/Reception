namespace Reception.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public Visit? Visit { get; set; }
        public int? TreatmentId { get; set; }


        public decimal? InvoiceTotal { get; set; }

        public decimal? AmmountToPay { get; set; }
        public decimal? TotalPaid { get; set; }

        public decimal? Discount { get; set; }

        public decimal? Remain { get; set; }


        public int? VisitId { get; set; }
        public List<Payment>? Payments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}


