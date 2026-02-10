using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using System.Linq;
using System;
using Reception.IService;
using Reception.Services;
using Microsoft.EntityFrameworkCore;

namespace Reception.Controllers
{
    public class BillingController : Controller
    {
        private readonly IBillingServices _invoiceService;
        private readonly ReceptionDbContext _context;

        public BillingController(IBillingServices services, ReceptionDbContext context)
        {
            _invoiceService = services;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice(AddInvoiceDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _invoiceService.AddInvoice(dto);
            return RedirectToAction("Invoices");
        }

        public async Task<IActionResult> Invoices(int visitId)
        {
            var invoices = await _invoiceService.GetAllInvoice(visitId);
            return View(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(Payment payment, decimal Discount)
        {
            if (!ModelState.IsValid)
                return View(payment);

            payment.IsActive = true;

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == payment.IdInvoice);

            if (invoice != null)
            {
                invoice.Discount = Discount;

                invoice.AmmountToPay = (invoice.InvoiceTotal ?? 0) - Discount;

                var payments = await _context.Payments
                    .Where(p => p.IdInvoice == invoice.Id && p.IsActive)
                    .ToListAsync();
                invoice.TotalPaid = payments.Sum(p => p.PaidValue ?? 0);

                invoice.Remain = invoice.AmmountToPay - invoice.TotalPaid;

                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();
            }

            if (invoice == null)
                return RedirectToAction("Index");

            return RedirectToAction("Payments", new
            {
                visitId = invoice.VisitId,
                treatmentId = invoice.TreatmentId,
                treatmentName = "Treatment",
                price = invoice.InvoiceTotal ?? 0
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInvoiceDetails(
        int InvoiceId,
        int VisitId,
        int TreatmentId,
        string TreatmentName,
        decimal InvoiceTotal,
        decimal Discount)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceById(InvoiceId);

                if (invoice != null)
                {
                    invoice.InvoiceTotal = InvoiceTotal;
                    invoice.Discount = Discount;

                    invoice.AmmountToPay = Math.Max(0m, InvoiceTotal - Discount);

                    decimal totalPaid = invoice.Payments?.Sum(p => p.PaidValue ?? 0m) ?? 0m;
                    invoice.TotalPaid = totalPaid;

                    invoice.Remain = Math.Max(0m, invoice.AmmountToPay.GetValueOrDefault() - totalPaid);

                    await _invoiceService.UpdateInvoice(invoice);
                }

                return RedirectToAction("Payments", new
                {
                    visitId = VisitId,
                    treatmentId = TreatmentId,
                    treatmentName = TreatmentName,
                    price = InvoiceTotal
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Payments", new
                {
                    visitId = VisitId,
                    treatmentId = TreatmentId,
                    treatmentName = TreatmentName,
                    price = InvoiceTotal
                });
            }
        }




        public IActionResult Print(int invoiceId)
        {
            var invoice = _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefault(i => i.Id == invoiceId);

            if (invoice == null)
                return NotFound();

            return View(invoice); 
        }
        public async Task<IActionResult> Payments(int visitId, int treatmentId, string treatmentName, decimal price)
        {
            var invoice = await _invoiceService.GetOrCreateInvoiceForTreatment(visitId, treatmentId, price);
            var payments = await _invoiceService.GetAllPaymentInvoice(invoice.Id);

            ViewBag.InvoiceId = invoice.Id;
            ViewBag.VisitId = visitId;
            ViewBag.TreatmentId = treatmentId;
            ViewBag.TreatmentName = treatmentName;
            ViewBag.InvoiceTotal = (invoice.InvoiceTotal ?? 0).ToString("0.000");
            ViewBag.Discount = (invoice.Discount ?? 0).ToString("0.000");
            ViewBag.AmountToPay = (invoice.AmmountToPay ?? 0).ToString("0.000");
            ViewBag.TotalPaid = (invoice.TotalPaid ?? 0).ToString("0.000");
            ViewBag.Remain = (invoice.Remain ?? 0).ToString("0.000");

            return View(payments);

        }
    }
}