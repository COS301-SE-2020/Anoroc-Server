using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }
        public DbSet<Location> Location { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Cluster> Cluster { get; set; }
    }
}
