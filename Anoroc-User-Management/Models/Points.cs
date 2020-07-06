namespace Anoroc_User_Management.Services
{
    public class Points
    {
        public Point[] PointArray { get; set; }
    }

    public class Point
    {
        public bool Carrier { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
}