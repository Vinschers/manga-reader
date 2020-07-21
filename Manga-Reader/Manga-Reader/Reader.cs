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
    public class Reader
    {
        int depth;
        string root;
        string organization;
        string template;
        string pageBreaker;
        int pageNumber;
        Hashtable hash;
        Page page;
        Container folder;
        PictureBox pictureBox;
        MyTreeView treeView;
        bool autoRename;
        double zoom;
        List<string> hashKeys;
        string defaultRenameKey;

        const double ZOOM_RATIO = 1.5;
        const double MAX_ZOOM = 5.0625;
        const string PAGE_KEY = "$page";

        public bool AutoRename { get => autoRename; set => autoRename = value; }
        public Page Page { get => page; }
        public string Name { get => folder.Name; }
        public string Root { get => root; }
        public PictureBox PictureBox { get => pictureBox; set => pictureBox = value; }
        public int Depth
        {
            get => depth;
            set
            {
                if (depth == -1)
                    depth = value;
            }
        }
        internal MyTreeView TreeView { get => treeView; set => treeView = value; }
        public double Zoom { get => zoom; }
        public Hashtable Hashtable { get => hash; }
        public string DefaultRenameKey
        {
            get
            {
                if (defaultRenameKey != null && defaultRenameKey != "")
                    return defaultRenameKey;
                return pageBreaker;
            }
        }

        public Reader(string root, PictureBox pb, MyTreeView tv)
        {
            this.root = root;
            this.pictureBox = pb;
            this.treeView = tv;
            this.hash = new Hashtable();

            this.folder = new Container(root, null);
            this.depth = IteratePath();
            this.page = folder.GetCurrentPage();
            this.pageNumber = 1;
            this.zoom = 1.0;

            UpdateImage();
        }

        public Reader(string root, string path, PictureBox pb, MyTreeView tv)
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
        }

        private int IteratePath()
        {
            int rec(string p, int val, MyTreeNode parent)
            {
                int m = val;
                var dirs = Directory.GetDirectories(p);
                dirs = dirs.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToArray();
                foreach (var dir in dirs)
                {
                    MyTreeNode child = new MyTreeNode(dir);
                    parent.Nodes.Add(child);
                    int r = rec(Path.Combine(p, dir), val + 1, child);
                    if (r > m)
                        m = r;
                }
                return m;
            }

            MyTreeNode rootNode = new MyTreeNode(root);

            int d = rec(root, 0, rootNode);
            this.treeView.Nodes.Add(rootNode);
            this.treeView.ExpandAll();
            var node = this.treeView.Nodes[0];
            while (node.Nodes != null && node.Nodes.Count > 0)
                node = node.Nodes[0];
            this.treeView.SelectedNode = node;

            return d;
        }

        public void UpdateImage()
        {
            Image img;
            using (var bmpTemp = new Bitmap(page.Path))
            {
                img = new Bitmap(bmpTemp);
            }
            pictureBox.Width = img.Width;
            pictureBox.Height = img.Height;
            if (pictureBox.Height > pictureBox.Parent.Height || pictureBox.Width > pictureBox.Parent.Width)
            {
                double rWidth = (double)pictureBox.Width / pictureBox.Height;
                pictureBox.Height = pictureBox.Parent.Height;
                pictureBox.Width = (int)Math.Round(rWidth * pictureBox.Height);

                if (pictureBox.Width > pictureBox.Parent.Width)
                {
                    double rHeight = (double)pictureBox.Height / pictureBox.Width;
                    pictureBox.Width = pictureBox.Parent.Width;
                    pictureBox.Height = (int)Math.Round(rHeight * pictureBox.Width);
                }
            }
            pictureBox.Left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
            pictureBox.Top = pictureBox.Parent.Height / 2 - pictureBox.Height / 2;
            pictureBox.Image = new Bitmap(img);
            pictureBox.Invalidate();
        }

        public string GeneratePossiblePathOrganization()
        {
            if (organization != null && organization != "")
                return organization;
            string possiblePathOrganization = "";

            switch (this.depth)
            {
                case 0:
                    possiblePathOrganization = "$Chapter\\";
                    break;

                case 1:
                    possiblePathOrganization = "$Volume\\$Chapter\\";
                    break;

                case 2:
                    possiblePathOrganization = "$Manga\\Volume $Volume\\$Chapter\\";
                    break;

                case 3:
                    possiblePathOrganization = "$Folder\\$Manga\\$Volume\\$Chapter\\";
                    break;
            }

            return possiblePathOrganization;
        }

        public string GeneratePossibleRenameTemplate()
        {
            if (template != null && template != "")
                return template;
            string possibleTemplate = "";

            switch (this.depth)
            {
                case 0:
                    possibleTemplate = "Chapter $Chapter Page " + PAGE_KEY;
                    break;

                case 1:
                    possibleTemplate = "Volume $Volume - %Chapter p. " + PAGE_KEY;
                    break;

                case 2:
                    possibleTemplate = "$Manga - Volume $Volume Page " + PAGE_KEY;
                    break;

                default:
                    possibleTemplate = "Volume $Volume - Page " + PAGE_KEY;
                    break;
            }

            return possibleTemplate;
        }

        public string GeneratePossiblePageBreaker()
        {
            if (pageBreaker != null && pageBreaker != "")
                return pageBreaker;
            string possibleBreaker = "";

            switch (this.depth)
            {
                case 0:
                    possibleBreaker = "$Chapter";
                    break;

                case 2:
                    possibleBreaker = "$Manga";
                    break;

                default:
                    possibleBreaker = "$Volume";
                    break;
            }

            return possibleBreaker;
        }

        public void SetPathOrganization(string org)
        {
            this.organization = org;
            UpdateHash();
        }

        public void SetRenameTemplate(string t)
        {
            if (this.template == t)
                return;
            var parts = t.Split(' ');
            foreach (var p in parts)
                if (p.Contains("$"))
                    if (!hash.Contains(p) && p != "$page")
                        throw new Exception("Unrecognized template!");
            this.template = t;
        }

        public void SetPageBreaker(string b)
        {
            if (b == "")
            {
                this.pageBreaker = b;
                return;
            }
            if (b == this.pageBreaker)
                return;

            var parts = b.Split(' ');
            foreach (var p in parts)
            {
                if (!p.Contains("$"))
                    throw new Exception("Unrecognized breaker!");
                else
                {
                    if (!hash.Contains(p))
                        throw new Exception("Unrecognized breaker!");
                }
            }
            this.pageBreaker = b;
        }

        public void ChangePage(int n)
        {
            try
            {
                folder.ChangePage(n, this.treeView);

                page = folder.GetCurrentPage();
                UpdateHash();
                UpdateImage();
                pageNumber += n;

                zoom = 1.0;
            }
            catch
            {
                if (n < 0)
                    MessageBox.Show("Start reached!");
                else
                    MessageBox.Show("End reached!");
            }
        }

        public void DeleteCurrentPage()
        {
            Page p = folder.GetCurrentPage();
            try
            {
                ChangePage(1);
                pageNumber--;
            }
            catch
            {
                ChangePage(-1);
                pageNumber++;
            }
            folder.DeletePage(p);
            page = folder.GetCurrentPage();
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

        public void ChangeContainer(string path)
        {
            path = path.Replace(root+"\\", "");
            var subParts = path.Split('\\');
            folder.ChangeCurrentContainer(subParts);
            page = folder.GetCurrentPage();
            UpdateHash();
            UpdateImage();

            var dir = folder;
            while (dir.Key != pageBreaker)
                dir = dir.CurrentDir;

            var newDir = folder;
            while (newDir.CurrentDir != null)
                newDir = newDir.CurrentDir;

            pageNumber = dir.CountPagesUntil(newDir.Name) + 1;
            zoom = 1.0;
        }

        private void UpdateContainerKeys()
        {
            var dir = folder;
            var i = 0;

            while (true)
            {
                dir.Key = hashKeys[i++];
                if (dir.CurrentDir == null)
                    break;
                dir = dir.CurrentDir;
            }
        }

        private void UpdateHash()
        {
            if (!CheckOrganizationMatch())
                throw new Exception("Structure given does not match actual folder structure");

            this.hashKeys = new List<string>();

            var orgParts = organization.Split('\\');
            if (orgParts[orgParts.Length - 1] != "")
            {
                var auxParts = orgParts.ToList();
                auxParts.Add("");
                orgParts = auxParts.ToArray();
            }
            var pathParts = page.Path.Split('\\');
            pathParts = pathParts.Skip(Math.Max(0, pathParts.Count() - orgParts.Length)).ToArray();

            for (int i = 0; i < orgParts.Length; ++i)
            {
                string strS = orgParts[i];
                string strP = pathParts[i];

                if (strS == "")
                    continue;

                var wordsS = strS.Split(' ').ToList();
                var wordsP = strP.Split(' ').ToList();

                List<string> keys = wordsS.ToList();
                List<string> values = wordsP.ToList();
                
                for(int iK = 0; iK < keys.Count; ++iK)
                {
                    int iV = values.IndexOf(keys.ElementAt(iK));
                    if (iV != -1)
                    {
                        values[iV] = "";
                        keys[iK] = "";
                    }
                }

                keys.Add("");
                values.Add("");

                while (keys.Contains(""))
                {
                    string join = "";
                    for(int iK = 0; iK < keys.Count; ++iK)
                    {
                        if(keys[iK] != "")
                            join += keys[iK];
                        if (keys[iK] == "")
                        {
                            keys.RemoveRange(0, iK+1);
                            if (join != "")
                                keys.Insert(0, join.Contains(" ") ? join.Substring(0, join.LastIndexOf(" ")) : join);
                            break;
                        }
                    }
                }

                while (values.Contains(""))
                {
                    string join = "";
                    for (int iV = 0; iV < values.Count; ++iV)
                    {
                        if (values[iV] != "")
                            join += values[iV] + " ";
                        if (values[iV] == "")
                        {
                            values.RemoveRange(0, iV+1);
                            if (join != "")
                                values.Insert(0, join.Contains(" ") ? join.Substring(0, join.LastIndexOf(" ")) : join);
                            break;
                        }
                    }
                }

                if (keys.Count != values.Count)
                    throw new Exception("Something went wrong...");
                int pageBreakerIndex = keys.IndexOf(pageBreaker);
                if (pageBreakerIndex != -1 && hash.Contains(pageBreaker))
                {
                    if (hash[pageBreaker].ToString() != values[pageBreakerIndex])
                        pageNumber = 0;
                }

                if (autoRename && values[keys.IndexOf(DefaultRenameKey)] != hash[DefaultRenameKey].ToString())
                    RenameKey(DefaultRenameKey);

                for (int k = 0; k < keys.Count; ++k)
                {
                    hash[keys[k]] = values[k];
                    hashKeys.Add(keys[k]);
                }
            }
            UpdateContainerKeys();
        }

        private bool CheckOrganizationMatch()
        {
            var orgParts = organization.Split('\\');
            if (orgParts[orgParts.Length - 1] != "")
            {
                var auxParts = orgParts.ToList();
                auxParts.Add("");
                orgParts = auxParts.ToArray();
            }
            var pathParts = page.Path.Split('\\');
            pathParts = pathParts.Skip(Math.Max(0, pathParts.Count() - orgParts.Length)).ToArray();

            for (int i = 0; i < orgParts.Length; ++i)
            {
                int lastS = -1;
                string strS = orgParts[i];
                string strP = pathParts[i];
                int iS = 0, iP = 0;

                while (iS < strS.Length && iP < strP.Length)
                {
                    char cS = strS[iS];
                    char cP = strP[iP];

                    if (cS == '$')
                    {
                        while(cS != ' ' && iS < strS.Length)
                        {
                            cS = strS[iS];
                            iS++;
                        }

                        while (cP != ' ' && iP < strP.Length)
                        {
                            cP = strP[iP];
                            iP++;
                        }

                        lastS = iS;
                    }
                    else if (cS == cP)
                    {
                        iS++;
                        iP++;
                    }
                    else
                    {
                        if (lastS == -1 || iP >= strP.Length)
                            return false;
                        iS = lastS;

                        while (cP != ' ' && iP < strP.Length)
                        {
                            cP = strP[iP];
                            iP++;
                        }
                    }
                }
            }
            return true;
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

        public void ApplyZoom(int times, int x, int y)
        {
            if (zoom * Math.Pow(ZOOM_RATIO, times) > MAX_ZOOM)
                return;

            int left, top;

            pictureBox.Width = (int)Math.Round(pictureBox.Width * Math.Pow(ZOOM_RATIO, times));
            pictureBox.Height = (int)Math.Round(pictureBox.Height * Math.Pow(ZOOM_RATIO, times));

            if (pictureBox.Width > pictureBox.Parent.Width)
            {
                left = (int)Math.Round(pictureBox.Parent.Width / 2 - x * Math.Pow(ZOOM_RATIO, times));
                top = (int)Math.Round(pictureBox.Parent.Height / 2 - y * Math.Pow(ZOOM_RATIO, times));

                if (left > 0)
                    left = 0;
                if (left + pictureBox.Width < pictureBox.Parent.Width)
                    left = pictureBox.Parent.Width - pictureBox.Width;

                if (top > 0)
                    top = 0;
                if (top + pictureBox.Height < pictureBox.Parent.Height)
                    top = pictureBox.Parent.Height - pictureBox.Height;

                left = Math.Abs(left) * -1;
                top = Math.Abs(top) * -1;
            }
            else
            {
                left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
                top = pictureBox.Parent.Height / 2 - pictureBox.Height / 2;
            }
            pictureBox.Left = left;
            pictureBox.Top = top;

            zoom *= Math.Pow(ZOOM_RATIO, times);
        }

        public void RemoveZoom(int times)
        {
            if (pictureBox.Height / Math.Pow(ZOOM_RATIO, times) < pictureBox.Parent.Height)
                return;
            pictureBox.Width = (int)Math.Round(pictureBox.Width / Math.Pow(ZOOM_RATIO, times));
            pictureBox.Height = (int)Math.Round(pictureBox.Height / Math.Pow(ZOOM_RATIO, times));
            pictureBox.Left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
            pictureBox.Top = 0;

            zoom /= Math.Pow(ZOOM_RATIO, times);
        }
    }
}
