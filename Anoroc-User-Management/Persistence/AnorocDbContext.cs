using Anoroc_User_Management.Models.ItineraryFolder;
using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// This class specifies the Context of the Databse by using set Models to capture each Table in the database.
    /// It is used to create a complete picture of the database that can be used alongside the database
    /// </summary>
    public class AnorocDbContext : DbContext
    {
        public AnorocDbContext(DbContextOptions<AnorocDbContext> options) : base(options) { }
        public DbSet<Location> Locations { get; private set; }
        public DbSet<Area> Areas { get; private set; }
        public DbSet<Cluster> Clusters { get; private set; }
        public DbSet<User> Users { get; private set; }
        public DbSet<OldCluster> OldClusters { get; private set; }
        public DbSet<OldLocation> OldLocations { get; private set; }
        public DbSet<PrimitiveItineraryRisk> ItineraryRisks {get; private set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cluster>()
                .HasMany(c => c.Coordinates)
                .WithOne(l => l.Cluster);
            modelBuilder.Entity<Location>()
                .HasKey(l => l.Location_ID);
            modelBuilder.Entity<OldLocation>()
                .HasKey(o => o.Old_Location_ID);
            modelBuilder.Entity<User>()
                .HasKey(u => u.AccessToken);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Location)
                .WithOne(u => u.User)
                .HasForeignKey<Location>(p => p.AccessToken);
            modelBuilder.Entity<User>()
                .HasOne(u => u.OldLocation)
                .WithOne(u => u.User)
                .HasForeignKey<OldLocation>(p => p.Access_Token);
            modelBuilder.Entity<PrimitiveItineraryRisk>()
                .HasOne(u => u.User)
                .WithMany(p => p.PrimitiveItineraryRisks)
                .HasForeignKey(u => u.AccessToken);
            modelBuilder.Entity<OldCluster>()
                .HasMany(c => c.Coordinates)
                .WithOne(l => l.Cluster);
        }
    }
}
