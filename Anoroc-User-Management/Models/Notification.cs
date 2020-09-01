using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model used to manage the Notificaitons to the user
    /// </summary>
    public class Notification
    {
        [Key]
        public long ID { get; }
        [ForeignKey("AccessToken")]
        public string AccessToken { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Created { get; set; }

        public int Risk { get; set; }

        //Following declarations are to create one to one relationships between models

        public User User { get; set; }

        public Notification()
        {

        }
        public Notification(string access_token,string title,string body)
        {
            Created = DateTime.Now;
            AccessToken = access_token;
            Title = title;
            Body = body;
        }
       
    }
}