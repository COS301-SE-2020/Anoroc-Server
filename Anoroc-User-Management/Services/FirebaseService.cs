using Anoroc_User_Management.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Anoroc_User_Management.Models;

// Firebase
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Notification = FirebaseAdmin.Messaging.Notification;


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

        /// <summary>
        /// Configures the Firebase app to be used
        /// </summary>
        private void Config()
        {
            _defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential =
                    GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "key.json")),
            });

        }

        /// <summary>
        /// Sends a notification to firebase asynchronously
        /// </summary>
        /// <returns>A Task object</returns>
        public async Task SendNotification(Location location)
        {
            var result = await _messaging.SendAsync(CreateNotification(location));
            Console.WriteLine(result);
        }

        /// <summary>
        /// Creates a notification to be sent to firebase
        /// </summary>
        /// <returns>A Message object</returns>
        private Message CreateNotification(Location location)
        {
            return new Message()
            {
                // get firebase token from database using location.token
                Token =
                    "eKpj5XPkQP8:APA91bG1NWX8akOflJSpmQoduQzgYDMolTsjKQXr4QX5ZMXLvzcGrKdsazjRWahamLXBWjNVlrthHcJ-ybzd5YWKbU_tvzQroYHhMq_Oy-YVCzQ-HnJ7sT3_tuxVko4Qixo5hml9CkyP",
                Notification = new Notification()
                {
                    Body = "You have come into contact with a carrier. Click for more info",
                    Title = "Contagion Encounter"
                },
                Data = new Dictionary<string, string>()
                {
                    { "DateTime", location.Created.ToString() },
                    { "longitude", location.Coordinate.Longitude.ToString() },
                    { "Latitude", location.Coordinate.Latitude.ToString() }
                }
            };
        }
    }
}