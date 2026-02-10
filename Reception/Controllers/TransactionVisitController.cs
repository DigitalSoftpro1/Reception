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
        public TransactionVisitController(IServices service )
        {
            _service = service;
         }

        [HttpGet]
        public async Task<IActionResult> Visits(string patientName = null, string phoneNumber = null, DateTime? visitDate = null, int? clinicId = null)
        {
            var visits = await _service.GetAllVisit(patientName, phoneNumber, visitDate);

            if (clinicId.HasValue)
            {
                visits = visits.Where(v => v.ClinicId == clinicId.Value).ToList();
            }

            var clinics = await _service.GetAllClinics(); 

            ViewBag.Clinics = clinics; 
            return View(visits);
        }
        [HttpGet]
        public async Task<JsonResult> GetPatientInfo(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return Json(new { success = false });

            try
            {
                var allVisits = await _service.GetAllVisit();

                var patient = allVisits
                    .Where(v =>
                        (!string.IsNullOrEmpty(v.PatientName) &&
                         v.PatientName.Contains(search, StringComparison.OrdinalIgnoreCase))
                        ||
                        (!string.IsNullOrEmpty(v.PhoneNumber) &&
                         v.PhoneNumber.Contains(search))
                    )
                    .OrderByDescending(v => v.VisitDate) 
                    .FirstOrDefault();

                if (patient == null)
                {
                    return Json(new { success = false });
                }

                return Json(new
                {
                    success = true,                 
                    patientName = patient.PatientName,
                    phoneNumber = patient.PhoneNumber
                });
            }
            catch
            {
                return Json(new { success = false });
            }
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