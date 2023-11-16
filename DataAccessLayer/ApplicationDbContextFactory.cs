using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer("server=DESKTOP-5FTTMPU\\MSSQLSERVER2022;database=ElectricShopDB;Trusted_Connection=true;TrustServerCertificate=True");

                return new AppDbContext(optionsBuilder.Options);
            }
        }
}
