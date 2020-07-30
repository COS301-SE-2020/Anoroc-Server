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
    }
    
}
