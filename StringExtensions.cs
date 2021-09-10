namespace System {
    public static class StringExtensions {
        public static string[] SplitToLines(this string str) {
            return str.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );
        }
    }
}
