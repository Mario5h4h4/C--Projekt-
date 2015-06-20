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
        private PictureBox[] PBs;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tAdapt">The ThreadAdapter which will be invoking the Event downloadFinished</param>
        /// <param name="pbArray">The PictureBoxes to be filled</param>
        public PictureBoxFiller(ThreadAdapter tAdapt, PictureBox[] pbArray)
        {
            this.ta = tAdapt;
            this.PBs = pbArray;
            this.ta.downLoadFinished += this.DownloadFinished;
        }

        private void DownloadFinished(object sender, EventArgs e)
        {
            //To avoid error, check if 'i' is smaller than the length of each Array,
            //and limit is the max amount if images we're showing at once (should always be 18)
            //It might happen that filepaths.Length is smaller than 18, due to non-downloadable content
            for (int i = ta.conWrap.offset; i < ta.conWrap.filepaths.ToArray().Length && i < PBs.Length && i < ta.conWrap.limit; i++)
            {
                if (ta.conWrap.filepaths.ToArray()[i] != null)
                {
                    PBs[i].ImageLocation = ta.conWrap.filepaths.ToArray()[i];
                }
            }
        }
    }
}
