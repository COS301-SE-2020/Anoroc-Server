using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    class AreaEqualityComparer : IEqualityComparer<Area>
    {
        public bool Equals(Area leftArea, Area rightArea)
        {
            if (leftArea == null || rightArea == null)
                return false;

            if (leftArea.Country == rightArea.Country)
                if (leftArea.Province == rightArea.Province)
                    if (leftArea.Suburb == rightArea.Suburb)
                        return true;

            return false;
        }        
        public int GetHashCode(Area obj)
        {
            return obj.Area_ID.GetHashCode();
        }
    }
}
