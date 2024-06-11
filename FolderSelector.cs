// Filename: FolderSelector.cs
using System;
using System.Windows.Forms;

public class FolderSelector
{
    public static string SelectFolder()
    {
        using (var folderDialog = new FolderBrowserDialog())
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                return folderDialog.SelectedPath;
            }
        }
        return string.Empty;
    }
}
