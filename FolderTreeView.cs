// Filename: FolderTreeView.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public class FolderTreeView : Form
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

        folderTreeView = new TreeView
        {
            CheckBoxes = true // Enable checkboxes
        };
        folderTreeView.AfterCheck += FolderTreeView_AfterCheck; // Event handler for AfterCheck
        checkAllButton = new Button { Text = "Check All" };
        uncheckAllButton = new Button { Text = "Uncheck All" };
        startButton = new Button { Text = "Start Operation" };

        checkAllButton.Click += CheckAllButton_Click;
        uncheckAllButton.Click += UncheckAllButton_Click;
        startButton.Click += StartButton_Click;

        LoadFolders(rootFolder);
        Controls.Add(folderTreeView);
        Controls.Add(checkAllButton);
        Controls.Add(uncheckAllButton);
        Controls.Add(startButton);

        // Layout
        folderTreeView.Dock = DockStyle.Fill;
        checkAllButton.Dock = DockStyle.Bottom;
        uncheckAllButton.Dock = DockStyle.Bottom;
        startButton.Dock = DockStyle.Bottom;
        this.SizeChanged += FolderTreeView_SizeChanged;

        this.Height = 600; // Adjust the initial height

        CheckForNewFolders();
    }

    private void LoadFolders(string path)
    {
        var rootNode = new TreeNode(path) { Tag = path };
        folderTreeView.Nodes.Add(rootNode);
        LoadSubFolders(rootNode);
        rootNode.Expand(); // Expand the selected folder
    }

    private void LoadSubFolders(TreeNode node)
    {
        var path = (string)node.Tag;
        foreach (var directory in Directory.GetDirectories(path))
        {
            var relativePath = Path.GetRelativePath(rootFolder, directory);
            var subNode = new TreeNode(Path.GetFileName(directory))
            {
                Tag = directory,
                Checked = folderSelection.SelectedFolders.Any(f => f.FolderPath == relativePath && f.Included)
            };
            node.Nodes.Add(subNode);
            LoadSubFolders(subNode);
        }

        foreach (var file in Directory.GetFiles(path))
        {
            var relativePath = Path.GetRelativePath(rootFolder, file);
            var fileNode = new TreeNode(Path.GetFileName(file))
            {
                Tag = file,
                Checked = folderSelection.SelectedFolders.Any(f => f.FolderPath == relativePath && f.Included)
            };
            node.Nodes.Add(fileNode);
        }
    }

    private void FolderTreeView_AfterCheck(object? sender, TreeViewEventArgs e)
    {
        if (e.Node == null) return; // Check for null

        // Prevent recursive calls
        folderTreeView.AfterCheck -= FolderTreeView_AfterCheck;

        CheckAllSubNodes(e.Node, e.Node.Checked);

        // Re-enable the event handler
        folderTreeView.AfterCheck += FolderTreeView_AfterCheck;
    }

    private void CheckAllSubNodes(TreeNode treeNode, bool nodeChecked)
    {
        if (treeNode == null) return; // Check for null

        foreach (TreeNode node in treeNode.Nodes)
        {
            node.Checked = nodeChecked;
            CheckAllSubNodes(node, nodeChecked);
        }
    }

    private void CheckAllButton_Click(object? sender, EventArgs e)
    {
        SetNodeCheckState(folderTreeView.Nodes, true);
    }

    private void UncheckAllButton_Click(object? sender, EventArgs e)
    {
        SetNodeCheckState(folderTreeView.Nodes, false);
    }

    private void StartButton_Click(object? sender, EventArgs e)
    {
        SaveFolderSelections();
        StartOperationAsync();
    }

    private async void StartOperationAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            DefaultExt = "txt",
            InitialDirectory = SelectionManager.LoadSelections().Folders
                .FirstOrDefault(f => f.SourceFolder == rootFolder)?.OutputFile // Use the saved output file path
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            folderSelection.OutputFile = saveFileDialog.FileName; // Update the output file path
            SelectionManager.UpdateFolderSelection(folderSelection);

            var includedFiles = GetIncludedFiles();
            using (var progressForm = new ProgressForm())
            {
                progressForm.Show();
                int totalFiles = includedFiles.Count();
                int processedFiles = 0;

                await Task.Run(() =>
                {
                    foreach (var file in includedFiles)
                    {
                        // Process the file (pseudo-code)
                        // ...

                        processedFiles++;
                        int progress = (int)((double)processedFiles / totalFiles * 100);
                        progressForm.Invoke(new Action(() => progressForm.SetProgress(progress)));
                    }
                    FileAggregator.AggregateFiles(rootFolder, includedFiles, saveFileDialog.FileName);
                });
                progressForm.Close();
            }
            MessageBox.Show("Operation completed!");
            this.Close();

            // Ask the user if they want to open the file
            var result = MessageBox.Show("Do you want to open the newly created file?", "Open File", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    var processInfo = new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName)
                    {
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}");
                }
            }
        }
    }

    public IEnumerable<string> GetIncludedFiles()
    {
        var includedFiles = new List<string>();
        GetIncludedFilesRecursive(folderTreeView.Nodes, includedFiles);
        return includedFiles;
    }

    private void GetIncludedFilesRecursive(TreeNodeCollection nodes, List<string> includedFiles)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Checked && File.Exists((string)node.Tag))
            {
                includedFiles.Add((string)node.Tag);
            }
            GetIncludedFilesRecursive(node.Nodes, includedFiles);
        }
    }

    private void CheckForNewFolders()
    {
        CheckForNewFoldersRecursive(folderTreeView.Nodes);
    }

    private void CheckForNewFoldersRecursive(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            var relativePath = Path.GetRelativePath(rootFolder, (string)node.Tag);
            if (!folderSelection.SelectedFolders.Any(f => f.FolderPath == relativePath))
            {
                node.ForeColor = Color.Green; // Indicate new folders
                node.Expand(); // Expand to show new folders
            }
            CheckForNewFoldersRecursive(node.Nodes);
        }
    }

    private void SaveFolderSelections()
    {
        folderSelection.SelectedFolders.Clear();
        SaveFolderSelectionsRecursive(folderTreeView.Nodes);
        SelectionManager.UpdateFolderSelection(folderSelection);
    }

    private void SaveFolderSelectionsRecursive(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            var relativePath = Path.GetRelativePath(rootFolder, (string)node.Tag);
            folderSelection.SelectedFolders.Add(new FolderStatus
            {
                FolderPath = relativePath,
                Included = node.Checked
            });
            SaveFolderSelectionsRecursive(node.Nodes);
        }
    }

    private void SetNodeCheckState(TreeNodeCollection nodes, bool checkState)
    {
        foreach (TreeNode node in nodes)
        {
            node.Checked = checkState;
            SetNodeCheckState(node.Nodes, checkState);
        }
    }

    private void FolderTreeView_SizeChanged(object? sender, EventArgs e)
    {
        folderTreeView.Height = this.ClientSize.Height - checkAllButton.Height - uncheckAllButton.Height - startButton.Height - 20;
    }
}
