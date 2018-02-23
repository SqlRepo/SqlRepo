using Microsoft.EntityFrameworkCore;

namespace SqlRepo.Benchmark
{
    public class SqlRepoBenchmarkDbContext : DbContext
    {
        public DbSet<BenchmarkEntity> BenchmarkEntities { get; set; }
        public DbSet<BenchmarkResult> BenchmarkResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString.Value);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BenchmarkEntity>().ToTable(nameof(BenchmarkEntity));
            builder.Entity<BenchmarkResult>().ToTable(nameof(BenchmarkResult));
        }
    }
}