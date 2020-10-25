using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public class Reader
    {
        protected Navigator navigator;
        protected PathWrapper pathWrapper;
        protected int globalPageNumber = 1, pageNumber = 1;

        public Navigator Navigator { get => navigator; }
        public PathWrapper PathWrapper { get => pathWrapper; }
        public string Name { get => navigator.Root.Name; }
        public Page Page { get => navigator.Page; }
        public Key PageBreaker { get => pathWrapper.PageBreaker; }
        public int PageNumber { get => pageNumber; }
        public int GlobalPageNumber { get => globalPageNumber; }

        public Reader(Navigator nav, PathWrapper pw, int globalPageNumber = 1)
        {
            navigator = nav;
            pathWrapper = pw;
            SetPage(globalPageNumber);
        }

        public void CopyToClipboard()
        {
            Clipboard.SetImage(Page.Image);
        }

        public void ChangePage(int deltaPages)
        {
            Container previous = navigator.CurrentContainer;
            navigator.ChangePage(deltaPages);

            if (navigator.GetContainerKey(previous, pathWrapper.DefaultRenameKey) == navigator.GetContainerKey(navigator.CurrentContainer, pathWrapper.DefaultRenameKey))
                pageNumber += deltaPages;
            else
                pageNumber = navigator.GetPageNumber(PageBreaker);

            globalPageNumber += deltaPages;
            pathWrapper.UpdateHash(Page.Parent.Parent);
        }
        public void SetPage(int page)
        {
            if (page == globalPageNumber)
                return;

            ChangePage(page - globalPageNumber);
        }
        public void ChangeContainer(Container container)
        {
            navigator.ChangeContainer(container);

            pageNumber = navigator.GetPageNumber(PageBreaker);
            pathWrapper.UpdateHash(Page.Parent.Parent);
        }
        public void DeleteCurrent()
        {
            int delta = navigator.DeletePage();
            pageNumber += delta;
        }
        public void RenameKey(Key key)
        {
            Container cont = navigator.CurrentContainer;

            for (int i = pathWrapper.Depth; cont.Key != key; i--)
                cont = cont.Parent;

            int indexKey = pathWrapper.Keys.IndexOf(key);
            int indexPB = pathWrapper.Keys.IndexOf(PageBreaker);

            if (indexKey < indexPB)
            {
                void RenameKeyRec(Container current)
                {
                    if (current.Key == PageBreaker)
                    {
                        pathWrapper.RenameContainer(current, 1);
                        return;
                    }
                    foreach (Container c in current.Containers)
                        RenameKeyRec(c);
                }
                RenameKeyRec(cont);
            }
            else if (indexKey == indexPB)
                pathWrapper.RenameContainer(cont, 1);
            else
                pathWrapper.RenameContainer(cont, navigator.GetContainerKey(navigator.CurrentContainer, PageBreaker).CountPagesUntil(cont) + 1);
        }
        public string ToFile()
        {
            string ret = "";
            ret += pathWrapper.GetConfigs();
            return ret;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Reader))
                return false;
            Reader reader = obj as Reader;
            if (!navigator.Equals(reader.navigator))
                return false;
            if (!pathWrapper.Equals(reader.pathWrapper))
                return false;
            return true;
        }
        public void Delete()
        {
            navigator.Delete();
            pathWrapper.Delete();
        }
    }
}
