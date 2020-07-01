using Microsoft.VisualBasic;

namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model used to store information of the Token allocated to the client
    /// </summary>
    public class Token
    {

        public string tokenString { get; set; }
        public DateAndTime Expiry { get; set; }
    }
}