using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Database
{
    public class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            LaptopStoreContext db = new LaptopStoreContext(serviceProvider.GetRequiredService<DbContextOptions<LaptopStoreContext>>());

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            // Brands
            Brand BOne = new Brand("Dell");
            Brand BTwo = new Brand("HP");
            Brand BThree = new Brand("Lenovo");

            if (!db.brands.Any())
            {
                db.brands.Add(BOne);
                db.brands.Add(BTwo);
                db.brands.Add(BThree);

                db.SaveChanges();
            }


            // Laptops
            Laptop LOne = new Laptop("ThinkPad", 1250, BThree, LaptopCondition.New);
            Laptop LTwo = new Laptop("ThinkBook", 1000, BThree, LaptopCondition.Rental);
            Laptop LThree = new Laptop("Envy", 1100, BTwo, LaptopCondition.New);
            Laptop LFour = new Laptop("Omen", 650, BTwo, LaptopCondition.Refurbished);
            Laptop LFive = new Laptop("Vostro", 900, BOne, LaptopCondition.Rental);
            Laptop LSix = new Laptop("XPS", 850, BOne, LaptopCondition.Refurbished);

            if(!db.laptops.Any())
            {
                db.laptops.Add(LOne);
                db.laptops.Add(LTwo);
                db.laptops.Add(LThree);
                db.laptops.Add(LFour);
                db.laptops.Add(LFive);
                db.laptops.Add(LSix);

                db.SaveChanges();
            }


            // StoreLocation
            StoreLocation S1 = new StoreLocation("Pembina Highway", 1650, "Manitoba");
            StoreLocation S2 = new StoreLocation("Ellice Ave", 88, "Manitoba");
            StoreLocation S3 = new StoreLocation("McDonald st", 54, "Alberta");

            if (!db.locations.Any())
            {
                db.locations.Add(S1);
                db.locations.Add(S2);
                db.locations.Add(S3);

                db.SaveChanges();
            }

            // StoreHasLaptops
            StoreHasLaptops SL01 = new StoreHasLaptops(S1, LOne, 6);
            StoreHasLaptops SL02 = new StoreHasLaptops(S2, LFour, 4);
            StoreHasLaptops SL03 = new StoreHasLaptops(S3, LTwo, 2);
            StoreHasLaptops SL04 = new StoreHasLaptops(S1, LFive, -6);
            StoreHasLaptops SL05 = new StoreHasLaptops(S2, LThree, 3);
            StoreHasLaptops SL06 = new StoreHasLaptops(S3, LSix, 5);

            if (!db.StoreHasLaptops.Any())
            {
                db.StoreHasLaptops.Add(SL01);
                db.StoreHasLaptops.Add(SL02);
                db.StoreHasLaptops.Add(SL03);
                db.StoreHasLaptops.Add(SL04);
                db.StoreHasLaptops.Add(SL05);
                db.StoreHasLaptops.Add(SL06);

                db.SaveChanges();
            }

        }
    }
}
