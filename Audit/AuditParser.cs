using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SBT.Audit
{
    public static class AuditParser
    {
        private static Dictionary<string, string> Regexes;

        private static readonly string[] TrimCharacters = { "/", "<", ">" };

        public static (string, List<AuditStruct>) Parse(string content)
        {
            if (string.IsNullOrEmpty(content))
                return ("Huinea 1", null);
            
            RegexesSetup();

            var lines = new List<string>();
            var audit = new List<AuditStruct>();
            var stack = new List<string>();

            foreach (var line in content.Split('\n'))
            {
                lines.Add(line.Trim());
            }

            for (var index = 0; index < lines.Count; index++)
            {
                var openRgx = new Regex(Regexes["open"]);
                var openMatches = openRgx.Matches(lines[index]);

                var closeRgx = new Regex(Regexes["close"]);
                var closeMatches = closeRgx.Matches(lines[index]);

                var propertyRgx = new Regex(Regexes["property"]);
                var propertyMatches = propertyRgx.Matches(lines[index]);

                if (openMatches.Count != 0)
                {
                    audit.Add(new AuditStruct(
                        index: index + 1, 
                        length: stack.Count, 
                        line: lines[index]));
                    stack.Add(Trim(openMatches[0].Value));
                }
                else if (closeMatches.Count != 0)
                {
                    if (stack.Count == 0)
                    {
                        return ("Huinea 2", null);
                    }

                    var last = Trim(closeMatches[0].Value);
                    if (last == stack.Last())
                    {
                        var length = stack.Count;
                        stack.RemoveAt(length - 1);
                    }
                    else
                    {
                        return ("Huinea 3", null);
                    }
                }
                else if (propertyMatches.Count != 0)
                {
                    var args = lines[index].Split(':');
                    var property = string.Empty;

                    if (args.Length > 0)
                    {
                        if (args.Length > 1)
                        {
                            for (var i = 1; i < args.Length; i++)
                            {
                                property += args[i].Trim();
                            }
                        }
                        
                        var key = args[0].Trim();
                        audit.Add(new AuditStruct(
                            index: index + 1, 
                            length: stack.Count, 
                            line: key + " :"));
                        audit.Add(new AuditStruct(
                            index: index + 1, 
                            length: stack.Count + 1, 
                            line: property));
                    }
                }
            }
            
            return (null, audit);
        }

        private static string Trim(string command)
        {
            var result = string.Copy(command);
            foreach (var character in TrimCharacters)
            {
                result = result.Replace(character, string.Empty);
            }

            result = result.Trim();
            return result;
        }

        private static void RegexesSetup()
        {
            if (Regexes == null)
            {
                Regexes = new Dictionary<string, string>();
            }

            Regexes["open"] = "^[ \\t]*<(item|custom_item|report|if|then|else|condition)[ \\t>]";
            Regexes["close"] = "^[ \\t]*</(item|custom_item|report|if|then|else|condition)[ \\t>]";
            Regexes["property"] = "^[ \\t]*\\w*[ \\t]*:[ \\t]*[[\"\\'\\w+]";
        }
    }
}