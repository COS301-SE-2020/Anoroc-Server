using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
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

        public void SaveNotificationToDatabase(Notification notification, string firebaseToken)
        {
            string accessToken = _databaseEngine.Get_Access_Token_Via_FirebaseToken(firebaseToken);

            _databaseEngine.Add_Notification(notification);
        }
    }
}