namespace Fuelcards.Models
{
    public class Site
    {
        public string? name { get; set; }
        public string? band { get; set; }
        public double? Surcharge { get; set; }
        public int? code { get; set; }   
        public double? transactionalSiteSurcharge { get;set; }
    }
}
