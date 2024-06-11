// Filename: Form1.cs
using System;
using System.Drawing;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private Button selectFolderButton=null!;
    private Button exitButton=null!;
    private RichTextBox instructionsTextBox = null!;
    private string? selectedFolderPath;
    private static string? lastSaveLocation = null;

    public Form1()
    {
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        this.Text = "Code Aggregator";
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        var menuStrip = new MenuStrip();
        var settingsMenu = new ToolStripMenuItem("Settings");
        var resetFileAssociationMenuItem = new ToolStripMenuItem("Reset Default File Association");
        resetFileAssociationMenuItem.Click += ResetFileAssociationMenuItem_Click;
        settingsMenu.DropDownItems.Add(resetFileAssociationMenuItem);
        menuStrip.Items.Add(settingsMenu);
        Controls.Add(menuStrip);

        instructionsTextBox = new RichTextBox
        {
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = this.BackColor,
            Margin = new Padding(25), // Add margins
            Dock = DockStyle.Fill
        };

        instructionsTextBox.Rtf = @"{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss Helvetica;}}
{\b\ul Code Aggregator}\par\par
This application allows you to select a root folder and aggregate all text files within the selected folder and its subfolders into a single text file. This is particularly useful for consolidating code files or other text-based files.\par\par
{\b How to Use:}\par
1. Click the 'Select Folder' button below to choose the root folder for aggregation.\par
2. Use the checkboxes to include or exclude specific files and folders. By default, all folders and files are included.\par
3. Click 'Start Operation' to begin the aggregation process. You will be prompted to specify the location and name of the output file.\par\par
Note: It is recommended to exclude folders like .git, .vs, or other dependency folders to avoid including unnecessary files.\par}";

        selectFolderButton = new Button
        {
            Text = "Select Folder",
            AutoSize = true,
            Dock = DockStyle.Right,
            Margin = new Padding(20) // Add margins
        };
        selectFolderButton.Click += SelectFolderButton_Click;

        exitButton = new Button
        {
            Text = "Exit",
            AutoSize = true,
            Dock = DockStyle.Left,
            Margin = new Padding(20) // Add margins
        };
        exitButton.Click += ExitButton_Click;

        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 40
        };
        buttonPanel.Controls.Add(selectFolderButton);
        buttonPanel.Controls.Add(exitButton);

        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10, 40, 10, 10) // Add padding around the panel and move it down
        };

        mainPanel.Controls.Add(instructionsTextBox);

        Controls.Add(mainPanel);
        Controls.Add(buttonPanel);
        MainMenuStrip = menuStrip;
    }

    private void SelectFolderButton_Click(object? sender, EventArgs e)
    {
        var folderPath = FolderSelector.SelectFolder();
        if (!string.IsNullOrEmpty(folderPath))
        {
            selectedFolderPath = folderPath;
            var settingsManager = new SettingsManager(folderPath);
            var includedFiles = settingsManager.LoadSettings();

            var folderTreeView = new FolderTreeView(folderPath)
            {
                Dock = DockStyle.Fill
            };
            folderTreeView.FormClosing += (s, args) =>
            {
                settingsManager.SaveSettings(folderTreeView.GetIncludedFiles());
            };

            folderTreeView.ShowDialog();
        }
        else
        {
            MessageBox.Show("No folder selected.");
        }
    }

    private void ExitButton_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }

    private void ResetFileAssociationMenuItem_Click(object? sender, EventArgs e)
    {
        lastSaveLocation = null;
        MessageBox.Show("Default file association has been reset.");
    }
}
