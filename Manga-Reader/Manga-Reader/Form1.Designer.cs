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
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbPage = new System.Windows.Forms.PictureBox();
            this.pnlPage = new System.Windows.Forms.Panel();
            this.lblManga = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.tvPath = new MyTreeView();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameDepth4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortcutsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPage)).BeginInit();
            this.pnlPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1461, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.saveSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            this.pbPage.DoubleClick += new System.EventHandler(this.PbPage_DoubleClick);
            this.pbPage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbPage_MouseDown);
            this.pbPage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PbPage_MouseMove);
            // 
            // pnlPage
            // 
            this.pnlPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPage.Controls.Add(this.pbPage);
            this.pnlPage.Location = new System.Drawing.Point(319, 181);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Size = new System.Drawing.Size(200, 100);
            this.pnlPage.TabIndex = 3;
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
            this.tvPath.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvPath_AfterSelect);
            this.tvPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TvPath_KeyDown);
            this.tvPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TvPath_KeyPress);
            this.tvPath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TvPath_KeyUp);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoRenameToolStripMenuItem,
            this.renameDepth1ToolStripMenuItem,
            this.renameDepth2ToolStripMenuItem,
            this.renameDepth3ToolStripMenuItem,
            this.renameDepth4ToolStripMenuItem});
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // autoRenameToolStripMenuItem
            // 
            this.autoRenameToolStripMenuItem.Name = "autoRenameToolStripMenuItem";
            this.autoRenameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.autoRenameToolStripMenuItem.Text = "Auto rename";
            this.autoRenameToolStripMenuItem.Click += new System.EventHandler(this.AutoRenameToolStripMenuItem_Click);
            // 
            // renameDepth1ToolStripMenuItem
            // 
            this.renameDepth1ToolStripMenuItem.Name = "renameDepth1ToolStripMenuItem";
            this.renameDepth1ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameDepth1ToolStripMenuItem.Text = "Rename depth 1";
            // 
            // renameDepth2ToolStripMenuItem
            // 
            this.renameDepth2ToolStripMenuItem.Name = "renameDepth2ToolStripMenuItem";
            this.renameDepth2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameDepth2ToolStripMenuItem.Text = "Rename depth 2";
            // 
            // renameDepth3ToolStripMenuItem
            // 
            this.renameDepth3ToolStripMenuItem.Name = "renameDepth3ToolStripMenuItem";
            this.renameDepth3ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameDepth3ToolStripMenuItem.Text = "Rename depth 3";
            // 
            // renameDepth4ToolStripMenuItem
            // 
            this.renameDepth4ToolStripMenuItem.Name = "renameDepth4ToolStripMenuItem";
            this.renameDepth4ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameDepth4ToolStripMenuItem.Text = "Rename depth 4";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortcutsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // shortcutsToolStripMenuItem
            // 
            this.shortcutsToolStripMenuItem.Name = "shortcutsToolStripMenuItem";
            this.shortcutsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.shortcutsToolStripMenuItem.Text = "Shortcuts";
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
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMangaReader_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmMangaReader_KeyUp);
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
        private System.Windows.Forms.ToolStripMenuItem autoRenameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameDepth4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shortcutsToolStripMenuItem;
    }
}

