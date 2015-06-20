using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProjekt
{
    public class DownloadFinishedEventArgs : EventArgs
    {
        public int picBoxIndex { get; set; }
        public string path { get; set; }

        public DownloadFinishedEventArgs(int i, string p)
        {
            this.picBoxIndex = i;
            this.path = p;
        }
    }
}
