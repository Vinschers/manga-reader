using System.Drawing;
using System.Windows.Forms;

public class MyTreeView : TreeView
{
    Color highlight = Color.DarkGreen;
    public MyTreeView()
    {
        this.DrawMode = TreeViewDrawMode.OwnerDrawText;
    }

    protected override void OnDrawNode(DrawTreeNodeEventArgs e)
    {
        TreeNodeStates state = e.State;
        Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
        Color fore = e.Node.ForeColor;
        if (fore == Color.Empty)
            fore = e.Node.TreeView.ForeColor;
        if (e.Node == e.Node.TreeView.SelectedNode)
        {
            fore = SystemColors.HighlightText;
            e.Graphics.FillRectangle(new SolidBrush(highlight), e.Bounds);
            ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, fore, highlight);
            TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, fore, highlight, TextFormatFlags.GlyphOverhangPadding);
        }
        else
        {
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
            TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, fore, TextFormatFlags.GlyphOverhangPadding);
        }
    }
}