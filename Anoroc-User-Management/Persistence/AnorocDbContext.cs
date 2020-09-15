using Anoroc_User_Management.Models.ItineraryFolder;
using Anoroc_User_Management.Models.TotalCarriers;
using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
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
        public DbSet<Notification> Notifications { get; private set; }
        public DbSet<Totals> Totals { get; private set; }
        public DbSet<Date> Dates { get; private set; }
        public DbSet<Carriers> Carriers { get; private set; }
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
                .HasMany(u => u.Location)
                .WithOne(u => u.User);
            modelBuilder.Entity<User>()
                .HasOne(u => u.OldLocation)
                .WithOne(u => u.User)
                .HasForeignKey<OldLocation>(p => p.Access_Token);
            modelBuilder.Entity<PrimitiveItineraryRisk>()
                .HasKey(p => p.AccessToken);
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.ID);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.AccessToken);
            modelBuilder.Entity<PrimitiveItineraryRisk>()
                .HasOne(u => u.User)
                .WithMany(p => p.PrimitiveItineraryRisks)
                .HasForeignKey(u => u.AccessToken);
            modelBuilder.Entity<OldCluster>()
                .HasMany(c => c.Coordinates)
                .WithOne(l => l.Cluster);
            modelBuilder.Entity<Totals>()
                .HasKey(t => t.ID);
            modelBuilder.Entity<Date>()
                .HasKey(d => d.ID);
            modelBuilder.Entity<Carriers>()
                .HasKey(c => c.ID);
            modelBuilder.Entity<Totals>()
                .HasMany(t => t.Date)
                .WithOne(d => d.Totals);
            modelBuilder.Entity<Totals>()
                .HasMany(t => t.TotalCarriers)
                .WithOne(d => d.Totals);
        }
    }
}
