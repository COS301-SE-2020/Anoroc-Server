using Anoroc_User_Management.Models.TotalCarriers;
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
       public Area region;

        [Key]
       public long Area_ID { get; set; }
       public string Country { get; set; }
       public string Province { get; set; }
       public string City { get; set; }
       public string Suburb { get; set; }
        
        public Totals Totals { get; set; }
        public Area()
        {
            Area_ID = 0;
            Country = "";
            Province = "";
            Suburb = "";
            City = "";
        }

        public Area(Area area)
        {
            Area_ID = area.Area_ID;
            Country = area.Country;
            Province = area.Province;
            Suburb = area.Suburb;
            City = area.City;
        }

        public Area(string country, string province,string city, string suburb)
        {
            Country = country;
            Province = province;
            Suburb = suburb;
            City = city;
        }

        public Area(int areaID,string country, string province,string city, string suburb)
        {
            Area_ID = areaID;
            Country = country;
            Province = province;
            Suburb = suburb;
            City = city;
        }


        /*public static bool operator == (Area leftArea, Area rightArea)
        {
            if (leftArea == null || rightArea == null)
                return false;
           
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
        }*/


        public bool AreaEquals(Area rightArea)
        {
            if (rightArea == null)
                return false;
            else
            {
               if(Country != null)
                    if(Province != null)
                        if(City != null)
                            if(Suburb != null)
                                if(rightArea.Country != null)
                                    if(rightArea.Province != null)
                                        if(rightArea.City != null)
                                            if(rightArea.Suburb != null)
                                                if (Country.ToLower() == rightArea.Country.ToLower())
                                                    if (Province.ToLower() == rightArea.Province.ToLower())
                                                        if (City.ToLower() == rightArea.City.ToLower())
                                                            if (Suburb.ToLower() == rightArea.Suburb.ToLower())
                                                                return true;

                return false;
            }
        }
      /*  public override int GetHashCode()
        {
            return base.GetHashCode();
        }*/
    }
    
}
