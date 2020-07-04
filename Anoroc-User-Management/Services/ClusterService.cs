using Anoroc_User_Management.Models;
using GeoCoordinatePortable;
using Nancy.Json;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Service used for Spatial Data Analytics that calculates to what cluster new location points belong to or generate a new cluster for outlier points
    /// </summary>
    public class ClusterService
    {
        List<Cluster> Clusters;
        Points items;
        public ClusterService()
        {

        }

        /// <summary>
        /// Main function for retrieving clusters
        /// Gets the clusters of 3 or more locations, The cluster returned only has one coord in it being the center point - meant for circles on the map
        /// </summary>
        /// <returns>List of Cluster Wrapper class objects </returns>
        public string GetClusters()
        {
            ReadJson();

            List<ClusterWrapper> returnClusters = new List<ClusterWrapper>();
            foreach (Cluster cluster in Clusters)
            {
                if (cluster.Coordinates.Count > 1)
                    returnClusters.Add(new ClusterWrapper(cluster.Coordinates.Count, cluster.Carrier_Data_Points, Calculate_Radius(cluster), Calculate_Center(cluster)));
            }
            return new JavaScriptSerializer().Serialize(returnClusters);
        }

        /// <summary>
        /// Gets the location points that are in a cluster of 2 or more locaitons
        /// </summary>
        /// <returns>List of Cluster class objects </returns>
        public string GetClustersPins()
        {
            ReadJson();

            List<Cluster> returnCluster = new List<Cluster>();
            foreach(Cluster cluster in Clusters)
            {
                if (cluster.Coordinates.Count > 1)
                    returnCluster.Add(cluster);
            }
            return new JavaScriptSerializer().Serialize(returnCluster);
        }


        /// <summary>
        /// Calcualtes which cluster the lcoaiton point belongs to, if none found a cluster is made with the lcoation point being the only memeber
        /// </summary>
        /// <returns> Temp: JSON object of the cluster </returns>
        public string Calculate_Cluster()
        {
            Location location;
            bool cluster_found = false;
            foreach (Point point in items.PointArray)
            {
                location = new Location(new GeoCoordinate(point.Latitude, point.Longitude));
                if (location.Carrier_Data_Point)
                {
                    foreach (Cluster cluster in Clusters)
                    {
                        cluster_found = cluster.Check_If_Belong(location);
                        if (cluster_found)
                        {
                            break;
                        }
                    }

                    // location didnt fit into any cluster, create its own
                    if (!cluster_found)
                    {
                        Clusters.Add(new Cluster(location));
                    }
                }
            }
            //temp
            string output = "";
            int clustercount = 0;
            foreach(Cluster cluster in Clusters)
            {
                output += "Cluster: " + clustercount;
                foreach(Location loc in cluster.Coordinates)
                {
                    output += loc.ToString();
                }
                clustercount++;
                output += "\n\n";
            }

            return output;
        }

        //-------------------------------------------------------------------------------------------------------
        // Helper Functions
        //-------------------------------------------------------------------------------------------------------
        public string ReadJson()
        {
            string json;
            using (StreamReader r = new StreamReader("TempData/Points.json"))
            {
                json = r.ReadToEnd();
                /*Debug.WriteLine(json);*/
                items = JsonConvert.DeserializeObject<Points>(json);
                Clusters = new List<Cluster>();
                json = Calculate_Cluster();
            }
            return json;
        }

        public double Calculate_Radius(Cluster cluster)
        {
            double radius = 0;
            int cluster_size = cluster.Coordinates.Count;
            double temp_distance;
            for (int i = 0; i < cluster_size-1; i++)
            {
                for (int j = 1; j < cluster_size; j++)
                {
                    temp_distance = cluster.Coordinates[i].Coordinate.GetDistanceTo(cluster.Coordinates[j].Coordinate);
                    if (temp_distance > radius)
                    {
                        radius = temp_distance;
                    }
                }
            }
            return radius;
        }

        public Location Calculate_Center(Cluster cluster)
        {
            Location center;
            double meanLat = 0;
            double meanLong = 0;
            foreach(Location location in cluster.Coordinates)
            {
                meanLat += location.Coordinate.Latitude;
                meanLong += location.Coordinate.Longitude;
            }

            meanLat /= cluster.Coordinates.Count;
            meanLong /= cluster.Coordinates.Count;

            center = new Location(meanLat, meanLong, cluster.Cluster_Created);

            return center;
        }
    }
}
