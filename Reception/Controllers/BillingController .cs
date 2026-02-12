using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using Reception.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        private bool CanEditPayment()
        {
            return HttpContext.Session.GetString("CanEditPayment") == "True";
        }

    
        public async Task<IActionResult> Invoices(int visitId)
        {
            var invoices = await _invoiceService.GetAllInvoice(visitId);
            return View(invoices);
        }

     
        [HttpPost]
        public async Task<IActionResult> AddInvoice(AddInvoiceDto dto)
        {
            if (!CanEditPayment())
                return Unauthorized();

            if (!ModelState.IsValid)
                return View(dto);

            await _invoiceService.AddInvoice(dto);
            return RedirectToAction("Invoices");
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

            // ✅ هذا السطر المهم
            var canEdit = HttpContext.Session.GetString("CanEditPayment") == "true";
            ViewBag.CanEditPayment = canEdit;

            return View(payments);
        }



        [HttpPost]
        public async Task<IActionResult> AddPayment(Payment payment, decimal Discount)
        {
           

            if (!ModelState.IsValid)
                return View(payment);

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == payment.IdInvoice);

            if (invoice == null)
                return RedirectToAction("Index");

             invoice.Discount = Discount;
            invoice.AmmountToPay = (invoice.InvoiceTotal ?? 0) - Discount;

             var totalPaidBefore = await _context.Payments
                .Where(p => p.IdInvoice == invoice.Id && p.IsActive)
                .SumAsync(p => p.PaidValue ?? 0);

            var remaining = invoice.AmmountToPay - totalPaidBefore;


            if ((payment.PaidValue ?? 0) > remaining)
            {
                TempData["PaymentError"] = $"لا يمكن إدخال دفعة أكبر من المتبقي ({remaining:0.000})";
                return RedirectToAction("Payments", new
                {
                    visitId = invoice.VisitId,
                    treatmentId = invoice.TreatmentId,
                    treatmentName = "Treatment",
                    price = invoice.InvoiceTotal ?? 0
                });
            }


            payment.IsActive = true;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

             invoice.TotalPaid = totalPaidBefore + (payment.PaidValue ?? 0);
            invoice.Remain = invoice.AmmountToPay - invoice.TotalPaid;

            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

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
            if (!CanEditPayment())
                return Unauthorized();

            var invoice = await _invoiceService.GetInvoiceById(InvoiceId);

            if (invoice != null)
            {
                invoice.InvoiceTotal = InvoiceTotal;
                invoice.Discount = Discount;

                invoice.AmmountToPay = Math.Max(0m, InvoiceTotal - Discount);

                decimal totalPaid =
                    invoice.Payments?.Sum(p => p.PaidValue ?? 0m) ?? 0m;

                invoice.TotalPaid = totalPaid;
                invoice.Remain = Math.Max(
                    0m,
                    invoice.AmmountToPay.GetValueOrDefault() - totalPaid);

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

      
        public IActionResult Print(int invoiceId)
        {
            var invoice = _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefault(i => i.Id == invoiceId);

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }
    }
}
