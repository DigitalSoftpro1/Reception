using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using Reception.View_Model;
using Reception.Extensions;
using System.Linq;
using System;

namespace Reception.Controllers
{
    public class TransactionVisitTreatmentController : Controller
    {
        private readonly ReceptionDbContext _context;

        public TransactionVisitTreatmentController(ReceptionDbContext context)
        {
            _context = context;
        }

        // تحميل العلاجات
        public IActionResult List(int visitId)
        {
            var treatments = _context.TransactionVisitTreatment
                .Where(x => x.TransactionVisitId == visitId && x.IsActive)
                .ToList()
                .ToViewModelList();

            return PartialView("_List", treatments);
        }

        // إضافة علاج
        [HttpPost]
        public IActionResult Create(TransactionVisitTreatmentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var entity = model.ToModel();
            entity.IsActive = true;

            _context.TransactionVisitTreatment.Add(entity);
            _context.SaveChanges();

            return Ok();
        }

        // حذف علاج
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var entity = _context.TransactionVisitTreatment.FirstOrDefault(x => x.Id == id);

            if (entity == null)
                return NotFound();

            entity.IsActive = false;
            _context.SaveChanges();

            return Ok();
        }
    }
}
