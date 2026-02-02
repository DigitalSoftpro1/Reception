using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using Reception.View_Model;
using Reception.Extensions;
using System.Linq;
using System;

namespace Reception.Controllers
{
    public class TransactionVisitController : Controller
    {
        private readonly ReceptionDbContext _context;

        public TransactionVisitController(ReceptionDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var visits = _context.TransactionVisit
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.VisitDate)
                .ToList()
                .ToViewModelList();

            return View(visits);
        }

        public IActionResult Details(int id)
        {
            var visit = _context.TransactionVisit
                .FirstOrDefault(x => x.Id == id && x.IsActive);

            if (visit == null)
                return NotFound();

            return PartialView("_Details", visit.ToViewModel());
        }
    }
}
