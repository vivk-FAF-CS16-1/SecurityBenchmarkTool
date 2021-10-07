namespace SBT.Utils
{
    public static class StringEx
    {
        public static string CustomTrim(this string source)
        {
            return source.Replace("\"", string.Empty);
        }
    }
}