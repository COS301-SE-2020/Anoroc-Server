using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class PredictionDataForWeb
    {
        public string Suburb { get; set; }
        public List<string> Dates { get; set; }
        public List<int> Values { get; set; }
        public PredictionDataForWeb(string _sub , string[] values)
        {
            Suburb = _sub;
            Dates = new List<string>();
            Values = new List<int>();
            for (int i = 0; i < values.Length; i++)
            {
                var line = values[i].Split(",");
                Dates.Add(line[0]);
                Values.Add(Convert.ToInt32(line[1]));
            }
        }
    }
}
