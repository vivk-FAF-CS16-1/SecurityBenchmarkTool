using System;
using System.Collections.Generic;

namespace SBT.Audit
{
    public class AuditItem : ICloneable
    {
        public readonly List<AuditFieldStruct> Fields;

        public Guid GUID;
        
        private const string ITEM_NAME_FIELD = "description";
        private const string DEFAULT_NAME = "unnamed";
        
        #region List implementation
        
        public AuditItem()
        {
            Fields = new List<AuditFieldStruct>();
        }

        public void AddField(AuditFieldStruct field)
        {
            Fields.Add(field);
        }

        public void RemoveField(AuditFieldStruct field)
        {
            Fields.Remove(field);
        }

        public AuditFieldStruct Get(int index)
        {
            return Fields[index];
        }

        public int CountFields()
        {
            return Fields.Count;
        }

        public void ClearFields()
        {
            Fields.Clear();
        }
        
        #endregion

        public string GetName()
        {
            var name = DEFAULT_NAME;
            foreach (var field in Fields)
            {
                if (field.Key == ITEM_NAME_FIELD)
                {
                    name = field.Value;
                }
            }

            return name;
        }

        #region ICloneable implementation

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}