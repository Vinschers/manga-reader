using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public abstract class PathWrapper
    {
        protected Container root;
        protected string organization, template, pageBreaker, defaultRenameKey;
        protected Hashtable hash;
        protected List<string> hashKeys;

        public const char FILE_SEPARATOR = ';';

        public int Depth { get => root.Depth; }
        public string Organization { get => organization; }
        public string Template { get => template; }
        public string DefaultRenameKey
        {
            get
            {
                if (defaultRenameKey != null && defaultRenameKey != "")
                    return defaultRenameKey;
                return pageBreaker;
            }
            set
            {
                if (hashKeys.IndexOf(value) != -1)
                    defaultRenameKey = value;
            }
        }
        public List<string> Keys { get => hashKeys; }
        public Hashtable Hash { get => hash; }
        public string PageBreaker { get => pageBreaker; set => SetPageBreaker(value); }

        public PathWrapper()
        {
            hash = new Hashtable();
            hashKeys = new List<string>();
        }

        public PathWrapper(Container root) : this()
        {
            this.root = root;
        }
        protected abstract string GetRelativePath(string fp);
        protected bool CheckOrganizationMatch(string fp)
        {
            var orgParts = organization.Split('\\');
            if (orgParts[orgParts.Length - 1] != "")
            {
                var auxParts = orgParts.ToList();
                auxParts.Add("");
                orgParts = auxParts.ToArray();
            }
            var pathParts = GetRelativePath(fp).Split('\\');
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
                        while (cS != ' ' && iS < strS.Length)
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

        public abstract string GeneratePossiblePathOrganization();
        public abstract string GeneratePossibleRenameTemplate();
        public abstract string GeneratePossiblePageBreaker();
        public void SetPathOrganization(string org, string path)
        {
            this.organization = org;
            UpdateHash(path);
        }
        public abstract void SetRenameTemplate(string t);
        public abstract void SetPageBreaker(string pBreak);
        public abstract void UpdateContainerKeys();
        public abstract void RenameContainer(Container cont, int start);
        protected Hashtable GetHash(string path)
        {
            if (!CheckOrganizationMatch(path))
                throw new Exception("Structure given does not match actual folder structure");

            List<string> hashKeys = new List<string>();
            Hashtable hash = new Hashtable();

            var orgParts = organization.Split('\\');
            if (orgParts[orgParts.Length - 1] != "")
            {
                var auxParts = orgParts.ToList();
                auxParts.Add("");
                orgParts = auxParts.ToArray();
            }
            var pathParts = GetRelativePath(path).Split('\\');
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

                for (int iK = 0; iK < keys.Count; ++iK)
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
                    for (int iK = 0; iK < keys.Count; ++iK)
                    {
                        if (keys[iK] != "")
                            join += keys[iK];
                        if (keys[iK] == "")
                        {
                            keys.RemoveRange(0, iK + 1);
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
                            values.RemoveRange(0, iV + 1);
                            if (join != "")
                                values.Insert(0, join.Contains(" ") ? join.Substring(0, join.LastIndexOf(" ")) : join);
                            break;
                        }
                    }
                }

                if (keys.Count != values.Count)
                    throw new Exception("Something went wrong...");

                for (int k = 0; k < keys.Count; ++k)
                {
                    hash[keys[k]] = values[k];
                    hashKeys.Add(keys[k]);
                }
            }

            return hash;
        }
        protected List<string> GetKeys(Hashtable hash)
        {
            var parts = organization.Split('\\').ToList();
            var orgParts = new List<string>();
            foreach (var part in parts)
                orgParts.AddRange(part.Split(' ').ToList().FindAll(p => p.Contains("$")));

            var keys = new List<string>();
            foreach (string key in hash.Keys)
                keys.Add(key);

            return orgParts.Intersect(keys).ToList();
        }
        public void UpdateHash(string path)
        {
            hash = GetHash(path);
            hashKeys = GetKeys(hash);
            UpdateContainerKeys();
        }

        public string GetConfigs()
        {
            string ret = "";
            ret += organization + FILE_SEPARATOR;
            ret += template + FILE_SEPARATOR;
            ret += pageBreaker + FILE_SEPARATOR;
            return ret;
        }
        public void LoadConfigs(string configs, string relativePath)
        {
            string[] parts = configs.Split(FILE_SEPARATOR);
            SetPathOrganization(parts[0], relativePath);
            SetRenameTemplate(parts[1]);
            SetPageBreaker(parts[2]);
        }
    }
}
