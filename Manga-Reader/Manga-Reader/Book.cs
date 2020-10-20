using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public class Book
    {
        protected string name;
        protected string path;
        protected string imgPath;
        protected DateTime lastOpened;
        protected Reader reader;
        protected string saveDirectory;

        public const char FILE_SEPARATOR = '\t';
        public static string DEFAULT_IMG = Library.DEFAULT_PATH + "default.jpg";
        public const string FILE_EXT = ".mrf";

        public string Name { get => name; set => SetName(value); }
        public string Path { get => path; set => path = value; }
        public Image Image { get => GetImage(); }
        public Reader Reader { get => reader; }
        public DateTime LastOpened { get => lastOpened; set => lastOpened = value; }
        public string ImagePath { get => imgPath; set => imgPath = value; }

        public Book(string name, string path, string imgPath, DateTime lastOpened, string saveDirectory)
        {
            this.name = name;
            this.path = path;
            this.imgPath = imgPath;
            this.lastOpened = lastOpened;
            this.saveDirectory = saveDirectory.EndsWith("\\") ? saveDirectory : saveDirectory + "\\";
        }
        public Book(string file)
        {
            StreamReader sReader = new StreamReader(file);

            string[] parts = sReader.ReadToEnd().Split(FILE_SEPARATOR);
            sReader.Close();

            name = parts[0];
            path = parts[1];
            imgPath = parts[2];
            lastOpened = DateTime.Parse(parts[3]);

            int page = 0;
            try
            {
                page = int.Parse(parts[4]);
            }
            catch { }

            Container root = new FileContainer(path);
            PathWrapper pw = new FilePathWrapper(root);
            Navigator nav = new Navigator(root);
            pw.LoadConfigs(parts[5], nav.GetDeepestContainer().Path);
            this.reader = new Reader(nav, pw, page);

            saveDirectory = file.Substring(0, file.LastIndexOf("\\")) + "\\";
        }
        public Book(Navigator navigator, PathWrapper pathWrapper, string saveDirectory)
        {
            this.name = navigator.Root.Name;
            this.path = navigator.Root.Path;
            this.imgPath = DEFAULT_IMG;
            this.lastOpened = DateTime.Now;
            this.reader = new Reader(navigator, pathWrapper);
            this.saveDirectory = saveDirectory.EndsWith("\\") ? saveDirectory : saveDirectory + "\\";
        }

        public string ToFile()
        {
            string ret = "";

            ret += name + FILE_SEPARATOR;
            ret += path + FILE_SEPARATOR;
            ret += imgPath + FILE_SEPARATOR;
            ret += lastOpened.ToString() + FILE_SEPARATOR;
            ret += reader.PageNumber.ToString() + FILE_SEPARATOR;
            ret += reader.ToFile() + FILE_SEPARATOR;

            return ret;
        }

        public override string ToString()
        {
            return Name;
        }

        public void SaveToFile()
        {
            string filePath = saveDirectory + GetFileName();
            StreamWriter writer = new StreamWriter(filePath);
            SaveToFile(writer);
            writer.Close();
        }
        private void SaveToFile(StreamWriter writer)
        {
            writer.Write(ToFile());
        }
        private void SetName(string name)
        {
            try
            {
                File.Move(saveDirectory + this.name + FILE_EXT, saveDirectory + name + FILE_EXT);
            }
            catch { }
            
            this.name = name;
        }

        protected Image GetImage()
        {
            Image image = null;
            try
            {
                using (var bmpTemp = new Bitmap(imgPath))
                {
                    image = new Bitmap(bmpTemp);
                }
            }
            catch { }
            return image;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Book))
                return false;
            Book book = obj as Book;
            if (name != book.name)
                return false;
            if (path != book.path)
                return false;
            if (imgPath != book.imgPath)
                return false;
            if (lastOpened != book.lastOpened)
                return false;
            if (!reader.Equals(book.reader))
                return false;
            return true;
        }

        public string GetFileName()
        {
            return Name + FILE_EXT;
        }
        public void Delete()
        {
            reader.Delete();
            File.Delete(saveDirectory + GetFileName());
        }
    }
}
