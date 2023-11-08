using LiteDB;
using System;
using System.IO;

namespace ZendureShellShared
{
    public class FileHandler
    {
        public static string? LoadFileFromAppData(string fileName)
        {
            
            try
            {
                string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string programName = "ZendureCmd";

                string programFolderPath = Path.Combine(appDataFolderPath, programName);
                string filePath = Path.Combine(programFolderPath, fileName);

                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    return content;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unbehandelter Fehler: " + ex.Message);
                return null;
            }
        }

        public static ZendureCredentials LoadConfigFromAppData()
        {
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string programName = "ZendureCmd";

            string programFolderPath = Path.Combine(appDataFolderPath, programName);
            string filePath = Path.Combine(programFolderPath, "ZendureConfig.dat");

            using (var db = new LiteDatabase(filePath))
            {
                var col = db.GetCollection<ZendureCredentials>("ZendureConfig");

                var results = col.Query().FirstOrDefault();

                if(results == null)
                {
                    results = new ZendureCredentials();
                }

                return results;
            }
        }

        public static void SaveConfigToAppData(ZendureCredentials credentials)
        {
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string programName = "ZendureCmd";

            string programFolderPath = Path.Combine(appDataFolderPath, programName);

            if(Directory.Exists(programFolderPath) == false)
            {
                Directory.CreateDirectory(programFolderPath);
            }   

            string filePath = Path.Combine(programFolderPath, "ZendureConfig.dat");


            using (var db = new LiteDatabase(filePath))
            {
                var col = db.GetCollection<ZendureCredentials>("ZendureConfig");

                if(col.Query().Count() > 0)
                {
                    var creds = col.Query().FirstOrDefault();
                    creds.AccountName = credentials.AccountName;
                    creds.Password = credentials.Password;
                    creds.BearerToken = credentials.BearerToken;
                    creds.IotUrl = credentials.IotUrl;
                    creds.IotUsername = credentials.IotUsername;
                    creds.IotPassword = credentials.IotPassword;
                    
                    col.Update(creds);
                   
                }
                else
                {
                    col.Insert(credentials);
                }

                
            }
        }   



        public static void SaveFileInAppData(string fileName, string content)
        {
            try
            {
                string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string programName = "ZendureCmd";

                string programFolderPath = Path.Combine(appDataFolderPath, programName);

                if (!Directory.Exists(programFolderPath))
                {
                    Directory.CreateDirectory(programFolderPath);
                }

                string filePath = Path.Combine(programFolderPath, fileName);

                File.WriteAllText(filePath, content);

                Console.WriteLine("Token gespeichert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token konnte nicht gespeichert werden: " + ex.Message);
            }
        }
    }
}
