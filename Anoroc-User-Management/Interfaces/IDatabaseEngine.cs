using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IDatabaseEngine
    {
        public bool Test_Connection();
        public dynamic Execute_Query(string sql, string table);
    }
}
