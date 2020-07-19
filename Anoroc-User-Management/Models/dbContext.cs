using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// This class specifies the Context of the Databse by using set Models to capture each Table in the database.
    /// It is used to create a complete picture of the database that can be used alongside the database
    /// </summary>
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options): base(options){ }
        public DbSet<Location> Location { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Cluster> Cluster { get; set; }
    }
}
