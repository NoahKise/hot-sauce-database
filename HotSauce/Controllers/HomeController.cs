using Microsoft.AspNetCore.Mvc;
using HotSauce.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace HotSauce.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotSauceContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager, HotSauceContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}