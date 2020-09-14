using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class DataService : IDataService
    {
        protected string URL_covid19za_provincial_cumulative_timeline_confirmed;
        List<DataOverTimeObject> CaseOverTime_ZA { get; set; }

        public DataService(string _covid19za_provincial_cumulative_timeline_confirmed)
        {
            //https://github.com/dsfsi/covid19za
            URL_covid19za_provincial_cumulative_timeline_confirmed = _covid19za_provincial_cumulative_timeline_confirmed;
            CaseOverTime_ZA = new List<DataOverTimeObject>();
        }

        public async Task<List<DataOverTimeObject>> GetCasesPerDate()
        {
            HttpClient client = new HttpClient();
            try
            {
                var responseString = await client.GetStringAsync(URL_covid19za_provincial_cumulative_timeline_confirmed);
                var stringAsArray = responseString.Split(new [] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                for(int i = 1; i < stringAsArray.Length; i++)
                {
                    if(stringAsArray[i] != "")
                    {
                        var line = stringAsArray[i].Split(",");
                        var dateCol = line[0].Split("-");
                        var date = new DateTime(Convert.ToInt32(dateCol[2]), Convert.ToInt32(dateCol[1]), Convert.ToInt32(dateCol[0]));
                        CaseOverTime_ZA.Add(new DataOverTimeObject(date, Convert.ToInt32(line[12])));
                    }
                }
            }
            catch(Exception)
            {
                return null;
            }
            return CaseOverTime_ZA;
        }
    }
}
