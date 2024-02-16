using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotSauce.Models
{
    public class Sauce
    {
        public int SauceId { get; set; }
        [Required(ErrorMessage = "You must provide a name for the sauce")]
        public string Name { get; set; }
        public List<FlavorSauce> JoinEntities { get; }
        public ApplicationUser User { get; set; }
    }
}