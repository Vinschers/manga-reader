using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    class UIHandler
    {
        protected Panel panel;
        protected PictureBox pictureBox;
        protected Label lblPage;
        protected Label lblManga;
        protected MyTreeView treeView;
        protected MenuStrip menuStrip;
        protected ToolStripMenuItem renameToolStripMenuItem;
        protected Point mouseDownLocation, mouseLocation;

        protected double zoom = 1.0;
        protected bool changeContainer = false;
        protected Form form;
        protected Action<Container> changeContainerTreeView;

        protected const double ZOOM_RATIO = 1.5;
        protected const double MAX_ZOOM = 5;

        public double Zoom { get => zoom; }

        public UIHandler(Form frm, Panel pnl, PictureBox pb, MyTreeView tv, MenuStrip ms, Label page, Label manga,
            ToolStripMenuItem rename, Action<Container> chgContainer)
        {
            form = frm;
            panel = pnl;
            pictureBox = pb;
            treeView = tv;
            menuStrip = ms;
            lblPage = page;
            lblManga = manga;
            renameToolStripMenuItem = rename;
            changeContainerTreeView = chgContainer;

            SetupEvents();
        }

        protected void SetupEvents()
        {
            pictureBox.DoubleClick += new EventHandler(PictureBox_DoubleClick);
            pictureBox.MouseWheel += new MouseEventHandler(PictureBox_MouseWheel);
            pictureBox.MouseDown += new MouseEventHandler(PictureBox_MouseDown);
            pictureBox.MouseMove += new MouseEventHandler(PictureBox_MouseMove);

            treeView.KeyDown += new KeyEventHandler(TreeView_KeyDown);
            treeView.KeyPress += new KeyPressEventHandler(TreeView_KeyPress);
            treeView.KeyUp += new KeyEventHandler(TreeView_KeyUp);
            treeView.MouseDown += new MouseEventHandler(TreeView_MouseDown);
            treeView.AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
        }

        public void SetupPanel()
        {
            panel.Left = treeView.Width + 15;
            panel.Top = menuStrip.Height + 10;
            panel.Width = form.Width - 2 * (treeView.Width + 15);
            panel.Height = form.Height - 150;

            lblPage.Top = panel.Top + panel.Height + 20;
            lblManga.Top = lblPage.Top + lblPage.Height + 5;
        }
        public void SetupRenameMenu(PathWrapper pw, EventHandler renameKey)
        {
            List<ToolStripMenuItem> menus = new List<ToolStripMenuItem>();
            List<ToolStripMenuItem> shortcutsMenu = new List<ToolStripMenuItem>();
            foreach (Key key in pw.Keys)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem();

                menuItem.Name = "renameMenu"+key.StringValue;
                menuItem.Size = new Size(180, 22);
                menuItem.Text = "Rename " + key.StringValue;
                menuItem.Click += new EventHandler(renameKey);
                menuItem.Tag = key;
                ((ToolStripDropDownMenu)menuItem.DropDown).ShowImageMargin = false;

                if (key == pw.PageBreaker)
                    menuItem.ShortcutKeys = Keys.Control | Keys.R;

                menus.Add(menuItem);
            }

            renameToolStripMenuItem.DropDownItems.AddRange(menus.ToArray());
        }
        public void SetupTreeView(Container root)
        {
            void SetupTreeViewRec(Container cont, MyTreeNode parent)
            {
                foreach (Container c in cont.Containers)
                {
                    MyTreeNode child = new MyTreeNode(c);
                    parent.Nodes.Add(child);
                    SetupTreeViewRec(c, child);
                }
            }

            MyTreeNode rootNode = new MyTreeNode(root);

            SetupTreeViewRec(root, rootNode);

            this.treeView.Nodes.Add(rootNode);
            this.treeView.ExpandAll();
            var node = this.treeView.Nodes[0];
            while (node.Nodes != null && node.Nodes.Count > 0)
                node = node.Nodes[0];
            treeView.SelectedNode = node;
            rootNode.EnsureVisible();
        }

        public void UpdateSelectedNode(Container cont)
        {
            treeView.SelectedNode = treeView.Nodes.Find(cont.Path, true)[0] as MyTreeNode;
        }

        public void UpdateImage(Image img)
        {
            pictureBox.Width = img.Width;
            pictureBox.Height = img.Height;
            if (pictureBox.Height > pictureBox.Parent.Height || pictureBox.Width > pictureBox.Parent.Width)
            {
                double rWidth = (double)pictureBox.Width / pictureBox.Height;
                pictureBox.Height = pictureBox.Parent.Height;
                pictureBox.Width = (int)Math.Round(rWidth * pictureBox.Height);

                if (pictureBox.Width > pictureBox.Parent.Width)
                {
                    double rHeight = (double)pictureBox.Height / pictureBox.Width;
                    pictureBox.Width = pictureBox.Parent.Width;
                    pictureBox.Height = (int)Math.Round(rHeight * pictureBox.Width);
                }
            }
            pictureBox.Left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
            pictureBox.Top = pictureBox.Parent.Height / 2 - pictureBox.Height / 2;
            pictureBox.Image = new Bitmap(img);
            pictureBox.Invalidate();
            int n = (int)Math.Round(Math.Log10(zoom) / Math.Log10(ZOOM_RATIO));
            zoom = 1.0;
            ApplyZoom(n, pictureBox.Width, 0);
        }
        public void ApplyZoom(int times, int x, int y)
        {
            if (zoom * Math.Pow(ZOOM_RATIO, times) > Math.Pow(ZOOM_RATIO, MAX_ZOOM))
                return;

            int left, top;

            pictureBox.Width = (int)Math.Round(pictureBox.Width * Math.Pow(ZOOM_RATIO, times));
            pictureBox.Height = (int)Math.Round(pictureBox.Height * Math.Pow(ZOOM_RATIO, times));

            if (pictureBox.Width > pictureBox.Parent.Width)
            {
                left = (int)Math.Round(pictureBox.Parent.Width / 2 - x * Math.Pow(ZOOM_RATIO, times));
                top = (int)Math.Round(pictureBox.Parent.Height / 2 - y * Math.Pow(ZOOM_RATIO, times));

                if (left > 0)
                    left = 0;
                if (left + pictureBox.Width < pictureBox.Parent.Width)
                    left = pictureBox.Parent.Width - pictureBox.Width;

                if (top > 0)
                    top = 0;
                if (top + pictureBox.Height < pictureBox.Parent.Height)
                    top = pictureBox.Parent.Height - pictureBox.Height;

                left = Math.Abs(left) * -1;
                top = Math.Abs(top) * -1;
            }
            else
            {
                left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
                top = pictureBox.Parent.Height / 2 - pictureBox.Height / 2;
            }
            pictureBox.Left = left;
            pictureBox.Top = top;

            zoom *= Math.Pow(ZOOM_RATIO, times);
        }
        public void ApplyZoom(int times)
        {
            ApplyZoom(times, mouseLocation.X, mouseLocation.Y);
        }
        public void RetreatZoom(int times)
        {
            if (times == 0)
                return;

            if (zoom / Math.Pow(ZOOM_RATIO, times) < 1.0)
            {
                RetreatZoom(times - 1);
                return;
            }
            pictureBox.Width = (int)Math.Round(pictureBox.Width / Math.Pow(ZOOM_RATIO, times));
            pictureBox.Height = (int)Math.Round(pictureBox.Height / Math.Pow(ZOOM_RATIO, times));
            pictureBox.Left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
            pictureBox.Top = pictureBox.Parent.Height / 2 - pictureBox.Height / 2;

            zoom /= Math.Pow(ZOOM_RATIO, times);
        }
        public void UpdateLabels(string pageName, string rootName)
        {
            lblPage.Text = pageName;
            lblManga.Text = rootName;

            lblPage.Left = form.Width / 2 - lblPage.Width / 2;
            lblManga.Left = form.Width / 2 - lblManga.Width / 2;
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Form.ModifierKeys.HasFlag(Keys.Control))
                return;

            if (e.Delta < 0)
                RetreatZoom(1);
            else
                ApplyZoom(1, mouseLocation.X, mouseLocation.Y);
        }
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLocation = e.Location;
            }
        }
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            if (zoom <= 1.0 || e.Button != MouseButtons.Left)
                return;

            int left = 0, top = 0;

            if (pictureBox.Width < panel.Width)
                left = pictureBox.Parent.Width / 2 - pictureBox.Width / 2;
            else
            {
                left = e.X + pictureBox.Left - mouseDownLocation.X;

                if (left > 0)
                    left = 0;
                if (left + pictureBox.Width < panel.Width)
                    left = panel.Width - pictureBox.Width;
            }

            if (pictureBox.Height < panel.Height)
                top = pictureBox.Parent.Height / 2 - pictureBox.Height / 2;
            else
            {
                top = e.Y + pictureBox.Top - mouseDownLocation.Y;

                if (top > 0)
                    top = 0;
                if (top + pictureBox.Height < panel.Height)
                    top = panel.Height - pictureBox.Height;
            }
            pictureBox.Left = left;
            pictureBox.Top = top;
        }
        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (zoom > 1.0)
                RetreatZoom(3);
            else
                ApplyZoom(3);
        }

        private void TreeView_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void TreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            changeContainer = true;
        }
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!changeContainer)
                return;
            try
            {
                var container = ((MyTreeNode)treeView.SelectedNode).Container;
                changeContainerTreeView(container);
                UpdateSelectedNode(container);
            }
            catch { }
            changeContainer = false;
        }
    }
}
