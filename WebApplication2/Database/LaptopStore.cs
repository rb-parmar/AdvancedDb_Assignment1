using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using System;

namespace WebApplication2.Database
{
    class LaptopStoreContext : DbContext
    {
        public LaptopStoreContext(DbContextOptions options) : base(options) { }

        public DbSet<Brand> brands { get; set; } = null!;
        public DbSet<Laptop> laptops { get; set; } = null!;
    }
}
