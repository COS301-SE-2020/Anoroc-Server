using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anoroc_User_Management.Models.TotalCarriers
{
    public class Totals
    {
        [Key]
        public long ID { get; set; }
        public ICollection<Date> Date { get; set; } = new List<Date>();
        public ICollection<Carriers> TotalCarriers { get; set; } = new List<Carriers>();
        public string Suburb { get; set; }
        public Totals()
        {
            Date = new List<Date>();
            TotalCarriers = new List<Carriers>();
            Suburb = null;
        }
    }
}
