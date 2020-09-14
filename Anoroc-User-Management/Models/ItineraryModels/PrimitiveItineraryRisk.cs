using Anoroc_User_Management.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Anoroc_User_Management.Models.ItineraryFolder
{
    /// <summary>
    /// A Primitive class that is going to be used to store the ItineraryRisk in the database.
    /// The primitive class will be converted to and from the original class.
    /// </summary>
    public class PrimitiveItineraryRisk
    {
        [Key]
        public int Itinerary_ID { get; set; }
        public DateTime Created { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        /// <summary>
        ///  The users email for the Itinerary.
        /// </summary>
        [ForeignKey("AccessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied, stored as a JSON object since EF cannot store Complex types like Dictionary
        /// </summary>
        public string LocationItineraryRisks { get; set; }

        public User User { get; set; }

        public PrimitiveItineraryRisk()
        {

        }

        public PrimitiveItineraryRisk(ItineraryRisk risk)
        {
            Created = risk.Created;
            AccessToken = risk.UserAccessToken;
            TotalItineraryRisk = risk.TotalItineraryRisk;

            /*var tempDictionary = new Dictionary<string, int>();
            for(int i = 0; i <  risk.LocationItineraryRisks.Count; i++)
            {
                tempDictionary.Add(risk.LocationItineraryRisks.Keys.ElementAt(i).ToString(), risk.LocationItineraryRisks.Values.ElementAt(i));
            }*/
            LocationItineraryRisks = JsonConvert.SerializeObject(risk.LocationItineraryRisks);
        }
    }
}
