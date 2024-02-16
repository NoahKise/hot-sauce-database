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

[Authorize]
public class SaucesController : Controller
{
    private readonly HotSauceContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public SaucesController(UserManager<ApplicationUser> userManager, HotSauceContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
        Sauce sauce = _db.Sauces
            .Include(s => s.JoinEntities)
            .ThenInclude(join => join.Flavor)
            .FirstOrDefault(s => s.SauceId == id);
        return View(sauce);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        Sauce sauce = _db.Sauces
            .Include(s => s.JoinEntities)
            .ThenInclude(join => join.Flavor)
            .FirstOrDefault(s => s.SauceId == id);

        if (sauce == null)
        {
            return NotFound();
        }

        return View(sauce);
    }

    [HttpPost]
    public IActionResult Edit(int id, Sauce model)
    {
        if (ModelState.IsValid)
        {
            Sauce existingSauce = _db.Sauces
                .Include(s => s.JoinEntities)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(s => s.SauceId == id);

            if (existingSauce == null)
            {
                return NotFound();
            }

            _db.FlavorSauces.RemoveRange(existingSauce.JoinEntities);

            existingSauce.Name = model.Name;
            existingSauce.Description = model.Description;
            existingSauce.ImageUrl = model.ImageUrl;

            foreach (var joinEntity in model.JoinEntities)
            {
                string trimmedFlavorName = joinEntity.Flavor.Name.Trim();
                Flavor existingFlavor = _db.Flavors.FirstOrDefault(f => f.Name == trimmedFlavorName);

                if (existingFlavor != null)
                {
                    joinEntity.Flavor = existingFlavor;
                }
                else
                {
                    Flavor newFlavor = new Flavor { Name = trimmedFlavorName };
                    joinEntity.Flavor = newFlavor;
                    _db.Flavors.Add(newFlavor);
                }

                existingSauce.JoinEntities.Add(new FlavorSauce
                {
                    Flavor = joinEntity.Flavor,
                    Sauce = existingSauce,
                });
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Sauce sauce = _db.Sauces.Find(id);
        if (sauce == null)
        {
            return NotFound();
        }

        return View(sauce);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        Sauce sauce = _db.Sauces.Find(id);
        if (sauce == null)
        {
            return NotFound();
        }

        _db.Sauces.Remove(sauce);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }
}