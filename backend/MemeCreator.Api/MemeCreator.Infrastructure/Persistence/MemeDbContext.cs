using Microsoft.EntityFrameworkCore;
using MemeCreator.Domain.Entities;

namespace MemeCreator.Infrastructure.Persistence
{
    public class MemeDbContext : DbContext
    {
        public MemeDbContext(DbContextOptions<MemeDbContext> options)
            : base(options)
        {
        }

        public DbSet<MemeConfig> MemeConfigs { get; set; }

        // opcionalno: fluent config kasnije
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MemeConfig>(entity =>
            {
                entity.Property(x => x.TopText).HasMaxLength(500);
                entity.Property(x => x.BottomText).HasMaxLength(500);
                entity.Property(x => x.FontFamily).HasMaxLength(100);
                entity.Property(x => x.TextColor).HasMaxLength(50);
                entity.Property(x => x.StrokeColor).HasMaxLength(50);
                entity.Property(x => x.TextAlign).HasMaxLength(20);
                entity.Property(x => x.WatermarkPosition).HasMaxLength(50);
            });
        }
    }
}
