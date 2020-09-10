using Anoroc_User_Management.Services;
using Newtonsoft.Json;
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
            LocationItineraryRisks = new Dictionary<string, int>();
            for (int i = 0; i < itineraryRisk.LocationItineraryRisks.Count; i++)
            {
                LocationItineraryRisks.Add(itineraryRisk.LocationItineraryRisks.Keys.ElementAt(i).ToString(), itineraryRisk.LocationItineraryRisks.Values.ElementAt(i));
                // TODO:
                // Handle the user making a location part of a different itinerary
            }
            Created = itineraryRisk.Created;
            ID = itineraryRisk.ID;
        }

        public DateTime Created { get; set; }
        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }
        public int ID { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public Dictionary<string, int> LocationItineraryRisks { get; set; }
    }
}
