using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpProjekt
{

    /// <summary>
    /// The EventListener for the event downloadFinished.
    /// The PictureBoxes will be filled after all images were downloaded
    /// </summary>
    class PictureBoxFiller
    {
        private ThreadAdapter ta;
        private DAPictureBox[] PBs;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tAdapt">The ThreadAdapter which will be invoking the Event downloadFinished</param>
        /// <param name="pbArray">The PictureBoxes to be filled</param>
        public PictureBoxFiller(ThreadAdapter tAdapt, DAPictureBox[] pbArray)
        {
            this.ta = tAdapt;
            this.PBs = pbArray;
            this.ta.downLoadFinished += this.DownloadFinished;
        }

        private void DownloadFinished(object sender, DownloadFinishedEventArgs e)
        {
            PBs[e.picBoxIndex].LoadAsync(e.dai.thumbnail_path);
        }
    }
}
