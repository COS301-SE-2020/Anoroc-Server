using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class Itinerary
    {
        /// <summary>
        ///     DateTime containing the date and time of the commencement of the travel
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     A list of locations to be visited.
        ///     The locations will be in order of visitations.
        ///     E.g. index 0 is the start location. index 1 follows start location.
        /// </summary>
        public List<Location> Locations;

        /// <summary>
        ///  The users email for the Itinerary.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int ItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public Dictionary<Location, int> LocationItineraryRisks { get; set; }
    }
}
