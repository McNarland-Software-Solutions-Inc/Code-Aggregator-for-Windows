using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

public partial class MainWindow : Window
{
    private Button selectFolderButton = null!;
    private Button exitButton = null!;
    private TextBlock instructionsTextBox = null!;
    private string? selectedFolderPath;
    private static string? lastSaveLocation = null;

    public MainWindow()
    {
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        this.Title = "Code Aggregator";

        instructionsTextBox = new TextBlock
        {
            Text = @"Code Aggregator

This application allows you to select a root folder and aggregate all text files within the selected folder and its subfolders into a single text file. This is particularly useful for consolidating code files or other text-based files.

How to Use:
1. Click the 'Select Folder' button below to choose the root folder for aggregation.
2. Use the checkboxes to include or exclude specific files and folders. By default, all folders and files are included.
3. Click 'Start Operation' to begin the aggregation process. You will be prompted to specify the location and name of the output file.

Note: It is recommended to exclude folders like .git, .vs, or other dependency folders to avoid including unnecessary files.",
            Margin = new Thickness(25),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top
        };

        selectFolderButton = new Button
        {
            Content = "Select Folder",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(20)
        };
        selectFolderButton.Click += SelectFolderButton_Click;

        exitButton = new Button
        {
            Content = "Exit",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(20)
        };
        exitButton.Click += ExitButton_Click;

        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        stackPanel.Children.Add(instructionsTextBox);
        stackPanel.Children.Add(selectFolderButton);
        stackPanel.Children.Add(exitButton);

        this.Content = stackPanel;
    }

    private async void SelectFolderButton_Click(object? sender, RoutedEventArgs e)
    {
        var folderDialog = new OpenFolderDialog();
        var folderPath = await folderDialog.ShowAsync(this);
        if (!string.IsNullOrEmpty(folderPath))
        {
            selectedFolderPath = folderPath;
            var settingsManager = new SettingsManager(folderPath);
            var includedFiles = settingsManager.LoadSettings();

            var folderTreeView = new FolderTreeView(folderPath)
            {
                Width = this.Width,
                Height = this.Height,
            };
            folderTreeView.Closed += (s, args) =>
            {
                settingsManager.SaveSettings(folderTreeView.GetIncludedFiles());
            };

            folderTreeView.ShowDialog(this);
        }
        else
        {
            var dialog = new Window();
            dialog.Content = new TextBlock { Text = "No folder selected." };
            dialog.Width = 200;
            dialog.Height = 100;
            await dialog.ShowDialog(this);
        }
    }

    private void ExitButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ResetFileAssociationMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        lastSaveLocation = null;
        var dialog = new Window();
        dialog.Content = new TextBlock { Text = "Default file association has been reset." };
        dialog.Width = 200;
        dialog.Height = 100;
        dialog.ShowDialog(this);
    }
}
