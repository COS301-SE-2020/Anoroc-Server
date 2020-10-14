using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Anoroc_User_Management.Services
{
    public class NotificationService : INotificationService
    {

        public readonly IDatabaseEngine _databaseEngine;
        public readonly SQL_DatabaseService sQL_DatabaseService;
        private IUserManagementService UserManagementService;
        private string OurEmail;
        private string Password;

        public NotificationService(SQL_DatabaseService databaseService)
        {
            sQL_DatabaseService = databaseService;
        }
        public NotificationService(IDatabaseEngine databaseEngine, string email, string pass, IUserManagementService user)
        {
            _databaseEngine = databaseEngine;
            OurEmail = email;
            Password = pass;
            UserManagementService = user;
        }
        public void SaveNotificationToDatabaseWithDBService(Notification notification)
        {
            sQL_DatabaseService.Add_Notification(notification);
        }
        public void EmailNotificationToUser(string accessToken, string body)
        {
            //$"<p>You may have come into contact with a carrier at this location.<br>Please ensure that you wear your mask and adhere to Social Distancing.</p><table><tr><th>Locality</th><th>Province</th><th>City</th><th>Country</th><th>DateTime</th></tr><tr><td>{location.Region.Suburb}</td><td>{location.Region.Province}</td><td>{location.Region.City}</td><td>{location.Region.Country}</td><td>{location.Created}</td></table><br><p>Feel free to email us for any queries</p><h1>South African Covid-19 Hotline: 0800 029 999</h1>";
            if (UserManagementService.GetEmailNotificationSettings(accessToken))
            {
                var userEmail = UserManagementService.GetUserEmail(accessToken);

                if (OurEmail != "")
                {
                    using (MailMessage mm = new MailMessage(OurEmail, userEmail))
                    {
                        mm.Subject = "Contagion Encounter";
                        mm.Body = body;
                        mm.IsBodyHtml = true;
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential(OurEmail, Password);
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);
                        }
                    }
                }
            }
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