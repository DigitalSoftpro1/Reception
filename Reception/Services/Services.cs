using Microsoft.EntityFrameworkCore;
using Reception.IService;
using Reception.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reception.Services
{
    public class Services : IServices
    {
        private readonly ReceptionDbContext _context;

        public Services(ReceptionDbContext context)
        {
            _context = context;
        }

        public async Task AddClinic(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Clinic name cannot be empty");

            var clinic = new Clinic { Name = name };
            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
        }

        public async Task AddTreatment(addTreatmentDto dto)
        {
            var clinicExists = await _context.Clinics.AnyAsync(c => c.Id == dto.ClinicId);
            if (!clinicExists)
                throw new ArgumentException($"Clinic with ID {dto.ClinicId} not found");

            var treatment = new Treatment
            {
                ClinicId = dto.ClinicId,
                Name = dto.Name,
                DefaultPrice = dto.DefaultPrice
            };

            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task AddVisit(AddVisitDto dto)
        {
            var clinicExists = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == dto.ClinicId);
            if (clinicExists == null)
                throw new ArgumentException($"Clinic with ID {dto.ClinicId} not found");

            var visit = new Visit
            {
                VisitDate = dto.VisitDate,
                PatientName = dto.PatientName,
                Status = dto.Status,
                Notes = dto.Notes,
                ClinicId = dto.ClinicId,
            };
            visit.ClinicName = clinicExists.Name;

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
        }

        public async Task AddVisitTreatment(AddVisitTreatmentDto dto)
        {
            var treatmentExists = await _context.Treatments.FirstOrDefaultAsync(t => t.Id == dto.TreatmentId);
            var visitExists = await _context.Visits.FirstOrDefaultAsync(v => v.Id == dto.VisitId);
            if (treatmentExists == null)
                throw new ArgumentException($"Treatment with ID {dto.TreatmentId} not found");
            if (visitExists == null)
                throw new ArgumentException($"Visit with ID {dto.VisitId} not found");

            var visitTreatment = new VisitTreatment
            {
                TreatmentId = dto.TreatmentId,
                VisitId = dto.VisitId,
                PatientPrice = dto.PatientPrice
            };

            _context.VisitTreatments.Add(visitTreatment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitTreatment(int id)
        {
            var visitTreatment = await _context.VisitTreatments.FindAsync(id);
            if (visitTreatment == null)
                throw new KeyNotFoundException($"VisitTreatment with ID {id} not found");

            _context.VisitTreatments.Remove(visitTreatment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Clinic>> GetAllClinic()
        {
            return await _context.Clinics
                                 .Include(c => c.Treatment)
                                 .ToListAsync();
        }

        public async Task<List<Treatment>> GetAllTreatmentClinic(int clinicId)
        {
            return await _context.Treatments
                                 .Where(t => t.ClinicId == clinicId)
                                 .ToListAsync();
        }

        public async Task<List<Visit>> GetAllVisit()
        {
            return await _context.Visits
                                 .Include(v => v.Clinic)
                                 .ToListAsync();
        }

        public async Task<List<VisitTreatment>> GetAllVisitTreatment(int visitId)
        {
            return await _context.VisitTreatments
                                 .Include(vt => vt.Treatment)
                                 .Where(vt => vt.VisitId == visitId)
                                 .ToListAsync();
        }

        public async Task<Visit> GetVisitById(int visitId)
        {
           return await _context.Visits
    .Include(v => v.Clinic)
    .FirstOrDefaultAsync(v => v.Id == visitId);
        }
        public async Task<List<Clinic>> GetAllClinics()
{
    return await _context.Clinics.ToListAsync();
}

        public async Task<int> GetTodayVisitsCount()
        {
            var today = DateTime.Today;
            return await _context.Visits.CountAsync(v => v.VisitDate.Date == today);
        }

        public async Task<int> GetWaitingListCount()
        {
            return await _context.Visits.CountAsync(v =>
                v.Status != null &&
                (v.Status.Trim().ToLower() == "waiting" || v.Status.Trim() == "قائمة الانتظار")
            );
        }



        public async Task<decimal> GetTodayTotalAmount()
        {
            var today = DateTime.Today;
            return await _context.VisitTreatments
                                 .Include(vt => vt.Visit)
                                 .Where(vt => vt.Visit.VisitDate.Date == today)
                                 .SumAsync(vt => (decimal?)vt.PatientPrice) ?? 0;
        }
public async Task<Invoice> GetOrCreateInvoiceForVisit(int visitId, int treatmentId, string treatmentName, decimal price)
        {
            // البحث عن Invoice موجود
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.VisitId == visitId);

            if (invoice == null)
            {
                // إنشاء Invoice جديد
                invoice = new Invoice
                {
                    VisitId = visitId,
                    InvoiceTotal = price,
                    Discount = 0,
                    AmmountToPay = price,
                    TotalPaid = 0,
                    Remain = price,
                    CreatedDate = DateTime.Now
                };
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
            }

            return invoice;
        }

        public async Task<Invoice> GetInvoiceById(int invoiceId)
        {
            return await _context.Invoices.FindAsync(invoiceId);
        }
    }
}
