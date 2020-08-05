using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Services;

namespace Anoroc_User_Management.Models
{
    public class OldCluster
    {
        [Key]
        public long OldCluster_Id { get; set; }
        [ForeignKey("OldClusterReferenceID")]
        public ICollection<Location> Coordinates { get; } = new List<Location>();
        public Location Center_Location { get; set; } = new Location();
        public int Carrier_Data_Points;
        public DateTime Cluster_Created { get; set; }
        public IDatabaseEngine DatabaseEngine;
        public double Cluster_Radius { get; set; }
        public OldCluster(Cluster cluster)
        {
            OldCluster_Id = cluster.Cluster_Id;
            Coordinates = cluster.Coordinates;
            foreach(Location location in cluster.Coordinates)
            {
                Coordinates.Add(location);
            }
            Center_Location = new Location(cluster.Center_Location);
            Carrier_Data_Points = cluster.Carrier_Data_Points;
            DatabaseEngine = cluster.DatabaseEngine;
            Cluster_Radius = cluster.Cluster_Radius;
        }
        public OldCluster()
        {

        }

        public Cluster toCluster()
        {
            return new Cluster(Coordinates, Center_Location, Carrier_Data_Points, Cluster_Created, Cluster_Radius);
        }
    }
}
