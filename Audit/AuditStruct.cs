using System;

namespace SBT.Audit
{
    public struct AuditStruct
    {
        public int Index; 
        public int Length;
        public string Line;
        public Guid? GUID;

        public AuditStruct(int index, int length, string line)
        {
            Index = index;
            Length = length;
            Line = line;

            GUID = null;
        }

        public override string ToString()
        {
            return Index + " " + Length + " " + Line;
        }
    }
}