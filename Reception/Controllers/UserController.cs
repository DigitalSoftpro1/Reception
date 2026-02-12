using Microsoft.AspNetCore.Mvc;
using Reception.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Reception.Controllers
{
    public class UserController : Controller
    {
        private readonly ReceptionDbContext _context;

        public UserController(ReceptionDbContext context)
        {
            _context = context;
        }

   
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Username);
            HttpContext.Session.SetString("CanEditPayment", user.CanEditPayment ? "true" : "false");

            return RedirectToAction("Index", "Clinic");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

      
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public  IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login"); 
            }

            return View(user);
        }

       
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    }
}
