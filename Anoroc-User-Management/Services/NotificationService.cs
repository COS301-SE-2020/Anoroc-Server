using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System.Collections.Generic;
using System.IO;

namespace Anoroc_User_Management.Services
{
    public class NotificationService : INotificationService
    {

        public readonly IDatabaseEngine _databaseEngine;
        public readonly SQL_DatabaseService sQL_DatabaseService;

        public NotificationService(SQL_DatabaseService databaseService)
        {
            sQL_DatabaseService = databaseService;
        }
        public NotificationService(IDatabaseEngine databaseEngine)
        {
            _databaseEngine = databaseEngine; 
        }
        public void SaveNotificationToDatabaseWithDBService(Notification notification)
        {
            sQL_DatabaseService.Add_Notification(notification);
        }
        public void SaveNotificationToDatabase(Notification notification)
        {
            _databaseEngine.Add_Notification(notification);
        }
        public List<Notification> SendNotificationToApp(string notification)
        {
            //_databaseEngine.Get_Access_Token_Via_FirebaseToken
            //_databaseEngine.Get_All_Notifications_Of_User(notification);
            //var result = sQL_DatabaseService.Get_All_Notifications_Of_User(notification.ToString());
            var notifications = _databaseEngine.Get_All_Notifications_Of_User(notification);
            return notifications;
        }
    }
}