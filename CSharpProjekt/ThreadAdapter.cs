using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt

{
    public delegate void DownloadFinishedHandler(object sender, EventArgs e);

    /// <summary>
    /// For threads, we need a void foo(void); function.
    /// We achieve this by wrapping the functions of out DAInterface, and storing variables/results
    /// in the ContentWrapper, which this class gets from Form1, making the usable there
    /// </summary>
    class ThreadAdapter
    {
        public event DownloadFinishedHandler downLoadFinished;
        internal ContentWrapper conWrap
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor of ThreadAdapter
        /// </summary>
        /// <param name="cw">ContentWrapper: containing an imageList, filepathList, the offset and limit needed</param>
        public ThreadAdapter(ContentWrapper cw)
        {
            conWrap = cw;
        }

        /// <summary>
        /// fire event if not null
        /// </summary>
        /// <param name="e">EventArgs. not really needed here</param>
        private void onDownloadFinished(EventArgs e)
        {
            if (downLoadFinished != null)
                downLoadFinished(this, e);
        }

        /// <summary>
        /// the wrapper function for DAInterface.getHotImages, as well as DAInterface.downloadThumbnail
        /// </summary>
        public void getHot()
        {
            //If authentication goes wrong, we probably have no connection, or our keys for the API are bad
            if (!DAInterface.Instance.checkAuthentication())
                if (!DAInterface.Instance.authenticate())
                {
                    conWrap.imageList = null;
                    throw new TimeoutException();
                }
            //WebException might happen in getHotImages
            try
            {
                conWrap.imageList = DAInterface.Instance.getHotImages(conWrap.offset, conWrap.limit);
            }
            catch (System.Net.WebException we)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + we.Message);
                Console.Error.WriteLine(we.StackTrace);
            }
            //We download the small image files (thumbnails) here.
            foreach (DAImage dai in conWrap.imageList) {
                conWrap.filepaths.Add(DAInterface.Instance.downloadThumbnail(dai));
            }
            //This event notifies that all Images are downloaded,
            //this should lead to the PictureBoxes being filled
            onDownloadFinished(EventArgs.Empty);
        }

        /// <summary>
        /// the wrapper function for DAInterface.getNewestImages, as well as DAInterface.downloadThumbnail
        /// </summary>
        public void getNew()
        {
            //If authentication goes wrong, we probably have no connection, or our keys for the API are bad
            if (!DAInterface.Instance.checkAuthentication())
                if (!DAInterface.Instance.authenticate())
                {
                    conWrap.imageList = null;
                    throw new TimeoutException();
                }
            //WebException might happen in getHotImages
            try
            {
                conWrap.imageList = DAInterface.Instance.getNewestImages(conWrap.offset, conWrap.limit);
            }
            catch (System.Net.WebException we)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + we.Message);
                Console.Error.WriteLine(we.StackTrace);
            }
            //We download the small image files (thumbnails) here.
            foreach (DAImage dai in conWrap.imageList)
            {
                conWrap.filepaths.Add(DAInterface.Instance.downloadThumbnail(dai));
            }
            //This event notifies that all Images are downloaded,
            //this should lead to the PictureBoxes being filled
            onDownloadFinished(EventArgs.Empty);
        }

        public void getSearch()
        {

        }
    }
}
