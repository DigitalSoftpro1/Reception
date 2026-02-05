namespace Reception.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public DateTime PaidDate { get; set; }
        public string? PaidType { get; set; } // Cash / Visa / etc
        public decimal? PaidValue { get; set; }


        public int IdInvoice { get; set; }

        public Invoice? Invoice { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
