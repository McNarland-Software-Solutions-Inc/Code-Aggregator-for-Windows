// Filename: SettingsManager.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SettingsManager
{
    private readonly string settingsFilePath;

    public SettingsManager(string folderPath)
    {
        settingsFilePath = Path.Combine(folderPath, "settings.json");
    }

    public void SaveSettings(IEnumerable<string> includedFiles)
    {
        File.WriteAllText(settingsFilePath, string.Join(Environment.NewLine, includedFiles));
    }

    public IEnumerable<string> LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            return File.ReadAllLines(settingsFilePath);
        }
        return Enumerable.Empty<string>();
    }
}
