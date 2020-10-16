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
        protected int pageNumber = -1;

        public Navigator Navigator { get => navigator; }
        public PathWrapper PathWrapper { get => pathWrapper; }
        public string Name { get => navigator.Root.Name; }
        public Page Page { get => navigator.Page; }
        public string PageBreaker { get => pathWrapper.PageBreaker; }

        public Reader(Navigator nav, PathWrapper pw)
        {
            navigator = nav;
            pathWrapper = pw;
            pageNumber = 0;
        }

        public void CopyToClipboard()
        {
            Clipboard.SetImage(Page.Image);
        }

        public void ChangePage(int deltaPages)
        {
            string pageBreakerValueBefore = navigator.GetContainerKey(PageBreaker).Path;
            navigator.ChangePage(deltaPages);
            Container pageBreakerAfter = navigator.GetContainerKey(PageBreaker);
            string pageBreakerValueAfter = pageBreakerAfter.Path;

            if (pageBreakerValueBefore == pageBreakerValueAfter)
                pageNumber += deltaPages;
            else
                pageNumber = navigator.GetPageNumber(PageBreaker);

            pathWrapper.UpdateHash(Page.Parent.Path);
        }
        public void ChangeContainer(Container container)
        {
            string pageBreakerValueBefore = navigator.GetContainerKey(PageBreaker).Path;
            navigator.ChangeContainer(container);
            Container pageBreakerAfter = navigator.GetContainerKey(PageBreaker);
            string pageBreakerValueAfter = pageBreakerAfter.Path;

            if (pageBreakerValueBefore == pageBreakerValueAfter)
                pageNumber = navigator.GetPageNumber(PageBreaker);
            else
                pageNumber = 1;

            pathWrapper.UpdateHash(Page.Parent.Path);
        }
        public void DeleteCurrent()
        {
            int delta = navigator.DeletePage();
            pageNumber += delta;
        }
        public void RenameKey(string key)
        {
            Container cont = navigator.CurrentContainer;

            int i;
            for (i = pathWrapper.Depth; cont.Key != key; i--)
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
                pathWrapper.RenameContainer(cont, navigator.GetContainerKey(PageBreaker).CountPagesUntil(cont) + 1);
        }
        public string ToFile()
        {
            string ret = "";
            ret += pathWrapper.GetConfigs();
            return ret;
        }
    }
}
