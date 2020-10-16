using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    partial class frmSetup : Form
    {
        Book book;
        public frmSetup(Book book)
        {
            InitializeComponent();

            this.book = book;

            txtName.Text = book.Name;
            txtPath.Text = book.Path;
            txtImgPath.Text = "";
            txtStructure.Text = book.Reader.PathWrapper.GeneratePossiblePathOrganization();
            txtRename.Text = book.Reader.PathWrapper.GeneratePossibleRenameTemplate();
            txtCounter.Text = book.Reader.PathWrapper.GeneratePossiblePageBreaker();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                book.Reader.PathWrapper.SetPathOrganization(txtStructure.Text, book.Reader.Navigator.CurrentContainer.PageWrapper.Path);
                book.Reader.PathWrapper.SetRenameTemplate(txtRename.Text);
                book.Reader.PathWrapper.SetPageBreaker(txtCounter.Text);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            btn.Focus();
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            btn.Focus();
        }

        private void BtnOpenImgPath_Click(object sender, EventArgs e)
        {
            dlgOpen.InitialDirectory = book.Path;
            dlgOpen.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif";
            if(dlgOpen.ShowDialog() == DialogResult.OK)
            {
                string path = dlgOpen.FileName;
                this.book.SetImagePath(path);
                txtImgPath.Text = path;
            }
        }
    }
}
