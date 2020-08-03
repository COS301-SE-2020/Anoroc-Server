using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anoroc_User_Management.Services;

namespace Anoroc_User_Management.Models
{
    public class OldLocations
    {
        [Key]
        public long Location_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        public long AreaReferenceID { get; set; }
        public Area Region { get; set; }
        [ForeignKey("Cluster_ID")]
        public long? ClusterReferenceID { get; set; }
        public Cluster Cluster { get; set; }
        public string Token { get; set; }
        public string UserAccessToken { get; set; }
        public OldLocations(Location location)
        {
            Location_ID = location.Location_ID;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Carrier_Data_Point = location.Carrier_Data_Point;
            Created = location.Created;
            AreaReferenceID = location.AreaReferenceID;
            Region =new Area(location.Region);
            ClusterReferenceID = location.ClusterReferenceID;
            Cluster = new Cluster(location.Cluster);
        }

        internal Location toLocation()
        {
            return new Location(Latitude, Longitude, Carrier_Data_Point, Created, Region, UserAccessToken);
        }
    }
}
