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
        public Form1()
        {
            InitializeComponent();
            this.tabbed_views.SelectedIndexChanged += new System.EventHandler(this.tabbed_views_SelectedIndexChanged);
            pbHot = new PictureBox[16];
            for (int i = 0; i < 16; i++) {
                pbHot[i] = new PictureBox();
                pbHot[i].MinimumSize = new Size(this.hot_tab_page.Width / 4 - 1, this.hot_tab_page.Height / 4 - 1);
                this.hot_flow_layout.Controls.Add(pbHot[i]);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void hot_flow_layout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabbed_views_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabbed_views.SelectedIndex)
            {
                case 0:
                    ContentWrapper cw = new ContentWrapper(0, 16);
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
