using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public enum ShortcutCodes
    {
        Copy=3,
        Delete=4,
        Rename=18,
    }
    class Shortcut
    {
        public ShortcutCodes Code { get; set; }
        public string Keys { get; set; }
        public Action Function { get; set; }
        public string Help { get; set; }
        public override string ToString()
        {
            return ((int)Code).ToString();
        }
    }
    class ReaderShortcuts
    {
        Readere reader;
        Hashtable shortcuts;
        public ReaderShortcuts(Readere r)
        {
            reader = r;
        }

        public Shortcut this[string key]
        {
            get => (Shortcut)shortcuts[key];
        }

        public void Update()
        {
            /*shortcuts = new Hashtable();

            Shortcut delete = new Shortcut();
            delete.Code = ShortcutCodes.Delete;
            delete.Keys = "Ctrl + D";
            delete.Function = reader.DeleteCurrentPage;
            delete.Help = "Deletes current page";
            shortcuts.Add(delete+"", delete);

            Shortcut rename = new Shortcut();
            rename.Code = ShortcutCodes.Rename;
            rename.Keys = "Ctrl + R";
            rename.Function = reader.Rename;
            rename.Help = "Renames specified key";
            shortcuts.Add(rename+"", rename);

            Shortcut copy = new Shortcut();
            copy.Code = ShortcutCodes.Copy;
            copy.Keys = "Ctrl + C";
            copy.Function = reader.CopyToClipboard;
            copy.Help = "Copies current page to clipboard";
            shortcuts.Add(copy+"", copy);*/
        }

        public List<Shortcut> Values
        {
            get => shortcuts.Values.Cast<Shortcut>().ToList();
        }
    }
}