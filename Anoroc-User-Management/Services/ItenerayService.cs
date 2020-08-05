using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class ItenerayService : IItenerayService
    {
        IClusterService ClusterService;
        public ItenerayService(IClusterService clusterService)
        {
            ClusterService = clusterService;
        }

        public void ProcessLocations(List<Location> locationList)
        {
            throw new NotImplementedException();
        }
    }
}
