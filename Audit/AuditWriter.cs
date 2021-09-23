using System.Collections.Generic;
using System.Linq;

namespace SBT.Audit
{
    public static class AuditWriter
    {
        public struct AuditWriterParams
        {
            public bool Indexing;
            public char TabCharacter;
            public string NewLineSymbols;

            public AuditWriterParams(bool indexing, char tabCharacter, string newLineSymbols)
            {
                Indexing = indexing;
                TabCharacter = tabCharacter;
                NewLineSymbols = newLineSymbols;
            }
        }

        public static AuditWriterParams DefaultParams = new AuditWriterParams(false, ' ', "\r\n");

        public static string ToString(List<AuditStruct> container, AuditWriterParams @params)
        {
            var result = string.Empty;

            var width = container.Last().Index.ToString().Length;
            foreach (var auditStruct in container)
            {
                if (auditStruct.GUID != null)
                {
                    
                }
                
                if (@params.Indexing)
                {
                    var index = GetIndexing(auditStruct.Index, width);
                    result += index + ' ';
                }

                var tabs = GetTabs(auditStruct.Length, @params.TabCharacter);
                result += tabs;

                result += auditStruct.Line;

                result += @params.NewLineSymbols;
            }

            return result;
        }
        
        public static string ToString(List<AuditStruct> container)
        {
            return ToString(container, DefaultParams);
        }

        private static string GetIndexing(int index, int width)
        {
            var result = string.Empty;

            var indexString = index.ToString();
            var substruct = width - indexString.Length;
            for (var i = 0; i < substruct; i++)
            {
                result += ' ';
            }

            return result + indexString;
        }

        private static string GetTabs(int count, char tabCharacter)
        {
            var result = string.Empty;

            for (var i = 0; i < count; i++)
            {
                result += tabCharacter + "   ";
            }

            return result;
        }

        
    }
}