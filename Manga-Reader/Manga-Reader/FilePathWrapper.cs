using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    class FilePathWrapper : PathWrapper
    {
        protected string currentPath;

        public const string PAGE_KEY = "$page";

        public string CurrentPath {get => currentPath; set => currentPath = value; }

        public FilePathWrapper(Container root) : base(root)
        {
            currentPath = root.Path;
        }

        public override string GeneratePossiblePathOrganization()
        {
            if (organization != null && organization != "")
                return organization;
            string possiblePathOrganization = "";

            switch (Depth)
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
        public override string GeneratePossibleRenameTemplate()
        {
            if (template != null && template != "")
                return template;
            string possibleTemplate = "";

            switch (Depth)
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
        public override string GeneratePossiblePageBreaker()
        {
            if (pageBreaker != null && pageBreaker != "")
                return pageBreaker;
            string possibleBreaker = "";

            switch (Depth)
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
        public override void SetRenameTemplate(string t)
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
        public override void SetPageBreaker(string b)
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
        
        protected override string GetRelativePath(string fullPath)
        {
            int partsAbsolutePath = currentPath.Split('\\').Length - 1;
            string[] parts = fullPath.Split('\\');
            string ret = "";
            for (int i = partsAbsolutePath; i < parts.Length; i++)
                ret += parts[i] + "\\";
            return ret;
        }

        public override void UpdateContainerKeys()
        {
            void UpdateContainerKeysRec(FileContainer start, int keyIndex)
            {
                start.Key = hashKeys[keyIndex];

                foreach (FileContainer fc in start.Containers)
                    UpdateContainerKeysRec(fc, keyIndex + 1);
            }

            UpdateContainerKeysRec(root as FileContainer, 0);
        }

        public override void RenameContainer(Container cont, int start)
        {
            Container innerContainer = Navigator.FindCurrentContainer(cont);
            Hashtable hash = GetHash(innerContainer.Path);

            start += (cont.PageWrapper as FilePageWrapper).RenamePages(template, hash, start, PAGE_KEY);

            foreach(Container c in cont.Containers)
            {
                RenameContainer(c, start);
                start += c.PagesCount(0);
            }
        }
    }
}
