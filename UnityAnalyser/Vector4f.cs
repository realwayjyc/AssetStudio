using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public struct Vector4f
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public override string ToString()
        {
            return x.ToString() + "  " + y.ToString() + "  " + z.ToString() + "  " + w.ToString();
        }
    }
}
