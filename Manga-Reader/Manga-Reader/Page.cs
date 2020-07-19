using System.Collections;
using System.IO;

namespace Manga_Reader
{
    public class Page
    {
        string name;
        string path;
        public Page(string path)
        {
            this.path = path;
            name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
        }

        public string Name { get => name; set => name = value; }
        public string Path { get => path; set => path = value; }

        public void Rename(string newName, Hashtable hash, int n)
        {
            foreach (string key in hash.Keys)
                newName = newName.Replace(key, hash[key].ToString());
            newName = newName.Replace("$page", n + "");
            newName = path.Substring(0, path.LastIndexOf("\\") + 1) + newName + path.Substring(path.LastIndexOf("."));
            if (newName != path)
            {
                File.Move(path, newName);
                path = newName;
                name = path.Substring(path.LastIndexOf("\\") + 1, path.LastIndexOf(".") - path.LastIndexOf("\\") - 1);
            }
        }

        public void Delete()
        {
            File.Delete(path);
        }
    }
}
