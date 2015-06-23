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
        //used for showing the big version of the Image
        private ImageForm imgForm = null;
        private DAImage curBigImage = null;

        //used for the gallery (loc stand for local)
        private DAPictureBox[] locPicBoxes = new DAPictureBox[elemCount];
        //we dont have a "next" or "prev" button, so it isnt actually needed
        private int locOffset = 0;
        private ContentWrapper locConWrap;
        private ThreadStart locThrStart;
        private ThreadAdapter locThrAdapter;
        private ContextMenu locDropDown = new ContextMenu();
        private PictureBoxFiller locPBFiller;

        //used for the tabs Hot, Newest, Search
        private ContextMenu dropDownMenu = new ContextMenu();
        private DAPictureBox[][] picBoxes = new DAPictureBox[3][];
        //the amount of deviations shown at once is limited to 18: 3 rows with 6 items
        private const int elemCount = 18;
        private ContentWrapper[] ConWraps = new ContentWrapper[3];
        //all functions that needed to be adapted for use in Threads are in the ThreadAdapter class
        //multiple objects so the methods may be run with individual ContentWrapper
        private ThreadAdapter[] thrAdapter = new ThreadAdapter[3];
        //fills the PictureBoxes given to it with the results of the ThreadAdapter given to it, after the event
        //DownloadFinished is called
        private PictureBoxFiller[] picBoxFiller = new PictureBoxFiller[3];
        private ThreadStart[] thrStarts = new ThreadStart[3];
        //no buttons to change pages, so the offsets arent actually needed for now
        private int[] offsets = { 0, 0, 0 };
        //used only for initialization
        private Dictionary<int, FlowLayoutPanel> LayoutMap = new Dictionary<int, FlowLayoutPanel>();
        public Form1()
        {
            InitializeComponent();

            //manually added the EventHandler for SelectedIndexChanged
            this.tabbed_views.SelectedIndexChanged += new System.EventHandler(this.tabbed_views_SelectedIndexChanged);

            //Initializing all of the Arrays above here
            LayoutMap[0] = this.hot_flow_layout;
            LayoutMap[1] = this.newest_flow_layout;
            LayoutMap[2] = this.search_flow_layout;

            dropDownMenu.MenuItems.Add("view Full-sized Image");
            dropDownMenu.MenuItems.Add("save Image");
            dropDownMenu.MenuItems[0].Click += new EventHandler(clickViewImage);
            dropDownMenu.MenuItems[1].Click += new EventHandler(clickSaveImage);

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

            //Allows the search to start by pressing ENTER
            textBox1.KeyPress += textBox1_KeyPress;

            //gallery inits here
            locConWrap = new ContentWrapper(locOffset, elemCount);
            locDropDown.MenuItems.Add(new MenuItem("view full-sized Image"));
            locDropDown.MenuItems[0].Click += this.clickViewImage;
            locThrAdapter = new ThreadAdapter(locConWrap);
            locPBFiller = new PictureBoxFiller(locThrAdapter, locPicBoxes);
            locThrStart = new ThreadStart(locThrAdapter.getLocal);
            for (int i = 0; i < elemCount; i++)
            {
                locPicBoxes[i] = new DAPictureBox();
                locPicBoxes[i].Size = new Size((this.local_flow_layout.Width / 6) - 8, (this.local_flow_layout.Height / 3) - 8);
                locPicBoxes[i].ContextMenu = locDropDown;
                this.local_flow_layout.Controls.Add(locPicBoxes[i]);
            }
        }

        //checks if ENTER was pressed, if yes call button1_Click
        //In short, if a user types Enter after writing the query term, the search is started
        void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.button1_Click(sender, EventArgs.Empty);
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

            if (tabbed_views.SelectedIndex == 3)
            {
                Thread t = new Thread(locThrStart);
                t.Start();
            }
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

        //Note: There are more different media types on deviant art, not only images and GIFs.
        //If a bad image (e.g. text, flash animations...) is chosen to be downloaded,
        //the programm gets stuck somewhere and isnt responsive anymore
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clickSaveImage(object sender, EventArgs e)
        {
            DAPictureBox dapb = ((MenuItem)sender).Parent.GetContextMenu().SourceControl as DAPictureBox;
            DAInterface.Instance.downloadImage(dapb.dai);
            DAInterface.Instance.serializeImgInfo(dapb.dai);
            DataBaseInterface.Instance.AddRow(dapb.dai.d_ID, dapb.dai.thumbnail_path, dapb.dai.image_path, "./metadata/" + dapb.dai.d_ID + ".json");
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

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Trying to authenticate");
            if (!DAInterface.Instance.authenticate())
            {
                MessageBox.Show("Authentication failed.\nLoading local library now");
                tabbed_views.SelectedIndex = 3;
            }
            else
            {
                tabbed_views.SelectedIndex = 0;
                this.tabbed_views_SelectedIndexChanged(sender, e);
            }
        }

    }
}
