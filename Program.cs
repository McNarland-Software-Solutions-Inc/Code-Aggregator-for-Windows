// Filename: Program.cs
using System;
using System.Windows.Forms;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var folderPath = FolderSelector.SelectFolder();
        if (!string.IsNullOrEmpty(folderPath))
        {
            var settingsManager = new SettingsManager(folderPath);
            var includedFiles = settingsManager.LoadSettings();

            using (var form = new FolderTreeView(folderPath))
            {
                form.FormClosing += (sender, args) =>
                {
                    settingsManager.SaveSettings(form.GetIncludedFiles());
                };

                Application.Run(form);
            }
        }
    }
}
