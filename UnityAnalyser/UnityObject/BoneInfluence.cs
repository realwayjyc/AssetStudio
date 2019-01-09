using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class BoneInfluence
    {
        private float[] weights=new float[4];
        public float[] Weights
        {
            get { return weights; }
        }

        private int[] boneIndex = new int[4];
        public int[] BoneIndex
        {
            get { return boneIndex; }
        }



        public static BoneInfluence Create(byte[] content, int objectOffset, ref int index)
        {
            BoneInfluence ret = new BoneInfluence();

            ret.weights[0] = BitConverter.ToSingle(content, index); index += 4;
            ret.weights[1] = BitConverter.ToSingle(content, index); index += 4;
            ret.weights[2] = BitConverter.ToSingle(content, index); index += 4;
            ret.weights[3] = BitConverter.ToSingle(content, index); index += 4;

            ret.boneIndex[0] = BitConverter.ToInt32(content, index); index += 4;
            ret.boneIndex[1] = BitConverter.ToInt32(content, index); index += 4;
            ret.boneIndex[2] = BitConverter.ToInt32(content, index); index += 4;
            ret.boneIndex[3] = BitConverter.ToInt32(content, index); index += 4;

            return ret;
        }
    }
}
