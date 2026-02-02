namespace Reception.View_Model
{
    public class TransactionVisitPaymentFullViewModel
    {
        public int Id { get; set; }

        public int? TransactionVisitId { get; set; }

        public List<TransactionVisitPaymentViewModel>? TransactionVisitPaymentList { get; set; }
    }
}
