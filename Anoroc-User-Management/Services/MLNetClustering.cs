using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Newtonsoft.Json;



namespace Anoroc_User_Management.Services
{

    public class MLNetClustering : IClusterService
    {
        Points items;
        PredictionEngine<Point, ClusterPrediction> predictor;
        MLContext mlContext;
        TransformerChain<ClusteringPredictionTransformer<KMeansModelParameters>> Trained_Model;
        VBuffer<float>[] centroids;
        IDataView dataView;
        public MLNetClustering()
        {
            string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "ClusteringModel.zip");
            mlContext = new MLContext(seed: 0);

            string json;
            using (StreamReader r = new StreamReader("TempData/Points.json"))
            {
                json = r.ReadToEnd();

                items = JsonConvert.DeserializeObject<Points>(json);
            }



            dataView = mlContext.Data.LoadFromEnumerable(items.PointArray);

            //var testTrainData = mlContext.Clustering.TrainTestSplit();

            string featuresColumnName = "Features";
            var pipeline = mlContext.Transforms
                .Concatenate(featuresColumnName, "Latitude", "Longitude")
                .Append(mlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: 15));

            Trained_Model = pipeline.Fit(dataView);

            var modelEnumerator = Trained_Model.GetEnumerator();
            modelEnumerator.MoveNext(); // The Concat Transform
            modelEnumerator.MoveNext();
            var kMeansModel = Trained_Model.LastTransformer;

            centroids = default;
            var modelParams = kMeansModel.Model;
            modelParams.GetClusterCentroids(ref centroids, out var k);



            using (var fileStream = new FileStream(_modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                mlContext.Model.Save(Trained_Model, dataView.Schema, fileStream);
            }
            var holder = centroids[0].GetValues();
            predictor = mlContext.Model.CreatePredictionEngine<Point, ClusterPrediction>(Trained_Model);
            var lat = holder[0];
        }

        public string Check_Close_To_Cluster(Point point)
        {
            var prediction = predictor.Predict(point);
            Debug.WriteLine($"Cluster: {prediction.PredictedClusterId}");
            Debug.WriteLine($"Cluster: {string.Join(" ", prediction.Distances)}");

            return ($"Cluster: {prediction.PredictedClusterId}") + "\n" + $"Distances: {string.Join(" ", prediction.Distances)}";
        }

        

        private class DataPoint
        {
            // The label is not used during training, just for comparison with the
            // predicted label
            [KeyType(2)]
            public uint Label { get; set; }

            [VectorType(50)]
            public float[] Features { get; set; }
        }

        public string Clustering()
        {
           

            var cleanCentroids = Enumerable.Range(0, 1).ToDictionary(x => (uint)x, x =>
            {
                var values = centroids[x - 1].GetValues().ToArray();
                return values;
            });

            var points = new Dictionary<uint, List<(double X, double Y)>>();
            /*foreach (var dp in items.PointArray)
            {
                var prediction = predictor.Predict(dp);

                var weightedCentroid = cleanCentroids[prediction.PredictedClusterId].Zip(dp.Features, (x, y) => x * y);
                var point = (X: weightedCentroid.Take(weightedCentroid.Count() / 2).Sum(), Y: weightedCentroid.Skip(weightedCentroid.Count() / 2).Sum());

                if (!points.ContainsKey(prediction.PredictedClusterId))
                    points[prediction.PredictedClusterId] = new List<(double X, double Y)>();
                points[prediction.PredictedClusterId].Add(point);
            }*/
            return "";
        }

        public dynamic GetClusters(Area area)
        {
            throw new NotImplementedException();
        }

        public dynamic GetClustersPins(Area area)
        {
            throw new NotImplementedException();
        }

        public void AddLocationToCluster(Location location)
        {
            throw new NotImplementedException();
        }

        public dynamic ClustersInRage(Location location, double Distance_To_Cluster_Center)
        {
            throw new NotImplementedException();
        }
    }
}
