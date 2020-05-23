# MongoClient
**A console app written in C# that use as a MongoDB Client**

![alt text](/MongoDBProjectImages/)

### Features and Advantages over MongoDB Shell
* Easy to implement
* Do not require specific knowledge of MongoDb Query for implementation of CRUD operation (only Create operation requires JSON knowledge) 
* Display all database and collection list on single screen
* Read document as JSON, BSON, or only pair of Field name and Value
* For Read, Update and Delete can be perform by readable text
* You can Import document for Insertion (Create Operation)
* Takes less system space
* It work on both local or cloud database

The main purpose of this article is to demonstrate some basic CRUD (Create Read Update Delete) operations without MongoDB Query in a MongoDB database with C#.

---

## ContextÂ 

![alt text](https://cdn-images.gif)

---

## Why MongoDB?

* **Dynamic schema**: MongoDB gives flexibility to change data schema without modifying any of existing data. It's perfect for this ongoing project.
* **Manageability**: The database is user friendly and doesn't require a database administrator.
* **Speed**: It's high performing for simple queries.
* **Flexibility**: Adding new columns or fields doesn't affect existing rows or application performance.
* **Scalability**: MongoDB is horizontally scalable, which helps reduce the workload and scale your business at ease.
* **It's cheap**: You can set up a MongoDB server in your own Virtual Machine or use a cloud database service like MongoDB Atlas starting for free with affordable pricing model.

---

## Getting our workspace ready
The complete project is available on Github here. Feel free to fork it and clone it for your own use!. In order to follow along, you will need a MongoDB database. You can use a MongoDB database running locally, or easily create a free database using MongoDB Atlas.
**make sure you have internet connection when you are using "MongoDB Compass connection string"**
* Install Visual Studio andÂ .NET Framework (Visual Studio 2019)
* Install MongoDB locally or Create a free database using MongoDB Atlas

---

## Creating and running our console application
1. Launch Visual Studio
1. Click Create a new project
1. In the Create a new project dialog, click C# Console App (.NET Framework or .NET Core)
1. Name the project "MongoDBClientApplication"
1. Click Create to create the project

![alt text](https://cdn-images.gif)

## Other Requirements
C# Dependencies
* MongoDB.Driver - for working with MongoDB
* MongoDB.Bson - to perform CRUD operation
* Newtonsoft.Json - for conversion Bson file to Json file or get only pair of field name and value

### Adding MongoDB C#/.NET Driver dependencies to our project**

#### MongoDB.Driver
What will allow us to work with the data in any MongoDB database from C# is a package called MongoDB C#/.NET Driver which creates a native interface between our application and a MongoDB server.

To install the driver, we'll go through NuGet Package Manager and install the package.
1. Open the **Package Manager Console** in Visual Studio with "Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution..."
1. Type: "**MongoDB.Driver**" on Browse tab
1. Tick all of your Project and Select Driver version (2.10.4) 
1. Hit Install button
It take few second to install

![alt text](https://cdn-images.gif)

#### MongoDB.Bson
It allow to perform CRUD operation on database using C#, this driver don't  need to install because it already install along with MongoDB.Drive but to make sure enter "**MongoDB.Bson**" on Browse tab and intall it if its not. 

#### Newtonsoft.Json
This dependencie use for conversion of bson documents to json and also use for extraction of data with only pair of field name and its value.
Follow the same process and search "**Newtonsoft.Json**" and install on your project.
---

## Application Structure

This project require four class with name
1. Program - wiring up everything and call different methods
2. Mongo - main connection string class that interact with MongoDB database
3. Crud - perform all CRUD and conversion operation by using Mongo Class
4. Display - Almost all the input and output operations perform by this class

---
### Adding Mongo Class

This class is a collection of methods for dealing with MongoDB connection and CRUD
1. Click **Project** -> **Add Class**
2. Type "**Mongo.cs**" in the name field
3. Click **Add** to add the new class to the project
Copy and paste the following code into the **Mongo.cs** file and save.

```csharp
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

        public IMongoDatabase GetDb(string dbName)//Return Database
        {
            var database = dbClient.GetDatabase(dbName);
            return database;
        }

    }
}

```
Class use only MongoDB.Driver as a declarative.
This class interact with MongoDB and return Database, here one constructor is use that takes connection string as an input, whenever this class use it require connection string;

---


### Adding Crud class

This class is a collection of methods for dealing with MongoDB connection and CRUD operations.
1. Click **Project** -> **Add Class**
2. Type "**Crud.cs**" in the name field
3. Click **Add** to add the new class to the project
Copy and paste the following code into the **Crud.cs** file and save.

```csharp
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
```
Here, BsonDoc(), JsonDoc(), TextDoc() and Find() method is use for read document from MongoDB
* BsonDoc() return default BSON file by using MongoDB.Bson and MongoDB.Bson.IO, 
* JsonDoc() return converted file to json by using Newtonsoft.Json,
* TextDoc() return converted file to pair of Field name and values using Newtonsoft.Json
* Find() method simply call one of the above method as user require.
I designed this methods in such a way that it takes pair of inputs if inputs are '*' then by default it return all documents otherwise it return filtered data and it able to identify which input is string or integer value.

* InsertOneAsync() method is use to insert data, it uses MongoDB.Bson to serialize data and it able to get input as JSON code or by importing JSON file.
* UpdateOne() and DeleteOne() method update of delete data using MongoDB.Bson
* InsertOne(), UpdateOne() and DeleteOne() able to identify which input value is string or integer, if string then treat as a string or if integer treat as a integer

---

### Adding Display Class

This class is responsible for pretty data in our console app
1. Click **Project** -> **Add Class**
1. Type "**Display.cs**" in the name field
1. Click **Add** to add the new class to the project
Copy and paste the following code into the Display.cs file and save.

```csharp
//Ref: https://github.com/histechup/guestlist-manager-cli-csharp/blob/master/guestlist-manager-cli-csharp/DialogHelper.cs

using System;
using System.Collections.Generic;

namespace MongoDBClientApplication
{
    public class Display
    {
        public static string ConnectionInput()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- MongoDB Client Application -");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\nEnter MongoDB connection Detail:- \n");
            Console.ResetColor();
            Console.Write(
                "You can enter only, Localhost connection string or MongoDB Compass Connection String \n" +
                "(Recomended: Use MongoDB Compass Connection String) \n\n" +
                "NOTE:- for MongoDB Compass Connection String (In MongoDB Atlas, Cluster > Connect > MongoDB Compass), \n" +
                "[Don't forget to enter password in string]\nexample: >>>mongodb+srv://user-0001:user-pass-0001@... \n>>>");
            var connectionString = Console.ReadLine();

            //Filter string and display connection detail
            connectionString = FilterConnectionString(connectionString);

            return connectionString;
        }
       
        public static int ShowMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Main Menu - ");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write(
                "Select operation to perform: \n\n" +
                "[1] Show DB/Collections List. \n" +
                "[2] Insert Document. \n" +
                "[3] Read/Find Document. \n" +
                "[4] Update Document. \n" +
                "[5] Delete Document. \n" +
                "[6] Change Connection String. \n"+
                "[7] Exit. \n");
            Console.Write("-------------------------------\n>>>");

            var entry = Console.ReadLine();
            if (!int.TryParse(entry, out int choice))
            {
                choice = 10;
            }
            return choice;
        }
        //public static int ShowReadFindMenu(string dbName, string collectionName)
        public static int ShowReadFindMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Read/Find Documents - ");
            Console.ResetColor();
            Console.WriteLine();
            //Console.ForegroundColor = ConsoleColor.DarkCyan;
            //Console.WriteLine(
            //    "Database: {0} \n" +
            //    "Collection: {1} \n", dbName, collectionName);
            //Console.ResetColor();
            Console.Write(
                "Select operation to perform: \n\n" +
                "[1] Read All as Text file (like only [Key]: [value]). \n" +
                    "[2] >> Read All as BSON. \n" +
                    "[3] >> Read All as JSON. \n" +
                "[4] Find/Search Document. \n" +
                "[5] Back to main menu. \n");
            Console.Write("-------------------------------\n>>>");

            string entry = Console.ReadLine();
            if (!int.TryParse(entry, out int choice))
            {
                choice = 10;
            }
            return choice;
        }

        public static void ShowList(Mongo clientApp)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Db/Collection List - ");
            Console.ResetColor();
            Console.WriteLine("Fetching...");
            int dbCount = 0, collectionCount = 0;//for db and collection count
            var dbList = clientApp.DBList();
            foreach (var db in dbList)
            {
                dbCount++;
                Console.WriteLine("Database {0}: {1}", dbCount, db);
                var collectionList = clientApp.CollectionList(db);
                foreach (var collection in collectionList)
                {
                    collectionCount++;
                    Console.WriteLine("\tCollection {0}: {1}", collectionCount, collection);
                }
                collectionCount = 0;
            }
            Console.Write("\nPress enter to continue...");
            Console.Read();
        }

        public static Crud DbDetails(Mongo clientApp)
        {
            Console.Write("Enter Database name: ");
            var dbName = Console.ReadLine();

            Console.Write("Enter Collection name: ");
            var collectionName = Console.ReadLine();

            Crud crud = new Crud(clientApp, dbName, collectionName);
            return crud;
        }
        public static List<string> FindInput()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Find - ");
            Console.ResetColor();

            List<string> input = new List<string>();
            Console.WriteLine("Enter Field Name and Value for search");
            Console.Write("Enter Key/Field name: ");
            input.Add(Console.ReadLine());
            Console.Write("Enter Value: ");
            input.Add(Console.ReadLine());
            return input;
        }

        public static List<string> UpdateInput()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Update - ");
            Console.ResetColor();

            List<string> input = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Update/Replace -");
            Console.ResetColor();

            Console.Write("\nEnter Document 'Unique field' and its 'value' on which to perform update operation \n" +
                           "Unique Field: ");
            input.Add(Console.ReadLine());
            Console.Write("Value: ");
            input.Add(Console.ReadLine());
            Console.Write("\nEnter Field name and update value on which update perform. \n" +
                "Field name: ");
            input.Add(Console.ReadLine());
            Console.Write("Update value: ");
            input.Add(Console.ReadLine());

            return input;
        }
        public static List<string> DeleteInput()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Delete - ");
            Console.ResetColor();

            List<string> input = new List<string>();
            Console.WriteLine("Enter Field Name and Value for delete perticular document");
            Console.Write("Enter Key/Field name: ");
            input.Add(Console.ReadLine());
            Console.Write("Enter Value: ");
            input.Add(Console.ReadLine());
            return input;
        }

        public static int CreateInput()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(20, 0);
            Console.WriteLine("- Insert(InsertOne) Document - ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n*To create document you need to enter or import json code");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can enter only one document at a time like InsertOne()");
            Console.ResetColor();

            Console.Write("Choose Option:- \n" +
                "[1] Enter Json Code \n" +
                "[2] Import Json Code \n" +
                "[3] goto main menu \n");
            Console.Write("-------------------------------\n>>>");

            var entry = Console.ReadLine();
            if (!int.TryParse(entry, out int choice))
            {
                choice = 10;
            }
            return choice;

        }
        public static string JsonImporter()
        {
            string jsonPath;
            string jsonCode="";
            Console.Write(@"Enter Json file directory with FileName.json like 'c:\...\FileName.json'"+"\n>>>");
            jsonPath = Console.ReadLine();
            try
            {
                jsonCode = System.IO.File.ReadAllText(jsonPath);
            }
            catch (Exception)
            {
                
            }
            
            return jsonCode;
        }
        public static string JsonCode()
        {
            string jsonCode;
            Console.Write("Enter Json code, example: "+ "{\"field_1\": \"value_1\", \"field_2\": \"value_2\", ..., \"field_n\": \"value_n\"} \n>>>");
            jsonCode = Console.ReadLine();
            return jsonCode;
        }
        private static string FilterConnectionString(string connectionString)
        {
            try
            {
                connectionString = connectionString.Trim();
                
                if(connectionString.Substring(0, 19) == "mongodb://localhost")
                {
                    Console.WriteLine("You're offline using local host server");
                }
                else
                {
                    string username = "";
                    int pointer = 14;
                    for (; connectionString[pointer] != ':'; pointer++)
                        username += connectionString[pointer];

                    string password = "";
                    for (pointer++; connectionString[pointer] != '@'; pointer++)
                        password += connectionString[pointer];

                    string hostname = "";
                    for (pointer++; connectionString[pointer] != '/'; pointer++)
                        hostname += connectionString[pointer];
                    if (password == "<password>")
                    {
                        Console.Write("\nEnter your password: ");
                        password = Console.ReadLine();

                        connectionString = connectionString.Replace("<password>", password);
                    }
                    //for C# MongoDB.Driver
                    connectionString += "?retryWrites=true&w=majority";

                    Console.WriteLine("\nHostname: {0}\nUsername: {1}\nPassword: {2}", hostname, username, password);
                }
                Console.Write("\nPress enter for main menu...");
                Console.Read();
            }
            catch (Exception)
            {
                Console.WriteLine("Are you trying to enter other connection string instead of 'Local Server or Compass connection string'!");
                Console.Write("\nPress enter to Exit...");
                Console.ReadLine();
                Environment.Exit(0);
            }
            return connectionString;
        }
    }
}

```
In this class, most of the methods is use to print menu driven options, here two method are different 
1. JsonImporter() - take json file directory as an input and convert the json file to string and return, and 
1. FilterConnectionString() - filter this input string and check whether it is local string or MongoDB Compass connection string, and then filtered the string and convert it for C# connection string

---

### Wiring up everything in Main method 

Now everything is good to go, we can wire up everything in Main method.
Open the file **Program.cs** and replace the content with the following code.

```csharp
using System;

namespace MongoDBClientApplication
{

    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.Title = "MongoDB Client Application";
            //for C# connection string
            var connectionString = Display.ConnectionInput();
            Mongo clientApp = new Mongo(connectionString);

            int input;
            do
            {
                input = Display.ShowMenu();
                switch (input)
                {
                    case 1:
                        try
                        {
                            Display.ShowList(clientApp);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something goes wrong! check your connection string.");
                            Console.Write("Press enter to exit");
                            Environment.Exit(0);
                        }
                        break;
                    case 2:
                        Crud crudCreate = Display.DbDetails(clientApp);
                        int createInput;
                        do
                        {
                            createInput = Display.CreateInput();
                            switch (createInput)
                            {
                                case 1:
                                    await crudCreate.InsertOneAsync(Display.JsonCode());
                                    break;
                                case 2:
                                    await crudCreate.InsertOneAsync(Display.JsonImporter());
                                    break;
                            }

                        } while (createInput != 3);
                        break;
                    case 3:
                        //Crud crud = new Crud(clientApp, dbName, collectionName);
                        Crud crudRead = Display.DbDetails(clientApp);
                        int readInput;
                        do
                        {
                            readInput = Display.ShowReadFindMenu();
                            switch (readInput)
                            {
                                case 1:
                                    //crud.TextDoc(string key, string value)
                                    //key and value = '*' represent convert all document
                                    crudRead.TextDoc("*","*");
                                    break;
                                case 2:
                                    crudRead.BsonDoc("*","*");
                                    break;
                                case 3:
                                    crudRead.JsonDoc("*","*");
                                    break;
                                case 4:
                                    //find method
                                    var findInput = Display.FindInput();
                                    crudRead.Find(findInput);
                                    break;
                            }
                        } while (readInput != 5);
                        break;
                    case 4:
                        Crud crudUpdate = Display.DbDetails(clientApp);
                        var updateInput = Display.UpdateInput();
                        crudUpdate.UpdateOne(updateInput);
                        break;
                    case 5:
                        Crud crudDelete = Display.DbDetails(clientApp);
                        var deleteInput = Display.DeleteInput();
                        crudDelete.DeleteOne(deleteInput);
                        break;
                    case 6:
                        clientApp = new Mongo(Display.ConnectionInput());
                        break;
                }
            } while (input != 7);
        }
    }

}

```

---

### That's all, our application is ready to run!

Press **ctrl + F5** to build and run the project

>Even if you don't have any existing connection string: **mongodb+srv://m001-student:m001-mongodb-basics@cluster0-jxeqq.mongodb.net/test**


---

## Wrapping up

Now, we have a fully functional **MongoDB Client Application** console app. We can create, read, update and delete documents in a MongoDB database.

---

## Resources

* [Thanks to MongoDB University to provide the  resources and teach how to use mongodb]
* [MongoDB Blog Quick Start C# and MongoDB](https://www.mongodb.com/blog/post/quick-start-c-sharp-and-mongodb-starting-and-setup)]
* [What is MongoDBâ€Š-â€Šfrom the official source](https://www.mongodb.com/what-is-mongodb)
* [Learn more about MongoDB C#/.NET Driver](https://docs.mongodb.com/ecosystem/drivers/csharp/)
* [Learn more about MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
* [Udemy Tutorial from Basic to Advance By Mosh

---

## Feedback

Thanks for reading! I hope this helped you. Please feel free to leave a comment here or create an issue in the Github repo.
