using System;
using System.Collections.Generic;
using SBT.Audit;

namespace SBT.DataBase
{
    public class DBItem
    {
        public Guid GUID;
        public string Name;
        public string Content;
        public string SourcePath;

        public List<AuditItem> Items;
    }
}