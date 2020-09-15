using System;
using Anoroc_User_Management.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anoroc_User_Management.Models.TotalCarriers
{
    public class Date
    {
        [Key]
        public long ID { get; set; }
        public DateTime CustomDate { get; set; }
        public Totals Totals { get; set; }

        public Date()
        {
            CustomDate = DateTime.UtcNow;
            Totals = null;
        }
    }
}
