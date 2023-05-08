using AvaiabilityReportApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AvaiabilityReportApi.Data
{
    public class GymAvaiabilityDbContext : DbContext
    {
        public GymAvaiabilityDbContext(DbContextOptions<GymAvaiabilityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<AvaiabilityReport> AvaiabilityReports { get; set; }
        public DbSet<MachinePlacement> MachinePlacements { get; set;}
        public DbSet <AvaiabilityReportFactSt> AvaiabilityReportFactSt { get; set; }

    }
}
