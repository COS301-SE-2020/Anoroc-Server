using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Data.Analysis;
using Microsoft.ML.Transforms.TimeSeries;
using XPlot.Plotly;
using System.IO;
using Anoroc_User_Management.Models.TotalCarriers;
using Anoroc_User_Management.Interfaces;

namespace Anoroc_User_Management.Services
{
    public class PredictionService : IPredictionService
    {
        const string CONFIRMED_DATASET_FILE = "Data/covid19za_provincial_cumulative_timeline_confirmed_transposed.csv";
        const string CONFIRMED_UP_SUBURB_DATASET_FILE = "Data/UP_Surburb_Cumulative_timeline.csv";
        const string ACTIVE_ELANDSPOORT_SUBURB_DATASET_FILE = "Data/elandspoort_suburb_active_timeline.csv";
        const string CONFIRMED__SUBURB_DATASET_FILE = "Data/test.csv";

        const string MODEL_PATH = "Data/model.zip";
        // Forecast API
        const int WINDOW_SIZE = 7;
        const int SERIES_LENGTH = 30;
        const int HORIZON = 7;
        const float CONFIDENCE_LEVEL = 0.95f;
        const double SPLIT_RATIO = 0.94;

        // Dataset
        const int DEFAULT_ROW_COUNT = 10;
        const string TOTAL_CONFIRMED_COLUMN = "TotalConfirmed";
        const string DATE_COLUMN = "Date";


