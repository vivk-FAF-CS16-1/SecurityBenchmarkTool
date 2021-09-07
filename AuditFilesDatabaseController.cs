using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT
{
    public class AuditFilesDatabaseController
    {
        //private FileInfo _databaseFileFileInfo;
        //private List<FileInfo> _databaseFilesInfo;

        private static AuditFilesDatabaseController _instance;

        private Dictionary<string, string> _databaseNamesFiles;

        private readonly string _databaseFilename = "TestDatabase.txt";

        public Action<Dictionary<string, string>> OnDatabaseChanged;
        
        private AuditFilesDatabaseController()
        {
            if (File.Exists(_databaseFilename))
            {
                //_databaseFileFileInfo = new FileInfo(_databaseFilename);

                _databaseNamesFiles = new Dictionary<string, string>();

                foreach (var fileString in File.ReadAllLines(_databaseFilename))
                {
                    _databaseNamesFiles.Add(fileString.Split('^')[0], fileString.Split('^')[1]);
                }
            }

            else
            {
                var temp = File.Create(_databaseFilename);
                temp.Close();
                
                //_databaseFileFileInfo = new FileInfo(_databaseFilename);
                _databaseNamesFiles = new Dictionary<string, string>();
            }

            OnDatabaseChanged?.Invoke(_databaseNamesFiles);


        }

        public static AuditFilesDatabaseController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AuditFilesDatabaseController();
            }
            return _instance;
        }

        public bool AddFileToDatabase(string uniqueName, string pathFilename)
        {
            if (!File.Exists(pathFilename))
                return false;

            if (_databaseNamesFiles.ContainsKey(uniqueName))
                return false;

            _databaseNamesFiles.Add(uniqueName, pathFilename);

            using (StreamWriter sw = File.AppendText(_databaseFilename))
            {
                sw.WriteLine(uniqueName + "^" + pathFilename);
            }

            

            OnDatabaseChanged?.Invoke(_databaseNamesFiles);
            return true;
        }

        public Dictionary<string, string> GetDatabaseNamesFilenames()
        {
            return _databaseNamesFiles;
        }

        public bool CheckIfNameExists(string uniqueName)
        {
            if (_databaseNamesFiles.ContainsKey(uniqueName))
                return true;

            return false;
        }

        public string ReadDatabaseFileByName(string uniqueName)
        {
            return File.ReadAllText(GetDatabaseNamesFilenames()[uniqueName]);
        }
    }

    
}
