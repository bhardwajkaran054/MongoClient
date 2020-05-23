///<summary>
///
///This application design and developed by KARAN BHARDWAJ, this is MongoDB Client Application like MongoDB Shell but 
///it more user friendly as compare to Shell, it doesn't require any MongoDB Query knowledge.
///If require any help feel free to contact at bhardwajkaran054@gmail.com or check out my GitHub Repos at 
///github.com/bhardwajkaran054
///
/// Enter MongoDB connection Detail:-
/// NOTE:- Use MongoDB Compass Connection String (In MongoDB Atlas, Cluster > Connect > MongoDB Compass)
/// [Don't forget to enter password in string]\nexample: >>>mongodb+srv://user-0001:user-pass-0001@... 
/// >>>
/// 
///             - Main Menu - 
///             
/// Select operation to perform:
/// 
///   [1] Show DB/Collections List.
///   [2] Insert Document.
///   [4] Update Document. 
///   [5] Delete Document.
///   [6] Change Connection String.
///   [7] Exit. \n");
///   >>>
///   
///</summary>

using System;

namespace MongoDBClientApplication
{

    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.Title = "MongoDB Client Application - Developed by Karan Bhardwaj";
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
