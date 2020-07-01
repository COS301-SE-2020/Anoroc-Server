using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model class used to store and work with GEO Location Points
    /// </summary>
    public class Location
    {
        public string Latitude
        {
            get; set;
        }
        public string Longitude
        {
            get; set;
        }
        public string Altitude
        {
            get; set;
        }
    }
}
