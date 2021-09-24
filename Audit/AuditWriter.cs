using System.Collections.Generic;
using System.Linq;

namespace SBT.Audit
{
    public static class AuditWriter
    {
        private const string NEW_LINE = "\r\n";
        private const string FIELD_SEPARATOR = ": ";

        public static string ToString(List<Audit2Struct> container)
        {
            var root = container.First();

            return ToString(root, 0, container);
        }

        private static string ToString(Audit2Struct auditElement, int depth, List<Audit2Struct> container)
        {
            var result = string.Empty;
            
            var tabs = GetTabs(depth);
            var header = tabs + auditElement.Header + NEW_LINE;

            result += header;
            
            foreach (var fieldElement in auditElement.Fields)
            {
                result += ToString(fieldElement, depth + 1) + NEW_LINE;
            }

            foreach (var childGUID in auditElement.Children)
            {
                var child = Audit2Struct.GetByGUID(container, childGUID);
                result += ToString(child, depth + 1, container);
            }

            var bottom = auditElement.Header;
            foreach (var character in new string[]{ "<", ">" })
            {
                bottom = bottom.Replace(character, string.Empty);
            }

            var args = bottom.Split(' ', ':');
            bottom = tabs + "</" + args[0] + ">" + NEW_LINE;
            
            result += bottom;

            return result;
        }

        private static string ToString(Audit2Field fieldElement, int depth)
        {
            var tabs = GetTabs(depth);
            var field = tabs + fieldElement.Key + FIELD_SEPARATOR + fieldElement.Value;
            return field;
        }

        private static string GetTabs(int count)
        {
            var result = string.Empty;
            for (int i = 0; i < count; i++)
            {
                result += "\t";
            }

            return result;
        }
    }
}