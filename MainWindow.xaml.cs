using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Code_Aggregator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Title = "Code Aggregator";

            var selectFolderButton = this.FindControl<Button>("SelectFolderButton");
            selectFolderButton.Click += SelectFolderButton_Click;

            var folderTreeView = this.FindControl<TreeView>("FolderTreeView");
            var outputTextBox = this.FindControl<TextBox>("OutputTextBox");
        }

        private async void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new OpenFolderDialog();
            var folderPath = await folderDialog.ShowAsync(this);
            if (!string.IsNullOrEmpty(folderPath))
            {
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

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
