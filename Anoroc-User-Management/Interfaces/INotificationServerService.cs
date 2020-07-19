namespace Anoroc_User_Management.Interfaces
{
    /// <summary>
    /// Interface for each notification server. i.e. Firebase, Azure Notification Hub
    /// </summary>
    public interface INotificationServerService
    {
        /// <summary>
        /// Configure the server service
        /// </summary>
        public void Config();
        
        /// <summary>
        /// Send a notification to the server
        /// </summary>
        public void SendNotification();
    }
}