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
        public long Old_Location_ID { get; set; }
        public long Reference_ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Carrier_Data_Point { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("RegionArea_ID")]
        public long Area_Reference_ID { get; set; }
        public Area Region { get; set; }
        [ForeignKey("Reference_ID")]
        public long Old_Cluster_Reference_ID { get; set; }
        [JsonIgnore]
        public OldCluster Cluster { get; set; }
        public string Token { get; set; }
        [ForeignKey("Access_Token")]
        public string Access_Token { get; set; }

        public User User { get; set; }
        public OldLocation(Location location)
        {
            Reference_ID = location.Location_ID;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
            Carrier_Data_Point = location.Carrier_Data_Point;
            Created = location.Created;
            Area_Reference_ID = location.RegionArea_ID;
            Region = null;
            Old_Cluster_Reference_ID = location.ClusterReferenceID;
            Cluster = null;
        }
        public OldLocation()
        {

        }

        internal Location toLocation()
        {
            return new Location(Latitude, Longitude, Carrier_Data_Point, Created, Region, Access_Token);
        }
    }
}
