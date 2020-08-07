using Anoroc_User_Management.Models;
using System.Collections.Generic;

namespace Anoroc_User_Management.Services
{
    public class ItineraryRisk
    {
        /// <summary>
        ///  The users email for the Itinerary.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public Dictionary<Location, int> LocationItineraryRisks { get; set; }
    }
}