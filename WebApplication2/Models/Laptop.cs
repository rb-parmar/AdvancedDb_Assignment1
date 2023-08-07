using System.Reflection.Metadata.Ecma335;

namespace WebApplication2.Models
{
    public class Laptop
    {
        public Guid ID { get; set; }

        private string _model;
        
        public string Model
        {
            get => _model;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Laptop model name must be at least three characters in length.");
                }
            }
        }

        private decimal _price;

        public decimal Price { get => _price; 
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Price cannot be less than zero.");
                }

                _price = value;
            }
        }
        
        public LaptopCondition Condition { get; set; }
        
        public Guid BrandId { get; set; }
        
        public Brand Brand { get; set; }

        public HashSet<StoreHasLaptops> LaptopsInStore { get; set; } = new HashSet<StoreHasLaptops>();
        public Laptop() { }
        public Laptop(string model, decimal price, Brand brand, LaptopCondition laptopCondition) 
        {
            Model = model;
            Price = price;
            Brand = brand;
            BrandId = brand.Id;
            Condition = laptopCondition;

            brand.Laptops.Add(this);
        }
    }

    public enum LaptopCondition
    {
        New,
        Refurbished,
        Rental
    }

}
