﻿using Anoroc_User_Management.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models.ItineraryFolder
{
    /// <summary>
    /// A Primitive class that is going to be used to store the ItineraryRisk in the database.
    /// The primitive class will be converted to and from the original class.
    /// </summary>
    public class PrimitiveItineraryRisk
    {
        public DateTime Created { get; set; }

        /// <summary>
        ///  The users email for the Itinerary.
        /// </summary>
        [ForeignKey("Email")]
        public string UserEmail { get; set; }

        /// <summary>
        /// The total risk of the journey
        /// </summary>
        public int TotalItineraryRisk { get; set; }

        /// <summary>
        /// Risk for each of the locations supplied, stored as a JSON object since EF cannot store Complex types like Dictionary
        /// </summary>
        public string LocationItineraryRisks { get; set; }

        public PrimitiveItineraryRisk()
        {

        }

            public PrimitiveItineraryRisk(ItineraryRisk risk)
        {
            Created = risk.Created;
            UserEmail = risk.UserEmail;
            TotalItineraryRisk = risk.TotalItineraryRisk;
            LocationItineraryRisks = JsonConvert.SerializeObject(risk.LocationItineraryRisks);
        }
    }
}
