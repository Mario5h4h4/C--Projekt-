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
using System.Net;

namespace CSharpProjekt
{
    public partial class Form1 : Form
    {
        private PictureBox[] pbHot;
        private const int elemCount = 18;
        private ContentWrapper[] ConWraps = new ContentWrapper[3];
        private int[] offsets = { 0, 0, 0 };
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
            for (int i = 0; i < ConWraps.Length; i++)
                ConWraps[i] = new ContentWrapper(offsets[i], elemCount);
        }

        private void hot_flow_layout_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// This method is called when the user selects another Tab that the current one.
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">EventArgs, not used here</param>
        private void tabbed_views_SelectedIndexChanged(object sender, EventArgs e)
        {
            //currently thinking about making an ThreadAdapter Array instead of making a new Object
            //at every call. Should be possible thanks to using ContentWrapper and the behavior of references.
            //Also applicable for pbHot, PictureBoxFiller and even ThreadStart.
            switch (tabbed_views.SelectedIndex)
            {
                case 0:
                    ThreadAdapter ta = new ThreadAdapter(ConWraps[tabbed_views.SelectedIndex]);
                    PictureBoxFiller pbf = new PictureBoxFiller(ta, pbHot);
                    Thread t = new Thread(new ThreadStart(ta.getHot));
                    try
                    {
                        t.Start();
                    }
                    catch (TimeoutException te)
                    {
                        MessageBox.Show("No connection could be etablished:\n" + te.Message);
                    }
                    catch (WebException we)
                    {
                        MessageBox.Show("An Error occured while downloading metadata:\n" + we.Message);
                    }
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
