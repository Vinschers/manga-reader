using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    class FilePage : Page
    {
        string path;
        public FilePage(string path, PageWrapper parent)
        {
            this.path = path;
            this.parent = parent;
            name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
        }
        public string Path { get => path; set => path = value; }

        public bool Rename(string newName)
        {
            try
            {
                if (newName != path)
                {
                    File.Move(path, newName);
                    path = newName;
                    name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override Image GetImage()
        {
            using (var bmpTemp = new Bitmap(path))
            {
                this.image = new Bitmap(bmpTemp);
            }
            return this.image;
        }
    }
}
