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
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Anoroc_User_Management.Services
{
    public class FirebaseService : IMobileMessagingClient
    {
        private FirebaseApp _defaultApp;
        private readonly FirebaseMessaging _messaging;
        private string OurEmail;
        private string Password;
        private IUserManagementService UserManagementService;
        //public readonly SQL_DatabaseService _databaseEngine;
        //public readonly NotificationService _notificationService;
        private static Boolean created = false;
        public FirebaseService(string ouremail, string pass, IUserManagementService userManagementService)
        {
            Config();
            _messaging = FirebaseMessaging.GetMessaging(_defaultApp);
            OurEmail = ouremail;
            Password = pass;
            UserManagementService = userManagementService;
        }

        public FirebaseService()
        {
            Config();
            _messaging = FirebaseMessaging.GetMessaging(_defaultApp);
        }

        ~FirebaseService()
        {
            _defaultApp?.Delete();    
        }

        /// <summary>
        /// Constructor with SQL_database service
        /// </summary>
        /*public FirebaseService(SQL_DatabaseService databaseEngine)
        {
           
            _databaseEngine = databaseEngine;
            _notificationService = new NotificationService(_databaseEngine);
            if(created == false)
            {
                Config();
                _messaging = FirebaseMessaging.GetMessaging(_defaultApp);
                created = true;
            }
   
        }*/

        /// <summary>
        /// Configures the Firebase app to be used
        /// </summary>
        private void Config()
        {
            _defaultApp = FirebaseApp.DefaultInstance;
            _defaultApp ??= FirebaseApp.Create(new AppOptions
            {
                Credential =
                    GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "key.json")),
            });

        }

        /// <summary>
        /// Sends a notification to firebase asynchronously
        /// </summary>
        /// <returns>A Task object</returns>
        public async Task SendNotification(Location location, string accessToken,string firebaseToken, int risk)
        {
            //Convert firebase token to access token
            string access_Token = accessToken;
            //Creating notification object
            string body = $"You have come into contact with a carrier near {location.Region.Suburb}. Click for more info";
            string title = "Contagion Encounter";
            Anoroc_User_Management.Models.Notification save = new Anoroc_User_Management.Models.Notification(access_Token, title, body);
          

            var result = await _messaging.SendAsync(CreateNotification(location, firebaseToken, risk));

            Console.WriteLine(result);
            //Saving notification object with access token to the database.
            //_databaseEngine.Add_Notification(save);
            saveNotification(save);
            EmailNotification(accessToken, $"<p>You may have come into contact with a carrier at this location.<br>Please ensure that you wear your mask and adhere to Social Distancing.</p><table><tr><th>Locality</th><th>Province</th><th>City</th><th>Country</th><th>DateTime</th></tr><tr><td>{location.Region.Suburb}</td><td>{location.Region.Province}</td><td>{location.Region.City}</td><td>{location.Region.Country}</td><td>{location.Created}</td></table><br><p>Feel free to email us for any queries.</p><h2>South African Emergency Hotline: 0800 029 999</h2>");
        }

        public async void EmailNotification(string user_token, string body)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler);

            Token token_object = new Token();
            token_object.Object_To_Server = body;
            token_object.access_token = user_token;
            var data = JsonConvert.SerializeObject(token_object);
            var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
            stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            Uri Anoroc_Uri = new Uri("https://localhost:5001/" + "notification/sendEmail");
            HttpResponseMessage responseMessage;

            try
            {
                responseMessage = await client.PostAsync(Anoroc_Uri, stringcontent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine("Saved!");

                }
            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {
                throw e;
            }
        }

        public async void saveNotification(Models.Notification save)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            HttpClient client = new HttpClient(clientHandler);

            
            Token token_object = new Token();
            token_object.Object_To_Server = JsonConvert.SerializeObject(save);
            var data = JsonConvert.SerializeObject(token_object);
            var stringcontent = new StringContent(data, Encoding.UTF8, "application/json");
            stringcontent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            Uri Anoroc_Uri = new Uri("https://localhost:5001/" + "notification/save");
            HttpResponseMessage responseMessage;

            try
            {
                responseMessage = await client.PostAsync(Anoroc_Uri, stringcontent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine("Saved!");
                    
                }
            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
            {
                throw e;
            }
        }
        

        /// <summary>
        /// Creates a notification to be sent to firebase
        /// </summary>
        /// <returns>A Message object</returns>
        private Message CreateNotification(Location location, string firebaseToken, int risk)
        {
            return new Message()
            {
                // get firebase token from database using location.token
                Token = firebaseToken,
                Notification = new Notification()
                {
                    Body = "You have come into contact with a carrier. Click for more info",
                    Title = "Contagion Encounter"
                },
                Data = new Dictionary<string, string>()
                {
                    { "DateTime", location.Created.ToString() },
                    { "Location", location.ToString() },
                    { "Risk", risk.ToString() }
                }
            };
        }
    }
}