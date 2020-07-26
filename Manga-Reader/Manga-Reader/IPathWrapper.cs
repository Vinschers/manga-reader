using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    interface IPathWrapper
    {
        int Depth { get; }
        string GeneratePossiblePathOrganization();
        string GeneratePossibleRenameTemplate();
        string GeneratePossiblePageBreaker();
        string SetPathOrganization(string org);
        string SetRenameTemplate(string t);
        string SetPageBreaker(string b);
        void UpdateHash();
    }
}
