using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public class Library
    {
        protected List<Book> books;
        protected string path;

        public static string DEFAULT_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manga-Reader");

        public List<Book> Books { get => books; }
        public string LocalPath { get => path; }

        public Library() : this(DEFAULT_PATH)
        { }
        public Library(string path)
        {
            this.path = path;
            SetupPath();
            CreateBooksList();
        }
        protected void SetupPath()
        {
            if (!path.EndsWith("\\"))
                path += "\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.GetFiles(path).Contains(Path.Combine(path, Book.DEFAULT_IMG)))
            {
                Bitmap defaultImg = new Bitmap(350, 700);
                using (Graphics gfx = Graphics.FromImage(defaultImg))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
                {
                    gfx.FillRectangle(brush, 0, 0, 350, 700);
                }
                defaultImg.Save(Path.Combine(path, Book.DEFAULT_IMG), ImageFormat.Jpeg);
            }
        }
        protected void CreateBooksList()
        {
            books = new List<Book>();

            string[] files = Directory.GetFiles(path).Where(f => f.EndsWith(Book.FILE_EXT)).ToArray();

            foreach (string file in files)
                books.Add(new Book(file));
        }

        public void Refresh()
        {
            CreateBooksList();
            books = books.OrderBy(b => b.LastOpened).ToList();
            books.Reverse();
        }

        public void Add(Book book)
        {
            books.Add(book);
            book.SaveToFile();
            Refresh();
        }

        public void Delete(Book book)
        {
            books.Remove(book);
        }
    }
}
