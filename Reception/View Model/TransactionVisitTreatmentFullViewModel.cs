namespace Reception.View_Model
{
    public class TransactionVisitTreatmentFullViewModel
    {
        public int Id { get; set; }

        public int? TransactionVisitId { get; set; }

        public List<TransactionVisitTreatmentViewModel>? TransactionVisitTreatmentList { get; set; }
    }
}