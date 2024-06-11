// Filename: Form1.cs
using System;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private Button selectFolderButton;
    private Label instructionsLabel;
    private FolderTreeView? folderTreeView;
    private string? selectedFolderPath;

    public Form1()
    {
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        instructionsLabel = new Label
        {
            Text = "Welcome to Code Aggregator. Click 'Select Folder' to choose the root folder for aggregation.",
            AutoSize = true,
            Location = new System.Drawing.Point(10, 10)
        };

        selectFolderButton = new Button
        {
            Text = "Select Folder",
            Location = new System.Drawing.Point(10, 40),
            AutoSize = true
        };
        selectFolderButton.Click += SelectFolderButton_Click;

        Controls.Add(instructionsLabel);
        Controls.Add(selectFolderButton);
    }

    private void SelectFolderButton_Click(object? sender, EventArgs e)
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

            Controls.Clear();
            Controls.Add(folderTreeView);
        }
        else
        {
            MessageBox.Show("No folder selected.");
        }
    }
}
