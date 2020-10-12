using System.Windows.Forms;

namespace Manga_Reader
{
    class MyTreeNode : TreeNode
    {
        Container container;

        public Container Container { get => container; }
        public MyTreeNode(Container cont)
        {
            container = cont;
            this.Text = cont.Path.Substring(cont.Path.LastIndexOf("\\") + 1);
            this.Name = cont.Path;
        }
    }
}