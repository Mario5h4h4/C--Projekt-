using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace CSharpProjekt
{
    public partial class Form1 : Form
    {
        private PictureBox[] pbHot;
        private const int elemCount = 18;
        public Form1()
        {
            InitializeComponent();
            this.tabbed_views.SelectedIndexChanged += new System.EventHandler(this.tabbed_views_SelectedIndexChanged);
            pbHot = new PictureBox[elemCount];
            for (int i = 0; i < elemCount; i++) {
                pbHot[i] = new PictureBox();
                pbHot[i].MinimumSize = new Size((this.hot_tab_page.Width / 6) - 10, (this.hot_tab_page.Height / 3) - 10);
                this.hot_flow_layout.Controls.Add(pbHot[i]);

            }
        }

        private void hot_flow_layout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabbed_views_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabbed_views.SelectedIndex)
            {
                case 0:
                    ContentWrapper cw = new ContentWrapper(0, elemCount);
                    ThreadAdapter ta = new ThreadAdapter(cw);
                    PictureBoxFiller pbf = new PictureBoxFiller(ta, pbHot);
                    Thread t = new Thread(new ThreadStart(ta.getHot));
                    t.Start();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default: 
                    break;
            }
        }
    }
}
