using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt
{
    /// <summary>
    /// EventArgs for OnDownloadFinished
    /// Used for filling the PictureBoxes with images
    /// </summary>
    class DownloadFinishedEventArgs : EventArgs
    {
        public int picBoxIndex { get; set; }
        public DAImage dai { get; set; }

        public DownloadFinishedEventArgs(int i, DAImage d)
        {
            this.picBoxIndex = i;
            this.dai = d;
        }
    }
}
