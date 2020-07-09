using Microsoft.ML.Data;

namespace Anoroc_User_Management.Services
{
    public class Points
    {
        public Point[] PointArray { get; set; }
    }

    public class Point
    {
       
        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}