using Anoroc_User_Management.Models;
using Anoroc_User_Management.Models.ItineraryFolder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Anoroc_User_Management.Services
{
    public class ItineraryRisk
    {
        public ItineraryRisk(DateTime created, string access_token)
        {
            Created = created;
            UserAccessToken = access_token;
        }
        public ItineraryRisk()
        {
            Created = DateTime.Now;
        }

        public DateTime Created { get; set; }

        /// <summary>
        ///  The users email for the Itinerary.
        /// </summary>
        public string UserAccessToken { get; set; }

        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied
        /// </summary>
        public Dictionary<Location, int> LocationItineraryRisks { get; set; }
        public int ID { get; set; }

        /// <summary>
        /// A copy constructor to quickly convert from a Primitive Itinerary Risk to a normal Itinerary Risk
        /// </summary>
        /// <param name="primitive">The primitive type that needs to be converted</param>
        public ItineraryRisk(PrimitiveItineraryRisk primitive)
        {
            Created = primitive.Created;
            UserAccessToken = primitive.AccessToken;
            TotalItineraryRisk = primitive.TotalItineraryRisk;
            var temp = JsonConvert.DeserializeObject<Dictionary<string, int>>(primitive.LocationItineraryRisks);
            LocationItineraryRisks = new Dictionary<Location, int>();
            if(temp!= null)
                for(var i = 0; i < temp.Count; i++)
                {
                    LocationItineraryRisks.Add(JsonConvert.DeserializeObject<Location>(temp.Keys.ElementAt(i)), temp.Values.ElementAt(i));
                }
        }
    }
}