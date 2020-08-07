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

       /// <summary>
       ///  Gets the past user inieraries for the user, based on access_token
       /// </summary>
       /// <param name="pagination">Number objects to return, multple of 10. eg 10  -> first 10 objects, 20 -> returns objects at 10 to 20</param>
       /// <param name="access_token">access token of the user so that the function can get the user email </param>
       /// <returns>A n instance of Intinerary risk wrapper whcih holds the dictionary of locations and risk values </returns>
        public ItineraryRiskWrapper GetItineraries(int pagination, string access_token);
    }
}
