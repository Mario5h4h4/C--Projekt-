﻿using System;
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

        /// <summary>
        /// If the Download is finished (or the local metadata is deserialized) load the image into the PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">DownloadFinishedEventArgs with DAImage and Index</param>
        private void DownloadFinished(object sender, DownloadFinishedEventArgs e)
        {
            PBs[e.picBoxIndex].dai = e.dai;
            PBs[e.picBoxIndex].LoadAsync(e.dai.thumbnail_path);

            //the code below was used to test the DataBaseInterface
            //DataBaseInterface.Instance.AddRow(e.dai.d_ID, e.dai.thumbnail_path, e.dai.image_path);
        }
    }
}
