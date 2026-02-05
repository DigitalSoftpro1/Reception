using Microsoft.AspNetCore.Mvc;
using Reception.IService;
using Reception.Models;
using System;
using System.Threading.Tasks;

namespace Reception.Controllers
{
    public class ClinicController : Controller
    {
        private readonly IServices _clinicService;

        public ClinicController(IServices clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clinics = await _clinicService.GetAllClinics();
            return View(clinics);
        }

        [HttpGet]
        public async Task<IActionResult> GetTreatments(int clinicId)
        {
            try
            {
                var treatments = await _clinicService.GetAllTreatmentClinic(clinicId);
                return Json(treatments); // سنحول JSON إلى جدول في JS
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClinic(string ClinicName)
        {
            try
            {
                await _clinicService.AddClinic(ClinicName);
                TempData["Success"] = "Clinic added successfully!";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddTreatment(addTreatmentDto dto)
        {
            try
            {
                await _clinicService.AddTreatment(dto);
                TempData["Success"] = "Treatment added successfully!";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
