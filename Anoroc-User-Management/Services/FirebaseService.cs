using Anoroc_User_Management.Interfaces;
using System;
using System.IO;

// Firebase
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;


namespace Anoroc_User_Management.Services
{
    public class FirebaseService : IMobileMessagingClient
    {
        private FirebaseApp DefaultApp;
        public FirebaseService()
        {
            Config();
        }

        private void Config()
        {
            DefaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
            });
            
            // TODO: Remove this line
            Console.WriteLine(DefaultApp.Name);
        }

        public void SendNotification()
        {
            
        }
    }
}