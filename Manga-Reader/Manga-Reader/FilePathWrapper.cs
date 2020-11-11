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
                    possiblePathOrganization = $"{VAR_CHAR}Chapter\\";
                    break;

                case 1:
                    possiblePathOrganization = $"{VAR_CHAR}Manga\\{VAR_CHAR}Chapter\\";
                    break;

                case 2:
                    possiblePathOrganization = $"{VAR_CHAR}Manga\\Volume {VAR_CHAR}Volume\\{VAR_CHAR}Chapter\\";
                    break;

                case 3:
                    possiblePathOrganization = $"{VAR_CHAR}Folder\\{VAR_CHAR}Manga\\{VAR_CHAR}Volume\\{VAR_CHAR}Chapter\\";
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
                    possibleTemplate = $"Chapter {VAR_CHAR}Chapter Page " + PAGE_KEY;
                    break;

                case 1:
                    possibleTemplate = $"Volume {VAR_CHAR}Volume - {VAR_CHAR}Chapter p. " + PAGE_KEY;
                    break;

                case 2:
                    possibleTemplate = $"{VAR_CHAR}Manga - Volume {VAR_CHAR}Volume page " + PAGE_KEY;
                    break;

                default:
                    possibleTemplate = $"Volume {VAR_CHAR}Volume - page " + PAGE_KEY;
                    break;
            }

            return possibleTemplate;
        }
        public override string GeneratePossiblePageBreaker()
        {
            if (pageBreaker != null && pageBreaker.StringValue != "")
                return pageBreaker.StringValue;
            string possibleBreaker = "";

            switch (Depth)
            {
                case 0:
                    possibleBreaker = $"{VAR_CHAR}Chapter";
                    break;

                case 1:
                    possibleBreaker = $"{VAR_CHAR}Manga";
                    break;

                default:
                    possibleBreaker = $"{VAR_CHAR}Volume";
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
                if (p.Contains(VAR_CHAR))
                    if (!hash.Contains(p) && p != PAGE_KEY)
                        throw new Exception("Unrecognized template!");
            this.template = t;
        }
        protected override void SetPageBreaker(Key b)
        {
            if (b.StringValue == "")
            {
                this.pageBreaker = b;
                return;
            }
            if (b == this.pageBreaker)
                return;

            var parts = b.StringValue.Split(' ');
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
        public override void SetPageBreaker(string b)
        {
            SetPageBreaker(hashKeys.Find(key => key.StringValue == b));
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
            void UpdateContainerKeysRec(Container start, int keyIndex)
            {
                try
                {
                    start.Key = hashKeys[keyIndex];

                    foreach (Container fc in start.Containers)
                        UpdateContainerKeysRec(fc, keyIndex + 1);
                }
                catch { }
            }

            UpdateContainerKeysRec(root as Container, 0);
        }

        public override void RenameContainer(Container cont, int start)
        {
            Container innerContainer = Navigator.FindCurrentContainer(cont);
            Hashtable hash = GetHash(innerContainer.Path);

            start += cont.PageWrapper.RenamePages(template, hash, start, PAGE_KEY);

            foreach(Container c in cont.Containers)
            {
                RenameContainer(c, start);
                start += c.PagesCount(0);
            }
        }
    }
}
