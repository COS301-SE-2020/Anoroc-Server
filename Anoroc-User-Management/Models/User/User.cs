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
        public string FirstName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        [Key]
        public string AccessToken { get; set; }
        public string Firebase_Token { get; set; }
        public bool loggedInFacebook { get; set; }
        public bool loggedInGoogle { get; set; }
        public bool loggedInAnoroc { get; set; }
        public bool carrierStatus { get; set; }
        public bool currentlyLoggedIn { get; set; }
        public int totalIncidents { get; set; }
        public string ProfilePicture { get; set; }
        
        public bool Anonymous { get; set; }
        public bool Subscribed { get; set; }

        //Following 3 declarations are to create one to one relationships between models
        public virtual ICollection<PrimitiveItineraryRisk> PrimitiveItineraryRisks { get; set; }
        public ICollection<Location> Location { get; set; }
        public OldLocation OldLocation { get; set; }
        public ICollection<Notification> Notifications { get; set; }

        public User()
        {
            Anonymous = false;
        }

        public User(string accessToken)
        {
            AccessToken = accessToken;
            Anonymous = false;
            Subscribed = false;
        }
        /// <summary>
        /// A helping function to show all the details of a user which will be used for debugging purposes only
        /// </summary>
        /// <returns>A string showing every attribute of a User</returns>
        public override string ToString()
        {
            string returnValue = "";
            returnValue += "Name: " + FirstName;
            returnValue += "Surname: " + UserSurname;
            returnValue += "Email: " + Email;
            returnValue += "Access Token: " + AccessToken;
            returnValue += "Firebase Token: " + Firebase_Token;
            returnValue += "Facebook Login: " + loggedInFacebook;
            returnValue += "Google Login: " + loggedInGoogle;
            returnValue += "Anoroc Login: " + loggedInAnoroc;
            returnValue += "Carrier Status: " + carrierStatus;
            returnValue += "Currently Logged In: " + currentlyLoggedIn;
            returnValue += "Anonymous: " + Anonymous;
            returnValue += "Subscribed" + Subscribed;
            return returnValue;
        }
        public User(string token, string firebase)
        {
            AccessToken = token;
            Firebase_Token = firebase;
            Email = "email";
            FirstName = "first";
            UserSurname = "surname";
            loggedInFacebook = false;
            loggedInGoogle = false;
            loggedInAnoroc = false;
            carrierStatus = false;
            currentlyLoggedIn = false;
            totalIncidents = 0;
            ProfilePicture = "none";
            Anonymous = false;
            Subscribed = false;
        }
        public User(string token, string firebase, bool carrier)
        {
            AccessToken = token;
            Firebase_Token = firebase;
            carrierStatus = carrier;
            Email = "email";
            FirstName = "first";
            UserSurname = "surname";
            loggedInFacebook = false;
            loggedInGoogle = false;
            loggedInAnoroc = false;
            currentlyLoggedIn = false;
            totalIncidents = 0;
            ProfilePicture = "none";
            Anonymous = false;
            Subscribed = false;
        }
        public User(string token, string firebase, bool carrier, string email)
        {
            AccessToken = token;
            Firebase_Token = firebase;
            carrierStatus = carrier;
            Email = email;
            FirstName = "first";
            UserSurname = "surname";
            loggedInFacebook = false;
            loggedInGoogle = false;
            loggedInAnoroc = false;
            currentlyLoggedIn = false;
            totalIncidents = 0;
            ProfilePicture = "none";
            Anonymous = false;
            Subscribed = false;
        }
        public User(string token, string firebase, int incidents)
        {
            AccessToken = token;
            Firebase_Token = firebase;
            totalIncidents = incidents;
            Email = "email";
            FirstName = "first";
            UserSurname = "surname";
            loggedInFacebook = false;
            loggedInGoogle = false;
            loggedInAnoroc = false;
            carrierStatus = false;
            currentlyLoggedIn = false;
            ProfilePicture = "none";
            Anonymous = false;
            Subscribed = false;
        }
        public User(string token, string firebase, string picture)
        {
            AccessToken = token;
            Firebase_Token = firebase;
            ProfilePicture = picture;
            totalIncidents = 0;
            Email = "email";
            FirstName = "first";
            UserSurname = "surname";
            loggedInFacebook = false;
            loggedInGoogle = false;
            loggedInAnoroc = false;
            carrierStatus = false;
            currentlyLoggedIn = false;
            Anonymous = false;
            Subscribed = false;
        }
    }
}
