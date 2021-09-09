namespace SBT.Audit
{
    public struct AuditStruct
    {
        public int Index; 
        public int Length;
        public string Line;

        public AuditStruct(int index, int length, string line)
        {
            Index = index;
            Length = length;
            Line = line;
        }

        public override string ToString()
        {
            return Index + " " + Length + " " + Line;
        }
    }
}