﻿namespace Manga_Reader
{
    partial class frmLibrary
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
            this.pnlBooks = new System.Windows.Forms.FlowLayoutPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBooks
            // 
            this.pnlBooks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBooks.AutoScroll = true;
            this.pnlBooks.Location = new System.Drawing.Point(0, 0);
            this.pnlBooks.Name = "pnlBooks";
            this.pnlBooks.Size = new System.Drawing.Size(1050, 315);
            this.pnlBooks.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.pnlBooks);
            this.panel.Location = new System.Drawing.Point(12, 5);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1042, 364);
            this.panel.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAdd.Font = new System.Drawing.Font("UD Digi Kyokasho NK-R", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAdd.Location = new System.Drawing.Point(181, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(175, 50);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add Title";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnAdd);
            this.pnlButtons.Controls.Add(this.btnRefresh);
            this.pnlButtons.Location = new System.Drawing.Point(74, 396);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(356, 50);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRefresh.Font = new System.Drawing.Font("UD Digi Kyokasho NK-R", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRefresh.Location = new System.Drawing.Point(0, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(175, 50);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // frmLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1139, 511);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.panel);
            this.Name = "frmLibrary";
            this.Text = "Library";
            this.Load += new System.EventHandler(this.FrmLibrary_Load);
            this.panel.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlBooks;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnRefresh;
    }
}