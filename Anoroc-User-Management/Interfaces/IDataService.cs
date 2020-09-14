using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IDataService
    {
        Task<List<DataOverTimeObject>> GetCasesPerDate();
    }
}
