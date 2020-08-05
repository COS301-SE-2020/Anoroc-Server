using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IItineraryService
    {
        /// <summary>
        ///  Function to process the locations and determine the risk of the iteneray.
        /// </summary>
        /// <param name="locationList"> The list of locations that make up the iteneray. </param>
        public void ProcessLocations(List<Location> locationList);
    }
}
