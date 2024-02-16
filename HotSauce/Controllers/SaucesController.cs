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
            // Retrieve the existing recipe from the database including related entities
            Sauce existingSauce = _db.Sauces
                .Include(s => s.JoinEntities)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(s => s.SauceId == id);

            if (existingSauce == null)
            {
                return NotFound();
            }

            // Clear existing IngredientRecipe relationships for the recipe
            _db.FlavorSauces.RemoveRange(existingSauce.JoinEntities);

            // Update properties of the existing recipe
            existingSauce.Name = model.Name;
            existingSauce.Description = model.Description;
            existingSauce.ImageUrl = model.ImageUrl;

            // Add new IngredientRecipe entities
            foreach (var joinEntity in model.JoinEntities)
            {
                // If the join has a new Ingredient, add it only if it doesn't already exist
                string trimmedFlavorName = joinEntity.Flavor.Name.Trim();
                Flavor existingFlavor = _db.Flavors.FirstOrDefault(f => f.Name == trimmedFlavorName);

                if (existingFlavor != null)
                {
                    // If the ingredient already exists, associate it with the join
                    joinEntity.Flavor = existingFlavor;
                }
                else
                {
                    // If the ingredient doesn't exist, add a new one
                    Flavor newFlavor = new Flavor { Name = trimmedFlavorName };
                    joinEntity.Flavor = newFlavor;
                    _db.Flavors.Add(newFlavor);
                }

                // Add new IngredientRecipe
                existingSauce.JoinEntities.Add(new FlavorSauce
                {
                    Flavor = joinEntity.Flavor,
                    Sauce = existingSauce,
                });
            }

            // Save changes to the database
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        return View(model);
    }
}