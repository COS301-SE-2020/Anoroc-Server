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
    }
}
