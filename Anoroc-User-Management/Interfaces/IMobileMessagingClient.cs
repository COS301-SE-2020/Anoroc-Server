using System.Threading.Tasks;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Interfaces
{
    /// <summary>
    /// Interface for each notification server. i.e. Firebase, Azure Notification Hub
    /// </summary>
    public interface IMobileMessagingClient
    {
        /// <summary>
        /// Send a notification to the server
        /// </summary>
        public Task SendNotification(Location location);
    }
}