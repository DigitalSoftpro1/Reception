using Microsoft.AspNetCore.Mvc;
using Reception.Models;

using System.Linq;
using System;
using Reception.IService;
using Microsoft.EntityFrameworkCore;

namespace Reception.Controllers
{

    public class TransactionVisitController : Controller
    {
        private readonly IServices _service;

        public TransactionVisitController(IServices service)
        {
            _service = service;
        }
  
            public async Task<IActionResult> Visits()
            {
                var visits = await _service.GetAllVisit();
                var clinics = await _service.GetAllClinics();

                var viewModel = new VisitsViewModel
                {
                    Visits = visits ?? new List<Visit>(),
                    Clinics = clinics ?? new List<Clinic>()
                };

                return View(viewModel);
            }

             [HttpPost]
            public async Task<IActionResult> AddVisit(AddVisitDto dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        TempData["Error"] = "Please fill all required fields!";
                        return RedirectToAction("Visits");
                    }

                    await _service.AddVisit(dto);
                    TempData["Success"] = "Visit added successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }

                return RedirectToAction("Visits");
            }

             [HttpGet]
        public async Task<IActionResult> GetVisitTreatments(int visitId)
        {
            try
            {
                var treatments = await _service.GetAllVisitTreatment(visitId);
                var visit = await _service.GetVisitById(visitId);

              

                if (visit == null)
                {
                    TempData["Error"] = "Visit not found";
                    return RedirectToAction("Visits");
                }

                ViewBag.Visit = visit;

                return View(treatments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Visits");
            }
        }
        [HttpPost]
            public async Task<IActionResult> AddVisitTreatment(AddVisitTreatmentDto dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        TempData["Error"] = "Please fill all required fields!";
                        return RedirectToAction("Visits");
                    }

                    await _service.AddVisitTreatment(dto);
                    TempData["Success"] = "Treatment added successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }

                return RedirectToAction("Visits");
            }
    
            [HttpPost]
        public async Task<IActionResult> DeleteVisitTreatments([FromBody] List<int> treatmentIds)
        {
            try
            {
                if (treatmentIds == null || !treatmentIds.Any())
                {
                    return Json(new { success = false, message = "No treatments selected" });
                }

                foreach (var id in treatmentIds)
                {
                    await _service.DeleteVisitTreatment(id);
                }

                return Json(new { success = true, message = $"{treatmentIds.Count} treatment(s) deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllClinics()
        {
            try
            {
                var clinics = await _service.GetAllClinics();
                return Json(clinics.Select(c => new { c.Id, c.Name }));
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

    }
}