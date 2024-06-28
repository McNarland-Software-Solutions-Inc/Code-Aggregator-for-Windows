using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class FolderTreeView : Window
{
    private TreeView folderTreeView;
    private Button checkAllButton;
    private Button uncheckAllButton;
    private Button startButton;
    private string rootFolder;
    private FolderSelection folderSelection;

    public FolderTreeView(string rootFolder)
    {
        this.rootFolder = rootFolder;
        folderSelection = SelectionManager.GetFolderSelection(rootFolder);

        folderTreeView = new TreeView();
        checkAllButton = new Button { Content = "Check All" };
        uncheckAllButton = new Button { Content = "Uncheck All" };
        startButton = new Button { Content = "Start Operation" };

        checkAllButton.Click += CheckAllButton_Click;
        uncheckAllButton.Click += UncheckAllButton_Click;
        startButton.Click += StartButton_Click;

        LoadFolders(rootFolder);

        var stackPanel = new StackPanel();
        stackPanel.Children.Add(checkAllButton);
        stackPanel.Children.Add(uncheckAllButton);
        stackPanel.Children.Add(startButton);
        stackPanel.Children.Add(folderTreeView);

        this.Content = stackPanel;

        this.Height = 600;
    }

    private void LoadFolders(string path)
    {
        var rootNode = new TreeViewItem { Header = path, Tag = path };
        folderTreeView.Items = new List<TreeViewItem> { rootNode };
        LoadSubFolders(rootNode);
        rootNode.IsExpanded = true;
    }

    private void LoadSubFolders(TreeViewItem node)
    {
        var path = (string)node.Tag;
        foreach (var directory in Directory.GetDirectories(path))
        {
            var relativePath = Path.GetRelativePath(rootFolder, directory);
            var subNode = new TreeViewItem
            {
                Header = Path.GetFileName(directory),
                Tag = directory,
                IsChecked = folderSelection.SelectedFolders.Any(f => f.FolderPath == relativePath && f.Included)
            };
            node.Items.Add(subNode);
            LoadSubFolders(subNode);
        }

        foreach (var file in Directory.GetFiles(path))
        {
            var relativePath = Path.GetRelativePath(rootFolder, file);
            var fileNode = new TreeViewItem
            {
                Header = Path.GetFileName(file),
                Tag = file,
                IsChecked = folderSelection.SelectedFolders.Any(f => f.FolderPath == relativePath && f.Included)
            };
            node.Items.Add(fileNode);
        }
    }

    private void CheckAllButton_Click(object sender, RoutedEventArgs e)
    {
        SetNodeCheckState(folderTreeView.Items.Cast<TreeViewItem>(), true);
    }

    private void UncheckAllButton_Click(object sender, RoutedEventArgs e)
    {
        SetNodeCheckState(folderTreeView.Items.Cast<TreeViewItem>(), false);
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFolderSelections();
        await StartOperationAsync();
    }

    private async Task StartOperationAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "Text files", Extensions = new List<string> { "txt" } },
                new FileDialogFilter { Name = "All files", Extensions = new List<string> { "*" } }
            },
            DefaultExtension = "txt"
        };

        var result = await saveFileDialog.ShowAsync(this);
        if (!string.IsNullOrEmpty(result))
        {
            folderSelection.OutputFile = result;
            SelectionManager.UpdateFolderSelection(folderSelection);

            var includedFiles = GetIncludedFiles();
            var progressForm = new ProgressForm
            {
                Width = 300,
                Height = 100
            };
            progressForm.Show();

            int totalFiles = includedFiles.Count();
            int processedFiles = 0;

            await Task.Run(() =>
            {
                foreach (var file in includedFiles)
                {
                    processedFiles++;
                    int progress = (int)((double)processedFiles / totalFiles * 100);
                    progressForm.SetProgress(progress);
                }
                FileAggregator.AggregateFiles(rootFolder, includedFiles, result);
            });

            progressForm.Close();
            var dialog = new Window();
            dialog.Content = new TextBlock { Text = "Operation completed!" };
            dialog.Width = 200;
            dialog.Height = 100;
            await dialog.ShowDialog(this);
            this.Close();

            var openFileDialog = new Window();
            openFileDialog.Content = new TextBlock { Text = "Do you want to open the newly created file?" };
            openFileDialog.Width = 200;
            openFileDialog.Height = 100;
            var resultOpenFile = await openFileDialog.ShowDialog<bool>(this);
            if (resultOpenFile)
            {
                try
                {
                    var processInfo = new System.Diagnostics.ProcessStartInfo(result)
                    {
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    var errorDialog = new Window();
                    errorDialog.Content = new TextBlock { Text = $"Error opening file: {ex.Message}" };
                    errorDialog.Width = 200;
                    errorDialog.Height = 100;
                    await errorDialog.ShowDialog(this);
                }
            }
        }
    }

    private IEnumerable<string> GetIncludedFiles()
    {
        var includedFiles = new List<string>();
        GetIncludedFilesRecursive(folderTreeView.Items.Cast<TreeViewItem>(), includedFiles);
        return includedFiles;
    }

    private void GetIncludedFilesRecursive(IEnumerable<TreeViewItem> nodes, List<string> includedFiles)
    {
        foreach (var node in nodes)
        {
            if (node.IsChecked == true && File.Exists((string)node.Tag))
            {
                includedFiles.Add((string)node.Tag);
            }
            GetIncludedFilesRecursive(node.Items.Cast<TreeViewItem>(), includedFiles);
        }
    }

    private void SaveFolderSelections()
    {
        folderSelection.SelectedFolders.Clear();
        SaveFolderSelectionsRecursive(folderTreeView.Items.Cast<TreeViewItem>());
        SelectionManager.UpdateFolderSelection(folderSelection);
    }

    private void SaveFolderSelectionsRecursive(IEnumerable<TreeViewItem> nodes)
    {
        foreach (var node in nodes)
        {
            var relativePath = Path.GetRelativePath(rootFolder, (string)node.Tag);
            folderSelection.SelectedFolders.Add(new FolderStatus
            {
                FolderPath = relativePath,
                Included = node.IsChecked == true
            });
            SaveFolderSelectionsRecursive(node.Items.Cast<TreeViewItem>());
        }
    }

    private void SetNodeCheckState(IEnumerable<TreeViewItem> nodes, bool checkState)
    {
        foreach (var node in nodes)
        {
            node.IsChecked = checkState;
            SetNodeCheckState(node.Items.Cast<TreeViewItem>(), checkState);
        }
    }
}
