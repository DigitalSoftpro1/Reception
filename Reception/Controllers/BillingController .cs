using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using System.Linq;
using System;
using Reception.IService;
using Reception.Services;

namespace Reception.Controllers
{
    public class BillingController : Controller
    {
        private readonly IBillingServices _invoiceService;

        public BillingController(IBillingServices context)
        {
            _invoiceService = context;
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
        public async Task<IActionResult> AddPayment(AddPaymentDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _invoiceService.AddPayment(dto);

            var invoice = await _invoiceService.GetInvoiceById(dto.IdInvoice);

            if (invoice == null)
                return View(dto);

             

            return RedirectToAction("Payments", new
            {
                visitId = invoice.VisitId,
                treatmentId = invoice.TreatmentId,
                treatmentName = "Treatment",
                price = invoice.InvoiceTotal ?? 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInvoiceDetails(int InvoiceId, int VisitId, int TreatmentId,
                                                               string TreatmentName, decimal InvoiceTotal, decimal Discount)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceById(InvoiceId);

                if (invoice != null)
                {
                    // Update invoice details
                    invoice.InvoiceTotal = InvoiceTotal;
                    invoice.Discount = Discount;
                    invoice.AmmountToPay = InvoiceTotal - Discount;

                    // Recalculate Total Paid and Remaining
                    decimal totalPaid = invoice.Payments?.Sum(p => p.PaidValue ?? 0) ?? 0;
                    invoice.TotalPaid = totalPaid;
                    invoice.Remain = invoice.AmmountToPay - totalPaid;

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