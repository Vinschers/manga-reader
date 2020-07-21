using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace Manga_Reader
{
    public enum DefaultPage
    {
        First=0,
        Last=-1
    }
    public class Container
    {
        List<string> pages;
        List<string> dirs;
        string name;
        string path;
        Page currentPage;
        Container currentDir;
        string key;

        public string Name { get => name; }
        public List<string> Pages { get => pages; }
        public Container CurrentDir { get => currentDir; }
        public string Key { get => key; set => key = value; }

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

        private void UpdatePages(int p)
        {
            var files = Directory.GetFiles(path);
            files = files.ToList().FindAll(x => IsRecognisedImageFile(x)).ToArray();
            if (files.Length > 0)
            {
                files = files.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToArray();
                pages = new List<string>();
                foreach (var file in files)
                    pages.Add(file);
                if (p < 0)
                    p = pages.Count - Math.Abs(p);
                currentPage = new Page(pages[p]);
            }
        }

        private void UpdateSubContainers()
        {
            dirs = Directory.GetDirectories(path).ToList();
            if (dirs.Count > 0)
            {
                dirs = dirs.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();
                currentDir = new Container(dirs[0], null);
            }
        }

        public Container(string path, string key, DefaultPage p=DefaultPage.First)
        {
            this.path = path;
            this.key = key;
            name = path.Substring(path.LastIndexOf("\\") + 1);

            UpdatePages((int)p);
            UpdateSubContainers();
        }

        public Page GetCurrentPage()
        {
            return GetLastSubContainer().currentPage;
        }

        public void ChangePage(int n, MyTreeView tv=null)
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

        public void ChangeCurrentContainer(string[] path)
        {
            Container curDir = this;
            foreach (string part in path)
            {
                curDir.currentDir = new Container(curDir.dirs.Find(container => container.Substring(container.LastIndexOf("\\") + 1) == part), null);

                if (curDir.currentDir == null)
                    curDir.currentDir = new Container(curDir.dirs[0], null);
                curDir.currentPage = curDir.GetCurrentPage();

                curDir = curDir.currentDir;
            }
        }

        protected Container GetLastSubContainer()
        {
            Container dir = this;
            while (dir.pages == null)
                dir = dir.currentDir;
            return dir;
        }

        public int PagesCount(int n)
        {
            if (dirs != null)
            {
                foreach (string dir in dirs)
                    n += new Container(dir, null).PagesCount(n);
            }
            if (pages != null)
                n += pages.Count();
            return n;
        }

        public int CountPagesUntil(string name)
        {
            if (Name == name)
                return 0;

            int n = 0;
            if (dirs != null)
            {
                foreach (string dir in dirs)
                {
                    var d = new Container(dir, null);
                    if (d.Name == name)
                        break;
                    n += d.PagesCount(0);
                }
            }
            if (pages != null)
                n += pages.Count();
            return n;
        }

        public int RenamePages(string pattern, Hashtable hash, int n, string pageKey)
        {
            int count = 0;
            if (pages != null)
            {
                var curPageIndex = pages.IndexOf(currentPage.Path);
                foreach (string page in pages)
                {
                    new Page(page).Rename(pattern, hash, n++, pageKey);
                    count++;
                }
                UpdatePages(curPageIndex);
            }
            
            if (dirs == null)
                return count;

            foreach (string dir in dirs)
            {
                var newDir = new Container(dir, null);
                count += newDir.RenamePages(pattern, hash, n + count, pageKey);
                if (newDir.path == currentDir.path)
                    currentDir = newDir;
            }
            currentPage = GetCurrentPage();
            return count;
        }

        public void DeletePage(Page page)
        {
            var dir = GetLastSubContainer();
            if (dir.pages != null)
            {
                dir.pages.Remove(page.Path);
                page.Delete();
            }
        }
    }
}
