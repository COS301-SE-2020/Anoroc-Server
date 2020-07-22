using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Interfaces
{
    public interface ICrossedPathsService
    {
        public void ProcessLocation(Location location);
    }
}
