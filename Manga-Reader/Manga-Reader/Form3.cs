using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class frmShortcuts : Form
    {
        Reader reader;
        public frmShortcuts(Reader r)
        {
            InitializeComponent();
            reader = r;
        }

        public Label GetLabel(string name, Point p)
        {
            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font("UD Digi Kyokasho NK-R", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            lbl.Location = p;
            lbl.Name = name;
            lbl.TabIndex = 0;
            lbl.Text = name;

            return lbl;
        }

        private void FrmShortcuts_Load(object sender, EventArgs e)
        {
            var pnl = pnlShortcuts.Controls;
            var point = new Point(0, 0);
            var padding = 10;

            foreach(Shortcut s in reader.Shortcuts.Values)
            {
                point.X = 0;
                var lblKeys = GetLabel(s.Keys, point);
                lblKeys.Text = s.Keys+":";
                pnl.Add(lblKeys);
                point.X += lblKeys.Width + padding;

                var lblHelp = GetLabel(s.Keys, point);
                lblHelp.Text = s.Help;
                pnl.Add(lblHelp);
                point.X += lblHelp.Width + padding;

                point.Y += lblHelp.Height + 5;
            }
        }
    }
}
