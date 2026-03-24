using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Entity;

namespace QuantityMeasurementApp.Repository.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<QuantityMeasurementEntity> Measurements { get; set; }
    }
}
