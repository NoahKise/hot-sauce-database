namespace HotSauce.Models
{
    public class FlavorSauce
    {
        public int FlavorSauceId { get; set; }
        public int SauceId { get; set; }
        public Sauce Sauce { get; set; }
        public int FlavorId { get; set; }
        public Flavor Flavor { get; set; }
    }
}