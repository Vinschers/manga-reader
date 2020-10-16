namespace Manga_Reader
{
    partial class TitleHolder
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

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbPicture = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            this.lbPath = new System.Windows.Forms.Label();
            this.container = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).BeginInit();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbPicture
            // 
            this.pbPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPicture.Location = new System.Drawing.Point(0, 0);
            this.pbPicture.Name = "pbPicture";
            this.pbPicture.Size = new System.Drawing.Size(200, 300);
            this.pbPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPicture.TabIndex = 0;
            this.pbPicture.TabStop = false;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("UD Digi Kyokasho NK-B", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbName.Location = new System.Drawing.Point(206, 15);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(99, 34);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "Name";
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPath.Location = new System.Drawing.Point(209, 71);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(40, 16);
            this.lbPath.TabIndex = 2;
            this.lbPath.Text = "Path";
            // 
            // container
            // 
            this.container.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.container.Controls.Add(this.pbPicture);
            this.container.Controls.Add(this.lbPath);
            this.container.Controls.Add(this.lbName);
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(750, 300);
            this.container.TabIndex = 3;
            // 
            // TitleHolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.container);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "TitleHolder";
            this.Size = new System.Drawing.Size(750, 300);
            this.Load += new System.EventHandler(this.TitleHolder_Load);
            this.ParentChanged += new System.EventHandler(this.TitleHolder_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pbPicture)).EndInit();
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPicture;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbPath;
        private System.Windows.Forms.Panel container;
    }
}
