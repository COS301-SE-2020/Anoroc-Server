using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Services
{
    /// <summary>
    /// Class responsible for providing functionality to check if users have crossed paths or not.
    /// </summary>
    public class CrossedPathsService
    {
        /// <summary>
        /// Processes a location point.
        /// If the point falls within a cluster (in danger), the user has to be notified.
        /// </summary>
        /// <param name="location">A location point that has to be processed</param>
        public void ProcessLocation(Location location)
        {
            //TODO Consider making this async
        }
    }
}