using Microsoft.AspNetCore.Mvc;
using HotSauce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

public class SaucesController : Controller
{
    private readonly HotSauceContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public SaucesController(UserManager<ApplicationUser> userManager, HotSauceContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    public ActionResult Index()
    {
        List<Sauce> model = _db.Sauces.ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new SauceViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> Create(SauceViewModel model)
    {
        if (ModelState.IsValid)
        {
            string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            ViewBag.UserId = currentUser.Id;
            Sauce sauce = new Sauce
            {
                Name = model.SauceName,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                UserId = model.UserId
            };

            foreach (var flavorViewModel in model.Flavors)
            {
                string flavorName = flavorViewModel.Name.Trim();
                Flavor existingFlavor = _db.Flavors.FirstOrDefault(i => i.Name == flavorName);
                Flavor flavor;
                if (existingFlavor != null)
                {
                    flavor = existingFlavor;
                }
                else
                {
                    flavor = new Flavor { Name = flavorName };
                    _db.Flavors.Add(flavor);
                }
                FlavorSauce flavorSauce = new FlavorSauce { Flavor = flavor, Sauce = sauce };
                sauce.JoinEntities.Add(flavorSauce);
                _db.FlavorSauces.Add(flavorSauce);
            }
            sauce.User = currentUser;
            _db.Sauces.Add(sauce);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(model);
    }

    public ActionResult Details(int id)
    {
        Sauce sauce = _db.Sauces
            .Include(s => s.JoinEntities)
            .ThenInclude(join => join.Flavor)
            .FirstOrDefault(s => s.SauceId == id);
        return View(sauce);
    }
}