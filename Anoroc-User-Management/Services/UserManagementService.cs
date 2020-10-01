using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Nancy.Routing.Trie;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public UserManagementService(IDatabaseEngine databaseEngine, int _Token_Length, string web_token, string xamarin_key)
        {
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

            builder.AppendJoin(";", columnUser);
            builder.AppendLine();
            var user = DatabaseEngine.Get_Single_User(token);
            string[] values = { user.FirstName, user.UserSurname, user.Email, user.carrierStatus.ToString(), user.totalIncidents.ToString(), user.ProfilePicture };
            builder.AppendJoin(";", values);
            builder.AppendLine();

            string[] columnNotification =
            {
                "Title",
                "Body",
                "Created",
                "Risk"
            };

            builder.AppendJoin(";", columnNotification);
            builder.AppendLine();
            var notifications = DatabaseEngine.Get_All_Notifications_Of_User(token);
            if (notifications != null)
            {
                notifications.ForEach(notification =>
                {
                    string[] row = { notification.Title, notification.Body, notification.Created.ToString(), notification.Risk.ToString() };
                    builder.AppendJoin(";", row);
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
            builder.AppendJoin(";", columnLocation);
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
                            builder.AppendJoin(";", row);
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
                    builder.AppendJoin(";", columnItinerary);
                    builder.AppendLine();
                    string[] row = { itinerary.Created.ToString(), itinerary.TotalItineraryRisk.ToString() };
                    builder.AppendJoin(";", row);
                    builder.AppendLine();
                    builder.AppendJoin(";", itineraryLocation);
                    builder.AppendLine();
                    var keys = itinerary.LocationItineraryRisks.Keys;
                    var values = itinerary.LocationItineraryRisks.Values;
                    for (int i = 0; i < itinerary.LocationItineraryRisks.Count; i++)
                    {
                        string[] row2 = { keys.ElementAt(i).Latitude.ToString(), keys.ElementAt(i).Longitude.ToString(), keys.ElementAt(i).Carrier_Data_Point.ToString(), keys.ElementAt(i).Created.ToString(), keys.ElementAt(i).Region.Country, keys.ElementAt(i).Region.Province, keys.ElementAt(i).Region.City, keys.ElementAt(i).Region.Suburb, values.ElementAt(i).ToString() };
                        builder.AppendJoin(";", row2);
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
    }
}
