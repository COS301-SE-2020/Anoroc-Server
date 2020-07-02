using Anoroc_User_Management.Models;
using GeoCoordinatePortable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public string Calculate_Cluster()
        {
            Location location;
            bool cluster_found = false;
            foreach (Point point in items.PointArray)
            {
               location = new Location(new GeoCoordinate(point.Latitude, point.Longitude));
               
               foreach(Cluster cluster in Clusters)
               {
                    cluster_found = cluster.Check_If_Belong(location);
                    if(cluster_found)
                    {
                        break;
                    }
               }

               // location didnt fit into any cluster, create its own
               if(!cluster_found)
               {
                    Clusters.Add(new Cluster(location));
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
    }
}
