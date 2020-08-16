using Anoroc_User_Management.Interfaces;
using Anoroc_User_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Services
{
    public class UserManagementService : IUserManagementService
    {
        IDatabaseEngine DatabaseEngine;
        public UserManagementService(IDatabaseEngine databaseEngine)
        {
            DatabaseEngine = databaseEngine;
        }
        public string addNewUser(User user)
        {
            // TODO:
            // Insert the user with the user's access token
            return "";
        }
    }
}
