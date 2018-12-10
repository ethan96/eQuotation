using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eQuotation.Utility
{
    public class Logger
    {
        private string TextFile { get; set; }

        public Logger(string txtFile)
        {
            this.TextFile = txtFile;
        }

        public void Write(string text, bool isTime = true)
        {
            if (isTime)
                File.AppendAllText(this.TextFile, string.Format("{0:yyyyMMdd HH:mm:ss.fff}\t{1}\r\n", DateTime.Now, text));
            else
                File.AppendAllText(this.TextFile, text);
        }
    }
}
