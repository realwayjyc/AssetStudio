using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class UVAnimation
    {
        private int xTile;
        public int XTile
        {
            get { return xTile; }
        }

        private int yTile;
        public int YTile
        {
            get { return yTile; }
        }

        private float cycles;
        public float Cycles
        {
            get { return cycles; }
        }

        public int CreateUVAnimation(ObjectInfo objectInfo, byte[] content, int objectOffset, int index)
        {
            xTile = BitConverter.ToInt32(content, index); index += 4;
            yTile = BitConverter.ToInt32(content, index); index += 4;
            cycles = BitConverter.ToSingle(content, index); index += 4;
            return index;
        }
    }
}
