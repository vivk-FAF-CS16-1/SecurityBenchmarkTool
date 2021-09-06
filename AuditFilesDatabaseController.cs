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

        private Dictionary<string, string> _databaseNamesFiles;

        private readonly string _databaseFilename;
        
        public AuditFilesDatabaseController()
        {
            if (File.Exists(_databaseFilename))
            {
                //_databaseFileFileInfo = new FileInfo(_databaseFilename);

                _databaseNamesFiles = new Dictionary<string, string>();

                foreach (var fileString in File.ReadAllLines(_databaseFilename))
                {
                    _databaseNamesFiles.Add(fileString.Split(':')[0], fileString.Split(':')[1]);
                }
            }

            else
            {
                File.Create(_databaseFilename);

                //_databaseFileFileInfo = new FileInfo(_databaseFilename);
                _databaseNamesFiles = new Dictionary<string, string>();
            }
        }

        public bool AddFileToDatabase(string uniqueName, string pathFilename)
        {
            if (!File.Exists(pathFilename))
                return false;

            if (_databaseNamesFiles.ContainsKey(uniqueName))
                return false;

            _databaseNamesFiles.Add(uniqueName, pathFilename);

            string[] str = { uniqueName + ":" + pathFilename };

            File.WriteAllLines(_databaseFilename, str);

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
    }

    
}
