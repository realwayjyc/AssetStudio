using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class StreamInfo
    {
        private int cbLeft;
        public int CbLeft
        {
            get { return cbLeft; }
        }

        private int cbCurr;
        public int CbCurr
        {
            get { return cbCurr; }
        }

        private uint format;
        public uint Format
        {
            get { return format; }
        }

        public static StreamInfo Create(byte[] content, int objectOffset, ref int index)
        {
            StreamInfo ret = new StreamInfo();

            ret.cbLeft = BitConverter.ToInt32(content, index); index += 4;
            ret.cbCurr = BitConverter.ToInt32(content, index); index += 4;
            ret.format = BitConverter.ToUInt32(content, index); index += 4;
            return ret;
        }
    }
}
