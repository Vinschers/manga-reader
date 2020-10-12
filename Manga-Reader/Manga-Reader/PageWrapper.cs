using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public abstract class PageWrapper
    {
        protected List<Page> pages;
        protected string path;
        protected Page currentPage;
        protected Container parent;

        public List<Page> Pages { get => pages; }
        public string Path { get => path; }
        public Page CurrentPage { get => currentPage; }
        public Container Parent { get => parent; }

        public abstract void Reset();

        public PageWrapper(Container parent)
        {
            this.path = parent.Path;
            this.parent = parent;
            GetPages();
            if (pages.Count() > 0)
                currentPage = pages.ElementAt(0);
        }

        protected abstract void GetPages();
        public abstract int ChangePage(int n);
        public abstract int DeleteCurrentPage();
        public abstract void SetPage(int p);
    }
}
