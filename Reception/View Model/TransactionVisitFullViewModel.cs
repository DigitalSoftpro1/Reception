namespace Reception.View_Model
{
    public class TransactionVisitFullViewModel
    {
        public int Id { get; set; }

        public string? VisitNo { get; set; }
        public DateTime? VisitDate { get; set; }
        public string? PatientName { get; set; }

        public bool IsActive { get; set; }

        public List<TransactionVisitViewModel>? TransactionVisitList { get; set; }
    }
}