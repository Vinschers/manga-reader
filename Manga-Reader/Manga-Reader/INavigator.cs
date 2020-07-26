using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    interface INavigator
    {
        IFile AdvancePage();
        IFile RetreatPage();
        IFolder Folder { get; }
        void ChangeFolder(IFolder newFolder);
        void DeletePage();
    }
}
