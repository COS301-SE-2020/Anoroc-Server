using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Data.Analysis;
using Microsoft.ML.Transforms.TimeSeries;
using XPlot.Plotly;
using System.IO;

namespace Anoroc_User_Management.Services
{
    public class PredictionService
    {
        const string CONFIRMED_DATASET_FILE = "Data/covid19za_provincial_cumulative_timeline_confirmed_transposed.csv";
        const string CONFIRMED__SUBURB_DATASET_FILE = "Data/test.csv";

        // Forecast API
        const int WINDOW_SIZE = 5;
        const int SERIES_LENGTH = 10;
        const int TRAIN_SIZE = 188;
        const int HORIZON = 7;

        // Dataset
        const int DEFAULT_ROW_COUNT = 10;
        const string TOTAL_CONFIRMED_COLUMN = "TotalConfirmed";
        const string DATE_COLUMN = "Date";


        public void predicateSuburbConfirmed()
        {
            PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("DateTimes"); // Default length is 0.
            PrimitiveDataFrameColumn<int> ints = new PrimitiveDataFrameColumn<int>("Ints"); // Makes a column of length 3. Filled with nulls initially
             // Makes a column of length 3. Filled with nulls initially

            dateTimes.Append(DateTime.Parse("2019/01/01"));
            dateTimes.Append(DateTime.Parse("2019/01/02"));
            dateTimes.Append(DateTime.Parse("2019/01/03"));
            dateTimes.Append(DateTime.Parse("2019/01/04"));
            dateTimes.Append(DateTime.Parse("2019/01/05"));
            dateTimes.Append(DateTime.Parse("2019/01/06"));
            dateTimes.Append(DateTime.Parse("2019/01/07"));
            dateTimes.Append(DateTime.Parse("2019/01/08"));
            dateTimes.Append(DateTime.Parse("2019/01/09"));
            dateTimes.Append(DateTime.Parse("2019/01/10"));
            dateTimes.Append(DateTime.Parse("2019/01/11"));

            ints.Append(1);
            ints.Append(2);
            ints.Append(3);
            ints.Append(4);
            ints.Append(5);
            ints.Append(6);
            ints.Append(7);
            ints.Append(8);
            ints.Append(9);
            ints.Append(10);
            ints.Append(11);



            DataFrame df = new DataFrame(dateTimes, ints); // This will throw if the columns are of different lengths

            

            var totalConfirmedDateColumn = df.Columns["DateTimes"];
            var totalConfirmedColumn = df.Columns["Ints"];

            var dates = new List<string>();
            var totalConfirmedCases = new List<string>();
            for (int index = 0; index < totalConfirmedDateColumn.Length; index++)
            {
                dates.Add(totalConfirmedDateColumn[index].ToString());
                totalConfirmedCases.Add(totalConfirmedColumn[index].ToString());
            }

            var title = "Number of Confirmed Cases over Time";
            var confirmedTimeGraph = new Graph.Scattergl()
            {
                x = dates.ToArray(),
                y = totalConfirmedCases.ToArray(),
                mode = "lines+markers"
            };



            var chart = Chart.Plot(confirmedTimeGraph);
            chart.WithTitle(title);
            Chart.Show(chart);

            var context = new MLContext();


            createFile();
            var data = context.Data.LoadFromTextFile<ConfirmedData>(CONFIRMED__SUBURB_DATASET_FILE, hasHeader: true, separatorChar: ',');


            var pipeline = context.Forecasting.ForecastBySsa(
                nameof(ConfirmedForecast.Forecast),
                nameof(ConfirmedData.TotalConfirmed),
                WINDOW_SIZE,
                SERIES_LENGTH,
                11,
                HORIZON);

            var model = pipeline.Fit(data);


            var forecastingEngine = model.CreateTimeSeriesEngine<ConfirmedData, ConfirmedForecast>(context);
            var forecasts = forecastingEngine.Predict();
            Console.WriteLine(forecasts.Forecast.Select(x => (int)x));
            //Chart.Show();


            var lastDate = DateTime.Parse(dates.LastOrDefault());
            var predictionStartDate = lastDate.AddDays(1);

            for (int index = 0; index < HORIZON; index++)
            {
                dates.Add(lastDate.AddDays(index + 1).ToShortDateString());
                totalConfirmedCases.Add(forecasts.Forecast[index].ToString());
            }
            var layout = new Layout.Layout();
            layout.shapes = new List<Graph.Shape>
            {
                new Graph.Shape
                {
                    x0 = predictionStartDate.ToShortDateString(),
                    x1 = predictionStartDate.ToShortDateString(),
                    y0 = "0",
                    y1 = "1",
                    xref = 'x',
                    yref = "paper",
                    line = new Graph.Line() {color = "red", width = 2}
                }
            };

            var chart1 = Chart.Plot(
            new[]
                {
                    new Graph.Scattergl()
                    {
                        x = dates.ToArray(),
                        y = totalConfirmedCases.ToArray(),
                        mode = "lines+markers"
                    }
                },
                layout
            );

            chart1.WithTitle(title);
            Chart.Show(chart1);

        }

