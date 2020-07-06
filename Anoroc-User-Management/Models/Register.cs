namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model to store user information that is registering an Anoroc account
    /// </summary>
    public class Register
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        
    }
}