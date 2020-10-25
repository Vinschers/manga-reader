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
        protected string organization, template;
        protected Hashtable hash;
        protected List<Key> hashKeys;
        protected Key pageBreaker, defaultRenameKey;

        public const char FILE_SEPARATOR = ';';
        public const string VAR_CHAR = "$";
        public const string PAGE_KEY = VAR_CHAR + "page";

        public int Depth { get => root.Depth; }
        public string Organization { get => organization; }
        public string Template { get => template; }
        public Key DefaultRenameKey
        {
            get
            {
                if (defaultRenameKey != null && defaultRenameKey.StringValue != "")
                    return defaultRenameKey;
                return pageBreaker;
            }
            set
            {
                if (hashKeys.IndexOf(value) != -1)
                    defaultRenameKey = value;
            }
        }
        public List<Key> Keys { get => hashKeys; }
        public Hashtable Hash { get => hash; }
        public Key PageBreaker { get => pageBreaker; set => SetPageBreaker(value); }

        public PathWrapper()
        {
            hash = new Hashtable();
            hashKeys = new List<Key>();
        }

        public PathWrapper(Container root) : this()
        {
            this.root = root;
        }
        public void Delete()
        {
            hashKeys.Clear();
            hash.Clear();
            root.Delete();
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

            for (int i = 0; i < pathParts.Length; ++i)
            {
                int lastS = -1;
                string strS = orgParts[i];
                string strP = pathParts[i];
                int iS = 0, iP = 0;

                while (iS < strS.Length && iP < strP.Length)
                {
                    char cS = strS[iS];
                    char cP = strP[iP];

                    if (cS == VAR_CHAR[0])
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
        public void SetPathOrganization(string org, Container cont)
        {
            this.organization = org;
            UpdateHash(cont);
            hashKeys = GetKeys(hash);
            UpdateContainerKeys();
        }
        public abstract void SetRenameTemplate(string t);
        protected abstract void SetPageBreaker(Key pBreak);
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

            for (int i = 0; i < pathParts.Length; ++i)
            {
                string strS = orgParts[i];
                string strP = pathParts[i];

                if (strS == "" || strP == "")
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
        protected List<Key> GetKeys(Hashtable hash)
        {
            var parts = organization.Split('\\').ToList();
            var orgParts = new List<string>();
            foreach (var part in parts)
                orgParts.AddRange(part.Split(' ').ToList().FindAll(p => p.Contains(VAR_CHAR)));

            var keys = new List<string>();
            foreach (string key in hash.Keys)
                keys.Add(key);

            orgParts = orgParts.Intersect(keys).ToList();

            List<Key> ret = new List<Key>();
            for (int i = 0; i < orgParts.Count(); i++)
                ret.Add(new Key { NumericValue= orgParts.Count()-i, StringValue= orgParts[i] });

            return ret;
        }
        public void UpdateHash(Container cont)
        {
            hash = GetHash(cont.Path);
        }

        public string GetConfigs()
        {
            string ret = "";
            ret += organization + FILE_SEPARATOR;
            ret += template + FILE_SEPARATOR;
            ret += pageBreaker.NumericValue + "," + pageBreaker.StringValue + FILE_SEPARATOR;
            return ret;
        }
        public void LoadConfigs(string configs, Container relativeContainer)
        {
            string[] parts = configs.Split(FILE_SEPARATOR);
            SetPathOrganization(parts[0], relativeContainer);
            SetRenameTemplate(parts[1]);
            SetPageBreaker(new Key { NumericValue=int.Parse(parts[2].Split(',')[0]), StringValue=parts[2].Split(',')[1] });
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PathWrapper))
                return false;
            PathWrapper pw = obj as PathWrapper;
            if (!root.Equals(pw.root))
                return false;
            if (organization != pw.organization || template != pw.template || pageBreaker != pw.pageBreaker || defaultRenameKey != pw.defaultRenameKey)
                return false;
            if (!hashKeys.SequenceEqual(pw.hashKeys))
                return false;
            return true;
        }
    }
}
