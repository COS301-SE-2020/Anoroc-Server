using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{

    /// <summary>
    /// Class for future use, will be used to load clusters based on the area the mobile app defines. (i.e. Gauteng/Western Cap/ etc)
    /// </summary>
    public class Area
    {
       public int Area_ID { get; set; }
       public string Country { get; set; }
       public string Province { get; set; }
       public string Suburb { get; set; }
    }
}
