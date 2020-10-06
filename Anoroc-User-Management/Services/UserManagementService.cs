using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Nancy.Routing.Trie;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class UserManagementService : IUserManagementService
    {
        IDatabaseEngine DatabaseEngine;
        int Token_Length;
        string WebAppToken;
        private string XamarinKey;
        private string OurEmail;
        private string SuperSecretPassword;
        public UserManagementService(IDatabaseEngine databaseEngine, int _Token_Length, string web_token, string xamarin_key, string email, string pass)
        {
            OurEmail = email;
            SuperSecretPassword = pass;
            DatabaseEngine = databaseEngine;
            Token_Length = _Token_Length;
            WebAppToken = web_token;
            XamarinKey = xamarin_key;
        }
        public string addNewUser(User user)
        {
            user.carrierStatus = false;
            user.AccessToken = TokenGenerator.NewToken(Token_Length);
            DatabaseEngine.Insert_User(user);
            return user.AccessToken;
        }

        public void InsertFirebaseToken(string access_token, string firebase)
        {
            DatabaseEngine.Insert_Firebase_Token(access_token, firebase);
        }

        public void UpdateCarrierStatus(string access_token, string status)
        {
            if (status.ToLower().Equals("negative"))
            {
                SetUserIncrements(access_token, 0);
            }
            DatabaseEngine.Update_Carrier_Status(access_token, status);
        }

        public bool ValidateUserToken(string user_access_token)
        {
            if (user_access_token == WebAppToken)
                return true;
            else
                return DatabaseEngine.Validate_Access_Token(user_access_token);
        }

        public string UserAccessToken(string userEmail)
        {
            var access_token = DatabaseEngine.Get_User_Access_Token(userEmail);
            if (string.IsNullOrEmpty(access_token))
                return null;
            else
                return access_token;
        }

        public int GetUserIncidents(string access_token)
        {
            return DatabaseEngine.Get_Incidents(access_token);
        }

        public void SetUserIncrements(string access_token, int incidents)
        {
            if (incidents == -1)
                DatabaseEngine.Increment_Incidents(access_token);
            else
                DatabaseEngine.Set_Incidents(access_token, incidents);
        }

        public void SaveProfileImage(string access_token, string image)
        {
            DatabaseEngine.Set_Profile_Picture(access_token, image);
        }

        public string GetProfileImage(string access_token)
        {
            return DatabaseEngine.Get_Profile_Picture(access_token);
        }

        public string ReturnUserData(string token)
        {
            string[] columnUser = {
                            "FirstName",
                            "UserSurname",
                            "Email",
                            "carrierStatus",
                            "totalIncidents",
                            "ProfilePicture"
                            };
            var builder = new StringBuilder();

            builder.AppendJoin(",", columnUser);
            builder.AppendLine();
            var user = DatabaseEngine.Get_Single_User(token);
            string[] values = { user.FirstName, user.UserSurname, user.Email, user.carrierStatus.ToString(), user.totalIncidents.ToString(), user.ProfilePicture };
            builder.AppendJoin(",", values);
            builder.AppendLine();

            string[] columnNotification =
            {
                "Title",
                "Body",
                "Created",
                "Risk"
            };

            builder.AppendJoin(",", columnNotification);
            builder.AppendLine();
            var notifications = DatabaseEngine.Get_All_Notifications_Of_User(token);
            if (notifications != null)
            {
                notifications.ForEach(notification =>
                {
                    string[] row = { notification.Title, notification.Body, notification.Created.ToString(), notification.Risk.ToString() };
                    builder.AppendJoin(",", row);
                    builder.AppendLine();
                });
            }


            string[] columnLocation =
            {
                "Latitude",
                "Longitude",
                "Carrier_Data_Point",
                "Created",
                "Country",
                "Province",
                "City",
                "Suburb"
            };
            builder.AppendJoin(",", columnLocation);
            builder.AppendLine();
            var locations = DatabaseEngine.Select_List_Locations().Where(loc => loc.AccessToken == token) as List<Location>;
            if (locations != null)
            {
                locations.ForEach(location =>
                {
                    if (location != null)
                    {
                        if (location.Region != null)
                        {
                            string[] row = { location.Latitude.ToString(), location.Longitude.ToString(), location.Carrier_Data_Point.ToString(), location.Created.ToString(), location.Region.Country, location.Region.Province, location.Region.City, location.Region.Suburb };
                            builder.AppendJoin(",", row);
                            builder.AppendLine();
                        }
                    }
                });
            }

            string[] columnItinerary =
            {
                "Created",
                "TotalItineraryRisk"
            };
            string[] itineraryLocation =
            {
                "Latitude",
                "Longitude",
                "Carrier_Data_Point",
                "Created",
                "Country",
                "Province",
                "City",
                "Suburb",
                "Risk"
            };
            
            var itineraries = DatabaseEngine.Get_Itinerary_Risks_By_Token(token);
            if (itineraries != null)
            {
                itineraries.ForEach(itinerary =>
                {
                    builder.AppendJoin(",", columnItinerary);
                    builder.AppendLine();
                    string[] row = { itinerary.Created.ToString(), itinerary.TotalItineraryRisk.ToString() };
                    builder.AppendJoin(",", row);
                    builder.AppendLine();
                    builder.AppendJoin(",", itineraryLocation);
                    builder.AppendLine();
                    var keys = itinerary.LocationItineraryRisks.Keys;
                    var values = itinerary.LocationItineraryRisks.Values;
                    for (int i = 0; i < itinerary.LocationItineraryRisks.Count; i++)
                    {
                        string[] row2 = { keys.ElementAt(i).Latitude.ToString(), keys.ElementAt(i).Longitude.ToString(), keys.ElementAt(i).Carrier_Data_Point.ToString(), keys.ElementAt(i).Created.ToString(), keys.ElementAt(i).Region.Country, keys.ElementAt(i).Region.Province, keys.ElementAt(i).Region.City, keys.ElementAt(i).Region.Suburb, values.ElementAt(i).ToString() };
                        builder.AppendJoin(",", row2);
                        builder.AppendLine();
                    }
                });
            }
            
            return builder.ToString();
        }


        public bool CheckXamarinKey(string key)
        {
            return XamarinKey == key;
        }

        public string GetUserEmail(string token)
        {
            return DatabaseEngine.Get_User_Email(token);
        }
        public string getXamarinKeyForTest()
        {
            return XamarinKey;
        }
        public bool SendData(string token)
        {
            var data = ReturnUserData(token);

            string fileName = "userdata_" + DateTime.UtcNow + ".csv";
            byte[] fileBytes = Encoding.UTF8.GetBytes(data);

            //return File(fileBytes, "text/csv", fileName);
            using (MailMessage mm = new MailMessage(OurEmail, GetUserEmail(token)))
            {
                mm.Subject = "Anoroc User Data";
                mm.Body = "Hi. Attached is all data we have of you in our database.";

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
                var result = new FileStreamResult(stream, "text/plain");
                result.FileDownloadName = "userdata_" + DateTime.Now + ".csv";

                mm.Attachments.Add(new Attachment(result.FileStream, result.FileDownloadName));
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(OurEmail, SuperSecretPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            return true;
        }

        public bool ToggleUserAnonomity(string token)
        {
            return DatabaseEngine.Set_User_Anonymous(token);
        }

        public bool GetAnonomity(string token)
        {
            return DatabaseEngine.Get_Single_User(token).Anonymous;
        }

        public void CompletelyDeleteUser(string token)
        {
            var notifications = DatabaseEngine.Get_All_Notifications_Of_User(token);
            notifications.ForEach(notification =>
            {
                DatabaseEngine.Delete_Notification(notification);
            });

            var locations = DatabaseEngine.Select_Locations_By_Access_Token(token);
            locations.ForEach(location =>
            {
                DatabaseEngine.Delete_Location(location);
            });

            var itineraries = DatabaseEngine.Get_Itinerary_Risks_By_Token(token);
            itineraries.ForEach(itinerary =>
            {
                DatabaseEngine.Delete_Itinerary_Risk_By_ID(itinerary.ID);
            });

            var user = DatabaseEngine.Get_Single_User(token);
            DatabaseEngine.Delete_User(user);
        }
    }
}
