using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Muuvis.Taste.DomainModel;

namespace Muuvis.Taste.EntityFramework.DataModel
{
    internal class TasteEntities : DbContext
    {
        public TasteEntities(DbContextOptions options) : base(options)
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

            modelBuilder.HasDefaultSchema("Taste");
            modelBuilder.Entity<Suggestion>().HasKey(m => m.Id);
            modelBuilder.Entity<Suggestion>().Property(m => m.Id).HasMaxLength(36);
            modelBuilder.Entity<Suggestion>().Property(m => m.MovieId).HasMaxLength(36);
        }

        public DbSet<Suggestion> Suggestions { get; set; }
    }
}
