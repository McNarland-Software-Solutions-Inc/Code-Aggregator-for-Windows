// Filename: SelectionManager.cs
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class SelectionManager
{
    private static readonly string jsonFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CodeAggregator", "selections.json");

    public static RootSelection LoadSelections()
    {
        if (File.Exists(jsonFilePath))
        {
            var json = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<RootSelection>(json) ?? new RootSelection();
        }
        return new RootSelection();
    }

    public static void SaveSelections(RootSelection rootSelection)
    {
        var directory = Path.GetDirectoryName(jsonFilePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(rootSelection, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(jsonFilePath, json);
    }

    public static FolderSelection GetFolderSelection(string sourceFolder)
    {
        var rootSelection = LoadSelections();
        var selection = rootSelection.Folders.Find(f => f.SourceFolder == sourceFolder);
        if (selection == null)
        {
            selection = new FolderSelection { SourceFolder = sourceFolder };
            rootSelection.Folders.Add(selection);
            SaveSelections(rootSelection);
        }
        return selection;
    }

    public static void UpdateFolderSelection(FolderSelection updatedSelection)
    {
        var rootSelection = LoadSelections();
        var index = rootSelection.Folders.FindIndex(f => f.SourceFolder == updatedSelection.SourceFolder);
        if (index != -1)
        {
            rootSelection.Folders[index] = updatedSelection;
        }
        else
        {
            rootSelection.Folders.Add(updatedSelection);
        }
        SaveSelections(rootSelection);
    }
}
