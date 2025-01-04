namespace ZephyrRendererWindows
{
    internal static class Debug
    {
        private static readonly string LogFilePath = "debug.log";

        private static readonly HashSet<uint> IgnoredMessages = new()
        {
            32, 36, 70, 71, 127, 132, 134, 160, 161, 274, 533, 641, 642
        };

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            #if DEBUG
                string formattedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";

                // Write to console
                Console.WriteLine(formattedMessage);

                // Optionally write to a file
                WriteToFile(formattedMessage);
            #endif
        }

        public static void LogMessage(string message, uint msg)
        {
            #if DEBUG
                if (IgnoredMessages.Contains(msg))
                    return;
                string formattedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Info] {message}";
                Console.WriteLine(formattedMessage);
                // Optionally write to a file
                WriteToFile(formattedMessage);
            #endif
        }

        private static void WriteToFile(string message)
        {
            try
            {
                File.AppendAllText(LogFilePath, message + Environment.NewLine);
            }
            catch
            {
                // Swallow exceptions for simplicity or handle them as needed.
            }
        }
    }

    internal enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}