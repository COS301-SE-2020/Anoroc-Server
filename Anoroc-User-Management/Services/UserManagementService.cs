using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using Nancy.Routing.Trie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class UserManagementService : IUserManagementService
    {
        IDatabaseEngine DatabaseEngine;
        int Token_Length;
        string WebAppToken;
        public UserManagementService(IDatabaseEngine databaseEngine, int _Token_Length, string web_token)
        {
            DatabaseEngine = databaseEngine;
            Token_Length = _Token_Length;
            WebAppToken = web_token;
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
    }
}
