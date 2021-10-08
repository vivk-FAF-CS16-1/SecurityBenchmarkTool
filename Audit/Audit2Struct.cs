using System;
using System.Collections.Generic;
using SBT.Utils;

namespace SBT.Audit
{
    public class Audit2Struct
    {
        public string Header;

        public Guid? Parent; 
        public List<Guid> Children;

        public List<Audit2Field> Fields;

        public Guid GUID;

        private const string ITEM_NAME_FIELD = "description";
        private const string DEFAULT_NAME = "unnamed";

        public bool IsItem => Fields.Count != 0;
        public bool IsActive { get => isActive; set => isActive = value; }

        private bool isActive = true;

        public Audit2Struct(string header, Guid? parent)
        {
            Header = header;
            Parent = parent;

            GUID = Guid.NewGuid();
            
            Children = new List<Guid>();

            Fields = new List<Audit2Field>();
        }

        /*
        
        public Audit2Struct(Audit2Struct audit)
        {
            Fields = new List<Audit2Field>(audit.Fields);

            GUID = audit.GUID;
        }
        
        */

        public string GetName()
        {
            if (IsItem == false)
                return DEFAULT_NAME;

            foreach (var field in Fields)
            {
                if (field.Key == ITEM_NAME_FIELD)
                {
                    return field.Value;
                }
            }

            return DEFAULT_NAME;
        }
        
        public static Audit2Struct GetByGUID(List<Audit2Struct> container, Guid guid)
        {
            foreach (var elem in container)
            {
                if (elem.GUID == guid)
                {
                    return elem;
                }
            }

            return null;
        }

        public Audit2Field GetField(string key)
        {
            return Fields.Find(match => match.Key.CustomTrim() == key);
        }

        public void AddField(string key, string value)
        {
            Fields.Add(new Audit2Field(key, value));
        }
    }
}