using Anoroc_User_Management.Models;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Interfaces
{
    public interface IUserManagementService
    {
        /// <summary>
        /// Adds the user to the database
        /// </summary>
        /// <param name="user">The user to add</param>
        /// <returns>The user's static custom access token</returns>
        string addNewUser(User user);

        /// <summary>
        ///  Checks if the user token passed exists in the database
        /// </summary>
        /// <param name="user_access_token"> The static custom token to check</param>
        /// <returns>True if the token exists, false otherwise. </returns>
        public bool ValidateUserToken(string user_access_token);

        /// <summary>
        ///  Updates the user's carrier status
        /// </summary>
        /// <param name="access_token">The token of the user that we are updating.</param>
        /// <param name="status">The new status that is to be set.</param>
        public void UpdateCarrierStatus(string access_token, string status);

        /// <summary>
        /// Inserts the user's new firebase token.
        /// </summary>
        /// <param name="access_token">The static custom access token of the user </param>
        /// <param name="firebase">The new firebase token. </param>
        public void InsertFirebaseToken(string access_token, string firebase);

        /// <summary>
        ///  Gets the user's custom access token.
        /// </summary>
        /// <param name="userEmail"> The email address of the user which is used to identify them. </param>
        /// <returns>The user's custom access token.</returns>
        public string UserAccessToken(string userEmail);

        public int GetUserIncidents(string access_token);
        public void SetUserIncrements(string access_token, int incidents);

        public void SaveProfileImage(string access_toke, string image);
        public string GetProfileImage(string access_token);
        string ReturnUserData(string token);
        bool GetAnonomity(string token);
        public bool CheckXamarinKey(string key);
        public string GetUserEmail(string token);
        public bool SendData(string token);
        public string getXamarinKeyForTest();
        public bool ToggleUserAnonomity(string token, bool value);
        public void CompletelyDeleteUser(string token);
    }
}
