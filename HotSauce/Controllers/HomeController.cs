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
            Sauce[] sauces = _db.Sauces.ToArray();
            Dictionary<string, object[]> model = new Dictionary<string, object[]>();
            model.Add("sauces", sauces);
            Flavor[] flavors = _db.Flavors.ToArray();
            model.Add("flavors", flavors);
            return View(model);
        }
    }
}