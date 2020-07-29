using DBSCAN;

namespace Anoroc_User_Management.Models
{
    public class PointData : IPointData
    {
        private readonly Point _point;
        public PointData(double x, double y)
        {
            _point = new Point(x, y);
        }

        ref readonly Point IPointData.Point => ref _point;
    }
}
