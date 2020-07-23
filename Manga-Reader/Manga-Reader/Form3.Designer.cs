namespace Manga_Reader
{
    partial class frmShortcuts
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
            this.pnlShortcuts = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlShortcuts
            // 
            this.pnlShortcuts.Location = new System.Drawing.Point(12, 12);
            this.pnlShortcuts.Name = "pnlShortcuts";
            this.pnlShortcuts.Size = new System.Drawing.Size(1079, 264);
            this.pnlShortcuts.TabIndex = 0;
            // 
            // frmShortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 283);
            this.Controls.Add(this.pnlShortcuts);
            this.Name = "frmShortcuts";
            this.Text = "Shortcuts setup";
            this.Load += new System.EventHandler(this.FrmShortcuts_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlShortcuts;
    }
}