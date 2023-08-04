using Microsoft.EntityFrameworkCore;
using WebApplication2.Database;

var builder = WebApplication.CreateBuilder(args);

// Adding DbContext to the app.
string connectionString = builder.Configuration.GetConnectionString("LaptopStoreConnection");

builder.Services.AddDbContext<LaptopStoreContext>(options =>
{
    options.UseSqlServer(connectionString);
});


var app = builder.Build();

app.Run();
