namespace SBT.Audit
{
    public struct Audit2Field
    {
        public string Key;
        public string Value;

        public Audit2Field(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}