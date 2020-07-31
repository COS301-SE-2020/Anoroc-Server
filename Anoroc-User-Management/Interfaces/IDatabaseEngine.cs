﻿using Anoroc_User_Management.Models;
using Anoroc_User_Management.Services;
using System.Collections.Generic;

namespace Anoroc_User_Management.Interfaces
{
    public interface IDatabaseEngine
    {
        /// <summary>
        /// Database Interface to add new location to Location table in the database
        /// </summary>
        /// <param name="location">An object of type Location is passed to the function to be added to the database</param>
        /// <returns>Returns a boolean based on whether the insert was succesfull or not</returns>
        public bool Insert_Location(Location location);
        /// <summary>
        /// Database Interface to delete a specific Location object from the database 
        /// </summary>
        /// <param name="location">An object of type Location that will be removed from the database</param>
        /// <returns>Returns a boolean based on whether the delete was succesfull or not</returns>
        public bool Delete_Location(Location location);
        /// <summary>
        /// Database interface to update a specific Location in the database
        /// </summary>
        /// <param name="location">An object of type Location that will update the database</param>
        /// <returns>Returns a boolean based on whether the update was succesfull or not</returns>
        public bool Update_Location(Location location);
        /// <summary>
        /// Database interface to return a list of Locations that are retireved from the database
        /// </summary>
        /// <returns>A list of Location Objects from the database</returns>
        public List<Location> Select_List_Locations();
        public List<Location> Select_Locations_By_Area(Area area);  //TODO: Select all locations that are in a specific area
        public List<Area> Select_Unique_Areas();                    //TODO: return list of Area without duplicates
        /// <summary>
        /// Updates a specific cluster in the database with the provided Cluster object
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns>Returns a boolean based on whether the Update was succesfull or not</returns>
        public bool Update_Cluster(Cluster cluster);
        /// <summary>
        /// Deletes the given Cluster object from the database
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns>Returns a boolean based on whether the delete was succesfull or not</returns>
        public bool Delete_Cluster(Cluster cluster);
        /// <summary>
        /// Adding a new Cluster object to the Database
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns>Returns a boolean based on whether the insert was succesfull or not</returns>
        public bool Insert_Cluster(Cluster cluster);
        /// <summary>
        /// This function is meant to retreive a list of Cluster object that will be used to populate the map on the mobile device
        /// </summary>
        /// <returns>A list of Cluster objects from the Database</returns>
        public List<Cluster> Select_List_Clusters();
        /// <summary>
        /// A simple test to ensure that the database is connected properly before operations are done on the database
        /// </summary>
        /// <returns>A boolean based on whether the database has been connected succesfully or not</returns>
        public bool Test_Connection();
        /// <summary>
        /// Retreives the Cluster ID from the Database for a specific cluster set
        /// </summary>
        /// <returns>This function returns the ID of a specific Cluster</returns>
        public long Get_Cluster_ID();
        /// <summary>
        /// Searches the databse for where the specified access token is and replaces the old firebase token with the new one passed as a parameter
        /// </summary>
        /// <param name="access_token">The access token used to find a specific record/instance</param>
        /// <param name="firebase_token">The new firebase token that will be used to update the record</param>
        public void Insert_Firebase_Token(string access_token, string firebase_token);
        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="access_token">The access token used to </param>
        /// <param name="carrier_status"></param>
        public void Update_Carrier_Status(string access_token, string carrier_status);
        /// <summary>
        /// A function to retrieve the Firebase Acess token from the database in the same record where the specified access token is is.
        /// </summary>
        /// <param name="access_token">The access token to search for the specific record to get the firebase token from</param>
        /// <returns>The firebase token that is retrieved from the database</returns>
        public string Get_Firebase_Token(string access_token);
        /// <summary>
        /// A function to check if the access token provided exists within the database
        /// </summary>
        /// <param name="access_token">Teh access token to search for in the database</param>
        /// <returns>A boolean value depending on whether the access token was found or not</returns>
        public bool Validate_Access_Token(string access_token);
        public void populate();
    }
}
