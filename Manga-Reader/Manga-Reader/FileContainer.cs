using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Manga_Reader
{
    class FileContainer : Container
    {
        public FileContainer(FileContainer parent, string path) : base(parent, path)
        { }
        public FileContainer(string path) : base(path)
        { }
        public FileContainer(FileContainer parent, string path, Key key) : base(parent, path, key)
        { }

        private void ResetContainers()
        {
            containers.Clear();

            var dirs = Directory.GetDirectories(path).ToList();
            if (dirs.Count > 0)
            {
                dirs = dirs.OrderBy(x => Regex.Replace(x, "[0-9]+", match => match.Value.PadLeft(10, '0'))).ToList();

                foreach (var dir in dirs)
                {
                    containers.Add(new FileContainer(this, dir));
                }
            }
        }

        protected override void Reset()
        {
            ResetContainers();
            pageWrapper = new FilePageWrapper(this);
        }
    }
}
