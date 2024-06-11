// Filename: FileAggregator.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class FileAggregator
{
    public static void AggregateFiles(string rootFolder, IEnumerable<string> includedFiles, string outputPath)
    {
        var sb = new StringBuilder();
        foreach (var file in includedFiles)
        {
            if (IsTextFile(file))
            {
                var content = File.ReadAllText(file);
                sb.AppendLine($"Folder: {Path.GetDirectoryName(file)}");
                sb.AppendLine($"Filename: {Path.GetFileName(file)}");
                sb.AppendLine(content);
                sb.AppendLine();
            }
        }
        File.WriteAllText(outputPath, sb.ToString());
    }

    private static bool IsTextFile(string filePath)
    {
        const int sampleSize = 1024;
        var buffer = new byte[sampleSize];
        using (var stream = File.OpenRead(filePath))
        {
            stream.Read(buffer, 0, sampleSize);
        }

        return buffer.All(b => b == 0 || b > 31 && b < 127);
    }
}
