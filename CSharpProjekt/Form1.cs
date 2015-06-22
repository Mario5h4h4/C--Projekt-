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
        private ImageForm imgForm = null;
        private DAImage curBigImage = null;

        private ContextMenu dropDownMenu = new ContextMenu();
        private DAPictureBox[][] picBoxes = new DAPictureBox[3][];
        private const int elemCount = 18;
        private ContentWrapper[] ConWraps = new ContentWrapper[3];
        private ThreadAdapter[] thrAdapter = new ThreadAdapter[3];
        private PictureBoxFiller[] picBoxFiller = new PictureBoxFiller[3];
        private ThreadStart[] thrStarts = new ThreadStart[3];
        private int[] offsets = { 0, 0, 0 };
        private Dictionary<int, FlowLayoutPanel> LayoutMap = new Dictionary<int, FlowLayoutPanel>();
        public Form1()
        {
            InitializeComponent();
            this.tabbed_views.SelectedIndexChanged += new System.EventHandler(this.tabbed_views_SelectedIndexChanged);

            //Initializing all of the Arrays above here
            LayoutMap[0] = this.hot_flow_layout;
            LayoutMap[1] = this.newest_flow_layout;
            LayoutMap[2] = this.search_flow_layout;

            dropDownMenu.MenuItems.Add("view Full-sized Image");
            dropDownMenu.MenuItems.Add("test2");
            dropDownMenu.MenuItems[0].Click += new EventHandler(clickViewImage);

            for (int i = 0; i < 3; i++)
            {
                picBoxes[i] = new DAPictureBox[elemCount];
                ConWraps[i] = new ContentWrapper(offsets[i], elemCount);
                thrAdapter[i] = new ThreadAdapter(ConWraps[i]);
                for (int j = 0; j < elemCount; j++)
                {
                    picBoxes[i][j] = new DAPictureBox();
                    picBoxes[i][j].Size = new Size((LayoutMap[i].Width / 6) - 8, (LayoutMap[i].Height / 3) - 8);
                    picBoxes[i][j].ContextMenu = dropDownMenu;
                    LayoutMap[i].Controls.Add(picBoxes[i][j]);
                }
                    picBoxFiller[i] = new PictureBoxFiller(thrAdapter[i], picBoxes[i]);
            }
            thrStarts[0] = new ThreadStart(thrAdapter[0].getHot);
            thrStarts[1] = new ThreadStart(thrAdapter[1].getNew);
            thrStarts[2] = new ThreadStart(thrAdapter[2].getSearch);

            textBox1.KeyPress += textBox1_KeyPress;

        }

        void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.button1_Click(sender, EventArgs.Empty);
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
            //Solution with many Arrays, but less temporary Objects, probably making the program faster
            if (tabbed_views.SelectedIndex < 2)
            {
                Thread t = new Thread(thrStarts[tabbed_views.SelectedIndex]);
                t.Start();
            }

            /*
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
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConWraps[2].queryTerm = this.removeWhitespace(textBox1.Text);
            Thread t = new Thread(thrStarts[2]);
            t.Start();
        }

        //simply removes all Whitespace. Needed for getImagesByTag:
        //Tags do not contain Whitespace Characters
        private string removeWhitespace(string input)
        {
            return new string(input.Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        private void clickViewImage(object sender, EventArgs e)
        {
            DAPictureBox dapb = ((MenuItem)sender).Parent.GetContextMenu().SourceControl as DAPictureBox;
            curBigImage = dapb.dai;
            if (imgForm == null)
            {
                imgForm = new ImageForm(this, curBigImage);
                DAInterface.Instance.downloadImageTemp(curBigImage);
                new Thread(new ThreadStart(loadImageForm)).Start();
            }
            else
            {
                //To do this, we'll need to use ImageForm.Invoke
                //imgForm.dai = curBigImage;
                setBigImageCallback setimg = new setBigImageCallback(setBigImage);
                imgForm.Invoke(setimg);
            }
        }

        delegate void setBigImageCallback();

        private void setBigImage()
        {
            imgForm.dai = curBigImage;
            imgForm.reload_pictureBox();
        }

        internal void disposeChild()
        {
            imgForm = null;
        }

        //tested if we could create a new Form from within this one.
        //succeeded with new Thread
        /*
        private void Form1_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(loadImageForm)).Start();
        }
        */
        //experiments with invoke
        /*private delegate void foo();
        private delegate void oof(Form f);
        private void setVisible(Form f)
        {
            f.Visible = true;
        }*/
        private void loadImageForm()
        {
            //this.AddOwnedForm(imgform);
            Application.Run(imgForm);
            //foo f = new foo(this.OwnedForms[0].Activate);
            //oof o = new oof(this.setVisible);
            //imgform.Invoke(f);
            //imgform.Invoke(o, new object[] { imgform });
        }

    }
}
