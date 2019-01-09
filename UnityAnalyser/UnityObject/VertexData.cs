using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class VertexData
    {
        private uint currentChannels;
        public uint CurrentChannels
        {
            get { return currentChannels; }
        }

        private uint vertexCount;
        public uint VertexCount
        {
            get { return vertexCount; }
        }

        private List<ChannelInfo> channelInfoList = new List<ChannelInfo>();
        public List<ChannelInfo> ChannelInfoList
        {
            get { return channelInfoList; }
        }

        public int Create(byte[] content, int objectOffset, int index)
        {


            currentChannels = BitConverter.ToUInt32(content, index);
            index += 4;

            vertexCount = BitConverter.ToUInt32(content, index);
            index += 4;

            int count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                channelInfoList.Add(ChannelInfo.Create(content, objectOffset, ref index));
            }

            if (Util.isNowParsingResourcesFiles() == false)
            {
                Console.WriteLine("Debug");
            }



            return index;
        }
    }
}
