using System.Collections;
using System.IO;

namespace Manga_Reader
{
    public class Page : IFile
    {
        string name;
        string path;
        Container parent;
        public IFolder Parent { get => parent; }
        public Page(string path)
        {
            this.path = path;
            name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
        }

        public string Name { get => name; set => name = value; }
        public string Path { get => path; set => path = value; }

        /*public bool Rename(string newName, Hashtable hash, int n, string pageKey)
        {
            foreach (string key in hash.Keys)
                newName = newName.Replace(key, hash[key].ToString());
            newName = newName.Replace(pageKey, n + "");
            newName = path.Substring(0, path.LastIndexOf("\\") + 1) + newName + path.Substring(path.LastIndexOf("."));
            if (newName != path)
            {
                File.Move(path, newName);
                path = newName;
                name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
            }
            return true;
        }*/

        public bool Rename(string newName)
        {
            try
            {
                if (newName != path)
                {
                    File.Move(path, newName);
                    path = newName;
                    name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
