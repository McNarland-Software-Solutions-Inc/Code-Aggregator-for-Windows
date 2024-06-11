// Filename: ProgressForm.cs
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public partial class ProgressForm : Form
{
    public ProgressForm()
    {
        InitializeComponent();
    }

    public void SetProgress(int progress)
    {
        progressBar.Value = progress;
    }
}
