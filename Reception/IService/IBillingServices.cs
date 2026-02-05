using Reception.Models;

namespace Reception.IService
{
    public interface IBillingServices
    {
        Task AddInvoice(AddInvoiceDto dto);
        Task AddPayment(AddPaymentDto dto);

        Task<List<Payment>> GetAllInvoice(int visitId);
        Task<List<Payment>> GetAllPaymentInvoice(int invoiceId);
        Task<Invoice> GetOrCreateInvoiceForTreatment(int visitId, int treatmentId, decimal price);
        Task<Invoice> GetInvoiceById(int invoiceId);
        Task UpdateInvoice(Invoice invoice);

    }
}