        public void predicateSuburbActiveViaSpreadSheet(string filename)
        {

            filename = "Data/" + filename;
            try
            {
                var predictedDf = DataFrame.LoadCsv(filename);
                predictedDf.Head(DEFAULT_ROW_COUNT);
                predictedDf.Tail(DEFAULT_ROW_COUNT);
                predictedDf.Description();

                // Number of confirmed cases over time
                var totalConfirmedDateColumn = predictedDf.Columns[DATE_COLUMN];
                var totalConfirmedColumn = predictedDf.Columns[TOTAL_CONFIRMED_COLUMN];

                var dates = new List<DateTime>();
                var totalConfirmedCases = new List<string>();
                for (int index = 0; index < totalConfirmedDateColumn.Length; index++)
                {
                    //DateTime date2 = Convert.ToDateTime(totalConfirmedDateColumn[index], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    dates.Add(Convert.ToDateTime(totalConfirmedDateColumn[index]));
                    totalConfirmedCases.Add(totalConfirmedColumn[index].ToString());
                }

                var title = "Number of Active Cases over Time for Elandspoort";
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

                var data = context.Data.LoadFromTextFile<ConfirmedData>(filename, hasHeader: true, separatorChar: ',');

                var totalRows = (int)data.GetColumn<float>("TotalConfirmed").ToList().Count;
                int numTrain = (int)(SPLIT_RATIO * totalRows);
                var confirmedAtSplit = (int)data.GetColumn<float>("TotalConfirmed").ElementAt(numTrain);
                var startingDate = data.GetColumn<DateTime>("Date").FirstOrDefault();
                var endDate = data.GetColumn<DateTime>("Date").LastOrDefault();
                var dateAtSplit = data.GetColumn<DateTime>("Date").ElementAt(numTrain);

                IDataView trainData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", upperBound: confirmedAtSplit);
                IDataView testData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", lowerBound: confirmedAtSplit);

                Console.WriteLine(($"Training dataset range : {startingDate.ToShortDateString()} to {dateAtSplit.ToShortDateString()}"));
                Console.WriteLine(($"Test dataset range : {dateAtSplit.AddDays(1).ToShortDateString()} to {endDate.ToShortDateString()}"));

                Console.WriteLine($"No of Training samples: {numTrain}");
                Console.WriteLine($"Series Lenght: {SERIES_LENGTH}");
                Console.WriteLine($"Window size: {WINDOW_SIZE}");
                Console.WriteLine($"Forecast perion(Days): {HORIZON}");
                Console.WriteLine($"CONFIDENCE: {CONFIDENCE_LEVEL}");

                var pipeline = context.Forecasting.ForecastBySsa(
                    nameof(ConfirmedForecast.Forecast),
                    nameof(ConfirmedData.TotalConfirmed),
                    WINDOW_SIZE,
                    SERIES_LENGTH,
                    trainSize: numTrain,
                    horizon: HORIZON,
                    confidenceLevel: CONFIDENCE_LEVEL,
                    confidenceLowerBoundColumn: nameof(ConfirmedForecast.LowerBoundConfirmed),
                    confidenceUpperBoundColumn: nameof(ConfirmedForecast.UpperBoundConfirmed));

                var model = pipeline.Fit(data);

                IDataView predictions = model.Transform(testData);

                IEnumerable<float> actual =
                    context.Data.CreateEnumerable<ConfirmedData>(testData, true)
                        .Select(observed => observed.TotalConfirmed);

                IEnumerable<float> forecast =
                    context.Data.CreateEnumerable<ConfirmedForecast>(predictions, true)
                        .Select(prediction => prediction.Forecast[0]);

                var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

                var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
                var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

                Console.WriteLine("Evaluation Metrics");
                Console.WriteLine("---------------------");
                Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
                Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");


                var forecastingEngine = model.CreateTimeSeriesEngine<ConfirmedData, ConfirmedForecast>(context);
                var forecasts = forecastingEngine.Predict();

                var forecastOutputs = context.Data.CreateEnumerable<ConfirmedData>(testData, reuseRowObject: false)
                    .Take(HORIZON)
                    .Select((ConfirmedData confirmedData, int index) =>
                    {
                        float lowerEstimate = Math.Max(0, forecasts.LowerBoundConfirmed[index]);
                        float estimate = forecasts.Forecast[index];
                        float upperEstimate = forecasts.UpperBoundConfirmed[index];

                        return new ForecastOutput
                        {
                            ActualConfirmed = confirmedData.TotalConfirmed,
                            Date = confirmedData.Date,
                            Forecast = estimate,
                            LowerEstimate = lowerEstimate,
                            UpperEstimate = upperEstimate
                        };
                    });

                    PrimitiveDataFrameColumn<DateTime> forecastDates = new PrimitiveDataFrameColumn<DateTime>("Date"); // Default length is 0.
                    PrimitiveDataFrameColumn<float> actualConfirmedCases = new PrimitiveDataFrameColumn<float>("ActualConfirmed"); // Makes a column of length 3. Filled with nulls initially
                    PrimitiveDataFrameColumn<float> forecastCases = new PrimitiveDataFrameColumn<float>("Forecast"); // Makes a column of length 3. Filled with nulls initially
                    PrimitiveDataFrameColumn<float> lowerEstimates = new PrimitiveDataFrameColumn<float>("LowerEstimate"); // Makes a column of length 3. Filled with nulls initially
                    PrimitiveDataFrameColumn<float> upperEstimates = new PrimitiveDataFrameColumn<float>("UpperEstimate"); // Makes a column of length 3. Filled with nulls initially

                    foreach (var output in forecastOutputs)
                    {
                        forecastDates.Append(output.Date);
                        actualConfirmedCases.Append(output.ActualConfirmed);
                        forecastCases.Append(output.Forecast);
                        lowerEstimates.Append(output.LowerEstimate);
                        upperEstimates.Append(output.UpperEstimate);
                    }

                    Console.WriteLine(("Total Active Cases Forecast for Elandspoort"));
                    var forecastDataFrame = new DataFrame(forecastDates, actualConfirmedCases, lowerEstimates, forecastCases, upperEstimates);
                    Console.WriteLine(forecastDataFrame);

                    //Console.WriteLine(forecasts.Forecast.Select(x => (int)x));
                    //Chart.Show();
                    var predictionStartDate = dateAtSplit.AddDays(-1); // lastDate.AddDays(1);

                    var newDates = new List<DateTime>();
                    var fullDates = new List<DateTime>();
                    fullDates.AddRange(dates.Take(numTrain));

                    var fullTotalConfirmedCases = new List<string>();
                    fullTotalConfirmedCases.AddRange(totalConfirmedCases.Take(numTrain));

                    int diff = totalRows - numTrain;
                    for (int index = 0; index < HORIZON + diff; index++)
                    {
                        if (index < diff)
                        {
                            var nextDate = predictionStartDate.AddDays(index + 1);
                            newDates.Add(nextDate);
                            fullTotalConfirmedCases.Add(actualConfirmedCases[index].ToString());
                        }
                        else
                        {
                            var nextDate = predictionStartDate.AddDays(index + 1);
                            newDates.Add(nextDate);
                            fullTotalConfirmedCases.Add(forecasts.Forecast[index - diff].ToString());
                        }

                    }

                    fullDates.AddRange(newDates);

                    var layout = new Layout.Layout();
                    layout.shapes = new List<Graph.Shape>
                    {
                        new Graph.Shape
                        {
                            x0 = predictionStartDate,
                            x1 = predictionStartDate,
                            y0 = "0",
                            y1 = "1",
                            xref = 'x',
                            yref = "paper",
                            line = new Graph.Line() {color = "red", width = 2}
                        }
                    };

                    var predictionChart = Chart.Plot(
                        new[]
                        {
                        new Graph.Scattergl()
                        {
                            x = fullDates.ToArray(),
                            y = fullTotalConfirmedCases.ToArray(),
                            mode = "lines+markers"
                        }
                        },
                        layout
                    );

                    predictionChart.WithTitle("Number of Confirmed Cases over Time");
                    Chart.Show(predictionChart);

                    Graph.Scattergl[] scatters = {
                    new Graph.Scattergl() {
                        x = newDates,
                        y = forecasts.UpperBoundConfirmed,
                        fill = "tonexty",
                        name = "Upper bound"
                    },
                    new Graph.Scattergl() {
                        x = newDates,
                        y = forecasts.Forecast,
                        fill = "tonexty",
                        name = "Forecast"
                    },
                    new Graph.Scattergl() {
                        x = newDates,
                        y = forecasts.LowerBoundConfirmed,
                        fill = "tonexty",
                        name = "Lower bound"
                    }
                        };

                    var predictionChart2 = Chart.Plot(scatters);
                    predictionChart2.Width = 600;
                    predictionChart2.Height = 600;
                    Chart.Show(predictionChart2);
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot find file");
            }
           
            

        }

