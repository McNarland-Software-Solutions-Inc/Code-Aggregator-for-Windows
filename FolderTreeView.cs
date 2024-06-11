// Filename: FolderTreeView.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class FolderTreeView : Form
{
    private TreeView folderTreeView;
    private Button checkAllButton;
    private Button uncheckAllButton;
    private Button startButton;

    public FolderTreeView(string rootFolder)
    {
        folderTreeView = new TreeView();
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
    }

    private void LoadFolders(string path)
    {
        var rootNode = new TreeNode(path) { Tag = path };
        folderTreeView.Nodes.Add(rootNode);
        LoadSubFolders(rootNode);
    }

    private void LoadSubFolders(TreeNode node)
    {
        var path = (string)node.Tag;
        foreach (var directory in Directory.GetDirectories(path))
        {
            var subNode = new TreeNode(Path.GetFileName(directory)) { Tag = directory };
            node.Nodes.Add(subNode);
            LoadSubFolders(subNode);
        }

        foreach (var file in Directory.GetFiles(path))
        {
            var fileNode = new TreeNode(Path.GetFileName(file)) { Tag = file };
            node.Nodes.Add(fileNode);
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
        StartOperationAsync();
    }

    private async void StartOperationAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            DefaultExt = "txt"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            var includedFiles = GetIncludedFiles();
            using (var progressForm = new ProgressForm())
            {
                progressForm.Show();
                await Task.Run(() => FileAggregator.AggregateFiles((string)folderTreeView.Nodes[0].Tag, includedFiles, saveFileDialog.FileName));
                progressForm.Close();
            }
            MessageBox.Show("Operation completed!");
        }
    }

    public IEnumerable<string> GetIncludedFiles()
    {
        var includedFiles = new List<string>();
        GetIncludedFilesRecursive(folderTreeView.Nodes, includedFiles);
        return includedFiles;
    }

