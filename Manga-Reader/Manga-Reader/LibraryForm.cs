using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class frmLibrary : Form
    {
        List<Book> books;
        public const string FILE_NAME = "library.txt";
        public frmLibrary()
        {
            InitializeComponent();
        }

        private void FrmLibrary_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            GetBooks(GetSaveFilePath());

            pnlBooks.Width = Width * 9/10;
            pnlBooks.Left = Width * 1 / 10;
            pnlBooks.Height = Height - 200;

            btnRefresh.PerformClick();

            panel.Width = pnlBooks.Width - SystemInformation.VerticalScrollBarWidth;
            panel.Height = pnlBooks.Height;
            pnlBooks.Parent = panel;

            pnlButtons.Location = new Point(panel.Width + panel.Left - pnlButtons.Width, panel.Height + panel.Top + 25);
        }

        private void GetBooks(string path)
        {
            books = new List<Book>();

            if (!File.Exists(path))
                return;

            StreamReader reader = new StreamReader(path);

            while(!reader.EndOfStream)
                books.Add(new Book(reader.ReadLine()));

            reader.Close();

            books = books.OrderBy(b => b.LastOpened).ToList();
        }

        private string GetSaveFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader") + "\\" + FILE_NAME;
        }

        private void SaveBookToFile(Book book)
        {
            string path = GetSaveFilePath();
            StreamWriter sw = new StreamWriter(path, append:true);

            sw.WriteLine(book.ToFile());

            sw.Close();
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
            Book book = new Book(new Navigator(rootContainer), new FilePathWrapper(rootContainer));

            if (!SetupConfigsBook(book))
                return;
            
            books.Insert(0, book);
            SaveBookToFile(book);

            btnRefresh.PerformClick();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            pnlBooks.Controls.Clear();
            foreach (Book book in books)
            {
                var th = new TitleHolder(book);

                th.Margin = new Padding(0);
                pnlBooks.Controls.Add(th);
            }
        }
    }
}
