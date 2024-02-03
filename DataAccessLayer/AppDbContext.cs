﻿using BusinessEntity;
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
            optionsBuilder.UseSqlServer("server=DESKTOP-5FTTMPU\\MSSQLSERVER2022;database=SalehDb;Trusted_Connection=true;TrustServerCertificate=True");
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
        
            base.OnModelCreating(builder);
            builder.Entity<AdjValue>().Property(p => p.Value).IsRequired(true);
            builder.Entity<General>().Property(p => p.label).IsRequired(false);
            builder.Entity<General>().Property(p => p.Link).IsRequired(false);
            builder.Entity<General>().Property(p => p.Description).IsRequired(false);
            builder.Entity<KeyToProduct>().HasKey(o => new {o.Key_Id , o.Product_Id});
            builder.Entity<KeyToSubCategory>().HasKey(o => new { o.key_Id, o.SubCategory_Id });
            builder.Entity<ValueToProduct>().HasKey(o => new { o.Value_Id, o.Product_Id });
            builder.Entity<AdjKey>().HasMany(c => c.adjValues).WithOne(a => a.adjKey).HasForeignKey(d => d.adjkey_Id);
            builder.Entity<KeyToProduct>().HasOne(c => c.adjKey).WithMany(b => b.KeyToProducts).HasForeignKey(v => v.Key_Id);
            builder.Entity<KeyToProduct>().HasOne(c => c.product).WithMany(b => b.keyToProducts).HasForeignKey(v => v.Product_Id);
            builder.Entity<KeyToSubCategory>().HasOne(c => c.adjKey).WithMany(b => b.keyToSubCategories).HasForeignKey(v => v.key_Id);
            builder.Entity<KeyToSubCategory>().HasOne(c => c.subCategory).WithMany(b => b.keyToSubCategories).HasForeignKey(v => v.SubCategory_Id);
            builder.Entity<Category>().HasMany(c => c.ChildCategories).WithOne(c => c.ParentCategory).HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasMany(c => c.orderDetails).WithOne(s => s.order).HasForeignKey(f => f.order_Id);
            builder.Entity<DiscountToProduct>().HasKey(k => new { k.Discount_Id, k.Product_Id });
            builder.Entity<DiscountToProduct>().HasOne(c => c.discount).WithMany(b => b.discountToProducts).HasForeignKey(v => v.Discount_Id);
            builder.Entity<DiscountToProduct>().HasOne(c => c.product).WithMany(b => b.discountToProducts).HasForeignKey(v => v.Product_Id);
            builder.Entity<Product>().HasMany(p => p.ProductPhotos).WithOne(p => p.product).HasForeignKey(p => p.Product_Id);
            builder.Entity<CategoryToProduct>().HasKey(o => new { o.Product_Id, o.Category_Id });
            builder.Entity<CategoryToProduct>().HasOne(c => c.Category).WithMany(p => p.CategoryToProducts).HasForeignKey(l => l.Category_Id).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CategoryToProduct>().HasOne(c => c.Product).WithMany(p => p.CategoryToProducts).HasForeignKey(l => l.Product_Id).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ValueToProduct>().HasOne(vp => vp.Product).WithMany(vp => vp.valueToProducts).HasForeignKey(vp => vp.Product_Id);
            builder.Entity<ValueToProduct>().HasOne(vp => vp.Value).WithMany(vp => vp.valueToProducts).HasForeignKey(vp => vp.Value_Id);
        }
        DbSet<Product> products { get; set; }
        DbSet<AdjKey> adjKeys { get; set; }
        DbSet<AdjValue> adjValues { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<BlogPost> blogPost { get; set; }
        DbSet<Discount> discounts { get; set; }
        DbSet<KeyToProduct> keyToProducts { get; set; }
        DbSet<KeyToSubCategory> keyToSubCategories { get; set; }
        DbSet<CategoryToProduct> categoryToProducts { get; set; }
        DbSet<ValueToProduct> valueToProducts { get; set; }
        DbSet<Order> orders { get; set; }
        DbSet<OrderDetails> orderDetails { get; set; }
        DbSet<Contact> contacts { get; set; }
        DbSet<General> generals { get; set; }
        DbSet<ProductPhoto> productPhotos { get; set; }
    }
}
