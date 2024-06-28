using System;
using Avalonia.Controls;

public class FolderSelector
{
    public static async Task<string> SelectFolder(Window parent)
    {
        var dialog = new OpenFolderDialog();
        return await dialog.ShowAsync(parent) ?? string.Empty;
    }
}
