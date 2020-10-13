using Anoroc_User_Management.Models.TotalCarriers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IPredictionService
    {
        public double[] getTrainningData();
        public double[] getUpperBoundData();
        public double[] getLowerBoundData();
        public double[] getForecastData();
        public double[] getAccuracy();

        public void predicateSuburbActiveViaSpreadSheet(string filename);
        public void predicateSuburbConfirmedViaSpreadSheet();
        public Dictionary<string, string[]> predicateSuburbConfirmedViaDatabase(Totals file);
    }
}
