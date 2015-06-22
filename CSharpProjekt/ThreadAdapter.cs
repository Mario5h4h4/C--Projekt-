using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt

{
    delegate void DownloadFinishedHandler(object sender, DownloadFinishedEventArgs e);

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
        private void onDownloadFinished(DownloadFinishedEventArgs e)
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
                //maybe use AddAll instead?
                conWrap.imageList = DAInterface.Instance.getHotImages(conWrap.offset, conWrap.limit);
            }
            catch (System.Net.WebException we)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + we.Message);
                Console.Error.WriteLine(we.StackTrace);
            }
            //We download the small image files (thumbnails) here.
            for (int i = conWrap.offset; i < conWrap.offset + conWrap.imageList.Count; i++)
            {
                DAImage dai = conWrap.imageList.ToArray()[i];
                try
                {
                    string path = DAInterface.Instance.downloadThumbnail(dai);
                    conWrap.filepaths.Add(path);
                    //rather than sending an event when all images were downloaded, an event is fired
                    //after each image, so user may see progress even with a slow connection
                    onDownloadFinished(new DownloadFinishedEventArgs(i - conWrap.offset, dai));
                }
                catch (System.Net.WebException we)
                {
                    Console.Error.WriteLine(we.StackTrace);
                }
            }
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
            //WebException might happen in getNewestImages
            try
            {
                //maybe use AddAll instead? look at getHot
                conWrap.imageList = DAInterface.Instance.getNewestImages(conWrap.offset, conWrap.limit);
            }
            catch (System.Net.WebException we)
            {
               // System.Windows.Forms.MessageBox.Show("Error: " + we.Message);
                Console.Error.WriteLine(we.StackTrace);
            }

            //We download the small image files (thumbnails) here.
            for (int i = conWrap.offset; i < conWrap.offset + conWrap.imageList.Count; i++)
            {
                DAImage dai = conWrap.imageList.ToArray()[i];
                try
                {
                    string path = DAInterface.Instance.downloadThumbnail(dai);
                    conWrap.filepaths.Add(path);
                    //rather than sending an event when all images were downloaded, an event is fired
                    //after each image, so user may see progress even with a slow connection
                    onDownloadFinished(new DownloadFinishedEventArgs(i - conWrap.offset, dai));
                }
                catch (System.Net.WebException we)
                {
                    Console.Error.WriteLine(we.StackTrace);
                }
            }
        }

        /// <summary>
        /// the wrapper function for DeviantArtInterface.getImagesByTag. Also downloads the Thumbnails.
        /// </summary>
        public void getSearch()
        {
            //If authentication goes wrong, we probably have no connection, or our keys for the API are bad
            if (!DAInterface.Instance.checkAuthentication())
                if (!DAInterface.Instance.authenticate())
                {
                    conWrap.imageList = null;
                    throw new TimeoutException();
                }
            //WebException might happen in getImagesByTag
            try
            {
                //If we were to extend the program so a user could view more deviations by pressing a "next" button,
                //imageList.AddAll would be more appropriate
                conWrap.imageList = DAInterface.Instance.getImagesByTag(conWrap.queryTerm, conWrap.offset, conWrap.limit);
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                System.Windows.Forms.MessageBox.Show("No images were found");
                return;
            }
            catch (System.Net.WebException we)
            {
                // System.Windows.Forms.MessageBox.Show("Error: " + we.Message);
                Console.Error.WriteLine(we.StackTrace);
            }

            //We download the small image files (thumbnails) here.
            for (int i = conWrap.offset; i < conWrap.offset + conWrap.imageList.Count; i++)
            {
                DAImage dai = conWrap.imageList.ToArray()[i];
                try
                {
                    string path = DAInterface.Instance.downloadThumbnail(dai);
                    conWrap.filepaths.Add(path);
                    //rather than sending an event when all images were downloaded, an event is fired
                    //after each image, so user may see progress even with a slow connection
                    onDownloadFinished(new DownloadFinishedEventArgs(i - conWrap.offset, dai));
                }
                catch (System.Net.WebException we)
                {
                    Console.Error.WriteLine(we.StackTrace);
                }
            }
        }

        /// <summary>
        /// gets the ID of every available Deviation, and loads the first conWrap.limit DAImages into the program,
        /// and continues to add the thumbnails to the picture boxes
        /// </summary>
        public void getLocal()
        {
            List<string> IDs = DataBaseInterface.Instance.getAllIDs();
            for (int i = conWrap.offset; i < IDs.Count && i < conWrap.offset + conWrap.limit; i++)
            {
                DAImage dai = DAInterface.Instance.deserializeImgInfo(IDs[i]);
                conWrap.imageList = new List<DAImage>();
                conWrap.imageList.Add(dai);
                conWrap.filepaths.Add(dai.thumbnail_path);
                onDownloadFinished(new DownloadFinishedEventArgs(i - conWrap.offset, dai));
            }
        }
    }
}
