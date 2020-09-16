using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
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
    }
}