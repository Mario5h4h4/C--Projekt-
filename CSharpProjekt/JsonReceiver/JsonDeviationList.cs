using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt.JsonReceiver
{
    /// <summary>
    /// We usually get a List of Deviations from the API, therefore this class to deserialize into
    /// </summary>
    class JsonDeviationList
    {
        public bool has_more;
        public int next_offset;
        public JsonDeviation[] results;
    }
}
