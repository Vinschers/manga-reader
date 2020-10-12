using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace Manga_Reader
{
    public class Readere
    {
        /*
        ReaderShortcuts shortcuts;

        internal ReaderShortcuts Shortcuts { get => shortcuts; }

        public Readere(string root, string path, PictureBox pb, MyTreeView tv)
        {
            this.root = root;
            this.pictureBox = pb;
            this.treeView = tv;
            this.hash = new Hashtable();
            this.folder = new Container(root, null);
            this.page = folder.GetCurrentPage();
            this.pageNumber = 1;
            this.zoom = 1.0;

            UpdateImage();

            var file = new StreamReader(path);

            this.depth = IteratePath();
            this.organization = file.ReadLine();
            this.template = file.ReadLine();
            this.pageBreaker = file.ReadLine();
            this.autoRename = bool.Parse(file.ReadLine());

            file.Close();

            shortcuts = new ReaderShortcuts(this);
        }

        public void RenameKey(string key)
        {
            var dir = folder;
            while (dir.Key != key)
                dir = dir.CurrentDir;
            int start = pageNumber;
            if (hashKeys.IndexOf(key) <= hashKeys.IndexOf(pageBreaker))
                start = 1;

            dir.RenamePages(template, hash, start, PAGE_KEY);
            page = folder.GetCurrentPage();
        }

        public void Rename()
        {
            RenameKey(DefaultRenameKey);
        }

        public string GetConfigs()
        {
            string ret = "";
            ret += organization + "\n";
            ret += template + "\n";
            ret += pageBreaker + "\n";
            ret += autoRename + "\n";
            return ret;
        }

        public bool Shortcut(int key)
        {
            bool shortcut = false;

            Shortcut s = shortcuts[key.ToString()];
            if (s != null)
            {
                s.Function();
                shortcut = true;
            }

            return shortcut;
        }
        */
    }
}
