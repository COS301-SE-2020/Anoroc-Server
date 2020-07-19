using Anoroc_User_Management.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

// Firebase
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;


namespace Anoroc_User_Management.Services
{
    public class FirebaseService : IMobileMessagingClient
    {
        private FirebaseApp _defaultApp;
        private readonly FirebaseMessaging _messaging;

        public FirebaseService()
        {
            Config();
            _messaging = FirebaseMessaging.GetMessaging(_defaultApp);
        }

        private void Config()
        {
            _defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential =
                    GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "key.json")),
            });

        }

        public async Task SendNotification()
        {
            var result = await _messaging.SendAsync(CreateNotification());
            Console.WriteLine(result);
        }

        private Message CreateNotification()
        {
            return new Message()
            {
                Token =
                    "eKpj5XPkQP8:APA91bG1NWX8akOflJSpmQoduQzgYDMolTsjKQXr4QX5ZMXLvzcGrKdsazjRWahamLXBWjNVlrthHcJ-ybzd5YWKbU_tvzQroYHhMq_Oy-YVCzQ-HnJ7sT3_tuxVko4Qixo5hml9CkyP",
                Notification = new Notification()
                {
                    Body = "You have come into contact",
                    Title = "Contagaion Encounter"
                }
            };
        }
    }
}