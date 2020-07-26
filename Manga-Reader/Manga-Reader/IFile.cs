using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public interface IFile
    {
        string Path { get; }
        string Name { get; }
        IFolder Parent { get; }

        bool Rename(string newName);
        bool Delete();
    }
}
