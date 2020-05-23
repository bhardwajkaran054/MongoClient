/// <summary>
/// 
/// This application design and developed by KARAN BHARDWAJ, this is MongoDB Client Application like MongoDB Shell but 
/// it more user friendly as compare to Shell, it doesn't require any MongoDB Query knowledge.
/// If require any help feel free to contact at bhardwajkaran054@gmail.com or check out my GitHub Repos at 
/// github.com/bhardwajkaran054
/// 
/// </summary>
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
