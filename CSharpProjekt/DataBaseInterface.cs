using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace CSharpProjekt
{
    class DataBaseInterface
    {
        private static DataBaseInterface instance;
        public static DataBaseInterface Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataBaseInterface();
                return instance;
            }
        }
        private DataSet dsImages;
        private DataTable mTable;
        private ReaderWriterLockSlim readWriteLock;
        private DataBaseInterface()
        {
            //setting up directories needed for the program
            if (!Directory.Exists("./thumbnails"))
                Directory.CreateDirectory("./thumbnails");
            if (!Directory.Exists("./images"))
                Directory.CreateDirectory("./images");
            if (!Directory.Exists("./imgTemp"))
                Directory.CreateDirectory("./imgTemp");
            if (!Directory.Exists("./metadata"))
                Directory.CreateDirectory("./metadata");
            //setting up the DataSet images.db with the DataTable images
            dsImages = new DataSet("images.db");
            mTable = new DataTable("images");
            dsImages.Tables.Add(mTable);
            mTable.Columns.Add(new DataColumn("deviation_id"));
            mTable.Constraints.Add("primary key", dsImages.Tables[0].Columns[0], true);
            mTable.Columns.Add(new DataColumn("thumbnail"));
            mTable.Columns.Add(new DataColumn("image"));
            mTable.Columns.Add(new DataColumn("meta"));
            //if an the images.db file exists, info is read out of it, otherwise an new one will be created
            if (!File.Exists("./images.db"))
                dsImages.WriteXml(new FileStream("./images.db", FileMode.Create));
            else
            {
                dsImages.ReadXml(new FileStream("./images.db", FileMode.Open));
            }
            readWriteLock = new ReaderWriterLockSlim();
        }

        //initializes the single object of this class
        //called before Application.Run(Form1)
        public static void init()
        {
            instance = new DataBaseInterface();
        }

        /// <summary>
        /// Adds a Row to the DataTable "images"
        /// </summary>
        /// <param name="dID">column deviation_id</param>
        /// <param name="thumb">column thumbnail</param>
        /// <param name="img">column image</param>
        /// <param name="md">column meta (metadata)</param>
        public void AddRow(string dID, string thumb, string img, string md)
        {
            readWriteLock.EnterWriteLock();
            DataRow tmp = mTable.NewRow();
            tmp["deviation_id"] = dID;
            tmp["thumbnail"] = thumb;
            tmp["image"] = img;
            tmp["meta"] = md;
            mTable.Rows.Add(tmp);
            readWriteLock.ExitWriteLock();
        }

        //functions i thought would be useful, but werent used in the end
        public string getThumbnail(string dID) {
            readWriteLock.EnterReadLock();
            string ret = (string) mTable.Rows.Find(dID)["thumbnail"];
            readWriteLock.ExitReadLock();
            return ret;
        }

        public string getImage(string dID)
        {
            readWriteLock.EnterReadLock();
            string ret = (string)mTable.Rows.Find(dID)["image"];
            readWriteLock.ExitReadLock();
            return ret;
        }

        public string getMetadata(string dID)
        {
            readWriteLock.EnterReadLock();
            string ret = (string)mTable.Rows.Find(dID)["meta"];
            readWriteLock.ExitReadLock();
            return ret;
        }

        /// <summary>
        /// returns the Row with the right deviation_id
        /// </summary>
        /// <param name="dID">deviation_id</param>
        /// <returns>follows the convention: {deviation_id, thumbnail, image, meta}</returns>
        public string[] getRow(string dID)
        {
            readWriteLock.EnterReadLock();
            string[] ret = new string[4];
            ret[0] = (string)mTable.Rows.Find(dID)["deviation_id"];
            ret[1] = (string)mTable.Rows.Find(dID)["thumbnail"];
            ret[2] = (string)mTable.Rows.Find(dID)["image"];
            ret[3] = (string)mTable.Rows.Find(dID)["meta"];
            readWriteLock.ExitReadLock();
            return ret;
        }

        /// <summary>
        /// Gets a List of Strings, each containing a deviation_id
        /// </summary>
        /// <returns>List of deviation IDs</returns>
        public List<string> getAllIDs()
        {
            readWriteLock.EnterReadLock();
            List<string> tmplist = new List<string>();
            foreach (DataRow row in mTable.Rows)
            {
                tmplist.Add((string) row["deviation_id"]);
            }
            return tmplist;
        }

        //saves the changes made to the database
        //this method is called in Form1.Dispose
        public void commit()
        {
            dsImages.WriteXml(new FileStream("./images.db", FileMode.Create));
        }
    }
}
