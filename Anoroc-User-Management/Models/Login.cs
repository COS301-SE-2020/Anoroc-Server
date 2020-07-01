namespace Anoroc_User_Management.Models
{
    /// <summary>
    /// Model used to manage the user that is loging in
    /// </summary>
    public class Login
    {
        //TODO: Capitalize email and password
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}