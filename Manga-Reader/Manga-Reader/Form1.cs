using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class frmMangaReader : Form
    {
        Reader reader;
        bool changeContainer = false;
        Point mouseDownLocation, mouseLocation;

        public frmMangaReader()
        {
            InitializeComponent();

            pbPage.MouseWheel += new MouseEventHandler(PbPage_MouseWheel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            string root = GetPreviousRoot();

            MessageBox.Show("Choose the folder of the manga.");

            bool ok = false;
            while (!ok)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = root;
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        root = fbd.SelectedPath;
                        ok = true;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        Application.Exit();
                        return;
                    }
                }
            }

            pnlPage.Left = tvPath.Width + 15;
            pnlPage.Top = menuStrip1.Height + 10;
            pnlPage.Width = this.Width - 2 * (tvPath.Width + 15);
            pnlPage.Height = this.Height - 150;

            reader = GetReader(root);

            var frm2 = new Form2(reader.GeneratePossiblePathOrganization(), reader.GeneratePossibleRenameTemplate(), reader.GeneratePossiblePageBreaker(), reader);
            var dialogResult = frm2.ShowDialog();

            if (dialogResult == DialogResult.Cancel)
                Application.Exit();

            lblPage.Top = pnlPage.Top + pnlPage.Height + 20;
            lblManga.Top = lblPage.Top + lblPage.Height + 5;
            UpdateLabels();

            SetupRenameMenu();
        }

        private void UpdateLabels()
        {
            lblPage.Text = reader.Page.Name;
            lblManga.Text = reader.Name;

            lblPage.Left = Width / 2 - lblPage.Width / 2;
            lblManga.Left = Width / 2 - lblManga.Width / 2;
        }

        private void FrmMangaReader_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                reader.NextPage();
                UpdateLabels();
            }
            else if (e.KeyCode == Keys.Left)
            {
                reader.PreviousPage();
                UpdateLabels();
            }
            e.Handled = true;
        }

        private void TvPath_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!changeContainer)
                return;
            try
            {
                string path = ((MyTreeNode)tvPath.SelectedNode).FilePath;
                reader.ChangeContainer(path);
                UpdateLabels();
            }
            catch { }
        }

        private void TvPath_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void TvPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TvPath_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void PbPage_DoubleClick(object sender, EventArgs e)
        {
            if (reader.Zoom > 1.0)
                reader.RemoveZoom(3);
            else
                reader.ApplyZoom(3, mouseLocation.X, mouseLocation.Y);
        }

        private void PbPage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLocation = e.Location;
            }
        }

        private void PbPage_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            if (reader.Zoom <= 1.0 || e.Button != MouseButtons.Left)
                return;

            int left = 0, top = 0;

            if (pbPage.Width < pnlPage.Width)
                left = pbPage.Parent.Width / 2 - pbPage.Width / 2;
            else
            {
                left = e.X + pbPage.Left - mouseDownLocation.X;

                if (left > 0)
                    left = 0;
                if (left + pbPage.Width < pnlPage.Width)
                    left = pnlPage.Width - pbPage.Width;
            }

            if (pbPage.Height < pnlPage.Height)
                top = pbPage.Parent.Height / 2 - pbPage.Height / 2;
            else
            {
                top = e.Y + pbPage.Top - mouseDownLocation.Y;

                if (top > 0)
                    top = 0;
                if (top + pbPage.Height < pnlPage.Height)
                    top = pnlPage.Height - pbPage.Height;
            }
            pbPage.Left = left;
            pbPage.Top = top;
        }

        private void FrmMangaReader_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((int)e.KeyChar)
            {
                case 4: //Ctrl + D
                    reader.DeleteCurrentPage();
                    break;

                case 18: //Ctrl + R
                    break;
            }
        }

        private void SaveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader");
            Directory.CreateDirectory(path);

            var sw = new StreamWriter(path + "\\configs.txt");
            sw.Write(reader.GetConfigs());
            sw.Close();

            sw = new StreamWriter(path + "\\root.txt");
            sw.Write(reader.Root);
            sw.Close();
        }

        private string GetPreviousRoot()
        {
            try
            {
                var sr = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader", "root.txt"));
                string ret = sr.ReadToEnd();
                sr.Close();
                return ret;
            }
            catch
            {
                return Path.GetFullPath(".");
            }
        }

        private Reader GetReader(string root)
        {
            PictureBox pb = pbPage;
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader");

            if (!Directory.Exists(path))
                return new Reader(root, pb, tvPath);

            path += "\\configs.txt";

            if (!File.Exists(path))
                return new Reader(root, pb, tvPath);

            return new Reader(root, path, pb, tvPath);
        }

        private void AutoRenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoRenameToolStripMenuItem.Checked = reader.AutoRename = !autoRenameToolStripMenuItem.Checked;
        }

        private void PbPage_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!ModifierKeys.HasFlag(Keys.Control))
                return;

            if (e.Delta < 0)
                reader.RemoveZoom(1);
            else
                reader.ApplyZoom(1, mouseLocation.X, mouseLocation.Y);
        }

        private void SetupRenameMenu()
        {
            var keys = reader.Hashtable.Keys;
            List<ToolStripMenuItem> menus = new List<ToolStripMenuItem>();
            foreach(string key in keys)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();

                item.Name = "renameMenu"+key;
                item.Size = new Size(180, 22);
                item.Text = "Rename " + key;
                item.Click += new EventHandler(RenameKey);
                ((ToolStripDropDownMenu)item.DropDown).ShowImageMargin = false;

                menus.Add(item);
            }

            renameToolStripMenuItem.DropDownItems.AddRange(menus.ToArray());
        }

        private void TvPath_MouseEnter(object sender, EventArgs e)
        {
            changeContainer = true;
        }

        private void TvPath_MouseLeave(object sender, EventArgs e)
        {
            changeContainer = false;
        }

        private void RenameKey(object sender, EventArgs e)
        {
            var key = ((ToolStripMenuItem)sender).Text.Split(' ')[1];
            reader.RenameKey(key);
        }
    }
}
