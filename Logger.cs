// Filename: Logger.cs
using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
    private static bool isLoggingEnabled = false; // Set this to false to disable logging

    public static void Log(string message)
    {
        if (!isLoggingEnabled)
            return;

        try
        {
            using (var writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logging failed: {ex.Message}");
        }
    }

    public static void EnableLogging(bool enable)
    {
        isLoggingEnabled = enable;
    }
}
