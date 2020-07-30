using DBSCAN;
using System;

namespace Anoroc_User_Management.Models
{
    public class PointData : IPointData
    {
        public readonly Point _point;
        public DateTime Created { get; set; }
        public bool CarrierDataPoint { get; set; }
        public Area Region { get; set; }
        public PointData(double x, double y, bool created, DateTime dateTime, Area area)
        {
            Region = area;
            Created = dateTime;
            CarrierDataPoint = created;
            _point = new Point(x, y);
        }

        ref readonly Point IPointData.Point => ref _point;
    }
}
