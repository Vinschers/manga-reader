using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class TitleHolder : UserControl
    {
        Color main = Color.White, hover = Color.FromArgb(255, 200, 200, 200);
        const float ZOOM = 0.1f;
        Book book;

        public TitleHolder(Book book)
        {
            InitializeComponent();

            this.book = book;

            lbName.Text = book.Name;
            lbPath.Text = book.Path;
            pbPicture.Image = book.Image;

            BackColor = main;
        }

        public TitleHolder()
        {
            InitializeComponent();
        }

        private void TitleHolder_Load(object sender, EventArgs e)
        {
            container.BackColor = main;

            var transparentControl = new TranspCtrl();
            transparentControl.Size = container.Size;

            transparentControl.MouseEnter += MouseEntered;
            transparentControl.MouseLeave += MouseLeave;
            transparentControl.Click += Click;

            transparentControl.Parent = container;
            transparentControl.BringToFront();

        }

        void MouseLeave(object sender, EventArgs e)
        {
            container.BackColor = main;
            Left = (int)(Left + Width / (ZOOM * 2));
            Scale(new SizeF(1/(1 + ZOOM), 1/(1 + ZOOM)));
        }

        void MouseEntered(object sender, EventArgs e)
        {
            container.BackColor = hover;
            Left = (int)(Left - Width * ZOOM/2);
            Scale(new SizeF(1 + ZOOM, 1 + ZOOM));
        }

        private void TitleHolder_ParentChanged(object sender, EventArgs e)
        {
            if (Parent == null)
                return;
            Width = Parent.Width - 10;
        }

        private void Click(object sender, EventArgs e)
        {
            var frmReader = new frmMangaReader(book.Reader);
            frmReader.ShowDialog();
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

    class ColorChanger
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
    }
}
