using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpProjekt
{
    public partial class ImageForm : Form
    {
        private Form1 parent;
        public DAImage dai = null;
        public bool PicIsLocal = false;
        internal ImageForm(Form1 parent, DAImage dai)
        {
            this.parent = parent;
            this.dai = dai;
            InitializeComponent();
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            this.reload_pictureBox();
        }

        internal void reload_pictureBox()
        {
            //If the picture isn't local, download it and show it
            if (!PicIsLocal)
            {
                DAInterface.Instance.downloadImageTemp(dai);
                this.pictureBox1.Size = new Size(dai.width, dai.height);
                this.pictureBox1.LoadAsync("./imgTemp/" + dai.d_ID + dai.filetype);
            }
            //else simply show it in the PictureBoc
            else
            {
                this.pictureBox1.Size = new Size(dai.width, dai.height);
                this.pictureBox1.LoadAsync(dai.image_path);
            }
        }
    }
}
