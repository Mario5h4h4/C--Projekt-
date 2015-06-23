using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt
{
    /// <summary>
    /// A helper class, containing important informations for the ThreadAdapter to use.
    /// Easier than putting every single variable into the Contructor of ThreadAdapter
    /// </summary>
    class ContentWrapper
    {
        public List<DAImage> imageList;
        public List<string> filepaths = new List<string>();
        public int offset;
        public int limit;
        public string queryTerm;
        public ContentWrapper(int o, int l)
        {
            offset = o;
            limit = l;
        }

        //following functions are useful if we were to add a "next" button on Hot, Newest, Search, and Gallery

        /// <summary>
        /// sets new values for offset and limit
        /// </summary>
        /// <param name="o">offset</param>
        /// <param name="l">limit</param>
        public void setOffsetAndLimit(int o, int l)
        {
            offset = o;
            limit = l;
        }

        /// <summary>
        /// sets new values for offset, limit, and the search query
        /// </summary>
        /// <param name="o">offset</param>
        /// <param name="l">limit</param>
        /// <param name="query">query term</param>
        public void setOffsetLimitQuery(int o, int l, string query)
        {
            offset = o;
            limit = l;
            queryTerm = query;
        }
    }
}
