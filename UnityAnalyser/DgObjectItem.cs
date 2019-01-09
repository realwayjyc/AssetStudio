using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class Vetex3fDgItem
    {
        private Vertex3f vertex3f;
        private int index;

        public float XValue
        {
            get
            {
                return vertex3f.x;
            }
        }

        public float YValue
        {
            get
            {
                return vertex3f.y;
            }
        }

        public float ZValue
        {
            get
            {
                return vertex3f.z;
            }
        }

        public int Id
        {
            get
            {
                return index;
            }
        }

        public Vetex3fDgItem(Vertex3f aVertex3f, int aIndex)
        {
            vertex3f = aVertex3f;
            index = aIndex;
        }
    }

    public class IndexDgItem
    {
        private int index;
        public int Id
        {
            get
            {
                return index;
            }
        }

        private ushort vertexIndex;
        public ushort VertexIndex
        {
            get { return vertexIndex; }
        }

        public IndexDgItem(int aIndex, ushort aVertexIndex)
        {
            index = aIndex;
            vertexIndex = aVertexIndex;
        }
    }
}
