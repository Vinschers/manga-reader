using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Manga_Reader
{
    public abstract class Page
    {
        protected string name;
        protected Image image;
        protected PageWrapper parent;
        public PageWrapper Parent { get => parent; }

        public string Name { get => name; set => name = value; }
        public Image Image { get => GetImage(); }

        protected abstract Image GetImage();

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

        public override int GetHashCode()
        {
            var hashCode = 7914707;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<PageWrapper>.Default.GetHashCode(parent);
            return hashCode;
        }
    }
}
