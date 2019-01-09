using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public enum GfxPrimitiveType
    {
        PrimitiveTriangles = 0,
        PrimitiveTriangleStripDeprecated = 1,
        PrimitiveQuads = 2,
        PrimitiveLines = 3,
        PrimitiveLineStrip = 4,
        PrimitivePoints = 5,
        PrimitiveTypeCount = 6,
        PrimitiveForce32BitInt = 0x7FFFFFFF
    }


    public class SubMesh
    {
        private uint firstByte;
        public uint FirstByte
        {
            get { return firstByte; }
        }

        private uint indexCount;
        public uint IndexCount
        {
            get { return indexCount; }
        }

        private GfxPrimitiveType topology;
        public GfxPrimitiveType Topology
        {
            get { return topology; }
        }

        private uint firstVertex;
        public uint FirstVertex
        {
            get { return firstVertex; }
        }

        private uint vertexCount;
        public uint VertexCount
        {
            get { return vertexCount; }
        }

        private Vector3f center;
        public Vector3f Center
        {
            get { return center; }
        }

        private Vector3f extent;
        public Vector3f Extent
        {
            get { return extent; }
        }



        public static List<SubMesh> GetSubMeshList(byte[] content, int objectOffset, ref int index)
        {
            List<SubMesh> ret = new List<SubMesh>();
            int count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                SubMesh subMesh = new SubMesh();
                subMesh.firstByte = BitConverter.ToUInt32(content, index);
                index += 4;

                subMesh.indexCount = BitConverter.ToUInt32(content, index);
                index += 4;

                subMesh.topology =(GfxPrimitiveType) BitConverter.ToUInt32(content, index);
                index += 4;

                subMesh.firstVertex = BitConverter.ToUInt32(content, index);
                index += 4;

                subMesh.vertexCount = BitConverter.ToUInt32(content, index);
                index += 4;

                subMesh.center = Util.ReadNextVector3f(content, ref index);
                subMesh.extent = Util.ReadNextVector3f(content, ref index);
                ret.Add(subMesh);
            }
            return ret;
        }
    }
}
