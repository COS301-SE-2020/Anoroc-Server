using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class DataOverTimeObject
    {
        public DateTime DateTime { get; set; }
        public int TotalCases { get; set; }
        public DataOverTimeObject(DateTime date, int total)
        {
            DateTime = date;
            TotalCases = total;
        }
    }
}
