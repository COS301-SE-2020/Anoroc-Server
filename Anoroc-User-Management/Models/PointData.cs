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
        public long Location_ID { get; set; }
        public PointData(double x, double y, bool created, DateTime dateTime, Area area)
        {
            Region = area;
            Created = dateTime;
            CarrierDataPoint = created;
            _point = new Point(x, y);
        }

        public PointData(long location_ID, double latitude, double longitude, bool carrier_Data_Point, DateTime created, Area region)
        {
            this.Location_ID = location_ID;
            Region = region;
            Created = created;
            CarrierDataPoint = carrier_Data_Point;
            _point = new Point(latitude, longitude);
        }

        ref readonly Point IPointData.Point => ref _point;
    }
}
