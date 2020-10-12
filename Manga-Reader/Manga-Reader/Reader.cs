using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    class Reader
    {
        protected Navigator navigator;
        protected PathWrapper pathWrapper;

        public Navigator Navigator { get => navigator; }
        public PathWrapper PathWrapper { get => pathWrapper; }
        public string Name { get => navigator.Root.Name; }
        public Page Page { get => navigator.Page; }

        public Reader(Navigator nav, PathWrapper pw)
        {
            navigator = nav;
            pathWrapper = pw;
        }

        public void CopyToClipboard()
        {
            Clipboard.SetImage(Page.Image);
        }

        public void ChangePage(int deltaPages)
        {
            navigator.ChangePage(deltaPages);
        }
        public void ChangeContainer(Container container)
        {
            navigator.ChangeContainer(container);
        }
    }
}
