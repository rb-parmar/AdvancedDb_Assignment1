namespace WebApplication2.Models
{
    public class StoreHasLaptops
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }
        public StoreLocation Location { get; set; }

        public Guid LaptopId { get; set; }
        public Laptop Laptop { get; set; }

        public int LaptopQuantity { get; set; }

        public StoreHasLaptops() { }
        public StoreHasLaptops(StoreLocation storeLocation, Laptop laptop, int quantity)
        {
            StoreId = storeLocation.Id;
            Location = storeLocation;

            LaptopId = laptop.ID; 
            Laptop = laptop;  

            LaptopQuantity = quantity;

            storeLocation.StoreHasLaptops.Add(this);
            laptop.LaptopsInStore.Add(this);
        }
    }

}
