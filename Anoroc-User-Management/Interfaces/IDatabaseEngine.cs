using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IDatabaseEngine
    {
        public int Next_Cluster_ID { get; set; }
        public bool Test_Connection();
        public int Get_Cluster_ID();
    }
}
