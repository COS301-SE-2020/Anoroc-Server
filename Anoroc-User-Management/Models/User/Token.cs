using Microsoft.VisualBasic;
using System;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model used to store information of the Token allocated to the client
    /// </summary>
    public class Token
    {

        public int TokenID { get; set; }
        public string access_token { get; set; }
        public string error_descriptions { get; set; }
        public DateTime expiry_date { get; set; }

        public string Object_To_Server { get; set; }

        public Token() { }
        public byte[] Profile_image { get; set; }
    }
}