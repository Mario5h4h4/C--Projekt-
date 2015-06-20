using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpProjekt
{
    class PictureBoxFiller
    {
        private ThreadAdapter ta;
        private PictureBox[] PBs;

        public PictureBoxFiller(ThreadAdapter tAdapt, PictureBox[] pbArray)
        {
            this.ta = tAdapt;
            this.PBs = pbArray;
            this.ta.downLoadFinished += this.DownloadFinished;
        }

        private void DownloadFinished(object sender, EventArgs e)
        {
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
