﻿// Filename: ProgressForm.Designer.cs
partial class ProgressForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.SuspendLayout();
        // 
        // progressBar
        // 
        this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
        this.progressBar.Location = new System.Drawing.Point(0, 0);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(284, 61);
        this.progressBar.TabIndex = 0;
        // 
        // ProgressForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(284, 61);
        this.Controls.Add(this.progressBar);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ProgressForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Progress";
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ProgressBar progressBar;
}
