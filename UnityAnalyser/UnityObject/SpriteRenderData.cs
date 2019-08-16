using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace UnityAnalyzer
{
    public class SpriteRenderData
    {
        private UnityObject parent;
        public UnityObject Parent
        {
          get { return parent; }
        }

        private SerializedObjectIdentifier texture2D;
        public SerializedObjectIdentifier Texture2D
        {
            get { return texture2D; }
        }

        private SerializedObjectIdentifier unknown_ptr;
        public SerializedObjectIdentifier Unknown_ptr
        {
            get { return unknown_ptr; }
        }

        private List<Vertex3f> vertices=new List<Vertex3f>();
        public List<Vertex3f> Vertices
        {
            get { return vertices; }
        }

        private List<ushort> indices=new List<ushort>();
        public List<ushort> Indices
        {
            get { return indices; }
        }

        private RectangleF textureRect;
        public RectangleF TextureRect
        {
            get { return textureRect; }
        }

        private float textureOffsetX;
        public float TextureOffsetX
        {
            get { return textureOffsetX; }
        }

        private float textureOffsetY;
        public float TextureOffsetY
        {
            get { return textureOffsetY; }
        }

        private int setting;
        public int Setting
        {
            get { return setting; }
        }

        private Vector4F uvTransform;
        public Vector4F UvTransform
        {
            get { return uvTransform; }
        }

        public static SpriteRenderData Create(UnityObject parent, byte[] content, int objectOffset, ref int index,ObjectInfo objectInfo)
        {
            SpriteRenderData ret = new SpriteRenderData();
            ret.parent = parent;

            int fileIndex = BitConverter.ToInt32(content, index); index += 4;
            int idinfile = BitConverter.ToInt32(content, index); index += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);
            ret.texture2D = new SerializedObjectIdentifier(fileIndex, idinfile);

            if (objectInfo.UnityFileVersion[0] == 5 &&
                objectInfo.UnityFileVersion[1] == 3)
            {
                ///这里还有一个SerializedObjectIdentifier，不知道是什么含义
                fileIndex = BitConverter.ToInt32(content, index); index += 4;
                idinfile = BitConverter.ToInt32(content, index); index += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);
                ret.unknown_ptr = new SerializedObjectIdentifier(fileIndex, idinfile);
            }

            int count = BitConverter.ToInt32(content, index); index += 4;
            for (int i = 0; i < count; i++)
            {
                Vertex3f vertex3f = new Vertex3f();
                vertex3f.x = BitConverter.ToSingle(content, index); index += 4;
                vertex3f.y = BitConverter.ToSingle(content, index); index += 4;
                vertex3f.z = BitConverter.ToSingle(content, index); index += 4;
                ret.vertices.Add(vertex3f);
            }

            count = BitConverter.ToInt32(content, index); index += 4;
            for (int i = 0; i < count; i++)
            {
                ret.indices.Add(BitConverter.ToUInt16(content, index)); index += 2;
            }

            int alignCount=Util.GetAlignCount(index, objectOffset);
            index += alignCount;


            ret.textureRect.X = BitConverter.ToSingle(content, index); index += 4;
            ret.textureRect.Y = BitConverter.ToSingle(content, index); index += 4;
            ret.textureRect.Width = BitConverter.ToSingle(content, index); index += 4;
            ret.textureRect.Height = BitConverter.ToSingle(content, index); index += 4;

            ret.textureOffsetX = BitConverter.ToSingle(content, index); index += 4;
            ret.textureOffsetY = BitConverter.ToSingle(content, index); index += 4;

            ret.setting = BitConverter.ToInt32(content, index); index += 4;

            ret.uvTransform.X = BitConverter.ToSingle(content, index); index += 4;
            ret.uvTransform.Y = BitConverter.ToSingle(content, index); index += 4;
            ret.uvTransform.Z = BitConverter.ToSingle(content, index); index += 4;
            ret.uvTransform.W = BitConverter.ToSingle(content, index); index += 4;


            return ret;
        }

        public Texture2D GetTexture2D()
        {
            return this.parent.GetUnityObjectBySerializedObjectIdentifier(this.texture2D) as Texture2D;
        }

    }
}
