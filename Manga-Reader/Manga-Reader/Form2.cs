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
        Reader reader;
        public frmSetup(string structure, string template, string counter, Reader r)
        {
            InitializeComponent();

            txtStructure.Text = structure;
            txtRename.Text = template;
            txtCounter.Text = counter;
            reader = r;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            try
            {
                reader.PathWrapper.SetPathOrganization(txtStructure.Text, reader.Navigator.CurrentContainer.PageWrapper.Path);
                reader.PathWrapper.SetRenameTemplate(txtRename.Text);
                reader.PathWrapper.SetPageBreaker(txtCounter.Text);

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
    }
}
