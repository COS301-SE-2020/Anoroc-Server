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
        public long Old_Cluster_Id { get; set; }
        [ForeignKey("Old_ClusterReferenceID")]
        public ICollection<OldLocation> Coordinates { get; } = new List<OldLocation>();
        public OldLocation Center_Location { get; set; } = new OldLocation();
        public int Carrier_Data_Points;
        public DateTime Cluster_Created { get; set; }
        public IDatabaseEngine DatabaseEngine;
        public double Cluster_Radius { get; set; }
        public OldCluster(Cluster cluster)
        {
            Old_Cluster_Id = cluster.Cluster_Id;
            foreach(Location location in cluster.Coordinates)
            {
                Coordinates.Add(new OldLocation(location));
            }
            Cluster_Created = cluster.Cluster_Created;
            Center_Location = new OldLocation(cluster.Center_Location);
            Carrier_Data_Points = cluster.Carrier_Data_Points;
            DatabaseEngine = cluster.DatabaseEngine;
            Cluster_Radius = cluster.Cluster_Radius;
        }
        public OldCluster()
        {

        }

        public Cluster toCluster()
        {
            var coords = new List<Location>();
            foreach(OldLocation old in Coordinates)
            {
                coords.Add(new Location(old));
            }
            return new Cluster(coords, new Location(Center_Location), Carrier_Data_Points, Cluster_Created, Cluster_Radius);
        }
    }
}
