using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public interface IFolder
    {
        string Path { get; }
        string Name { get; }
        IFolder Parent { get; }
        IFolder Dir { get; }
        ICollection<string> Folders { get; }
        ICollection<string> Files { get; }
        IFile GetCurrentFile();
    }
}
