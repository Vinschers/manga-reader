using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class frmMangaReader : Form
    {
        UIHandler uiHandler;
        Book book;

        public frmMangaReader(Book book)
        {
            InitializeComponent();
            this.book = book;

            uiHandler = new UIHandler(this, pnlPage, pbPage, tvPath, menuStrip1, lblPage, lblManga, lblProgress, renameToolStripMenuItem, ChangeTreeViewNode);

            SetupShortcuts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            uiHandler.SetupPanel();

            uiHandler.SetupRenameMenu(book.Reader.PathWrapper, RenameKey);
            uiHandler.SetupTreeView(book.Reader.Navigator.Root);

            book.Reader.PathWrapper.DefaultRenameKey = book.Reader.PathWrapper.Keys.Last();

            RefreshUI();
        }

        private void SetupShortcuts()
        {
            deleteCurrentPageToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
        }

        void ChangeTreeViewNode(Container container)
        {
            book.Reader.ChangeContainer(container);
            RefreshUI();
        }

        private void FrmMangaReader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right)
                {
                    AdvancePage();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    RetreatPage();
                }
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
            book.SaveToFile();
        }

        private void RenameKey(object sender, EventArgs e)
        {
            var key = ((ToolStripMenuItem)sender).Tag;
            book.Reader.RenameKey(key as Key);
            RefreshUI();
        }

        private void DeleteCurrentPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            book.Reader.DeleteCurrent();
            RefreshUI();
        }

        private void FrmMangaReader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && ModifierKeys == Keys.Control)
                book.Reader.CopyToClipboard();
        }

        private void SettingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new frmSetup(book).ShowDialog();
        }

        private void FrmMangaReader_FormClosing(object sender, FormClosingEventArgs e)
        {
            book.SaveToFile();
        }

        private void FrmMangaReader_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                AdvancePage();
            }
            else if (e.Button == MouseButtons.Right)
            {
                RetreatPage();
            }
        }

        private void AdvancePage()
        {
            try
            {
                book.Reader.ChangePage(1);
                RefreshUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RetreatPage()
        {
            try
            {
                book.Reader.ChangePage(-1);
                RefreshUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshUI()
        {
            uiHandler.UpdateImage(book.Reader.Page.Image);
            uiHandler.UpdateLabels(book.Reader.Navigator);
            uiHandler.UpdateSelectedNode(book.Reader.Navigator.CurrentContainer);
        }
    }
}
