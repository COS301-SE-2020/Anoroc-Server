using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Service called everytime the client sends a new location point.
    /// This service calculates if a user has come in contact with a carrier, it then sends the location points to the ClusterService to see if the user is getting close
    /// to a hotspot area.
    /// </summary>
    public class CrossedPathService
    {
    }
}
