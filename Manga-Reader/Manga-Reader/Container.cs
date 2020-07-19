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
        List<Page> pages;
        List<string> dirs;
        string name;
        string path;
        string renamePattern;
        Page currentPage;
        Container currentDir;

        public string Name { get => name; }
        public string RenamePattern
        {
            get
            {
                return GetLastSubContainer().renamePattern;
            }
        }
        public List<Page> Pages { get => pages; }
        public Container CurrentDir { get => currentDir; }

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
                pages = new List<Page>();
                foreach (var file in files)
                    pages.Add(new Page(file));
                switch (p)
                {
                    case DefaultPage.First:
                        currentPage = pages.First();
                        break;

                    case DefaultPage.Last:
                        currentPage = pages.Last();
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
                currentDir = new Container(dirs[0]);
            }
        }

        public Container(string path, DefaultPage p=DefaultPage.First)
        {
            this.path = path;
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
                int index = pages.IndexOf(currentPage);
                index++;

                if (index > pages.Count)
                    throw new Exception();
                else
                    currentPage = pages.ElementAt(index);
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
                    currentDir = new Container(dirs.ElementAt(dirIndex + 1));
                    currentPage = currentDir.GetCurrentPage();
                }
            }
        }

        public void RetreatPage()
        {
            if (pages != null)
            {
                int index = pages.IndexOf(currentPage);
                index--;

                if (index < 0)
                {
                    throw new Exception();
                }
                else
                {
                    currentPage = pages.ElementAt(index);
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
                    currentDir = new Container(dirs.ElementAt(dirIndex - 1), DefaultPage.Last);
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
                    curDir.currentDir = new Container(curDir.dirs[0]);
                    curDir.currentPage = null;
                }
                else
                    curDir.currentPage = curDir.pages[0];
                curDir.currentDir = new Container(curDir.dirs.Find(container => container.Substring(container.LastIndexOf("\\")+1) == part));
                if (curDir.currentDir == null)
                    curDir.currentDir = new Container(curDir.dirs[0]);
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

        public void RenamePages(string pattern, Hashtable hash, int n)
        {
            GetLastSubContainer().RenamePages(pattern, hash, n);
        }
    }
}
