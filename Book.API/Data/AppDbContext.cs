using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Data
{
    public class AppDbContext:IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData( new Product() { ProductId = 1, ProductName ="Kitap 1", Price=10});
            modelBuilder.Entity<Product>().HasData( new Product() { ProductId = 2, ProductName ="Kitap 2", Price=20});
            modelBuilder.Entity<Product>().HasData( new Product() { ProductId = 3, ProductName ="Kitap 3", Price=30});
                
        }
        
        public DbSet<Product> Products { get; set; }
    }
}