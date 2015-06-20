using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt
{

    class ContentWrapper
    {
        public List<DAImage> imageList;
        public List<string> filepaths = new List<string>();
        public int offset;
        public int limit;
        public ContentWrapper(int o, int l)
        {
            offset = o;
            limit = l;
        }

        public void setOffsetAndLimit(int o, int l)
        {
            offset = o;
            limit = l;
        }

        public List<DAImage> getImageList()
        {
            return imageList;
        }
    }
}
