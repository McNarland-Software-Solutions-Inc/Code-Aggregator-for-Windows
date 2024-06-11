// Filename: Form1.cs
using System;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private FolderTreeView? folderTreeView;
    private string? selectedFolderPath;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        var folderPath = FolderSelector.SelectFolder();
        if (!string.IsNullOrEmpty(folderPath))
        {
            selectedFolderPath = folderPath;
            var settingsManager = new SettingsManager(folderPath);
            var includedFiles = settingsManager.LoadSettings();

            folderTreeView = new FolderTreeView(folderPath)
            {
                Dock = DockStyle.Fill
            };
            folderTreeView.FormClosing += (s, args) =>
            {
                settingsManager.SaveSettings(folderTreeView.GetIncludedFiles());
            };

            Controls.Add(folderTreeView);
        }
        else
        {
            MessageBox.Show("No folder selected. The application will exit.");
            Application.Exit();
        }
    }
}
