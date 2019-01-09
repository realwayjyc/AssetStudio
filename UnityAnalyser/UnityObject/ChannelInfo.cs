using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class ChannelInfo
    {
        private byte stream;
        public byte Stream
        {
            get { return stream; }
        }

        private byte offset;
        public byte Offset
        {
            get { return offset; }
        }

        private byte format;
        public byte Format
        {
            get { return format; }
        }

        private byte dimension;
        public byte Dimension
        {
            get { return dimension; }
        }

        public static ChannelInfo Create(byte[] content, int objectOffset, ref int index)
        {
            ChannelInfo ret = new ChannelInfo();
            ret.stream = content[index++];
            ret.offset = content[index++];
            ret.format = content[index++];
            ret.dimension = content[index++];

            return ret;
        }
    }
}
