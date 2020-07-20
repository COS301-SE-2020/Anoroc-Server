using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public class User: DbContext
    {
        public long User_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Access_Token { get; set; }
        public string Firebase_Token { get; set; }
        public bool Facebook_Log_In { get; set; }
        public bool Google_Log_In { get; set; }
        public bool Anoroc_Log_In { get; set; }
        public bool Carrier_Status { get; set; }
        /// <summary>
        /// Constructor to initialise every class memeber defined for type User
        /// </summary>
        /// <param name="userID">User ID specified in databse</param>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <param name="password">password a user has selected</param>
        /// <param name="email">User's email address</param>
        /// <param name="accessToken">Generated access token created by the server</param>
        /// <param name="firebaseToken">Generated token to connect to Firebase</param>
        /// <param name="facebookLogin">True or false showing if the user logged in with facebook</param>
        /// <param name="googleLogin">True or false showing if the user logged in with Google</param>
        /// <param name="anorocLogin">True or false showing if the user logged in with the Anoroc server</param>
        /// <param name="carrierStatus">True or false showing whether the user is a contagent or not</param>
        public User(long userID, string firstName, string lastName, string password, string email, string accessToken, string firebaseToken, bool facebookLogin, bool googleLogin, bool anorocLogin, bool carrierStatus)
        {
            User_ID = userID;
            First_Name= firstName;
            Last_Name=lastName;
            Password=password;
            Email=email;
            Access_Token=accessToken;
            Firebase_Token = accessToken;
            Facebook_Log_In =facebookLogin;
            Google_Log_In=googleLogin;
            Anoroc_Log_In=anorocLogin;
            Carrier_Status=carrierStatus;
        }
        /// <summary>
        /// A helping function to show all the details of a user which will be used for debugging purposes only
        /// </summary>
        /// <returns>A string showing every attribute of a User</returns>
        public override string ToString()
        {
            string returnValue = "";
            returnValue += "ID: " + User_ID;
            returnValue += "Name: " + First_Name;
            returnValue += "Surname: " + Last_Name;
            returnValue += "Password: " + Password;
            returnValue += "Email: " + Email;
            returnValue += "Access Token: " + Access_Token;
            returnValue += "Firebase Token: " + Firebase_Token;
            returnValue += "Facebook Login: " + Facebook_Log_In;
            returnValue += "Google Login: " + Google_Log_In;
            returnValue += "Anoroc Login: " + Anoroc_Log_In;
            returnValue += "Carrier Status: " + Carrier_Status;
            return returnValue;
        }
        /// <summary>
        /// A simple toggle function to change the User's carrier status to the opposite of what it was, so after a certain quarantine period or after a user comes into contact with a contageon,
        /// The function will change their Carrier status.
        /// </summary>
        public void toggleCarrierStatus()
        {
            Carrier_Status = !Carrier_Status;
        }
    }
}
