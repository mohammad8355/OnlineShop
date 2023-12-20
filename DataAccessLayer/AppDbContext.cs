using BusinessEntity;
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
            optionsBuilder.UseSqlServer("server=DESKTOP-5FTTMPU\\MSSQLSERVER2022;database=Celebcom_DB;Trusted_Connection=true;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AdjValue>().Property(p => p.StringValue).IsRequired(false);
            builder.Entity<BlogSection>().Property(p => p.photo).IsRequired(false);
            builder.Entity<General>().Property(p => p.label).IsRequired(false);
            builder.Entity<General>().Property(p => p.Link).IsRequired(false);
            builder.Entity<General>().Property(p => p.Description).IsRequired(false);
            builder.Entity<KeyToProduct>().HasKey(o => new {o.Key_Id , o.Product_Id});
            builder.Entity<KeyToSubCategory>().HasKey(o => new { o.key_Id, o.SubCategory_Id });
            builder.Entity<AdjKey>().HasMany(c => c.adjValues).WithOne(a => a.adjKey);
            builder.Entity<KeyToProduct>().HasOne(c => c.adjKey).WithMany(b => b.KeyToProducts).HasForeignKey(v => v.Key_Id);
            builder.Entity<KeyToProduct>().HasOne(c => c.product).WithMany(b => b.keyToProducts).HasForeignKey(v => v.Product_Id);
            builder.Entity<KeyToSubCategory>().HasOne(c => c.adjKey).WithMany(b => b.keyToSubCategories).HasForeignKey(v => v.key_Id);
            builder.Entity<KeyToSubCategory>().HasOne(c => c.subCategory).WithMany(b => b.keyToSubCategories).HasForeignKey(v => v.SubCategory_Id);
            builder.Entity<SubCategory>().HasMany(c => c.products).WithOne(s => s.subCategory);
            builder.Entity<Category>().HasMany(c => c.subCategories).WithOne(s => s.category);
            builder.Entity<Weblog>().HasMany(c => c.blogSections).WithOne(s => s.Weblog);
            builder.Entity<Order>().HasMany(c => c.orderDetails).WithOne(s => s.order);
            builder.Entity<DiscountToProduct>().HasKey(k => new { k.Discount_Id, k.Product_Id });
            builder.Entity<DiscountToProduct>().HasOne(c => c.discount).WithMany(b => b.discountToProducts).HasForeignKey(v => v.Discount_Id);
            builder.Entity<DiscountToProduct>().HasOne(c => c.product).WithMany(b => b.discountToProducts).HasForeignKey(v => v.Product_Id);


        }
        DbSet<Product> products { get; set; }
        DbSet<AdjKey> adjKeys { get; set; }
        DbSet<AdjValue> adjValues { get; set; }
        DbSet<Category> categories { get; set; }
        DbSet<SubCategory> subCategories { get; set; }
        DbSet<Weblog> weblogs { get; set; }
        DbSet<BlogSection> blogSections { get; set; }
        DbSet<Discount> discounts { get; set; }
        DbSet<KeyToProduct> keyToProducts { get; set; }
        DbSet<KeyToSubCategory> keyToSubCategories { get; set; }
        DbSet<Order> orders { get; set; }
        DbSet<OrderDetails> orderDetails { get; set; }
        DbSet<Contact> contacts { get; set; }
        DbSet<General> generals { get; set; }
         
    }
}
