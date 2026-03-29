using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    public class SparqDbContextFactory : IDesignTimeDbContextFactory<SparqDbContext>
    {
        public SparqDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddUserSecrets<SparqDbContextFactory>() // 🔥 EZ A LÉNYEG
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SparqDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new SparqDbContext(optionsBuilder.Options);
        }
    }
}
