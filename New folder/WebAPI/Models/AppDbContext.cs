using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Vivo", Qty = 20, Rate = 30000,Profile="img.jpg",IsActive=true, CatId = 1 },
                new Product { Id = 2, Name = "Oppo", Qty = 45, Rate = 20000, Profile = "img.jpg", IsActive = true, CatId = 1 },
                new Product { Id = 3, Name = "Santoor", Qty = 20, Rate = 50, Profile = "img.jpg", IsActive = true, CatId = 2 },
                new Product { Id = 4, Name = "Dove", Qty = 20, Rate = 70, Profile = "img.jpg", IsActive = true, CatId = 2 }
               );

            modelBuilder.Entity<Category>().HasData(
              new Category { Id = 1, Name = "Mobiles" }
              ) ;

            modelBuilder.Entity<Subcategory>().HasData(
                new Subcategory { Sid = 1, Name = "Vivo", CatId = 1 }
                );

            

        }
    }
}
