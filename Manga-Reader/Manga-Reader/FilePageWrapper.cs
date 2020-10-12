using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manga_Reader
{
    class FilePageWrapper : PageWrapper
    {
        public FilePageWrapper(Container parent) : base(parent)
        { }
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
            var files = Directory.GetFiles(path);
            files = files.ToList().FindAll(x => IsRecognisedImageFile(x)).ToArray();
            files = files.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToArray();

            foreach (string file in files)
                pages.Add(new FilePage(file, this));
        }

        public override void Reset()
        {
            GetPages();
        }
        public override int ChangePage(int n)
        {
            int currentIndex = pages.IndexOf(currentPage);

            if (currentIndex + n >= pages.Count())
                return currentIndex + n - pages.Count() + 1;
            else if (currentIndex + n < 0)
                return currentIndex + n;

            currentPage = pages.ElementAt(currentIndex + n);
            return 0;
        }
        public override void SetPage(int p)
        {
            if (p < 0)
                p = pages.Count() + p;
            currentPage = pages.ElementAt(p);
        }
        private string BuildNewPageName(string pattern, Hashtable hash, int n, string pageKey, string ext)
        {
            string newName = pattern;

            foreach (string key in hash.Keys)
                newName = newName.Replace(key, hash[key].ToString());

            newName = newName.Replace(pageKey, n + "");
            newName = path + "\\" + newName + ext;

            return newName;
        }
        public int RenamePages(string pattern, Hashtable hash, int n, string pageKey)
        {
            if (pages.Count() == 0)
                return 0;
            int count = 0;
            int currentIndex = pages.IndexOf(currentPage);

            foreach (FilePage page in pages)
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
            var pageFile = (FilePage)pages.Find(p => p.Equals(page));
            pages.Remove(pageFile);
            pageFile.Delete();
        }

        public override int DeleteCurrentPage()
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
}
