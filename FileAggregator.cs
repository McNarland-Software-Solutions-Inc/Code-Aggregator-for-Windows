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
        sb.AppendLine($"Source folder: {rootFolder}");

        foreach (var file in includedFiles)
        {
            bool isTextFile = IsTextFile(file);
            Logger.Log($"File: {file}, IsTextFile: {isTextFile}");

            if (isTextFile)
            {
                var content = File.ReadAllText(file);
                var relativePath = Path.GetRelativePath(rootFolder, file);
                var folder = Path.GetDirectoryName(relativePath);
                var fileName = Path.GetFileName(file);

                sb.AppendLine($"Folder: {folder}");
                sb.AppendLine($"Filename: {fileName}");
                sb.AppendLine(content);
                sb.AppendLine();

                Logger.Log($"First 20 characters of {file}: {content.Substring(0, Math.Min(20, content.Length))}");
            }
        }
        File.WriteAllText(outputPath, sb.ToString());
    }

    private static bool IsTextFile(string filePath)
    {
        try
        {
            using (var stream = new StreamReader(filePath, detectEncodingFromByteOrderMarks: true))
            {
                char[] buffer = new char[512];
                int charsRead = stream.Read(buffer, 0, buffer.Length);
                if (charsRead == 0)
                    return false;

                for (int i = 0; i < charsRead; i++)
                {
                    if (char.IsControl(buffer[i]) && buffer[i] != '\r' && buffer[i] != '\n' && buffer[i] != '\t')
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}
