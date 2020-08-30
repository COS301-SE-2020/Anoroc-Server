using System;
using System.Collections.Generic;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model used to manage the Notificaitons to the user
    /// </summary>
    public class Notification
    {
        public string UserAccessToken { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }


        public Notification(string access_token,string title,string body)
        {
            Created = DateTime.Now;
            UserAccessToken = access_token;
            Title = title;
            Body = body;

        }
       

        // TODO: Save notification for each user. (User is identified by the access token.) 
        // TDOD: Retrieve all notification from that specific user.
    }
}