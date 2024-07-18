using Lab6.Models;
using Lab6.Models.Prediction;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Data
{
    public class PredictionDataContext : DbContext
    {
        public PredictionDataContext(DbContextOptions<PredictionDataContext> options) : base(options) { }
        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prediction>().ToTable("PredictionTable");
        }
    }
}
