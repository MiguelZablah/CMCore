using CMCore.Models;
using CMCore.Models.RelacionClass;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Type = CMCore.Models.Type;

namespace CMCore.Data
{
    public static class SeedData
    {
        public static IWebHost SeedDatabase(this IWebHost webHost)
        {
            //using (var c = new ContentManagerDbContext(new DbContextOptions<ContentManagerDbContext>()))
            //{
            //    c.Clubs.Add(new Club());
            //}

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var _context = services.GetRequiredService<ContentManagerDbContext>();
                    TablesToSeed(_context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            return webHost;
        }

        public static void TablesToSeed(ContentManagerDbContext _context)
        {
            _context.Database.Migrate();

            if (!_context.Files.Any())
            {
                _context.Files.Add(new File { Name = "FileTest", Description = "Testing a file", ExtensionId = 1 });
                _context.SaveChanges();
            }

            if (!_context.Clubs.Any())
            {
                _context.Clubs.Add(new Club { Name = "Bebes Paradise" });
                _context.SaveChanges();
            }

            if (!_context.Companies.Any())
            {
                _context.Companies.Add(new Companie { Name = "SVA" });
                _context.SaveChanges();
            }

            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Countrie { Name = "Costa Rica", RegionId = 1 });
                _context.SaveChanges();
            }


            if (!_context.Extensions.Any())
            {
                _context.Extensions.Add(new Extension { Name = "JPG" });
                _context.SaveChanges();
            }

            if (!_context.Regions.Any())
            {
                _context.Regions.Add(new Region { Name = "Cenam" });
                _context.SaveChanges();
            }

            if (!_context.Tags.Any())
            {
                _context.Tags.Add(new Tag { Name = "Sexy" });
                _context.SaveChanges();
            }

            if (!_context.Types.Any())
            {
                _context.Types.Add(new Type { Name = "Mainstream" });
                _context.SaveChanges();
            }

            if (!_context.FileClubs.Any())
            {
                _context.FileClubs.Add(new FileClub { FileId = 1, ClubId = 1 });
                _context.SaveChanges();
            }

            if (!_context.FileCompanies.Any())
            {
                _context.FileCompanies.Add(new FileCompanie { FileId = 1, CompanieId = 1 });
                _context.SaveChanges();
            }

            if (!_context.FileTags.Any())
            {
                _context.FileTags.Add(new FileTag { FileId = 1, TagId = 1 });
                _context.SaveChanges();
            }

            if (!_context.ClubRegions.Any())
            {
                _context.ClubRegions.Add(new ClubRegion { ClubId = 1, RegionId = 1 });
                _context.SaveChanges();
            }

            if (!_context.ClubTypes.Any())
            {
                _context.ClubTypes.Add(new ClubType { ClubId = 1, TypeId = 1 });
                _context.SaveChanges();
            }
        }
    }
}
