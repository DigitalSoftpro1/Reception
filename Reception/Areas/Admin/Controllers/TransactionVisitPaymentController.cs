using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using Reception.View_Model;
using Reception.Extensions;
using System.Linq;
using System;

namespace Reception.Controllers
{
    public class TransactionVisitPaymentController : Controller
    {
        private readonly ReceptionDbContext _context;

        public TransactionVisitPaymentController(ReceptionDbContext context)
        {
            _context = context;
        }

        // تحميل الدفعات
        public IActionResult List(int visitId)
        {
            var payments = _context.TransactionVisitPayment
                .Where(x => x.TransactionVisitId == visitId && x.IsActive)
                .ToList()
                .ToViewModelList();

            return PartialView("_List", payments);
        }

        [HttpPost]
        public IActionResult Create(TransactionVisitPaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var entity = model.ToModel();
            entity.IsActive = true;

            _context.TransactionVisitPayment.Add(entity);
            _context.SaveChanges();

            return Ok();
        }
    }
}