        public void createFile()
        {
            string path = @"Data/Test.csv";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Date,TotalConfirmed");
                    sw.WriteLine("2019/01/01,1");
                    sw.WriteLine("2019/01/02,2");
                    sw.WriteLine("2019/01/03,3");
                    sw.WriteLine("2019/01/04,4");
                    sw.WriteLine("2019/01/05,5");
                    sw.WriteLine("2019/01/06,6");
                    sw.WriteLine("2019/01/07,7");
                    sw.WriteLine("2019/01/08,8");
                    sw.WriteLine("2019/01/09,9");
                    sw.WriteLine("2019/01/10,10");
                    sw.WriteLine("2019/01/11,11");
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Date,TotalConfirmed");
                    sw.WriteLine("2019/01/01,1");
                    sw.WriteLine("2019/01/02,2");
                    sw.WriteLine("2019/01/03,3");
                    sw.WriteLine("2019/01/04,4");
                    sw.WriteLine("2019/01/05,5");
                    sw.WriteLine("2019/01/06,6");
                    sw.WriteLine("2019/01/07,7");
                    sw.WriteLine("2019/01/08,8");
                    sw.WriteLine("2019/01/09,9");
                    sw.WriteLine("2019/01/10,10");
                    sw.WriteLine("2019/01/11,11");
                }
            }

  /*          // Open the file to read from.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }*/
        }


        public void predicateTotalSouthAfricaConfirmed()
        {                             
            

            var predictedDf = DataFrame.LoadCsv(CONFIRMED_DATASET_FILE);
            predictedDf.Head(DEFAULT_ROW_COUNT);
            predictedDf.Tail(DEFAULT_ROW_COUNT);
            predictedDf.Description();

            // Number of confirmed cases over time
            var totalConfirmedDateColumn = predictedDf.Columns[DATE_COLUMN];
            var totalConfirmedColumn = predictedDf.Columns[TOTAL_CONFIRMED_COLUMN];

            var dates = new List<string>();
            var totalConfirmedCases = new List<string>();
            for (int index = 0; index < totalConfirmedDateColumn.Length; index++)
            {
                dates.Add(totalConfirmedDateColumn[index].ToString());
                totalConfirmedCases.Add(totalConfirmedColumn[index].ToString());
            }

            var title = "Number of Confirmed Cases over Time";
            var confirmedTimeGraph = new Graph.Scattergl()
            {
                x = dates.ToArray(),
                y = totalConfirmedCases.ToArray(),
                mode = "lines+markers"
            };



            var chart = Chart.Plot(confirmedTimeGraph);
            chart.WithTitle(title);
            Chart.Show(chart);

            var context = new MLContext();

            var data = context.Data.LoadFromTextFile<ConfirmedData>(CONFIRMED_DATASET_FILE, hasHeader: true, separatorChar: ',');


            var pipeline = context.Forecasting.ForecastBySsa(
                nameof(ConfirmedForecast.Forecast),
                nameof(ConfirmedData.TotalConfirmed),
                WINDOW_SIZE,
                SERIES_LENGTH,
                TRAIN_SIZE,
                HORIZON);

            var model = pipeline.Fit(data);


            var forecastingEngine = model.CreateTimeSeriesEngine<ConfirmedData, ConfirmedForecast>(context);
            var forecasts = forecastingEngine.Predict();
            Console.WriteLine(forecasts.Forecast.Select(x => (int)x));
            //Chart.Show();


            var lastDate = DateTime.Parse(dates.LastOrDefault());
            var predictionStartDate = lastDate.AddDays(1);

            for (int index = 0; index < HORIZON; index++)
            {
                dates.Add(lastDate.AddDays(index + 1).ToShortDateString());
                totalConfirmedCases.Add(forecasts.Forecast[index].ToString());
            }
            var layout = new Layout.Layout();
            layout.shapes = new List<Graph.Shape>
            {
                new Graph.Shape
                {
                    x0 = predictionStartDate.ToShortDateString(),
                    x1 = predictionStartDate.ToShortDateString(),
                    y0 = "0",
                    y1 = "1",
                    xref = 'x',
                    yref = "paper",
                    line = new Graph.Line() {color = "red", width = 2}
                }
            };

                        var chart1 = Chart.Plot(
                        new[]
                            {
                    new Graph.Scattergl()
                    {
                        x = dates.ToArray(),
                        y = totalConfirmedCases.ToArray(),
                        mode = "lines+markers"
                    }
                            },
                            layout
                        );

                        chart1.WithTitle(title);
            Chart.Show(chart1);
        }

        /// <summary>
        /// Represent data for confirmed cases with a mapping to columns in a dataset
        /// </summary>
        public class ConfirmedData
        {
            /// <summary>
            /// Date of confirmed case
            /// </summary>
            [LoadColumn(0)]
            public DateTime Date;

            /// <summary>
            /// Total no of confirmed cases on a particular date
            /// </summary>
            [LoadColumn(1)]
            public float TotalConfirmed;
        }

        /// <summary>
        /// Prediction/Forecast for Confirmed cases
        /// </summary>
        internal class ConfirmedForecast
        {
            /// <summary>
            /// No of predicted confirmed cases for multiple days
            /// </summary>
            public float[] Forecast { get; set; }
        }
    }
}
