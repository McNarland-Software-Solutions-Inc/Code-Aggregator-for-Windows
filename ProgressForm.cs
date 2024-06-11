// Filename: ProgressForm.cs
using System.Windows.Forms;

public partial class ProgressForm : Form
{
    public ProgressForm()
    {
        InitializeComponent();
    }

    public void SetProgress(int progress)
    {
        progressBar.Value = progress;
        progressBar.Update();
    }
}
