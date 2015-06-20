using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt

{
    public delegate void DownloadFinishedHandler(object sender, EventArgs e);

    class ThreadAdapter
    {
        public event DownloadFinishedHandler downLoadFinished;
        internal ContentWrapper conWrap
        {
            get;
            set;
        }

        public ThreadAdapter(ContentWrapper cw)
        {
            conWrap = cw;
        }

        private void onDownloadFinished(EventArgs e)
        {
            if (downLoadFinished != null)
                downLoadFinished(this, e);
        }

        public void getHot()
        {
            if (!DAInterface.Instance.checkAuthentication())
                if (!DAInterface.Instance.authenticate())
                {
                    conWrap.imageList = null;
                    throw new TimeoutException();
                }
            try
            {
                conWrap.imageList = DAInterface.Instance.getHotImages(conWrap.offset, conWrap.limit);
            }
            catch (System.Net.WebException we)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + we.Message);
                Console.Error.WriteLine(we.StackTrace);
            }
            foreach (DAImage dai in conWrap.imageList) {
                conWrap.filepaths.Add(DAInterface.Instance.downloadThumbnail(dai));
            }
            onDownloadFinished(EventArgs.Empty);
        }
    }
}
