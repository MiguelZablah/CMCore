using CMCore.Models;
using CMCore.Models.RelationModel;
using Microsoft.EntityFrameworkCore;
using Type = CMCore.Models.Type;

namespace CMCore.Data
{
    public class ContentManagerDbContext : DbContext
    {
        public ContentManagerDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<File> Files { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<FileClub> FileClubs { get; set; }
        public DbSet<FileTag> FileTags { get; set; }
        public DbSet<FileCompany> FileCompanies { get; set; }
        public DbSet<ClubType> ClubTypes { get; set; }
        public DbSet<ClubRegion> ClubRegions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many to Many
            modelBuilder.Entity<FileTag>()
                .HasKey(ft => new { ft.FileId, ft.TagId });

            modelBuilder.Entity<FileClub>()
                .HasKey(fc => new { fc.FileId, fc.ClubId });

            modelBuilder.Entity<FileCompany>()
                .HasKey(fco => new { fco.FileId, CompanieId = fco.CompanyId });

            modelBuilder.Entity<ClubType>()
                .HasKey(ct => new { ct.ClubId, ct.TypeId });

            modelBuilder.Entity<ClubRegion>()
                .HasKey(cr => new { cr.ClubId, cr.RegionId });

            // One to Many
            modelBuilder.Entity<Extension>()
                .HasMany(e => e.Files)
                .WithOne(f => f.Extension)
                .HasForeignKey(f => f.ExtensionId);

            modelBuilder.Entity<Region>()
                .HasMany(c => c.Countries)
                .WithOne(c => c.Region)
                .HasForeignKey(c => c.RegionId).IsRequired(false);

        }
    }
}
