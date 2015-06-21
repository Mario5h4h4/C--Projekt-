using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using CSharpProjekt.JsonReceiver;

namespace CSharpProjekt
{
    //Class used for communication with DeviantArt as well as for downloading the images.
    sealed class DAInterface
    {
        //We are doing all communications with this class, on other words it may become quite big.
        //In short, we only want one instance of it to not waste ressources

        private static DAInterface instance;

        private DAInterface()
        {
        }

        public static DAInterface Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DAInterface();
                }
                return instance;
            }
        }

        private WebClient webclient = new WebClient();

        private JsonAccessToken accToken;

        private string accessToken;

        private string clientID = "2709";
        private string clientSecret = "620053b188693f93ba78a51093da65d5";

        private readonly object o = new object();

        /// <summary>
        /// authenticates this app using client_id and client_secret at deviantart, so we may get stuff from their api
        /// </summary>
        /// <returns>true on successful authentication</returns>
        public bool authenticate()
        {
            //no differentiation between exceptions. if anything happens, return false;
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://www.deviantart.com/oauth2/token?client_id=" + clientID + "&client_secret=" + clientSecret + "&grant_type=client_credentials");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream inStr = response.GetResponseStream();
                StreamReader strRead = new StreamReader(inStr);
                string str = strRead.ReadToEnd();
                accToken = JsonConvert.DeserializeObject<JsonAccessToken>(str);
            } catch (Exception)
            {
                return false;
            }
            //if either the accToken is null or the status says "error", go in there.
            if (accToken != null ? accToken.status == "error" : true)
            {
                return false;
            }
            accessToken = accToken.access_token;
            return true;
            //handle errors
            //DONE
        }

        /// <summary>
        /// checks if we still are authenticated on DeviantArt.com
        /// </summary>
        /// <returns>true if still authenticated</returns>
        public bool checkAuthentication()
        {
            JsonPlacebo placebo = null;
            //no differentiation between exceptions, if anything happens, just return false.
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp("https://www.deviantart.com/api/v1/oauth2/placebo");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader strRead = new StreamReader(response.GetResponseStream());
                placebo = JsonConvert.DeserializeObject<JsonPlacebo>(strRead.ReadToEnd());
            }
            catch (Exception)
            {
                return false;
            }
                if (placebo != null ? placebo.status == "error" : true)
                    return false;
                return true;
        }

        //currently using Downloads as tempfile folder. 
        //We should consider using AppData\Local\da_app\ (dir da_app will be made at installation)
        //C:%HOMEPATH%\AppData\Local\da_app
        //webclient.DownloadFile doesnt resolve the directory to an absolute one
        //->using relative directory for now
        private string tempdir = @"C:%HOMEPATH%\Downloads\";

        /// <summary>
        /// download the image located at dai.img_url
        /// </summary>
        /// <param name="dai">DAImage class containing core information about a deviation</param>
        public string downloadImage(DAImage dai)
        {
            string filedir = dai.d_ID + dai.filetype;
            while (webclient.IsBusy)
            {
                Thread.Sleep(20);
            }
            webclient.DownloadFileAsync(new Uri(dai.img_url), filedir);
            return filedir;
        }

        /// <summary>
        /// download the thumbnail located at dai.thumbnail_url
        /// </summary>
        /// <param name="dai">DAImage class containing core information about a deviation</param>
        public string downloadThumbnail(DAImage dai)
        {
            string filedir = "thumb_" + dai.d_ID + dai.filetype;
            lock (o)
            {
                while (webclient.IsBusy)
                {
                    Monitor.Wait(o);
                }
                webclient.DownloadFile(new Uri(dai.thumbnail_url), filedir);
                Monitor.Pulse(o);
            }
            return filedir;
        }

        /// <summary>
        /// gets the next 'limit' deviationobjects starting at offset 'offset' in category 'hot'
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit">max No. of deviations to get. max. 20</param>
        /// <returns>List of DAImages</returns>
        public List<DAImage> getHotImages(int offset, int limit)
        {
            if (limit < 0 || limit >20)
                throw new ArgumentOutOfRangeException();
            List<DAImage> ldai = new List<DAImage>(limit);
            HttpWebRequest request = WebRequest.CreateHttp("https://www.deviantart.com/api/v1/oauth2/browse/hot?offset=" 
                + offset + "&limit=" + limit + "&access_token=" + accessToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRead = new StreamReader(response.GetResponseStream());
            JsonDeviationList deviList = JsonConvert.DeserializeObject<JsonDeviationList>(strRead.ReadToEnd());
            foreach (JsonDeviation devi in deviList.results) 
            {
                if (devi.content != null)
                {
                    ldai.Add(new DAImage(devi.deviationid, devi.title,
                        devi.category, devi.author.username,
                        devi.content, devi.thumbs[0].src));
                }
            }
            return ldai;
            //TODO: error/exception handling.
            //Done at function call
        }

        /// <summary>
        /// gets the next 'limit' deviationobjects starting at offset 'offset' in category 'newest'
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit">max No. of deviations to get. max. 20</param>
        /// <returns>List of DAImages</returns>
        public List<DAImage> getNewestImages(int offset, int limit)
        {
            if (limit < 0 || limit > 20)
                throw new ArgumentOutOfRangeException();
            List<DAImage> ldai = new List<DAImage>(limit);
            HttpWebRequest request = WebRequest.CreateHttp("https://www.deviantart.com/api/v1/oauth2/browse/newest?offset="
                + offset + "&limit=" + limit + "&access_token=" + accessToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRead = new StreamReader(response.GetResponseStream());
            JsonDeviationList deviList = JsonConvert.DeserializeObject<JsonDeviationList>(strRead.ReadToEnd());
            foreach (JsonDeviation devi in deviList.results)
            {
                if (devi.content != null)
                {
                    ldai.Add(new DAImage(devi.deviationid, devi.title,
                        devi.category, devi.author.username,
                        devi.content, devi.thumbs[0].src));
                }
            }
            return ldai;
            //TODO: error/exception handling.
            //Done at function call
        }

        /// <summary>
        /// gets a list of DAImages related with the tag, starting at offset, with limit members
        /// </summary>
        /// <param name="tag">the tag required</param>
        /// <param name="offset"></param>
        /// <param name="limit">max. 20 deviations to get</param>
        /// <returns></returns>
        public List<DAImage> getImagesByTag(string tag, int offset, int limit)
        {
            if (limit < 0 || limit > 20)
                throw new ArgumentOutOfRangeException();
            List<DAImage> ldai = new List<DAImage>(limit);
            HttpWebRequest request = WebRequest.CreateHttp("https://www.deviantart.com/api/v1/oauth2/browse/tags?tag=" + tag + "&offset="
                + offset + "&limit=" + limit + "&access_token=" + accessToken);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader strRead = new StreamReader(response.GetResponseStream());
            JsonDeviationList deviList = null;
            deviList = JsonConvert.DeserializeObject<JsonDeviationList>(strRead.ReadToEnd());
            foreach (JsonDeviation devi in deviList.results)
            {
                if (devi.content != null)
                {
                    ldai.Add(new DAImage(devi.deviationid, devi.title,
                        devi.category, devi.author.username,
                        devi.content, devi.thumbs[0].src));
                }
            }
            return ldai;
            //TODO: error/exception handling.
            //Done at function call
        }
    }
}
