using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using SBT.Utils;

namespace SBT.Audit
{
    public static class Auditor
    {
        public static Audit2Struct Audit(Audit2Struct source)
        {
            
            var item = new Audit2Struct(source.Header, source.Parent)
            {
                GUID = source.GUID,
                Fields = new List<Audit2Field>(source.Fields)
            };

            var type = item.GetField("type");
            if (type != null && type.Value.CustomTrim() == "REGISTRY_SETTING")
            {
                return AuditRegistrySettings(item);
            }

            return null;
            
        }

        private static Audit2Struct AuditRegistrySettings(Audit2Struct item)
        {
            var enforced = item.GetField("enforced");
            if (enforced != null)
            {
                item.AddField(
                    key: "audit_status", 
                    value: "Passed");
                return item;
            }
            
            var valueDataField = item.GetField("value_data");
            if (valueDataField == null)
            {
                item.AddField(
                    key: "audit_status", 
                    value: "Failed");
                item.AddField(
                    key: "reason", 
                    value: "Does not exist option `value_data`");
                return item;
            }

            var valueData = valueDataField.Value.CustomTrim();
            
            var regKey = item.GetField("reg_key");
            var regItem = item.GetField("reg_item");
            if (regKey != null && regItem != null)
            {
                var registry = regKey.Value.CustomTrim()
                    .Replace("HKCR\\", "HKEY_CLASSES_ROOT\\")
                    .Replace("HKCC\\", "HKEY_CURRENT_CONFIG\\")
                    .Replace("HKCU\\", "HKEY_CURRENT_USER\\")
                    .Replace("HKDD\\", "HKEY_DYN_DATA\\")
                    .Replace("HKLM\\", "HKEY_LOCAL_MACHINE\\")
                    .Replace("HKPD\\", "HKEY_PERFORMANCE_DATA\\")
                    .Replace("HKU\\", "HKEY_USERS\\");

                var value = Registry.GetValue(registry, 
                    regItem.Value.CustomTrim(), null);

                if (value != null)
                {
                    item.AddField(
                        key: "current_data", 
                        value: "\"" + value + "\""); 
                }
                else
                {
                    item.AddField(
                        key: "current_data", 
                        value: "null"); 
                }

                var regOption = item.GetField("reg_option");
                if (regOption != null)
                {
                    if (regOption.Value.CustomTrim() == "CAN_NOT_BE_NULL" && value == null)
                    {
                        item.AddField(
                            key: "audit_status", 
                            value: "Failed");
                        item.AddField(
                            key: "reason", 
                            value: "Value can not be `NULL`, because of `CAN_NOT_BE_NULL` option");
                        return item;
                    }
                    else if (regOption.Value.CustomTrim() == "CAN_BE_NULL" && value != null)
                    {
                        item.AddField(
                            key: "audit_status", 
                            value: "Failed");
                        item.AddField(
                            key: "reason", 
                            value: "Value must be `NULL`, because of `CAN_BE_NULL` option");
                        return item; 
                    }
                }

                if (value == null)
                {
                    item.AddField(
                        key: "audit_status", 
                        value: "Failed");
                    item.AddField(
                        key: "reason", 
                        value: "Value is not found OR is equal to NULL");
                    return item;  
                }
                
                var checkType = item.GetField("check_type");
                if (checkType != null)
                {
                    var checkTypeValue = checkType.Value.CustomTrim();
                    if (checkTypeValue == "CHECK_EQUAL" && !value.Equals(valueData))
                    {
                        item.AddField(
                            key: "audit_status", 
                            value: "Failed");
                        item.AddField(
                            key: "reason", 
                            value: "Value: \"" + value + "\" is NOT equal to `value_data`: \"" + valueData + "\" with `CHECK_EQUAL`");
                        return item;
                    }
                    else if (checkTypeValue == "CHECK_NOT_EQUAL" && value.Equals(valueData))
                    {
                        item.AddField(
                            key: "audit_status", 
                            value: "Failed");
                        item.AddField(
                            key: "reason", 
                            value: "Value: \"" + value + "\" is equal to `value_data`: \"" + valueData + "\" with `CHECK_NOT_EQUAL`");
                        return item;
                    }
                    else if (checkTypeValue == "CHECK_REGEX")
                    {
                        var dataRgx = new Regex(valueData);
                        var dataMatches = dataRgx.Matches(value.ToString());
                        if (dataMatches.Count == 0)
                        {
                            item.AddField(
                                key: "audit_status", 
                                value: "Failed");
                            item.AddField(
                                key: "reason", 
                                value: "Value: \"" + value + "\" is NOT equal to `value_data`: \"" + valueData + "\" with `CHECK_REGEX`");
                            return item; 
                        }
                    }
                    else
                    {
                        item.AddField(
                            key: "audit_status", 
                            value: "Failed");
                        item.AddField(
                            key: "reason", 
                            value: "Unknown `check_type` value: \"" + checkTypeValue + "\"");
                        return item;  
                    }
                }
            }
            else
            {
                item.AddField(
                    key: "audit_status", 
                    value: "Failed");
                item.AddField(
                    key: "reason", 
                    value: "Does not exist option `reg_key` or `reg_item`");
                return item;
            }
            
            item.AddField(
                key: "audit_status", 
                value: "Passed");
            return item;
        }

        public static void Enforce(Audit2Struct item, bool status)
        {
            var enforced = item.GetField("enforced");
            if (status)
            {
                if (enforced == null)
                {
                    item.AddField("enforced", "true");
                }
            }
            else
            {
                if (enforced != null)
                {
                    item.Fields.Remove(enforced);
                }
            }
            
            Thread.Sleep(10);
        }
    }
}