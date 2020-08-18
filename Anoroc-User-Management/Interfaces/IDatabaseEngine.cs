using Anoroc_User_Management.Models;
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

        public List<Location> Select_Location_By_Cluster_Reference(long reference);
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

        /// <summary>
        /// Select a list of Locations that are flagged as contagoen locations
        /// </summary>
        /// <returns>A list of all locatoins where the Carrier_Data_Point is set to true</returns>
        public List<Location> Select_List_Carrier_Locations();

        /// <summary>
        /// Select a list of locations that fall into a specific area
        /// </summary>
        /// <param name="area">The area to search all locations by</param>
        /// <returns>A list of all locations that are in the specified area</returns>
        public List<Location> Select_Locations_By_Area(Area area);
        /// <summary>
        /// Select a list of locations based on their ID parameter
        /// </summary>
        /// <param name="id">The Id to search for</param>
        /// <returns>A list of locations that have a specific ID</returns>
        public List<Location> Select_Locations_By_ID(long id);

        /// <summary>
        /// Select a list of locations in a specific area that do not fall within any specific cluster
        /// </summary>
        /// <param name="area">The area where locations will be searched for</param>
        /// <returns>A list of locations within the are provided that do not fall into any cluster</returns>
        public List<Location> Select_Unclustered_Locations(Area area);

        /// <summary>
        /// Toggle the carrier data point for each location that a specific user has in the locations table in the databse.
        /// </summary>
        /// <param name="access_token">The user's access token used to uniquely identify all their locations</param>
        /// <param name="status">The new carrier data point that all the locations will be updated to</param>
        public void Update_Carrier_Locations(string access_token, bool status);

        /// <summary>
        /// Select a list of all Areas but without any duplicate areas
        /// </summary>
        /// <returns>A non duplicate list of all Areas in the database</returns>
        public List<Area> Select_Unique_Areas();

        /// <summary>
        /// Updates a specific cluster in the database with the provided Cluster object
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns>Returns a boolean based on whether the Update was succesfull or not</returns>        
        public bool Update_Cluster(Cluster cluster);
        /// <summary>
        /// Delete al lthe locations that are older than 4 hours from the locations table, and add all these locations to the OldLocations table
        /// </summary>
        /// <returns>A boolean depicting whether the function was successful or not</returns>
        public bool Delete_Locations_Older_Than_Hours(int hours);

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
        /// Retreive a list of Cluster object that will be used to populate the map on the mobile device
        /// </summary>
        /// <returns>A list of Cluster objects from the Database</returns>
        public List<Cluster> Select_List_Clusters();

        /// <summary>
        /// Select all the Clusters from the database that are within a specific area
        /// </summary>
        /// <param name="area">The search parameter to identify which clusters to return</param>
        /// <returns>A list of clusters that are within the specified area</returns>
        public List<Cluster> Select_Clusters_By_Area(Area area);//From OldClusters!!!!
                
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
        /// Updates a specific user's carrier status
        /// </summary>
        /// <param name="access_token">The access token used to determine which user to update</param>
        /// <param name="carrier_status">The new value for the carrier status that will be changed to</param>
        public void Update_Carrier_Status(string access_token, string carrier_status);

        /// <summary>
        /// Search through the databse to retrieve a specific user's email address
        /// </summary>
        /// <param name="access_token">The unique user token parameter to uniquely identify a single user</param>
        /// <returns>A string of a user's email</returns>
        public string GetUserEmail(string access_token);

        /// <summary>
        /// A function to search for a specific user and udate their access token that is stored in the database
        /// </summary>
        /// <param name="user">The user to search for that needs to be updated</param>
        /// <param name="token">The new value that needs to be stored as the access token for the user</param>
        /// <returns>A boolean depicting whether or not the update has changed or not</returns>
        //public bool updateUserToken(User user, string token);

        /// <summary>
        /// Gets a list of users from the database
        /// </summary>
        /// <returns>A list of all the users from the database</returns>
        public List<User> Select_List_Users();

        /// <summary>
        /// Update a specific user
        /// </summary>
        /// <param name="user">the new user the will be updated</param>
        /// <returns>Boolean showing whether or not the update was successful or not</returns>
        public bool Update_User(User user);

        /// <summary>
        /// Deletes a specific user from the user table
        /// </summary>
        /// <param name="user">The user to delete from the database</param>
        /// <returns>A boolean depicting whether or not the delete was successful or not</returns>
        public bool Delete_User(User user);

        /// <summary>
        /// Add a new user to the user table
        /// </summary>
        /// <param name="user">The new user to be added</param>
        /// <returns>A boolean showing whether or not the insert was successful</returns>
        public bool Insert_User(User user);

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

        /// <summary>
        /// Insert a new Area into the database but only insert if that area does not yet exist in the database
        /// </summary>
        /// <param name="area">The new area to be inserted</param>
        /// <returns>A boolean depending on whether the area has been added or not</returns>
        public bool Insert_Area(Area area);

        /// <summary>
        /// Delete a Area from the database
        /// </summary>
        /// <param name="area">The new area to be deleted</param>
        /// <returns>A boolean depending on whether the area has been deleted or not</returns>
        public bool Delete_Area(Area area);

        /// <summary>
        /// Select a single Area from the database that has a specific Area ID
        /// </summary>
        /// <param name="id">The Area ID to filter by when searching through all areas</param>
        /// <returns>A single Areas with a specific ID</returns>
        public Area Select_Area_By_Id(long id);

        /// <summary>
        /// Select all Clusters from the Old Clusters table
        /// </summary>
        /// <returns>A list of oldClusters from the database</returns>
        public List<OldCluster> Select_All_Old_Clusters();

        /// <summary>
        /// Select all old Clusters that are within a specific Area
        /// </summary>
        /// <param name="area">The Area used to determine which clusters to return</param>
        /// <returns>A list of Old Clusters that are within the specified area</returns>        
        public List<OldCluster> Select_Old_Clusters_By_Area(Area area);

        /// <summary>
        /// Insert a cluster into the Old Clusters table
        /// </summary>
        /// <param name="cluster">The old cluster that is to be added</param>
        /// <returns>A boolean depending on whether the insert was successful or not</returns>
        public bool Insert_Old_Cluster(Cluster cluster);

        /// <summary>
        /// Select all old loctions that are not part of a cluster and that are in a specific area
        /// </summary>
        /// <param name="area">The specific are to search by</param>
        /// <returns>A list of Old Locations that are not in a cluster and that are in a specific area</returns>
        public List<OldLocation> Select_Old_Unclustered_Locations(Area area);

        /// <summary>
        /// Similar to Update_Carrier_Locations, but updating all the rows in the Old Locations table to toggle the carrier status
        /// </summary>
        /// <param name="access_token">The access token of a user to uniquely identify all their old locations</param>
        /// <param name="status">The new value to be stored as the carrier data point</param>
        public void Update_Old_Carrier_Locations(string access_token, bool status);//you wasted your time here, remove

        /// <summary>
        /// A temporary function being used to populate our database with mock data for testing purposes
        /// </summary>
        public void populate();

        /// <summary>
        /// Add a new Itinerary Risk to the database,
        /// by first converting it to a primitive type that can be stored in the database
        /// </summary>
        /// <param name="risk">The original complex type that needs to be converted to a primitive type and then added to the database</param>
        public void insertItineraryRisk(ItineraryRisk risk);

        /// <summary>
        /// Select all Itinerary Risks from the database
        /// </summary>
        /// <returns>A list of all the Itinerary Risks in the database</returns>
        public List<ItineraryRisk> GetAllItineraryRisks();

        /// <summary>
        /// Select all Itinerary Risks from the database that have a specific access token
        /// </summary>
        /// <param name="token">The specific access token used to identify which risks to return</param>
        /// <returns>A list of Itinerary Risks that have a specific access token</returns>
        public List<ItineraryRisk> GetItineraryRisksByToken(string token);
    }
}
