using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System.Collections.Generic;
using System.IO;

namespace Anoroc_User_Management.Services
{
    public class NotificationService : INotificationService
    {

        public readonly IDatabaseEngine _databaseEngine;
        public NotificationService(IDatabaseEngine databaseEngine)
        {
            _databaseEngine = databaseEngine; 
        }

        public List<Notification> getNotificationFromDatabase(string access_token)
        {
            return _databaseEngine.Get_All_Notifications_Of_User(access_token);
        }

        public void SaveNotificationToDatabase(Notification notification)
        {
            _databaseEngine.Add_Notification(notification);
        }
    }
}