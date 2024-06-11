// Filename: Form1.Designer.cs
partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Name = "Form1";
        this.Text = "Code Aggregator";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.ResumeLayout(false);
    }
}
