using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SBT.DataBase
{
    public class JSONSaver
    {
        #region Public fields
        
        public static JSONSaver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JSONSaver();
                }

                return _instance;
            }
        }

        public ref List<DBItem> Container
        {
            get
            {
                if (_container == null)
                {
                    // TODO : Error for container null case
                    _container = new List<DBItem>();
                }
                
                return ref _container;
            }
        }

        public bool IsLoaded => _isLoaded;

        #endregion
        
        #region Private fields

        private static string _filePath = "";
        private static readonly string _fileName = "db.json";

        private static JSONSaver _instance = null;

        private List<DBItem> _container;

        private bool _isLoaded = false;

        #endregion
        
        #region Contructor

        private JSONSaver()
        {
            _container = new List<DBItem>();
        }
        
        #endregion

        #region Private methods
        

        #endregion
        
        #region Public methods

        public ref List<DBItem> Load()
        {
            var exist = File.Exists(_fileName);
            if (exist == false)
            {
                var fileStream = File.Create(_fileName);
                fileStream.Close();
                
                _container.Clear();
                return ref _container;
            }

            using (var streamReader = new StreamReader(_fileName))
            {
                var json = streamReader.ReadToEnd(); 
                _container = JsonConvert.DeserializeObject<List<DBItem>>(json);
            }

            _isLoaded = true;

            return ref _container;
        }
        
        public void Save()
        {
            var json = JsonConvert.SerializeObject(_container);
            File.WriteAllText(_fileName, json);
        }

        #endregion
        
    }
}