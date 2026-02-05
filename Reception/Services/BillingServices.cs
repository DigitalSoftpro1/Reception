


using Microsoft.EntityFrameworkCore;
using Reception.IService;
using Reception.Models;

namespace Reception.Services
{
    public class BillingServices : IBillingServices
    {
        private readonly ReceptionDbContext _context;

        public BillingServices(ReceptionDbContext context)
        {
            _context = context;
        }

        public async Task AddInvoice(AddInvoiceDto dto)
        {
            var invoice = new Invoice
            {
                VisitId = dto.VisitId,
                InvoiceTotal = dto.InvoiceTotal,
                AmmountToPay = dto.AmmountToPay,
                TotalPaid = dto.TotalPaid,
                Discount = dto.Discount,
                Remain = dto.Remain,
                CreatedDate = DateTime.Now
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task AddPayment(AddPaymentDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var invoice = await _context.Invoices
                    .FirstOrDefaultAsync(i => i.Id == dto.IdInvoice);

                if (invoice == null)
                    throw new Exception("Invoice not found");

                var amountToPay =
                    (invoice.InvoiceTotal ?? 0) - (invoice.Discount ?? 0);

                var payment = new Payment
                {
                    IdInvoice = dto.IdInvoice,
                    PaidDate = dto.PaidDate,
                    PaidType = dto.PaidType,
                    PaidValue = dto.PaidValue
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                var totalPaid = await _context.Payments
                    .Where(p => p.IdInvoice == invoice.Id)
                    .SumAsync(p => p.PaidValue);

                if (totalPaid > amountToPay)
                    throw new Exception("Paid amount exceeds invoice total");

                invoice.TotalPaid = totalPaid;
                invoice.AmmountToPay = amountToPay;
                invoice.Remain = amountToPay - totalPaid;

                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Invoice> GetOrCreateInvoiceForTreatment(int visitId, int treatmentId, decimal price)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.VisitId == visitId && i.TreatmentId == treatmentId );

            if (invoice == null)
            {
                var visit = await _context.Visits.FindAsync(visitId);

                invoice = new Invoice
                {
                    VisitId = visitId,
                    TreatmentId = treatmentId,
                    InvoiceTotal = price,
                    Discount = 0,
                    AmmountToPay = price,
                    TotalPaid = 0,
                    Remain = price,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
            }
            

            return invoice;
        }

        public async Task<Invoice> GetInvoiceById(int invoiceId)
        {
            return await _context.Invoices
                .Include(i => i.Visit)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);
        }

        public async Task<List<Payment>> GetAllInvoice(int visitId)
        {
            return await _context.Payments
                .Include(p => p.Invoice)
                .Where(p => p.Invoice.VisitId == visitId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetAllPaymentInvoice(int invoiceId)
        {
            return await _context.Payments
                .Where(p => p.IdInvoice == invoiceId)
                .OrderByDescending(p => p.PaidDate)
                .ToListAsync();
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
    }
}