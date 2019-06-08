using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Muuvis.Catalog.EntityFramework.DataModel
{
    internal class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>().Property(m => m.OriginalCulture).HasConversion(new CultureInfoValueConverter());
            modelBuilder.Entity<TitleTranslation>().Property(m => m.Culture).HasConversion(new CultureInfoValueConverter());
            modelBuilder.Entity<TitleTranslation>().HasKey(t => new { t.MovieId, t.Culture });

            modelBuilder.HasDefaultSchema("Catalog");
        }

        public DbSet<Movie> Movies { get; set; }
    }
}