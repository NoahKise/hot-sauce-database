using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotSauce.Models
{
    public class SauceViewModel
    {
        [Required(ErrorMessage = "Sauce Name is required")]
        public string SauceName { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public List<FlavorViewModel> Flavors { get; set; } = new List<FlavorViewModel>();
    }

    public class FlavorViewModel
    {
        [Required(ErrorMessage = "Flavor Name is required")]
        public string Name { get; set; }
    }
}