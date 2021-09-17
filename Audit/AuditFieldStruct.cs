namespace SBT.Audit
{
    public class AuditFieldStruct
    {
        public string Key;
        public string Value;
        public Type ValueType;

        public override string ToString()
        {
            return Key + ": " + Value;
        }
        
        public enum Type
        {
            Dynamic,
            String
        }
    }
}