using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    interface IUIHandler
    {
        PictureBox PictureBox { get; }
        void ApplyZoom(int lvl);
        void RetreatZoom(int lvl);
        void UpdateImage();
        void UpdateTreeview();
        void UpdateLabels();
        void CopyToClipboard();
    }
}
