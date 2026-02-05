using Reception.Models;

namespace Reception.IService
{
    public interface IServices
    {
        public Task AddClinic(string name);
        public Task AddTreatment(addTreatmentDto dto);
        public Task AddVisit(AddVisitDto dto);
        public Task AddVisitTreatment(AddVisitTreatmentDto dto);
        public Task DeleteVisitTreatment(int id );
        Task<Invoice> GetInvoiceById(int invoiceId); 

        public Task<List<Clinic>> GetAllClinics();
        public Task<List<Treatment>> GetAllTreatmentClinic(int ClinicId);

        public Task<List<Visit>> GetAllVisit();
        public Task<List<VisitTreatment>> GetAllVisitTreatment(int visit);
        public  Task<Visit> GetVisitById(int visitId);

        Task<int> GetTodayVisitsCount();
        Task<int> GetWaitingListCount();
        Task<decimal> GetTodayTotalAmount();

    }
}
