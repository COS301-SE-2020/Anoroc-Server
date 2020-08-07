using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IItineraryService
    {
        /// <summary>
        ///  Function to process the locations and determine the risk of the Itinerary.
        /// </summary>
        /// <param name="locationList"> The list of locations that make up the Itinerary. </param>
        /// <returns> An Itenry Risk object containing a dictionary of risks where the key is the location. </returns>
        public ItineraryRiskWrapper ProcessItinerary(Itinerary itinerary, string access_token);
    }
}
