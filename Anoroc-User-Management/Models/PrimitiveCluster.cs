using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Class helping the cluster service
    /// </summary>
    /// 
    public class PrimitiveCluster
    {
        public long Cluster_ID { get; }
        public string Coordinates { get; set; }
        public string Center_Location { get; set; }
        public int Carrier_Data_Points;
        public DateTime Cluster_Created { get; set; }
        public double Cluster_Radius { get; set; }
       
        public PrimitiveCluster(Cluster cluster)
        {
            Cluster_ID = cluster.Cluster_Id;
            Coordinates += JsonConvert.SerializeObject(cluster.Coordinates);
            Center_Location = JsonConvert.SerializeObject(cluster.Center_Location);
            Carrier_Data_Points = cluster.Carrier_Data_Points;
            Cluster_Created = cluster.Cluster_Created;
            Cluster_Radius = cluster.Cluster_Radius;
        }
        public PrimitiveCluster()
        {
            
        }
    }

}