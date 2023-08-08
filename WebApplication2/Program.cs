using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication2.Database;
using WebApplication2.Models;
using Microsoft.AspNetCore.Http.Json;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Adding DbContext to the app.
string connectionString = builder.Configuration.GetConnectionString("LaptopStoreConnection");

builder.Services.Configure<JsonOptions>(options => {
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<LaptopStoreContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    await SeedData.Initialize(serviceProvider);
}

// endpoints
app.MapGet("stores/{storeNumber}/inventory", (LaptopStoreContext db, Guid storeNumber) =>
{
    try
    {
        if (!db.locations.Any(s => s.Id == storeNumber))
        {
            throw new ArgumentOutOfRangeException(nameof(db));
        }

        List<Laptop> laptops = db.laptops.Where(l => l.LaptopsInStore.Any(s => s.Id == storeNumber && s.LaptopQuantity > 0)).ToList();

        return Results.Ok(laptops);
    } catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapGet("/{storeNumber}/{laptopNumber}/changeQuantity", (LaptopStoreContext db , Guid storeNumber, Guid laptopNumber, int quantity) =>
{
    try
    {
        StoreHasLaptops? store = db.StoreHasLaptops.FirstOrDefault(s => s.Id == storeNumber && s.LaptopId == laptopNumber);

        if (store == null)
        {
            throw new ArgumentOutOfRangeException($"{storeNumber} is not found.");
        }

        store.LaptopQuantity = quantity;
        db.SaveChanges();

        return Results.Accepted("Qantity successfully updated.", store);
    } catch (Exception e) 
    {
        return Results.BadRequest(e.Message);
    }
});

app.MapGet("/brands/{brandId}/laptops/averagePrice", (LaptopStoreContext db, Guid brandId) =>
{
    try
    {
        List<Laptop> laptops = db.laptops.Where(l => l.BrandId == brandId).ToList();

        if (laptops.Count <= 0)
        {
            throw new ArgumentOutOfRangeException("Specified laptop brand could not be found.");
        }
        int laptopCount = laptops.Count();
        decimal avgPrice = laptops.Average(l => l.Price);

        object output = new
        {
            LaptopCount = laptopCount,
            AveragePrice = avgPrice, 
        };

        return Results.Ok(output);

    } catch (Exception ex)
    {
        return Results.BadRequest($"{ex.Message}");
    }
});

app.MapGet("/stores/groupedByProvince", (LaptopStoreContext db) => 
{
    try
    {
        List<StoresInProvince> storesInProvince =
        db.locations
            .GroupBy(s => s.Province)
            .Where(g => g.Any()) // empty provinces
            .Select(g => new StoresInProvince
            {
                Province = g.Key.ToString(),
                Stores = g.ToList()
            }).ToList();

        return Results.Ok(storesInProvince);
    } catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapGet("/laptops/search", (LaptopStoreContext db, decimal? amountAbove, decimal? amountBelow, Guid? storeId, string? province, LaptopCondition? condition, Guid? BrandId, string? searchPhrase) =>
{
    List<Laptop> laptops = new List<Laptop>();

    if (amountAbove < 0 || amountBelow < 0)
    {
        throw new ArgumentException("Price cannot be less than 0");
    }

    if (amountAbove.HasValue)
    {
        laptops = db.laptops.Where(l => l.Price > amountAbove).ToList();
    } else if (amountAbove.HasValue)
    {
        laptops = db.laptops.Where(l => l.Price < amountBelow).ToList();
    }

    if (!string.IsNullOrEmpty(province))
    {
        laptops = db.laptops.Where(l => l.LaptopsInStore.Any(s => s.Location.Province == province)).ToList();
    }

    if (condition.HasValue)
    {
        laptops = db.laptops.Where(l => l.Condition == condition).ToList();
    }

    if (BrandId.HasValue)
    {
        laptops = db.laptops.Where(l => l.BrandId == BrandId).ToList();
    }

    if (!string.IsNullOrEmpty(searchPhrase))
    {
        laptops = db.laptops.Where(l => l.Model.Contains(searchPhrase)).ToList();
    }

    return Results.Ok(laptops);
});

app.Run();

class StoresInProvince
{
    public string Province { get; set; }
    public List<StoreLocation> Stores { get; set; }
}


