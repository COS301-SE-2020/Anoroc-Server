using Anoroc_User_Management.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class ItineraryRiskWrapper
    {
        public ItineraryRiskWrapper(ItineraryRisk itineraryRisk)
        {
            TotalItineraryRisk = itineraryRisk.TotalItineraryRisk;
            LocationItineraryRisks = itineraryRisk.LocationItineraryRisks;
            Created = itineraryRisk.Created;
        }

        public DateTime Created { get; set; }
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
