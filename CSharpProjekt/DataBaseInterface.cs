using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly object o = new object();
        private bool writing = false;
        private DataBaseInterface()
        {
            if (!Directory.Exists("./thumbnails"))
                Directory.CreateDirectory("./thumbnails");
            if (!Directory.Exists("./images"))
                Directory.CreateDirectory("./images");
            dsImages = new DataSet("images.db");
            mTable = new DataTable("images");
            dsImages.Tables.Add(mTable);
            mTable.Columns.Add(new DataColumn("deviation_id"));
            mTable.Constraints.Add("primary key", dsImages.Tables[0].Columns[0], true);
            mTable.Columns.Add(new DataColumn("thumbnail"));
            mTable.Columns.Add(new DataColumn("image"));
            if (!File.Exists("./images.db"))
                dsImages.WriteXml(new FileStream("./images.db", FileMode.Create));
            else
            {
                dsImages.ReadXml(new FileStream("./images.db", FileMode.Open));
            }
        }

        public static void init()
        {
            instance = new DataBaseInterface();
        }
    }
}
