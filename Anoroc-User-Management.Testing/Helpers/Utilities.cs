using System;
using System.Collections.Generic;
using System.Diagnostics;
using Anoroc_User_Management.Models;

namespace Anoroc_User_Management.Testing.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AnorocDbContext db)
        {
            db.Users.AddRange(GetSeedingUsers());
            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void ReinitializeDbForTests(AnorocDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            InitializeDbForTests(db);
        }

        public static List<User> GetSeedingUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    AccessToken = "12345abcd",
                    carrierStatus = false,
                    currentlyLoggedIn = true,
                    Email = "tn.selahle@gmail.com",
                    Firebase_Token = Guid.NewGuid().ToString(),
                    FirstName = "Tebogo",
                    UserSurname = "Selahle",
                    Location = new Location(55.5, 12.2),
                    loggedInAnoroc = false,
                    loggedInFacebook = false,
                    loggedInGoogle = false
                },
                new User()
                {
                    AccessToken = Guid.NewGuid().ToString(),
                    carrierStatus = false,
                    currentlyLoggedIn = true,
                    Email = "anrich96@gmail.com",
                    Firebase_Token = Guid.NewGuid().ToString(),
                    FirstName = "Anrich",
                    UserSurname = "Hildebrand",
                    Location = new Location(155.5, 122.2),
                    loggedInAnoroc = false,
                    loggedInFacebook = false,
                    loggedInGoogle = false
                },
                new User()
                {
                    AccessToken = Guid.NewGuid().ToString(),
                    carrierStatus = true,
                    currentlyLoggedIn = true,
                    Email = "jacob.zuma@gmail.com",
                    Firebase_Token = Guid.NewGuid().ToString(),
                    FirstName = "Jacob",
                    UserSurname = "Zuma",
                    Location = new Location(195.3, 132.2),
                    loggedInAnoroc = false,
                    loggedInFacebook = false,
                    loggedInGoogle = false,
                    
                }
            };
        }
    }
}