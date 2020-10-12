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
        public FilePageWrapper(string path) : base(path)
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
                pages.Add(new FilePage(path + "\\" + file));
        }

        public override void Reset()
        {
            GetPages();
        }
        /*
        public void ChangePage(int n, MyTreeView tv = null)
        {
            if (pages != null)
            {
                int index = pages.IndexOf(currentPage.Path);
                index += n;

                if (index >= pages.Count || index < 0)
                {
                    if (tv != null)
                        tv.SelectedNode = n > 0 ? tv.SelectedNode.NextNode : tv.SelectedNode.PrevNode;
                    throw new Exception();
                }
                else
                    currentPage = new Page(pages.ElementAt(index));
            }
            else
            {
                try
                {
                    currentDir.ChangePage(n, tv);
                    currentPage = currentDir.GetCurrentPage();
                }
                catch
                {
                    int dirIndex = dirs.IndexOf(currentDir.path);
                    if (n > 0)
                        currentDir = new Container(dirs.ElementAt(dirIndex + 1), key);
                    else
                        currentDir = new Container(dirs.ElementAt(dirIndex - 1), key, DefaultPage.Last);
                    currentPage = currentDir.GetCurrentPage();
                }
            }
        }
        */
        public override int ChangePage(int n)
        {
            int currentIndex = pages.IndexOf(currentPage);

            if (currentIndex + n > pages.Count())
                return currentIndex + n - pages.Count();
            else if (currentIndex + n < 0)
                return currentIndex + n;

            return 0;
        }
        private string BuildNewPageName(string pattern, Hashtable hash, int n, string pageKey)
        {
            string newName = pattern;

            foreach (string key in hash.Keys)
                newName = newName.Replace(key, hash[key].ToString());

            newName = newName.Replace(pageKey, n + "");
            newName = path.Substring(0, path.LastIndexOf("\\") + 1) + newName + path.Substring(path.LastIndexOf("."));

            return newName;
        }
        public int RenamePages(string pattern, Hashtable hash, int n, string pageKey)
        {
            int count = 0;
            int currentIndex = pages.IndexOf(currentPage);

            foreach (FilePage page in pages)
            {
                page.Rename(BuildNewPageName(pattern, hash, n++, pageKey));
                count++;
            }
            GetPages();
            currentPage = pages.ElementAt(currentIndex);

            return count;
        }

        public void DeletePage(Page page)
        {
            var pageFile = (FilePage)pages.Find(p => p.Equals(page));
            pages.Remove(pageFile);
            pageFile.Delete();
        }

        public override void DeleteCurrentPage()
        {
            DeletePage(currentPage);
        }
    }
}
