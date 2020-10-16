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

        public const char FILE_SEPARATOR = '\t';
        public const string DEFAULT_IMG_PATH = "";

        public string Name { get => name; }
        public string Path { get => path; }
        public Image Image { get => GetImage(); }
        public Reader Reader { get => reader; }
        public DateTime LastOpened { get => lastOpened; }

        public Book(string name, string path, string imgPath, DateTime lastOpened)
        {
            this.name = name;
            this.path = path;
            this.imgPath = imgPath;
            this.lastOpened = lastOpened;
        }
        public Book(string fileString)
        {
            string[] parts = fileString.Split(FILE_SEPARATOR);

            name = parts[0];
            path = parts[1];
            imgPath = parts[2];
            lastOpened = DateTime.MinValue;
            //lastOpened = DateTime.Parse(parts[3]);

            Container root = new FileContainer(path);
            this.reader = new Reader(new Navigator(root), new FilePathWrapper(root));
            this.reader.PathWrapper.LoadConfigs(parts[4], reader.Page.Parent.Path);
        }
        public Book(Navigator navigator, PathWrapper pathWrapper)
        {
            this.name = navigator.Root.Name;
            this.path = navigator.Root.Path;
            this.imgPath = DEFAULT_IMG_PATH;
            this.lastOpened = DateTime.Now;
            this.reader = new Reader(navigator, pathWrapper);
        }

        public string ToFile()
        {
            string ret = "";

            ret += name + FILE_SEPARATOR;
            ret += path + FILE_SEPARATOR;
            ret += imgPath + FILE_SEPARATOR;
            ret += lastOpened.ToString() + FILE_SEPARATOR;
            ret += reader.ToFile() + FILE_SEPARATOR;

            return ret;
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
        public void SetImagePath(string pathImg)
        {
            imgPath = pathImg;
        }
    }
}
