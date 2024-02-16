using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotSauce.Models
{
    public class Sauce
    {
        public int SauceId { get; set; }
        [Required(ErrorMessage = "You must provide a name for the sauce")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public List<FlavorSauce> JoinEntities { get; } = new List<FlavorSauce>();
        public ApplicationUser User { get; set; }
    }
}