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
        UIHandler uiHandler;
        Reader reader;

        public frmMangaReader(Reader reader)
        {
            InitializeComponent();

            uiHandler = new UIHandler(this, pnlPage, pbPage, tvPath, menuStrip1, lblPage, lblManga, renameToolStripMenuItem,
                (container) => { reader.ChangeContainer(container); uiHandler.UpdateLabels(reader.Page.Name, reader.Name); uiHandler.UpdateImage(reader.Page.Image); });

            SetupShortcuts();

            this.reader = reader;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            /*string root = GetPreviousRoot();

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

            

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader");
            if (Directory.Exists(path))
                reader.PathWrapper.LoadConfigs(path + "\\configs.txt", reader.Page.Parent.Path);

            

            var frm2 = new frmSetup(reader.PathWrapper.GeneratePossiblePathOrganization(), reader.PathWrapper.GeneratePossibleRenameTemplate(),
                reader.PathWrapper.GeneratePossiblePageBreaker(), reader);
            var dialogResult = frm2.ShowDialog();

            if (dialogResult == DialogResult.Cancel)
                Application.Exit();
            */
            uiHandler.SetupPanel();
            uiHandler.UpdateLabels(reader.Page.Name, reader.Name);

            uiHandler.SetupRenameMenu(reader.PathWrapper, RenameKey);
            uiHandler.SetupTreeView(reader.Navigator.Root);

            reader.PathWrapper.DefaultRenameKey = reader.PathWrapper.Keys.Last();

            uiHandler.UpdateImage(reader.Page.Image);
        }

        private void SetupShortcuts()
        {
            deleteCurrentPageToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
        }


        private void FrmMangaReader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right)
                {
                    reader.ChangePage(1);
                    uiHandler.UpdateLabels(reader.Page.Name, reader.Name);
                }
                else if (e.KeyCode == Keys.Left)
                {
                    reader.ChangePage(-1);
                    uiHandler.UpdateLabels(reader.Page.Name, reader.Name);
                }
                uiHandler.UpdateImage(reader.Page.Image);
                uiHandler.UpdateSelectedNode(reader.Navigator.CurrentContainer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                e.Handled = true;
            }
        }

        private void SaveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader");
            Directory.CreateDirectory(path);

            /*var sw = new StreamWriter(path + "\\configs.txt");
            sw.Write(reader.GetConfigs());
            sw.Close();

            sw = new StreamWriter(path + "\\root.txt");
            sw.Write(reader.Navigator.Root.Path);
            sw.Close();*/
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

        private void RenameKey(object sender, EventArgs e)
        {
            var key = ((ToolStripMenuItem)sender).Text.Split(' ')[1];
            reader.RenameKey(key);
            uiHandler.UpdateLabels(reader.Page.Name, reader.Name);
        }

        private void DeleteCurrentPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reader.DeleteCurrent();
            uiHandler.UpdateImage(reader.Page.Image);
            uiHandler.UpdateLabels(reader.Page.Name, reader.Name);
        }

        private void FrmMangaReader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && ModifierKeys == Keys.Control)
                reader.CopyToClipboard();
        }
    }
}
