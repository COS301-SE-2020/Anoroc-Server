using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anoroc_User_Management.Models.TotalCarriers
{
    public class Totals
    {
        [Key]
        public long ID { get; set; }
        public ICollection<DateTime> Date = new List<DateTime>
        public ICollection<int> TotalCarriers { get; set; }
        [ForeignKey("RegionArea_ID")]
        public long RegionArea_ID { get; set; }
        public Area Region { get; set; }

        public Totals()
        {
            Date = new List<DateTime>();
            TotalCarriers = new List;
            RegionArea_ID = 0;
            Region = null;
        }
        public Totals(DateTime date, int total, Area region)
        {
            Date = date;
            TotalCarriers = total;
            Region = region;
        }
    }
}
