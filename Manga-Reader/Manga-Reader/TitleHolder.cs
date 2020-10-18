using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace Manga_Reader
{
    public partial class TitleHolder : UserControl
    {
        Color main = Color.White, hover = Color.FromArgb(255, 200, 200, 200);
        const float ZOOM = 0.1f;
        Book book;
        Library library;
        TranspCtrl transparentCtrl;
        bool mouseInside;
        int initialHeight, deltaHeight, initialLeft, deltaLeft, initialTop, deltaTop;

        public Book Book { get => book; }

        public TitleHolder(Library library, Book book)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            container.EnableDoubleBuferring();
            lbName.EnableDoubleBuferring();
            lbPath.EnableDoubleBuferring();

            SetupEvents();

            this.book = book;
            this.library = library;
        }

        public TitleHolder()
        {
            InitializeComponent();
        }

        private void SetupEvents()
        {
            container.DoubleClick += ClickTitle;
            lbName.Click += ClickTitle;

            container.MouseEnter += MouseEnterEvent;
            container.MouseLeave += MouseLeaveTitleEvent;
            pbPicture.MouseEnter += MouseEnterEvent;
            pbPicture.MouseLeave += MouseLeaveTitleEvent;
            lbName.MouseEnter += MouseEnterEvent;
            lbName.MouseLeave += MouseLeaveTitleEvent;
        }

        private void TitleHolder_Load(object sender, EventArgs e)
        {
            lbName.Text = book.Name;
            lbPath.Text = book.Path;
            lbLastOpened.Text += book.LastOpened.ToLongDateString();
            pbPicture.Image = book.Image;

            lbName.BackColor = Color.Transparent;
            lbPath.BackColor = Color.Transparent;
            container.BackColor = Color.Transparent;
            lbLastOpened.BackColor = Color.Transparent;

            initialHeight = Height;
            initialLeft = Location.X;
            initialTop = Location.Y;

            ChangeColor(main);
        }

        private void Expand()
        {
            SizeF scaleSize = new SizeF(1 + ZOOM, 1 + ZOOM);
            container.Scale(scaleSize);

            Location = new Point((int)(initialLeft - Width * ZOOM / 2), Math.Max((int)(initialTop - Height * ZOOM / 2), 0));
            Height = Math.Min((int)(Height * scaleSize.Height), Parent.Height);

            deltaLeft = Location.X - initialLeft;
            deltaTop = Location.Y - initialTop;
            deltaHeight = Height - initialHeight;

            var parentChildren = new List<TitleHolder>();
            foreach (Control c in Parent.Controls)
            {
                if (c is TitleHolder)
                    parentChildren.Add(c as TitleHolder);
            }

            try
            {
                var nextElem = parentChildren.ElementAt(parentChildren.IndexOf(this) + 1);
                nextElem.Location = new Point(nextElem.Location.X, nextElem.Location.Y + deltaHeight + deltaTop);
            }
            catch { }
            try
            {
                var previousElem = parentChildren.ElementAt(parentChildren.IndexOf(this) - 1);
                previousElem.Location = new Point(previousElem.Location.X, previousElem.Location.Y + deltaTop);
            }
            catch { }
        }
        private void Retreat()
        {
            container.Scale(new SizeF(1 / (1 + ZOOM), 1 / (1 + ZOOM)));
            Height = initialHeight;
            Location = new Point(initialLeft, initialTop);

            var parentChildren = new List<TitleHolder>();
            foreach (Control c in Parent.Controls)
            {
                if (c is TitleHolder)
                    parentChildren.Add(c as TitleHolder);
            }

            try
            {
                var nextElem = parentChildren.ElementAt(parentChildren.IndexOf(this) + 1);
                nextElem.Location = new Point(nextElem.Location.X, nextElem.Location.Y - deltaHeight - deltaTop);
            }
            catch { }
            try
            {
                var previousElem = parentChildren.ElementAt(parentChildren.IndexOf(this) - 1);
                previousElem.Location = new Point(previousElem.Location.X, previousElem.Location.Y - deltaTop);
            }
            catch { }
        }
        void MouseLeaveTitleEvent(object sender, EventArgs e)
        {
            MouseLeave();
        }
        void MouseLeave()
        {
            if (this.ClientRectangle.Contains(PointToClient(Control.MousePosition)))
                return;

            ChangeColor(main);
            Retreat();

            btnDelete.Visible = !mouseInside;
            btnEdit.Visible = !mouseInside;

            mouseInside = false;
        }

        void MouseEnterEvent(object sender, EventArgs e)
        {
            MouseEntered();
        }
        void MouseEntered()
        {
            if (mouseInside)
                return;

            UnselectRest();
            ChangeColor(hover);
            Expand();

            btnDelete.Visible = !mouseInside;
            btnEdit.Visible = !mouseInside;

            mouseInside = true;
        }

        private void UnselectRest()
        {
            foreach (Control c in Parent.Controls)
            {
                if (c is TitleHolder)
                {
                    var th = c as TitleHolder;
                    th.ChangeColor(main);

                    if (th.Height != th.initialHeight)
                        th.Retreat();
                }
            }
        }

        private void ChangeColor(Color c)
        {
            BackColor = c;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            new frmSetup(book).ShowDialog();
            book.SaveToFile();
            library.Refresh();

            lbName.Text = book.Name;
            lbPath.Text = book.Path;
            lbLastOpened.Text += book.LastOpened.ToLongDateString();
            pbPicture.Image = book.Image;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!ConfirmDeletion())
                return;

            library.Delete(book);
            book.Delete();

            library.Refresh();
        }

        private bool ConfirmDeletion()
        {
            DialogResult result = MessageBox.Show("This operation is irreversible. Continue?", "Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            return result == DialogResult.OK;
        }

        private void ClickTitle(object sender, EventArgs e)
        {
            this.book.LastOpened = DateTime.Now;
            var frmReader = new frmMangaReader(book);
            frmReader.ShowDialog();
        }

        public override string ToString()
        {
            return book.ToString();
        }
    }

    public class TranspCtrl : Control
    {
        public TranspCtrl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }
    }

    public static class Extensions
    {
        public static void EnableDoubleBuferring(this Control control)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(control, true, null);
        }
    }

    /*class ColorChanger
    {
        System.Windows.Forms.Timer timer;
        UserControl component;
        Color start, end, delta;
        int signalA, signalR, signalG, signalB;

        const int ticks = 64;
        public ColorChanger(UserControl comp, Color start, Color end)
        {
            this.component = comp;
            this.start = start;
            this.end = end;
            delta = Color.FromArgb(Math.Abs(end.A - start.A), Math.Abs(end.R - start.R), Math.Abs(end.G - start.G), Math.Abs(end.B - start.B));
            signalA = end.A - start.A > 0 ? 1 : -1;
            signalR = end.R - start.R > 0 ? 1 : -1;
            signalG = end.G - start.G > 0 ? 1 : -1;
            signalB = end.B - start.B > 0 ? 1 : -1;
        }

        public void SmoothTransition()
        {
            for (int counter = 0; counter < ticks; counter++)
            {
                int diffA = (int)((double)delta.A / ticks * counter) * signalA;
                int diffR = (int)((double)delta.R / ticks * counter) * signalR;
                int diffG = (int)((double)delta.G / ticks * counter) * signalG;
                int diffB = (int)((double)delta.B / ticks * counter) * signalB;

                Color color = Color.FromArgb(start.A + diffA, start.R + diffR, start.G + diffG, start.B + diffB);

                component.BackColor = color;

                Application.DoEvents();
                Thread.Sleep(1);
            }
        }
    }*/
}
