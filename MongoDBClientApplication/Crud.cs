/// <summary>
/// 
/// This application design and developed by KARAN BHARDWAJ, this is MongoDB Client Application like MongoDB Shell but 
/// it more user friendly as compare to Shell, it doesn't require any MongoDB Query knowledge.
/// If require any help feel free to contact at bhardwajkaran054@gmail.com or check out my GitHub Repos at 
/// github.com/bhardwajkaran054
/// 
/// </summary>
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MongoDBClientApplication
{
    public class Crud
    {
        private readonly Mongo clientApp;
        private readonly string dbName;
        private readonly string collectionName;
        public Crud(Mongo clientApp, string dbName, string collectionName)
        {
            this.clientApp = clientApp;
            this.dbName = dbName;
            this.collectionName = collectionName;
        }
        /// <summary>
        /// Read Methods:- BsonDoc(), JsonDoc(), TextDoc()
        /// </summary>
      
        public void BsonDoc(string key, string value)
        {
            var database = clientApp.GetDb(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            List<BsonDocument> documents;
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            // '*' represent all, if key and value = '*' then it search all document
            if (key != "*" && value !="*")
            {
                FilterDefinition<BsonDocument> filter;
                if (int.TryParse(value, out int intValue))//if the key's value is integer then parse into integer
                {
                    filter = Builders<BsonDocument>.Filter.Eq(key, intValue);
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.Eq(key, value);
                }
                documents = collection.Find(filter).Project(projection).ToList();
            }
            else
            {
                documents = collection.Find(new BsonDocument()).Project(projection).ToList();
            }
            
            foreach (BsonDocument doc in documents)
            {
                Console.WriteLine("\nBSON Doc:{0}", doc.ToString());
            }
           
            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
        }
        public void JsonDoc(string key, string value)
        {
            var database = clientApp.GetDb(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            List<BsonDocument> documents;
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");

            if (key != "*" && value != "*")
            {
                FilterDefinition<BsonDocument> filter;
                if (int.TryParse(value, out int intValue))//if the key value is integer then parse into integer
                {
                    filter = Builders<BsonDocument>.Filter.Eq(key, intValue);
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.Eq(key, value);
                }
                documents = collection.Find(filter).Project(projection).ToList();
            }
            else
            {
                documents = collection.Find(new BsonDocument()).Project(projection).ToList();
            }
            foreach (BsonDocument doc in documents)
            {
                var jsonWritersetting = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                JObject json = JObject.Parse(doc.ToJson<MongoDB.Bson.BsonDocument>(jsonWritersetting));
                Console.WriteLine("\nJSON Doc:{0}", json.ToString());
            }
           
            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
        }
        public void TextDoc(string key, string value)
        {
            var database = clientApp.GetDb(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            List<BsonDocument> documents;
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");

            if (key != "*" && value != "*")
            {
                FilterDefinition<BsonDocument> filter;
                if (int.TryParse(value, out int intValue))//if the key value is integer
                {
                    filter = Builders<BsonDocument>.Filter.Eq(key, intValue);
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.Eq(key, value);
                }
                documents = collection.Find(filter).Project(projection).ToList();
            }
            else
            {
                documents = collection.Find(new BsonDocument()).Project(projection).ToList();
            }
            var docCount = 1;
            foreach (BsonDocument doc in documents)
            {
                var jsonWritersetting = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                JObject json = JObject.Parse(doc.ToJson<MongoDB.Bson.BsonDocument>(jsonWritersetting));
                Console.WriteLine("\nDocuments: {0}", docCount);
                foreach (KeyValuePair<string, JToken> property in json)
                {
                    Console.Write("\t" + property.Key + ": ");
                    Console.Write(property.Value);
                    Console.WriteLine();
                }
                docCount++;
            }
            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
        }
        public void Find(List<string> input)
        {
            TextDoc(input[0], input[1]);
        }

        public void UpdateOne(List<string> input)
        {
            var database = clientApp.GetDb(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            FilterDefinition<BsonDocument> filter;
            if (int.TryParse(input[1], out int intValue1))
            {
                filter = Builders<BsonDocument>.Filter.Eq(input[0], intValue1);
            }
            else
            {
                filter = Builders<BsonDocument>.Filter.Eq(input[0], input[1]);
            }
            UpdateDefinition<BsonDocument> update;
            if (int.TryParse(input[3], out int intValue2))
            {
                update = Builders<BsonDocument>.Update.Set(input[2], intValue2);
            }
            else
            {
                update = Builders<BsonDocument>.Update.Set(input[2], input[3]);
            }

            collection.UpdateOne(filter, update);
            
            Console.WriteLine("Updated...");
            Console.Write("Press enter to continue...");
            Console.ReadLine();
        }
        public void DeleteOne(List<string> input)
        {
            var database = clientApp.GetDb(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            FilterDefinition<BsonDocument> filter;
            if (int.TryParse(input[1], out int intValue))
            {
                filter = Builders<BsonDocument>.Filter.Eq(input[0], intValue);
            }
            else
            {
                filter = Builders<BsonDocument>.Filter.Eq(input[0], input[1]);
            }
            collection.DeleteOne(filter);

            Console.WriteLine("Deleted...");
            Console.Write("Press enter to continue...");
            Console.ReadLine();
            
        }
        public async System.Threading.Tasks.Task InsertOneAsync(string jsonCode)
        {
            var database = clientApp.GetDb(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            try
            {
                
                var document = BsonSerializer.Deserialize<BsonDocument>(jsonCode);
                await collection.InsertOneAsync(document);
                Console.WriteLine("Successfully Inserted.");
            }
            catch (Exception)
            {
                Console.WriteLine("Something goes wrong!, InsertOne Operation failed.");
            }
            Console.Write("Press enter to continue...");
            Console.ReadLine();
        }
    }
}