        public void predicateSuburbConfirmedViaSpreadSheet()
        {


            var predictedDf = DataFrame.LoadCsv(CONFIRMED_UP_SUBURB_DATASET_FILE);
            predictedDf.Head(DEFAULT_ROW_COUNT);
            predictedDf.Tail(DEFAULT_ROW_COUNT);
            predictedDf.Description();

            // Number of confirmed cases over time
            var totalConfirmedDateColumn = predictedDf.Columns[DATE_COLUMN];
            var totalConfirmedColumn = predictedDf.Columns[TOTAL_CONFIRMED_COLUMN];

            var dates = new List<DateTime>();
            var totalConfirmedCases = new List<string>();
            for (int index = 0; index < totalConfirmedDateColumn.Length; index++)
            {
                //DateTime date2 = Convert.ToDateTime(totalConfirmedDateColumn[index], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                dates.Add(Convert.ToDateTime(totalConfirmedDateColumn[index]));
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

            var data = context.Data.LoadFromTextFile<ConfirmedData>(CONFIRMED_UP_SUBURB_DATASET_FILE, hasHeader: true, separatorChar: ',');

            var totalRows = (int)data.GetColumn<float>("TotalConfirmed").ToList().Count;
            int numTrain = (int)(SPLIT_RATIO * totalRows);
            var confirmedAtSplit = (int)data.GetColumn<float>("TotalConfirmed").ElementAt(numTrain);
            var startingDate = data.GetColumn<DateTime>("Date").FirstOrDefault();
            var endDate = data.GetColumn<DateTime>("Date").LastOrDefault();
            var dateAtSplit = data.GetColumn<DateTime>("Date").ElementAt(numTrain);

            IDataView trainData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", upperBound: confirmedAtSplit);
            IDataView testData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", lowerBound: confirmedAtSplit);

            Console.WriteLine(($"Training dataset range : {startingDate.ToShortDateString()} to {dateAtSplit.ToShortDateString()}"));
            Console.WriteLine(($"Test dataset range : {dateAtSplit.AddDays(1).ToShortDateString()} to {endDate.ToShortDateString()}"));

            Console.WriteLine($"No of Training samples: {numTrain}");
            Console.WriteLine($"Series Lenght: {SERIES_LENGTH}");
            Console.WriteLine($"Window size: {WINDOW_SIZE}");
            Console.WriteLine($"Forecast perion(Days): {HORIZON}");
            Console.WriteLine($"CONFIDENCE: {CONFIDENCE_LEVEL}");

            var pipeline = context.Forecasting.ForecastBySsa(
                nameof(ConfirmedForecast.Forecast),
                nameof(ConfirmedData.TotalConfirmed),
                WINDOW_SIZE,
                SERIES_LENGTH,
                trainSize: numTrain,
                horizon: HORIZON,
                confidenceLevel: CONFIDENCE_LEVEL,
                confidenceLowerBoundColumn: nameof(ConfirmedForecast.LowerBoundConfirmed),
                confidenceUpperBoundColumn: nameof(ConfirmedForecast.UpperBoundConfirmed));

            var model = pipeline.Fit(data);

            IDataView predictions = model.Transform(testData);

            IEnumerable<float> actual =
                context.Data.CreateEnumerable<ConfirmedData>(testData, true)
                    .Select(observed => observed.TotalConfirmed);

            IEnumerable<float> forecast =
                context.Data.CreateEnumerable<ConfirmedForecast>(predictions, true)
                    .Select(prediction => prediction.Forecast[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");


            var forecastingEngine = model.CreateTimeSeriesEngine<ConfirmedData, ConfirmedForecast>(context);
            var forecasts = forecastingEngine.Predict();

            var forecastOutputs = context.Data.CreateEnumerable<ConfirmedData>(testData, reuseRowObject: false)
                .Take(HORIZON)
                .Select((ConfirmedData confirmedData, int index) =>
                {
                    float lowerEstimate = Math.Max(0, forecasts.LowerBoundConfirmed[index]);
                    float estimate = forecasts.Forecast[index];
                    float upperEstimate = forecasts.UpperBoundConfirmed[index];

                    return new ForecastOutput
                    {
                        ActualConfirmed = confirmedData.TotalConfirmed,
                        Date = confirmedData.Date,
                        Forecast = estimate,
                        LowerEstimate = lowerEstimate,
                        UpperEstimate = upperEstimate
                    };
                });

            PrimitiveDataFrameColumn<DateTime> forecastDates = new PrimitiveDataFrameColumn<DateTime>("Date"); // Default length is 0.
            PrimitiveDataFrameColumn<float> actualConfirmedCases = new PrimitiveDataFrameColumn<float>("ActualConfirmed"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> forecastCases = new PrimitiveDataFrameColumn<float>("Forecast"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> lowerEstimates = new PrimitiveDataFrameColumn<float>("LowerEstimate"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> upperEstimates = new PrimitiveDataFrameColumn<float>("UpperEstimate"); // Makes a column of length 3. Filled with nulls initially

            foreach (var output in forecastOutputs)
            {
                forecastDates.Append(output.Date);
                actualConfirmedCases.Append(output.ActualConfirmed);
                forecastCases.Append(output.Forecast);
                lowerEstimates.Append(output.LowerEstimate);
                upperEstimates.Append(output.UpperEstimate);
            }

            Console.WriteLine(("Total Confirmed Cases Forecast"));
            var forecastDataFrame = new DataFrame(forecastDates, actualConfirmedCases, lowerEstimates, forecastCases, upperEstimates);
            Console.WriteLine(forecastDataFrame);

            Console.WriteLine(forecasts.Forecast.Select(x => (int)x));
            //Chart.Show();
            var predictionStartDate = dateAtSplit.AddDays(-1); // lastDate.AddDays(1);

            var newDates = new List<DateTime>();
            var fullDates = new List<DateTime>();
            fullDates.AddRange(dates.Take(numTrain));

            var fullTotalConfirmedCases = new List<string>();
            fullTotalConfirmedCases.AddRange(totalConfirmedCases.Take(numTrain));

            int diff = totalRows - numTrain;
            for (int index = 0; index < HORIZON + diff; index++)
            {
                if (index < diff)
                {
                    var nextDate = predictionStartDate.AddDays(index + 1);
                    newDates.Add(nextDate);
                    fullTotalConfirmedCases.Add(actualConfirmedCases[index].ToString());
                }
                else
                {
                    var nextDate = predictionStartDate.AddDays(index + 1);
                    newDates.Add(nextDate);
                    fullTotalConfirmedCases.Add(forecasts.Forecast[index - diff].ToString());
                }

            }

            fullDates.AddRange(newDates);

            var layout = new Layout.Layout();
            layout.shapes = new List<Graph.Shape>
            {
                new Graph.Shape
                {
                    x0 = predictionStartDate,
                    x1 = predictionStartDate,
                    y0 = "0",
                    y1 = "1",
                    xref = 'x',
                    yref = "paper",
                    line = new Graph.Line() {color = "red", width = 2}
                }
            };

            var predictionChart = Chart.Plot(
                new[]
                {
                    new Graph.Scattergl()
                    {
                        x = fullDates.ToArray(),
                        y = fullTotalConfirmedCases.ToArray(),
                        mode = "lines+markers"
                    }
                },
                layout
            );

            predictionChart.WithTitle("Number of Confirmed Cases over Time");
            Chart.Show(predictionChart);

            Graph.Scattergl[] scatters = {
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.UpperBoundConfirmed,
                    fill = "tonexty",
                    name = "Upper bound"
                },
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.Forecast,
                    fill = "tonexty",
                    name = "Forecast"
                },
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.LowerBoundConfirmed,
                    fill = "tonexty",
                    name = "Lower bound"
                }
            };

            var predictionChart2 = Chart.Plot(scatters);
            predictionChart2.Width = 600;
            predictionChart2.Height = 600;
            Chart.Show(predictionChart2);

        }

        public Dictionary<string,string[]> predicateSuburbConfirmedViaDatabase(Totals file)
        {
            
            PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("DateTimes"); // Default length is 0.
            PrimitiveDataFrameColumn<int> ints = new PrimitiveDataFrameColumn<int>("Ints"); // Makes a column of length 3. Filled with nulls initially



            for(int i = 0; i< file.Date.Count();i++)
            {
                dateTimes.Append(DateTime.Parse(file.Date.ToArray()[i].ToString()));
                ints.Append(file.TotalCarriers.ToArray()[i].TotalCarriers);
            }


            createFile(file);


            DataFrame df = new DataFrame(dateTimes, ints); // This will throw if the columns are of different lengths



            var totalConfirmedDateColumn = df.Columns["DateTimes"];
            var totalConfirmedColumn = df.Columns["Ints"];

            var dates = new List<DateTime>();
            var totalConfirmedCases = new List<string>();
            for (int index = 0; index < totalConfirmedDateColumn.Length; index++)
            {
                //DateTime date2 = Convert.ToDateTime(totalConfirmedDateColumn[index], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                dates.Add(Convert.ToDateTime(totalConfirmedDateColumn[index]));
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
            //Chart.Show(chart);

            var context = new MLContext();

            var data = context.Data.LoadFromTextFile<ConfirmedData>(CONFIRMED__SUBURB_DATASET_FILE, hasHeader: true, separatorChar: ',');

            var totalRows = (int)data.GetColumn<float>("TotalConfirmed").ToList().Count;
            int numTrain = (int)(SPLIT_RATIO * totalRows);
            var confirmedAtSplit = (int)data.GetColumn<float>("TotalConfirmed").ElementAt(numTrain);
            var startingDate = data.GetColumn<DateTime>("Date").FirstOrDefault();
            var endDate = data.GetColumn<DateTime>("Date").LastOrDefault();
            var dateAtSplit = data.GetColumn<DateTime>("Date").ElementAt(numTrain);

            IDataView trainData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", upperBound: confirmedAtSplit);
            IDataView testData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", lowerBound: confirmedAtSplit);

            Console.WriteLine(($"Training dataset range : {startingDate.ToShortDateString()} to {dateAtSplit.ToShortDateString()}"));
            Console.WriteLine(($"Test dataset range : {dateAtSplit.AddDays(1).ToShortDateString()} to {endDate.ToShortDateString()}"));

            Console.WriteLine($"No of Training samples: {numTrain}");
            Console.WriteLine($"Series Lenght: {SERIES_LENGTH}");
            Console.WriteLine($"Window size: {WINDOW_SIZE}");
            Console.WriteLine($"Forecast perion(Days): {HORIZON}");
            Console.WriteLine($"CONFIDENCE: {CONFIDENCE_LEVEL}");

            var pipeline = context.Forecasting.ForecastBySsa(
                nameof(ConfirmedForecast.Forecast),
                nameof(ConfirmedData.TotalConfirmed),
                4,
                SERIES_LENGTH,
                trainSize: numTrain,
                horizon: HORIZON,
                confidenceLevel: CONFIDENCE_LEVEL,
                confidenceLowerBoundColumn: nameof(ConfirmedForecast.LowerBoundConfirmed),
                confidenceUpperBoundColumn: nameof(ConfirmedForecast.UpperBoundConfirmed));

            var model = pipeline.Fit(data);

            IDataView predictions = model.Transform(testData);

            IEnumerable<float> actual =
                context.Data.CreateEnumerable<ConfirmedData>(testData, true)
                    .Select(observed => observed.TotalConfirmed);

            IEnumerable<float> forecast =
                context.Data.CreateEnumerable<ConfirmedForecast>(predictions, true)
                    .Select(prediction => prediction.Forecast[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");


            var forecastingEngine = model.CreateTimeSeriesEngine<ConfirmedData, ConfirmedForecast>(context);
            forecastingEngine.CheckPoint(context, MODEL_PATH);

            var forecasts = forecastingEngine.Predict();

            var forecastOutputs = context.Data.CreateEnumerable<ConfirmedData>(testData, reuseRowObject: false)
                .Take(HORIZON)
                .Select((ConfirmedData confirmedData, int index) =>
                {
                    float lowerEstimate = Math.Max(0, forecasts.LowerBoundConfirmed[index]);
                    float estimate = forecasts.Forecast[index];
                    float upperEstimate = forecasts.UpperBoundConfirmed[index];

                    return new ForecastOutput
                    {
                        ActualConfirmed = confirmedData.TotalConfirmed,
                        Date = confirmedData.Date,
                        Forecast = estimate,
                        LowerEstimate = lowerEstimate,
                        UpperEstimate = upperEstimate
                    };
                });

            PrimitiveDataFrameColumn<DateTime> forecastDates = new PrimitiveDataFrameColumn<DateTime>("Date"); // Default length is 0.
            PrimitiveDataFrameColumn<float> actualConfirmedCases = new PrimitiveDataFrameColumn<float>("ActualConfirmed"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> forecastCases = new PrimitiveDataFrameColumn<float>("Forecast"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> lowerEstimates = new PrimitiveDataFrameColumn<float>("LowerEstimate"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> upperEstimates = new PrimitiveDataFrameColumn<float>("UpperEstimate"); // Makes a column of length 3. Filled with nulls initially

            foreach (var output in forecastOutputs)
            {
                forecastDates.Append(output.Date);
                actualConfirmedCases.Append(output.ActualConfirmed);
                forecastCases.Append(output.Forecast);
                lowerEstimates.Append(output.LowerEstimate);
                upperEstimates.Append(output.UpperEstimate);
            }

            Console.WriteLine(("Total Confirmed Cases Forecast"));
            var forecastDataFrame = new DataFrame(forecastDates, actualConfirmedCases, lowerEstimates, forecastCases, upperEstimates);
            Console.WriteLine(forecastDataFrame);

            //Console.WriteLine(forecasts.Forecast.Select(x => (int)x));
            //Chart.Show();
            var predictionStartDate = dateAtSplit.AddDays(-1); // lastDate.AddDays(1);

            var newDates = new List<DateTime>();
            var fullDates = new List<DateTime>();
            fullDates.AddRange(dates.Take(numTrain));

            var fullTotalConfirmedCases = new List<string>();
            fullTotalConfirmedCases.AddRange(totalConfirmedCases.Take(numTrain));

            int diff = totalRows - numTrain;
            for (int index = 0; index < HORIZON+diff; index++)
            {
                if(index < diff)
                {
                    var nextDate = predictionStartDate.AddDays(index + 1);
                    newDates.Add(nextDate);
                    fullTotalConfirmedCases.Add(actualConfirmedCases[index].ToString());
                }
                else
                {
                    var nextDate = predictionStartDate.AddDays(index + 1);
                    newDates.Add(nextDate);
                    fullTotalConfirmedCases.Add(forecasts.Forecast[index-diff].ToString());
                }
               
            }

            Dictionary<string, string[]> result = new Dictionary<string, string[]>();
            string[] activeArray = new string[7];
            for (int i = 0; i < HORIZON;i++)
            {
                var nextDate = predictionStartDate.AddDays(i + 1);
                activeArray[i] = nextDate.ToString() + "," + forecasts.Forecast[i].ToString();
            }
            result.Add(file.Suburb, activeArray);
            fullDates.AddRange(newDates);

            var layout = new Layout.Layout();
            layout.shapes = new List<Graph.Shape>
            {
                new Graph.Shape
                {
                    x0 = predictionStartDate,
                    x1 = predictionStartDate,
                    y0 = "0",
                    y1 = "1",
                    xref = 'x',
                    yref = "paper",
                    line = new Graph.Line() {color = "red", width = 2}
                }
            };

            var predictionChart = Chart.Plot(
                new[]
                {
                    new Graph.Scattergl()
                    {
                        x = fullDates.ToArray(),
                        y = fullTotalConfirmedCases.ToArray(),
                        mode = "lines+markers"
                    }
                },
                layout
            );

            predictionChart.WithTitle("Number of Confirmed Cases over Time");
            //Chart.Show(predictionChart);

            Graph.Scattergl[] scatters = {
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.UpperBoundConfirmed,
                    fill = "tonexty",
                    name = "Upper bound"
                },
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.Forecast,
                    fill = "tonexty",
                    name = "Forecast"
                },
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.LowerBoundConfirmed,
                    fill = "tonexty",
                    name = "Lower bound"
                }
            };

            var predictionChart2 = Chart.Plot(scatters);
            predictionChart2.Width = 600;
            predictionChart2.Height = 600;
            //Chart.Show(predictionChart2);


            return result;

        }

        

        public void createFile(Totals file)
        {
            string path = @"Data/Test.csv";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Date,TotalConfirmed");
                    for (int i = 0; i < file.Date.Count(); i++)
                    {
                        sw.WriteLine(file.Date.ToArray()[i].ToString()+"," + file.TotalCarriers.ToArray()[i].TotalCarriers);
                    }
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Date,TotalConfirmed");
                    for (int i = 0; i < file.Date.Count(); i++)
                    {
                        sw.WriteLine(file.Date.ToArray()[i].ToString() + "," + file.TotalCarriers.ToArray()[i].TotalCarriers);
                    }
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
            

            var predictedDf = DataFrame.LoadCsv(CONFIRMED_UP_SUBURB_DATASET_FILE);
            predictedDf.Head(DEFAULT_ROW_COUNT);
            predictedDf.Tail(DEFAULT_ROW_COUNT);
            predictedDf.Description();

            // Number of confirmed cases over time
            var totalConfirmedDateColumn = predictedDf.Columns[DATE_COLUMN];
            var totalConfirmedColumn = predictedDf.Columns[TOTAL_CONFIRMED_COLUMN];

            var dates = new List<DateTime>();
            var totalConfirmedCases = new List<string>();
            for (int index = 0; index < totalConfirmedDateColumn.Length; index++)
            {
                //DateTime date2 = Convert.ToDateTime(totalConfirmedDateColumn[index], System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                dates.Add(Convert.ToDateTime(totalConfirmedDateColumn[index]));
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

            var data = context.Data.LoadFromTextFile<ConfirmedData>(CONFIRMED_UP_SUBURB_DATASET_FILE, hasHeader: true, separatorChar: ',');

            var totalRows = (int)data.GetColumn<float>("TotalConfirmed").ToList().Count;
            int numTrain = (int)(SPLIT_RATIO * totalRows);
            var confirmedAtSplit = (int)data.GetColumn<float>("TotalConfirmed").ElementAt(numTrain);
            var startingDate = data.GetColumn<DateTime>("Date").FirstOrDefault();
            var endDate = data.GetColumn<DateTime>("Date").LastOrDefault();
            var dateAtSplit = data.GetColumn<DateTime>("Date").ElementAt(numTrain);

            IDataView trainData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", upperBound: confirmedAtSplit);
            IDataView testData = context.Data.FilterRowsByColumn(data, "TotalConfirmed", lowerBound: confirmedAtSplit);

            Console.WriteLine(($"Training dataset range : {startingDate.ToShortDateString()} to {dateAtSplit.ToShortDateString()}"));
            Console.WriteLine(($"Test dataset range : {dateAtSplit.AddDays(1).ToShortDateString()} to {endDate.ToShortDateString()}"));

            Console.WriteLine($"No of Training samples: {numTrain}");
            Console.WriteLine($"Series Lenght: {SERIES_LENGTH}");
            Console.WriteLine($"Window size: {WINDOW_SIZE}");
            Console.WriteLine($"Forecast perion(Days): {HORIZON}");
            Console.WriteLine($"CONFIDENCE: {CONFIDENCE_LEVEL}");

            var pipeline = context.Forecasting.ForecastBySsa(
                nameof(ConfirmedForecast.Forecast),
                nameof(ConfirmedData.TotalConfirmed),
                WINDOW_SIZE,
                SERIES_LENGTH,
                trainSize: numTrain,
                horizon: HORIZON,
                confidenceLevel: CONFIDENCE_LEVEL,
                confidenceLowerBoundColumn: nameof(ConfirmedForecast.LowerBoundConfirmed),
                confidenceUpperBoundColumn: nameof(ConfirmedForecast.UpperBoundConfirmed));

            var model = pipeline.Fit(data);

            IDataView predictions = model.Transform(testData);

            IEnumerable<float> actual =
                context.Data.CreateEnumerable<ConfirmedData>(testData, true)
                    .Select(observed => observed.TotalConfirmed);

            IEnumerable<float> forecast =
                context.Data.CreateEnumerable<ConfirmedForecast>(predictions, true)
                    .Select(prediction => prediction.Forecast[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");


            var forecastingEngine = model.CreateTimeSeriesEngine<ConfirmedData, ConfirmedForecast>(context);
            var forecasts = forecastingEngine.Predict();

            var forecastOutputs = context.Data.CreateEnumerable<ConfirmedData>(testData, reuseRowObject: false)
                .Take(HORIZON)
                .Select((ConfirmedData confirmedData, int index) =>
                {
                    float lowerEstimate = Math.Max(0, forecasts.LowerBoundConfirmed[index]);
                    float estimate = forecasts.Forecast[index];
                    float upperEstimate = forecasts.UpperBoundConfirmed[index];

                    return new ForecastOutput
                    {
                        ActualConfirmed = confirmedData.TotalConfirmed,
                        Date = confirmedData.Date,
                        Forecast = estimate,
                        LowerEstimate = lowerEstimate,
                        UpperEstimate = upperEstimate
                    };
                });

            PrimitiveDataFrameColumn<DateTime> forecastDates = new PrimitiveDataFrameColumn<DateTime>("Date"); // Default length is 0.
            PrimitiveDataFrameColumn<float> actualConfirmedCases = new PrimitiveDataFrameColumn<float>("ActualConfirmed"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> forecastCases = new PrimitiveDataFrameColumn<float>("Forecast"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> lowerEstimates = new PrimitiveDataFrameColumn<float>("LowerEstimate"); // Makes a column of length 3. Filled with nulls initially
            PrimitiveDataFrameColumn<float> upperEstimates = new PrimitiveDataFrameColumn<float>("UpperEstimate"); // Makes a column of length 3. Filled with nulls initially

            foreach (var output in forecastOutputs)
            {
                forecastDates.Append(output.Date);
                actualConfirmedCases.Append(output.ActualConfirmed);
                forecastCases.Append(output.Forecast);
                lowerEstimates.Append(output.LowerEstimate);
                upperEstimates.Append(output.UpperEstimate);
            }

            Console.WriteLine(("Total Confirmed Cases Forecast"));
            var forecastDataFrame = new DataFrame(forecastDates, actualConfirmedCases, lowerEstimates, forecastCases, upperEstimates);
            Console.WriteLine(forecastDataFrame);

            Console.WriteLine(forecasts.Forecast.Select(x => (int)x));
            //Chart.Show();
            var predictionStartDate = dateAtSplit.AddDays(-1); // lastDate.AddDays(1);

            var newDates = new List<DateTime>();
            var fullDates = new List<DateTime>();
            fullDates.AddRange(dates.Take(numTrain));

            var fullTotalConfirmedCases = new List<string>();
            fullTotalConfirmedCases.AddRange(totalConfirmedCases.Take(numTrain));

            int diff = totalRows - numTrain;
            for (int index = 0; index < HORIZON + diff; index++)
            {
                if (index < diff)
                {
                    var nextDate = predictionStartDate.AddDays(index + 1);
                    newDates.Add(nextDate);
                    fullTotalConfirmedCases.Add(actualConfirmedCases[index].ToString());
                }
                else
                {
                    var nextDate = predictionStartDate.AddDays(index + 1);
                    newDates.Add(nextDate);
                    fullTotalConfirmedCases.Add(forecasts.Forecast[index - diff].ToString());
                }

            }

            fullDates.AddRange(newDates);

            var layout = new Layout.Layout();
            layout.shapes = new List<Graph.Shape>
            {
                new Graph.Shape
                {
                    x0 = predictionStartDate,
                    x1 = predictionStartDate,
                    y0 = "0",
                    y1 = "1",
                    xref = 'x',
                    yref = "paper",
                    line = new Graph.Line() {color = "red", width = 2}
                }
            };

                        var predictionChart = Chart.Plot(
                            new[]
                            {
                    new Graph.Scattergl()
                    {
                        x = fullDates.ToArray(),
                        y = fullTotalConfirmedCases.ToArray(),
                        mode = "lines+markers"
                    }
                            },
                            layout
                        );

                        predictionChart.WithTitle("Number of Confirmed Cases over Time");
                        Chart.Show(predictionChart);

                        Graph.Scattergl[] scatters = {
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.UpperBoundConfirmed,
                    fill = "tonexty",
                    name = "Upper bound"
                },
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.Forecast,
                    fill = "tonexty",
                    name = "Forecast"
                },
                new Graph.Scattergl() {
                    x = newDates,
                    y = forecasts.LowerBoundConfirmed,
                    fill = "tonexty",
                    name = "Lower bound"
                }
            };

            var predictionChart2 = Chart.Plot(scatters);
            predictionChart2.Width = 600;
            predictionChart2.Height = 600;
            Chart.Show(predictionChart2);

           /* var lastDate = DateTime.Parse(dates.LastOrDefault());
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
            Chart.Show(chart1);*/
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

            /// <summary>
            /// The predicted minimum values for the forecasted period.
            /// </summary>
            public float[] LowerBoundConfirmed { get; set; }

            /// <summary>
            /// The predicted maximum values for the forecasted period.
            /// </summary>
            public float[] UpperBoundConfirmed { get; set; }
        }

        /// <summary>
        /// Represents the output to be used for display
        /// </summary>
        public class ForecastOutput
        {
            /// <summary>
            /// Date of confirmed case
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Number of actual confirmed cases
            /// </summary>
            public float ActualConfirmed { get; set; }

            /// <summary>
            /// Lower bound confirmed cases
            /// </summary>
            public float LowerEstimate { get; set; }

            /// <summary>
            /// Predicted confirmed cases
            /// </summary>
            public float Forecast { get; set; }

            /// <summary>
            /// Upper bound confirmed cases
            /// </summary>
            public float UpperEstimate { get; set; }
        }
    }
}
