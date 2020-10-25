namespace Manga_Reader
{
    partial class frmMangaReader
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCurrentPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbPage = new System.Windows.Forms.PictureBox();
            this.pnlPage = new System.Windows.Forms.Panel();
            this.lblManga = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.renameDepth1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tvPath = new MyTreeView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPage)).BeginInit();
            this.pnlPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1461, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.deleteCurrentPageToolStripMenuItem,
            this.settingsToolStripMenuItem1,
            this.saveSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.settingsToolStripMenuItem.Text = "General";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // deleteCurrentPageToolStripMenuItem
            // 
            this.deleteCurrentPageToolStripMenuItem.Name = "deleteCurrentPageToolStripMenuItem";
            this.deleteCurrentPageToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.deleteCurrentPageToolStripMenuItem.Text = "Delete current page";
            this.deleteCurrentPageToolStripMenuItem.Click += new System.EventHandler(this.DeleteCurrentPageToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(177, 22);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.SettingsToolStripMenuItem1_Click);
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.saveSettingsToolStripMenuItem.Text = "Save settings";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.SaveSettingsToolStripMenuItem_Click);
            // 
            // pbPage
            // 
            this.pbPage.Location = new System.Drawing.Point(55, 31);
            this.pbPage.Name = "pbPage";
            this.pbPage.Size = new System.Drawing.Size(18, 16);
            this.pbPage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPage.TabIndex = 2;
            this.pbPage.TabStop = false;
            // 
            // pnlPage
            // 
            this.pnlPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPage.Controls.Add(this.pbPage);
            this.pnlPage.Location = new System.Drawing.Point(319, 181);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Size = new System.Drawing.Size(200, 100);
            this.pnlPage.TabIndex = 3;
            this.pnlPage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FrmMangaReader_MouseClick);
            // 
            // lblManga
            // 
            this.lblManga.AutoSize = true;
            this.lblManga.Font = new System.Drawing.Font("UD Digi Kyokasho NK-R", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblManga.Location = new System.Drawing.Point(504, 864);
            this.lblManga.Name = "lblManga";
            this.lblManga.Size = new System.Drawing.Size(0, 22);
            this.lblManga.TabIndex = 4;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("UD Digi Kyokasho NK-B", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPage.Location = new System.Drawing.Point(503, 821);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(0, 28);
            this.lblPage.TabIndex = 5;
            // 
            // renameDepth1ToolStripMenuItem
            // 
            this.renameDepth1ToolStripMenuItem.Name = "renameDepth1ToolStripMenuItem";
            this.renameDepth1ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // renameDepth2ToolStripMenuItem
            // 
            this.renameDepth2ToolStripMenuItem.Name = "renameDepth2ToolStripMenuItem";
            this.renameDepth2ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // renameDepth3ToolStripMenuItem
            // 
            this.renameDepth3ToolStripMenuItem.Name = "renameDepth3ToolStripMenuItem";
            this.renameDepth3ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // renameDepth4ToolStripMenuItem
            // 
            this.renameDepth4ToolStripMenuItem.Name = "renameDepth4ToolStripMenuItem";
            this.renameDepth4ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // tvPath
            // 
            this.tvPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.tvPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvPath.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.tvPath.Location = new System.Drawing.Point(0, 181);
            this.tvPath.Name = "tvPath";
            this.tvPath.Size = new System.Drawing.Size(201, 628);
            this.tvPath.TabIndex = 1;
            // 
            // frmMangaReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(1461, 895);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.lblManga);
            this.Controls.Add(this.pnlPage);
            this.Controls.Add(this.tvPath);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMangaReader";
            this.Text = "Manga reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMangaReader_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMangaReader_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMangaReader_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FrmMangaReader_MouseClick);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPage)).EndInit();
            this.pnlPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private MyTreeView tvPath;
        private System.Windows.Forms.PictureBox pbPage;
        private System.Windows.Forms.Panel pnlPage;
        private System.Windows.Forms.Label lblManga;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCurrentPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
    }
}

