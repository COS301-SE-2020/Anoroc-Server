using Anoroc_User_Management.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Class helping the cluster service
    /// </summary>
    /// 

    public class PrimitiveCluster : DbContext
    {
        public long Cluster_ID { get; }
        public string Coordinates { get; set; }
        public string Center_Location { get; set; }
        public int Carrier_Data_Points;
        public DateTime Cluster_Created { get; set; }

        public double Cluster_Radius { get; set; }
        public PrimitiveCluster()
        {
            // TODO:
        }
        // Create a function that scans through the list of clusters and removes the ones that have been there the longest


        public PrimitiveCluster(string loc, long cluster_id)
        {

            Coordinates=loc;

            Cluster_Created = DateTime.Now;

            Cluster_ID = cluster_id;

        }
        public PrimitiveCluster(Cluster cluster)
        {
            Cluster_ID = cluster.Cluster_ID;
            Coordinates += JsonConvert.SerializeObject(cluster.Coordinates);
            Center_Location = JsonConvert.SerializeObject(cluster.Center_Location);
            Carrier_Data_Points = cluster.Carrier_Data_Points;
            Cluster_Created = cluster.Cluster_Created;
            Cluster_Radius = cluster.Cluster_Radius;
        }
    }

}