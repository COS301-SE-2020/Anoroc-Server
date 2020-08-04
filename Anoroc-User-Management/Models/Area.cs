using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Anoroc_User_Management.Models
{

    /// <summary>
    /// Class for future use, will be used to load clusters based on the area the mobile app defines. (i.e. Gauteng/Western Cap/ etc)
    /// </summary>
    public class Area
    {
       private Area region;
        private string v1;
        private string v2;
        private string v3;

        [Key]
       public long Area_ID { get; set; }
       public string Country { get; set; }
       public string Province { get; set; }
       public string Suburb { get; set; }
        public Area()
        {
            Area_ID = 0;
            Country = "";
            Province = "";
            Suburb = "";
        }

        public Area(Area area)
        {
            Area_ID = area.Area_ID;
            Country = area.Country;
            Province = area.Province;
            Suburb = area.Suburb;
        }

        public Area(string country, string province, string suburb)
        {
            Country = country;
            Province = province;
            Suburb = suburb;
        }

        public static bool operator == (Area leftArea, Area rightArea)
        {
            if (leftArea.Area_ID == rightArea.Area_ID)            
                if (leftArea.Country == rightArea.Country)
                    if (leftArea.Province == rightArea.Province)
                        if (leftArea.Suburb == rightArea.Suburb)
                            return true;
            
            return false;
        }
        public static bool operator != (Area leftArea, Area rightArea)
        {
            if (leftArea == rightArea)
                return false;
            else
                return true;
        }
    }
    
}
