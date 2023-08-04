namespace WebApplication2.Models
{
    public class StoreHasLaptops
    {   
        public Guid StoreId { get; set; }
        public StoreLocation Location { get; set; }

        public Guid LaptopId { get; set; }
        public Laptop Laptop { get; set; }

        public StoreHasLaptops(StoreLocation storeLocation, Laptop laptop)
        {
            StoreId = storeLocation.Id;
            Location = storeLocation;

            LaptopId = laptop.ID; 
            Laptop = laptop;  
        }
    }

}
