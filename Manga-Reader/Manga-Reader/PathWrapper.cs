using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    abstract class PathWrapper
    {
        protected Container root;
        protected string organization, template, pageBreaker, defaultRenameKey;
        protected Hashtable hash;
        protected List<string> hashKeys;
        protected int pageNumber;

        public int Depth { get => root.Depth; }
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

        public PathWrapper()
        {
            hash = new Hashtable();
            hashKeys = new List<string>();
            pageNumber = -1;
        }

        public PathWrapper(string path) : this()
        {
            root = GetRootContainer(path);
        }

        protected abstract Container GetRootContainer(string path);
        protected abstract string GetCurrentPath();
        protected bool CheckOrganizationMatch()
        {
            var orgParts = organization.Split('\\');
            if (orgParts[orgParts.Length - 1] != "")
            {
                var auxParts = orgParts.ToList();
                auxParts.Add("");
                orgParts = auxParts.ToArray();
            }
            var pathParts = GetCurrentPath().Split('\\');
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
        public void SetPathOrganization(string org)
        {
            this.organization = org;
            UpdateHash();
        }
        public abstract void SetRenameTemplate(string t);
        protected abstract void UpdateContainerKeys();
        protected void UpdateHash()
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
            var pathParts = GetCurrentPath().Split('\\');
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
                int pageBreakerIndex = keys.IndexOf(pageBreaker);
                if (pageBreakerIndex != -1 && hash.Contains(pageBreaker))
                {
                    if (hash[pageBreaker].ToString() != values[pageBreakerIndex])
                        pageNumber = 0;
                }

                //if (autoRename && values[keys.IndexOf(DefaultRenameKey)] != hash[DefaultRenameKey].ToString())
                //    Rename();

                for (int k = 0; k < keys.Count; ++k)
                {
                    hash[keys[k]] = values[k];
                    hashKeys.Add(keys[k]);
                }
            }
            UpdateContainerKeys();
        }
    }
}
