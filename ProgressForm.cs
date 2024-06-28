using Avalonia.Controls;

public partial class ProgressForm : Window
{
    private ProgressBar progressBar;

    public ProgressForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        progressBar = new ProgressBar
        {
            Minimum = 0,
            Maximum = 100
        };

        var stackPanel = new StackPanel();
        stackPanel.Children.Add(progressBar);

        this.Content = stackPanel;
    }

    public void SetProgress(int progress)
    {
        progressBar.Value = progress;
    }
}
