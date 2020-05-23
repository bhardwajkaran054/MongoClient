/// <summary>
/// 
/// This application design and developed by KARAN BHARDWAJ, this is MongoDB Client Application like MongoDB Shell but 
/// it more user friendly as compare to Shell, it doesn't require any MongoDB Query knowledge.
/// If require any help feel free to contact at bhardwajkaran054@gmail.com or check out my GitHub Repos at 
/// github.com/bhardwajkaran054
/// 
/// </summary>

using MongoDB.Driver;
using System.Collections.Generic;

namespace MongoDBClientApplication
{
    public class Mongo
    {
        private readonly MongoClient dbClient;//Store the connection string
        public Mongo(string connectionString)//constructor
        {
            dbClient = new MongoClient(connectionString);
        }

        public List<string> DBList()//Return Database List
        {
            var dbList = dbClient.ListDatabaseNames().ToList();
            return dbList;
        }

        public List<string> CollectionList(string DbName)//Return Collection List
        {
            var database = dbClient.GetDatabase(DbName);
            var collectionList = database.ListCollectionNames().ToList();
            return collectionList;
        }

        public IMongoDatabase GetDb(string dbName)
        {
            var database = dbClient.GetDatabase(dbName);
            return database;
        }

    }
}
