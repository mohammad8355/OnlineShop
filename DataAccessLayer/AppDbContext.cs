using BusinessEntity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AppDbContext :IdentityDbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-5FTTMPU\\MSSQLSERVER2022;database=ElectricShopDB;Trusted_Connection=true;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
        DbSet<Product> products { get; set; }
        DbSet<Image> images { get; set; }
        DbSet<Brand> brands { get; set; }
        DbSet<ProductAttribute> productAttributes { get; set; }
        DbSet<Category> categories { get; set; }
    }
}
