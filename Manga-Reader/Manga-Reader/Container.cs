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
        First,
        Last
    }
    public class Container
    {
        List<string> pages;
        List<string> dirs;
        string name;
        string path;
        string renamePattern;
        Page currentPage;
        Container currentDir;
        string key;

        public string Name { get => name; }
        public string RenamePattern
        {
            get
            {
                return GetLastSubContainer().renamePattern;
            }
        }
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

        private void UpdatePages(DefaultPage p)
        {
            var files = Directory.GetFiles(path);
            files = files.ToList().FindAll(x => IsRecognisedImageFile(x)).ToArray();
            if (files.Length > 0)
            {
                files = files.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToArray();
                pages = new List<string>();
                foreach (var file in files)
                    pages.Add(file);
                switch (p)
                {
                    case DefaultPage.First:
                        currentPage = new Page(pages.First());
                        break;

                    case DefaultPage.Last:
                        currentPage = new Page(pages.Last());
                        break;
                }
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
            renamePattern = "";

            UpdatePages(p);
            UpdateSubContainers();
        }

        public Page GetCurrentPage()
        {
            if (currentPage != null)
                return currentPage;
            return currentDir.GetCurrentPage();
        }

        public void AdvancePage()
        {
            if (pages != null)
            {
                int index = pages.IndexOf(currentPage.Path);
                index++;

                if (index > pages.Count)
                    throw new Exception();
                else
                    currentPage = new Page(pages.ElementAt(index));
            }
            else
            {
                try
                {
                    currentDir.AdvancePage();
                    currentPage = currentDir.GetCurrentPage();
                }
                catch
                {
                    int dirIndex = dirs.IndexOf(currentDir.path);
                    currentDir = new Container(dirs.ElementAt(dirIndex + 1), key);
                    currentPage = currentDir.GetCurrentPage();
                }
            }
        }

        public void RetreatPage()
        {
            if (pages != null)
            {
                int index = pages.IndexOf(currentPage.Path);
                index--;

                if (index < 0)
                {
                    throw new Exception();
                }
                else
                {
                    currentPage = new Page(pages.ElementAt(index));
                }
            }
            else
            {
                try
                {
                    currentDir.RetreatPage();
                    currentPage = currentDir.GetCurrentPage();
                }
                catch
                {
                    int dirIndex = dirs.IndexOf(currentDir.path);
                    currentDir = new Container(dirs.ElementAt(dirIndex - 1), key, DefaultPage.Last);
                    currentPage = currentDir.GetCurrentPage();
                }
            }
        }

        public void ChangeCurrentContainer(string[] path)
        {
            var curDir = this;

            foreach (string part in path)
            {
                if (curDir.dirs != null)
                {
                    curDir.currentDir = new Container(curDir.dirs[0], null);
                    curDir.currentPage = null;
                }
                else
                    curDir.currentPage = new Page(curDir.pages[0]);
                curDir.currentDir = new Container(curDir.dirs.Find(container => container.Substring(container.LastIndexOf("\\")+1) == part), null);
                if (curDir.currentDir == null)
                    curDir.currentDir = new Container(curDir.dirs[0], null);
                curDir = curDir.currentDir;
            }
        }

        private Container GetLastSubContainer()
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

        public int RenamePages(string pattern, Hashtable hash, int n)
        {
            int count = 0;
            if (pages != null)
            {
                foreach (string page in pages)
                {
                    new Page(page).Rename(pattern, hash, n++);
                    count++;
                }
            }
            
            if (dirs == null)
                return -1;

            foreach(string dir in dirs)
            {
                count += new Container(dir, null).RenamePages(pattern, hash, n + count);
            }
            return count;
        }
    }
}
