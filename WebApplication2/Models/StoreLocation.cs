namespace WebApplication2.Models
{
    public class StoreLocation
    {
        public Guid Id { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        private string _province;
        public string Province { 
            get { return _province; }
            set 
            {
                List<string> CanadianProvinces = new List<string>
                {
                    "Alberta", "British Columbia", "Manitoba", "New Brunswick",
                    "Newfoundland and Labrador", "Nova Scotia", "Ontario",
                    "Prince Edward Island", "Quebec", "Saskatchewan"
                };

                if (CanadianProvinces.Any(cp => cp.ToLower().Equals(value.ToLower())))
                {
                    _province = value;
                }
            }
        }
        public int LaptopQuantity { get; set; }

        public HashSet<StoreHasLaptops> StoreHasLaptops { get; set; } = new HashSet<StoreHasLaptops>();
    }
}
