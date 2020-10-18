using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class frmLibrary : Form
    {
        Library library;
        public frmLibrary()
        {
            InitializeComponent();
        }

        private void FrmLibrary_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            library = new Library();

            btnRefresh.PerformClick();
        }

        private string SelectRootFolder()
        {
            bool ok = false;
            string root = "";
            while (!ok)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        root = fbd.SelectedPath;
                        ok = true;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return "";
                    }
                }
            }
            return root;
        }

        private bool SetupConfigsBook(Book book)
        {
            var frmAdd = new frmSetup(book);
            var dialogResult = frmAdd.ShowDialog();

            if (dialogResult == DialogResult.Cancel)
                return false;
            return true;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string root = SelectRootFolder();
            if (root == "")
                return;

            FileContainer rootContainer = new FileContainer(root);
            Book book = new Book(new Navigator(rootContainer), new FilePathWrapper(rootContainer), Library.DEFAULT_PATH);

            if (!SetupConfigsBook(book))
                return;

            library.Add(book);

            btnRefresh.PerformClick();
        }

        private bool IsEqualPanelLibrary()
        {
            if (pnlBooks.Controls.Count != library.Books.Count)
                return false;

            var controls = pnlBooks.Controls.Cast<TitleHolder>().ToList();
            for (int i = 0; i < controls.Count; i++)
            {
                if (!controls[i].Book.Equals(library.Books[i]))
                    return false;
            }

            return true;
        }
        private void AdjustPanel(int width, int height)
        {
            pnlBooks.Size = new Size(width, height);
            pnlBooks.Left = (Width - pnlBooks.Width) / 2;
            pnlButtons.Location = new Point(pnlBooks.Width + pnlBooks.Left - pnlButtons.Width, pnlBooks.Height + pnlBooks.Top + 25);

            Panel scrollbarHider = new Panel();
            scrollbarHider.Size = new Size(SystemInformation.VerticalScrollBarWidth, pnlBooks.Height);
            scrollbarHider.Location = new Point(pnlBooks.Left + pnlBooks.Width - scrollbarHider.Width, pnlBooks.Top);
            scrollbarHider.BackColor = Color.White;

            Controls.Add(scrollbarHider);
            scrollbarHider.BringToFront();
        }
        private void AddBooksToPanel()
        {
            pnlBooks.Controls.Clear();
            pnlBooks.AutoScroll = true;

            int height = 0;

            foreach (Book book in library.Books)
            {
                var th = new TitleHolder(library, book);
                th.Margin = new Padding(0);
                th.Location = new Point(50, height);
                th.Name = book.GetFileName();
                height += th.Height;
                pnlBooks.Controls.Add(th);
            }

            foreach(Control c in pnlBooks.Controls)
            {
                if (c is TitleHolder)
                {
                    c.BringToFront();
                    return;
                }
            }
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            library.Refresh();

            if (!IsEqualPanelLibrary())
                AddBooksToPanel();

            var th = new TitleHolder();
            AdjustPanel(th.Width + 50 + SystemInformation.VerticalScrollBarWidth, 3 * th.Height);
        }

        private void FrmLibrary_MouseEnter(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }
    }
}
