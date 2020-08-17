using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Anoroc_User_Management.Models.ItineraryFolder;

namespace Anoroc_User_Management.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public string UserSurname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [Key]
        public string AccessToken { get; set; }
        public string Firebase_Token { get; set; }
        public bool loggedInFacebook { get; set; }
        public bool loggedInGoogle { get; set; }
        public bool loggedInAnoroc { get; set; }
        public bool carrierStatus { get; set; }

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
            UserID = userID;
            FirstName= firstName;
            UserSurname=lastName;
            Password=password;
            Email=email;
            AccessToken=accessToken;
            Firebase_Token = accessToken;
            loggedInFacebook =facebookLogin;
            loggedInGoogle=googleLogin;
            loggedInAnoroc=anorocLogin;
            this.carrierStatus= carrierStatus;
        }
        public User()
        {

        }
        /// <summary>
        /// A helping function to show all the details of a user which will be used for debugging purposes only
        /// </summary>
        /// <returns>A string showing every attribute of a User</returns>
        public override string ToString()
        {
            string returnValue = "";
            returnValue += "ID: " + UserID;
            returnValue += "Name: " + FirstName;
            returnValue += "Surname: " + UserSurname;
            returnValue += "Password: " + Password;
            returnValue += "Email: " + Email;
            returnValue += "Access Token: " + AccessToken;
            returnValue += "Firebase Token: " + Firebase_Token;
            returnValue += "Facebook Login: " + loggedInFacebook;
            returnValue += "Google Login: " + loggedInGoogle;
            returnValue += "Anoroc Login: " + loggedInAnoroc;
            returnValue += "Carrier Status: " + carrierStatus;
            return returnValue;
        }
        /// <summary>
        /// A simple toggle function to change the User's carrier status to the opposite of what it was, so after a certain quarantine period or after a user comes into contact with a contageon,
        /// The function will change their Carrier status.
        /// </summary>
        public void toggleCarrierStatus()
        {
            carrierStatus = !carrierStatus;
        }
    }
}
