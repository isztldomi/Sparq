using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess
{
    /// <summary>
    /// Design-time DbContext factory.
    ///
    /// Az EF Core tooling (pl. migration generálás) futás közben
    /// nem a WebApi projektből indul, ezért ilyenkor manuálisan kell
    /// létrehozni a DbContext-et.
    ///
    /// Ezt a factory-t használja például:
    /// - dotnet ef migrations add
    /// - dotnet ef database update
    /// - Visual Studio Package Manager Console
    ///
    /// Így az EF Core tud adatbázis kapcsolatot létrehozni
    /// anélkül, hogy az egész alkalmazást el kellene indítani.
    /// </summary>
    public class SparqDbContextFactory : IDesignTimeDbContextFactory<SparqDbContext>
    {
        /// <summary>
        /// Létrehozza a SparqDbContext példányt design-time műveletekhez.
        /// </summary>
        public SparqDbContext CreateDbContext(string[] args)
        {
            // Lekéri az aktuális ASP.NET Core environmentet
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Konfiguráció felépítése
            var configuration = new ConfigurationBuilder()
                // Az aktuális futási könyvtár lesz a config alapja
                .SetBasePath(Directory.GetCurrentDirectory())
                // Általános appsettings
                .AddJsonFile("appsettings.json", optional: true)
                // Environment specifikus appsettings
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                // User Secrets támogatás
                .AddUserSecrets<SparqDbContextFactory>()
                // Konfiguráció buildelése
                .Build();

            // Connection string kiolvasása
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // DbContext opciók létrehozása
            var optionsBuilder = new DbContextOptionsBuilder<SparqDbContext>();
            // PostgreSQL provider konfigurálása
            optionsBuilder.UseNpgsql(connectionString);

            // DbContext példány visszaadása
            return new SparqDbContext(optionsBuilder.Options);
        }
    }
}
