using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SBT.Audit
{
    public static class AuditParser
    {
        private static Dictionary<string, string> Regexes;

        private static readonly string[] TrimCharacters = { "/", "<", ">" };

        public static (string, List<Audit2Struct>) ParseV2(string content)
        {
            if (string.IsNullOrEmpty(content))
                return ("Huinea 1", null);
            
            RegexesSetup();

            var lines = new List<string>();
            var stack = new List<string>();
            var auditStack = new List<Audit2Struct>();

            Audit2Struct currentAuditElement = null;
            Guid? currentAuditElementGUID = null;

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
                    if (currentAuditElement != null)
                    {
                        currentAuditElementGUID = currentAuditElement.GUID;
                    }
                    else
                    {
                        currentAuditElementGUID = null;
                    }

                    var newAuditElement = new Audit2Struct(
                        header: lines[index], 
                        parent: currentAuditElementGUID);

                    currentAuditElement?.Children.Add(newAuditElement.GUID);
                    currentAuditElement = newAuditElement;

                    auditStack.Add(newAuditElement);
                    var match = openMatches[0].Groups.Count >= 2 
                        ? openMatches[0].Groups[1].Value 
                        : openMatches[0].Value;
                    
                    stack.Add(match);
                }
                else if (closeMatches.Count != 0)
                {
                    if (stack.Count == 0)
                    {
                        return ("Huinea 2", null);
                    }
                    
                    var last = closeMatches[0].Groups.Count >= 2 
                        ? closeMatches[0].Groups[1].Value 
                        : closeMatches[0].Value;

                    if (last == stack.Last())
                    {
                        if (currentAuditElement == null)
                        {
                            return ("Huinea 10", null); 
                        }
                        
                        var length = stack.Count;
                        stack.RemoveAt(length - 1);
                        
                        var parentGUID = currentAuditElement.Parent;
                        if (parentGUID == null)
                        {
                            currentAuditElement = null;
                        }
                        else
                        {
                            currentAuditElement = Audit2Struct.GetByGUID(auditStack, (Guid) parentGUID);
                        }
                    }
                    else
                    {
                        return ("Huinea 3", null);
                    }
                }
                else if (propertyMatches.Count != 0)
                {
                    var args = lines[index].Split(':');
                    if (char.IsUpper(args[0][0]))
                    {
                        continue;
                    }
                    
                    if (args.Length > 0)
                    {
                        if (args.Length > 1)
                        {
                            var value = string.Empty;
                            for (var i = 1; i < args.Length; i++)
                            {
                                value += args[i].Trim();
                            }

                            var key = args[0].Trim();

                            if (currentAuditElement != null)
                            {
                                currentAuditElement.Fields.Add(new Audit2Field(
                                    key: key, 
                                    value: value));
                            }
                        }
                        else
                        {
                            if (currentAuditElement != null)
                            {
                                var lastField = currentAuditElement.Fields.Last();
                                lastField.Value += ' ' + args[0].Trim();
                            }
                        }
                    }
                }
            }
            
            return (null, auditStack);
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

            Regexes["open"] = "^[ \\t]*<(item|custom_item|report|if|then|else|condition|check_type)((.)*)[ \\t>]";
            Regexes["close"] = "^[ \\t]*</(item|custom_item|report|if|then|else|condition|check_type)((.)*)[ \\t>]";
            Regexes["property"] = "^[ \\t]*\\w*[ \\t]*:[ \\t]*[[\"\\'\\w+]";
            Regexes["item"] = "^[ \\t]*<(item|custom_item)[ \\t>]";
        }
        
        
    }
}