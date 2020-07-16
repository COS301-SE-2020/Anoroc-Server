using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using System.Collections.Generic;

namespace Anoroc_User_Management.Interfaces
{
    public interface IDatabaseEngine
    {
        public bool Insert_Location(Location location);
        public bool Delete_Location(Location location);
        public bool Update_Location(Location location);
        public List<Location> Select_ListLocations();

        public bool Update_Cluster(Cluster cluster);
        public bool Delete_Cluster(Cluster cluster);
        public bool Insert_Cluster(Cluster cluster);
        public List<Cluster> Select_ListClusters();

        public bool Test_Connection();
        public int Get_Cluster_ID();
    }
}
