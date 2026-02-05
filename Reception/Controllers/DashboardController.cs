using Microsoft.AspNetCore.Mvc;
using Reception.DTO;
using Reception.IService;
using Reception.Services; 
using System.Threading.Tasks;

namespace Reception.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IServices _services;

        public DashboardController(IServices services)
        {
            _services = services;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalVisits = await _services.GetTodayVisitsCount(),
                WaitingList = await _services.GetWaitingListCount(),
                TotalAmount = await _services.GetTodayTotalAmount()
            };

            return Json(stats);
        }
    }
}
