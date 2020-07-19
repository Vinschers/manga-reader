using System.Windows.Forms;

class MyTreeNode : TreeNode
{
    private string filePath;

    public MyTreeNode(string fp)
    {
        filePath = fp;
        this.Text = fp.Substring(fp.LastIndexOf("\\")+1);
    }

    public string FilePath { get => filePath; }
}
