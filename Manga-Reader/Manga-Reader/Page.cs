using System.Collections;
using System.Drawing;
using System.IO;

namespace Manga_Reader
{
    public abstract class Page
    {
        protected string name;
        protected Image image;
        protected Container parent;
        public Container Parent { get => parent; }

        public string Name { get => name; set => name = value; }
        public Image Img { get => image; }

        public abstract Image GetImage();

        public override bool Equals(object obj)
        {
            if (!(obj is Page))
                return false;
            Page p = (Page)obj;

            if (p.name != name)
                return false;
            if (p.image != image)
                return false;
            if (p.parent != parent)
                return false;

            return true;
        }
    }
}
