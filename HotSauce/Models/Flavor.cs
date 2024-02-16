using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotSauce.Models
{
    public class Flavor
    {
        public int FlavorId { get; set; }
        public string Name { get; set; }
        public List<FlavorSauce> JoinEntities { get; }
    }
}