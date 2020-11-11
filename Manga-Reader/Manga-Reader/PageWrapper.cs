using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public class PageWrapper
    {
        protected string path;
        protected int page;
        protected Container parent;
        private PageWrapperTemplate currentPageWrapper;
        private List<PageWrapperTemplate> pageWrappers;

        public string Path { get => path; }
        public Container Parent { get => parent; }

        public List<Page> Pages { get => GetPages(); }

        public Page CurrentPage { get => currentPageWrapper.CurrentPage; }

        public PageWrapper(Container parent)
        {
            this.path = parent.Path;
            this.parent = parent;
            pageWrappers = new List<PageWrapperTemplate>();

            GetPageWrappers();
            if (pageWrappers.Count() > 0)
                currentPageWrapper = pageWrappers[0];

            SetPage(1);
        }

        protected void GetPageWrappers()
        {
            pageWrappers.Add(new ImagePageWrapper(this));
            pageWrappers.Add(new PDFPageWrapper(this));
        }

        public void Delete()
        {
            foreach (PageWrapperTemplate pw in pageWrappers)
                pw.Delete();
        }

        private List<Page> GetPages()
        {
            List<Page> pages = new List<Page>();

            foreach (PageWrapperTemplate pw in pageWrappers)
                pages.AddRange(pw.Pages);

            return pages;
        }
        public int GetPagesCount()
        {
            int count = 0;
            foreach (PageWrapperTemplate pw in pageWrappers)
                count += pw.Pages.Count();
            return count;
        }
        public void ChangePage(int p)
        {
            SetPage(page + p);
        }

        public void SetPage(int p)
        {
            if (p < 0)
                p = GetPagesCount() + p;
            else
                p--;

            int indexPageWrapper = 0, pageCounter = 0;
            while (pageCounter < p && indexPageWrapper < pageWrappers.Count())
            {
                pageCounter += pageWrappers.ElementAt(indexPageWrapper).Pages.Count();
                indexPageWrapper++;
            }

            indexPageWrapper--;

            if (indexPageWrapper < 0)
                return;

            currentPageWrapper = pageWrappers.ElementAt(indexPageWrapper);
            currentPageWrapper.SetPage(pageCounter - p + 1);

            page = p;
        }

        public int DeletCurrentPage()
        {
            return currentPageWrapper.DeletCurrentPage();
        }

        public int RenamePages(string template, Hashtable hash, int start, string key)
        {
            return currentPageWrapper.RenamePages(template, hash, start, key);
        }

        abstract class PageWrapperTemplate
        {
            PageWrapper parent;
            public abstract List<Page> Pages { get; }
            public abstract Page CurrentPage { get; }
            public PageWrapper Parent { get => parent; }

            public PageWrapperTemplate(PageWrapper pw)
            {
                parent = pw;
            }

            public abstract void Delete();
            public abstract void Reset();
            protected abstract void GetPages();
            public abstract void ChangePage(int n);
            public abstract int DeletCurrentPage();
            public abstract int RenamePages(string template, Hashtable hash, int start, string key);
            public abstract void SetPage(int p);
        }

        class ImagePageWrapper : PageWrapperTemplate
        {
            class ImagePage : Page
            {
                string path;
                public ImagePage(string path, PageWrapper parent)
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
                    Image image = null;
                    using (var bmpTemp = new Bitmap(path))
                    {
                        image = new Bitmap(bmpTemp);
                    }
                    return image;
                }
            }

            protected List<Page> pages;
            protected Page currentPage;

            public override List<Page> Pages { get => pages; }
            public override Page CurrentPage { get => currentPage; }

            public ImagePageWrapper(PageWrapper parent) : base(parent)
            {
                GetPages();
                if (pages.Count() > 0)
                    currentPage = pages.ElementAt(0);
            }

            private bool IsRecognisedImageFile(string fileName)
            {
                string targetExtension = System.IO.Path.GetExtension(fileName);
                if (String.IsNullOrEmpty(targetExtension))
                {
                    return false;
                }

                var recognisedImageExtensions = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().SelectMany(codec => codec.FilenameExtension.ToLowerInvariant().Split(';'));

                targetExtension = "*" + targetExtension.ToLowerInvariant();
                return recognisedImageExtensions.Contains(targetExtension);
            }

            protected override void GetPages()
            {
                pages = new List<Page>();
                var files = Directory.GetFiles(Parent.Path);
                files = files.ToList().FindAll(x => IsRecognisedImageFile(x)).ToArray();
                files = files.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToArray();

                foreach (string file in files)
                    pages.Add(new ImagePage(file, Parent));
            }

            public override void Delete()
            {
                pages.Clear();
                currentPage = null;
            }

            public override void Reset()
            {
                GetPages();
            }
            public override void ChangePage(int n)
            {
                int currentIndex = pages.IndexOf(currentPage);

                if (currentIndex + n >= pages.Count())
                    currentPage = pages.Last();
                else if (currentIndex + n < 0)
                    currentPage = pages.First();
                else
                    currentPage = pages.ElementAt(currentIndex + n);
            }
            public override void SetPage(int p)
            {
                if (p < 0)
                    p = pages.Count() + p;
                else
                    p--;
                currentPage = pages.ElementAt(p);
            }
            private string BuildNewPageName(string pattern, Hashtable hash, int n, string pageKey, string ext)
            {
                string newName = pattern;

                foreach (string key in hash.Keys)
                    newName = newName.Replace(key, hash[key].ToString());

                newName = newName.Replace(pageKey, n + "");
                newName = Parent.Path + "\\" + newName + ext;

                return newName;
            }
            public override int RenamePages(string pattern, Hashtable hash, int n, string pageKey)
            {
                if (pages.Count() == 0)
                    return 0;
                int count = 0;
                int currentIndex = pages.IndexOf(currentPage);

                foreach (ImagePage page in pages)
                {
                    page.Rename(BuildNewPageName(pattern, hash, n++, pageKey, System.IO.Path.GetExtension(page.Path)));
                    count++;
                }
                GetPages();
                try
                {
                    currentPage = pages.ElementAt(currentIndex);
                }
                catch
                {
                    currentPage = pages.ElementAt(0);
                }

                return count;
            }

            public void DeletePage(Page page)
            {
                var pageFile = (ImagePage)pages.Find(p => p.Equals(page));
                pages.Remove(pageFile);
                pageFile.Delete();
            }

            public override int DeletCurrentPage()
            {
                int indexCurrent = pages.IndexOf(currentPage);
                DeletePage(currentPage);
                if (indexCurrent < pages.Count())
                {
                    currentPage = pages.ElementAt(indexCurrent);
                    return 1;
                }
                else
                {
                    currentPage = pages.ElementAt(indexCurrent - 1);
                    return -1;
                }
            }
        }
        class PDFPageWrapper : PageWrapperTemplate
        {
            public PDFPageWrapper(PageWrapper parent) : base(parent)
            { }

            public override List<Page> Pages => throw new NotImplementedException();

            public override Page CurrentPage => throw new NotImplementedException();

            public override void ChangePage(int n)
            {
                throw new NotImplementedException();
            }

            public override int DeletCurrentPage()
            {
                throw new NotImplementedException();
            }

            public override void Delete()
            {
                throw new NotImplementedException();
            }

            public override int RenamePages(string template, Hashtable hash, int start, string key)
            {
                throw new NotImplementedException();
            }

            public override void Reset()
            {
                throw new NotImplementedException();
            }

            public override void SetPage(int p)
            {
                throw new NotImplementedException();
            }

            protected override void GetPages()
            {
                throw new NotImplementedException();
            }
        }
    }
}
