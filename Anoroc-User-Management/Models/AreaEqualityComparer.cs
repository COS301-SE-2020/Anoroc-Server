using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    class AreaEqualityComparer : IEqualityComparer<Area>
    {
        public bool Equals(Area x, Area y)
        {
            return x.Area_ID == y.Area_ID;
        }

        public int GetHashCode(Area obj)
        {
            return obj.Area_ID.GetHashCode();
        }
    }
}
