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
        public AnorocDbContext(DbContextOptions<AnorocDbContext> options): base(options){ }
        public DbSet<Location> Locations { get; private set; }
        public DbSet<Area> Areas { get; private set; }
        public DbSet<Cluster> Clusters { get; private set; }
        public DbSet<User> Users { get; private set; }
        public DbSet<OldCluster> OldClusters { get; private set; }
        public DbSet<OldLocation> OldLocations{ get; private set; }
        /*public DbSet<ItineraryRisk> ItineraryRisks { get; private set; }
        public DbSet<Dictionary<Location, int>> Dictionaries { get; private set; }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cluster>()
                .HasMany(c => c.Coordinates)
                .WithOne(l => l.Cluster);
            modelBuilder.Entity<Location>()
                .HasOne(r => r.Region);
            modelBuilder.Entity<ItineraryRisk>()
                .HasNoKey();
            /*modelBuilder.Entity<ItineraryRisk>()
                .HasOne(d => d.LocationItineraryRisks);
            modelBuilder.Entity<Dictionary<Location, int>>()
                .HasNoKey()
                .HasMany(d => d.Values);*/
        }
    }
}
