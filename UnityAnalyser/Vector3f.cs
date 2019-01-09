using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class Vector3f
    {
        public float x;
        public float y;
        public float z;

        public override string ToString()
        {
            return x.ToString() + "  " + y.ToString() + "  " + z.ToString();
        }
    }
}
