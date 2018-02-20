using CMCore.Models;
using CMCore.Models.RelacionClass;
using Microsoft.AspNetCore.Hosting;
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
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ContentManagerDbContext>();
                    TablesToSeed(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            return webHost;
        }

        public static void TablesToSeed(ContentManagerDbContext context)
        {
            //context.Database.Migrate();

            if (!context.Clubs.Any())
            {
                context.Clubs.Add(new Club { Name = "Bebes Paradise" });
                context.SaveChanges();
            }

            if (!context.Regions.Any())
            {
                context.Regions.Add(new Region { Name = "Cenam" });
                context.SaveChanges();
            }

            if (!context.Companies.Any())
            {
                context.Companies.Add(new Companie { Name = "SVA" });
                context.SaveChanges();
            }

            if (!context.Countries.Any())
            {
                context.Countries.Add(new Countrie { Name = "Costa Rica", RegionId = 1 });
                context.SaveChanges();
            }


            if (!context.Extensions.Any())
            {
                context.Extensions.Add(new Extension { Name = "JPG" });
                context.SaveChanges();
            }

            if (!context.Tags.Any())
            {
                context.Tags.Add(new Tag { Name = "Sexy" });
                context.SaveChanges();
            }

            if (!context.Types.Any())
            {
                context.Types.Add(new Type { Name = "Mainstream" });
                context.SaveChanges();
            }

            if (!context.Files.Any())
            {
                context.Files.Add(new File { Name = "FileTest", Description = "Testing a file", ExtensionId = 1 });
                context.SaveChanges();
            }

            if (!context.FileClubs.Any())
            {
                context.FileClubs.Add(new FileClub { FileId = 1, ClubId = 1 });
                context.SaveChanges();
            }

            if (!context.FileCompanies.Any())
            {
                context.FileCompanies.Add(new FileCompanie { FileId = 1, CompanieId = 1 });
                context.SaveChanges();
            }

            if (!context.FileTags.Any())
            {
                context.FileTags.Add(new FileTag { FileId = 1, TagId = 1 });
                context.SaveChanges();
            }

            if (!context.ClubRegions.Any())
            {
                context.ClubRegions.Add(new ClubRegion { ClubId = 1, RegionId = 1 });
                context.SaveChanges();
            }

            if (!context.ClubTypes.Any())
            {
                context.ClubTypes.Add(new ClubType { ClubId = 1, TypeId = 1 });
                context.SaveChanges();
            }
        }
    }
}
