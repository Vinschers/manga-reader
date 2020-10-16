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
        public frmLibrary()
        {
            InitializeComponent();
        }

        private void FrmLibrary_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader");

            List<Book> books = GetBooks(path + "\\library.txt");

            pnlBooks.Width = Width * 9/10;
            pnlBooks.Left = Width * 1 / 10;
            pnlBooks.Height = Height - 100;

            foreach (Book book in books)
            {
                var th = new TitleHolder(book);

                th.Margin = new Padding(0);
                pnlBooks.Controls.Add(th);
            }

            panel.Width = pnlBooks.Width - SystemInformation.VerticalScrollBarWidth;
            panel.Height = pnlBooks.Height;
            pnlBooks.Parent = panel;
        }

        private List<Book> GetBooks(string path)
        {
            List<Book> books = new List<Book>();

            if (!File.Exists(path))
                return books;

            StreamReader reader = new StreamReader(path);

            while(!reader.EndOfStream)
                books.Add(new Book(reader.ReadLine()));

            reader.Close();

            return books;
        }

        private void PnlBooks_Click(object sender, EventArgs e)
        {
            TitleHolder th = sender as TitleHolder;
        }
    }
}
