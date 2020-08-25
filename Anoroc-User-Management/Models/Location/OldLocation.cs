using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anoroc_User_Management.Services;
using Newtonsoft.Json;

namespace Anoroc_User_Management.Models
{
    public class OldLocation
    {
        [Key]
        public long OldLocation_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("RegionArea_ID")]
        public long AreaReferenceID { get; set; }
        public Area Region { get; set; }
        [ForeignKey("Old_Cluster_ID")]
        public long? Old_ClusterReferenceID { get; set; }
        [JsonIgnore]
        public OldCluster Cluster { get; set; }
        public string Token { get; set; }
        [ForeignKey("AccessToken")]
        public string AccessToken { get; set; }

        public User User { get; set; }
        public OldLocation(Location location)
        {
            OldLocation_ID = location.Location_ID;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Carrier_Data_Point = location.Carrier_Data_Point;
            Created = location.Created;
            AreaReferenceID = location.RegionArea_ID;
            Region =new Area(location.Region);
            Old_ClusterReferenceID = location.ClusterReferenceID;
            Cluster = new OldCluster(location.Cluster);
        }
        public OldLocation()
        {

        }

        internal Location toLocation()
        {
            return new Location(Latitude, Longitude, Carrier_Data_Point, Created, Region, AccessToken);
        }
    }
}
