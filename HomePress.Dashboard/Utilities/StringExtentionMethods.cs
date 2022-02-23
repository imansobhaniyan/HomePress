namespace HomePress.Dashboard.Utilities
{
    public static class StringExtentionMethods
    {
        public static string ToWebSafePath(this string path)
        {
            return path.Replace("\\", "/");
        }

        public static string ToCrossPlatformSafePath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}
