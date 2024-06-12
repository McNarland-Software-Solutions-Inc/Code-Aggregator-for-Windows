// Filename: FolderSelection.cs
using System.Collections.Generic;

public class FolderSelection
{
    public string SourceFolder { get; set; } = string.Empty;
    public string OutputFile { get; set; } = string.Empty;
    public List<FolderStatus> SelectedFolders { get; set; } = new List<FolderStatus>();
}

public class FolderStatus
{
    public string FolderPath { get; set; } = string.Empty; // relative path from the source folder
    public bool Included { get; set; }
}

public class RootSelection
{
    public List<FolderSelection> Folders { get; set; } = new List<FolderSelection>();
}
