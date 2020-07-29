using DBSCAN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class PointData : IPointData
    {
        public PointData(double x, double y)
        {
           
        }

        ref readonly Point IPointData.Point => new Point(1, 2);
    }
}
