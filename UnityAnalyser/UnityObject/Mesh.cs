using System;
using System.Collections.Generic;

namespace UnityAnalyzer
{
    public class Mesh : UnityObject
    {
        private string meshName;
        public string MeshName
        {
            get { return meshName; }
        }

        private bool _bUse16BitIndices = true; //3.5.0 and newer always uses 16bit indices;

        /// <summary>
        /// 暂时不实现其功能
        /// </summary>
        private BlendShapeData blendShapeData;
        public BlendShapeData BlendShapeData
        {
            get { return blendShapeData; }
        }

        private List<SubMesh> subMeshList;
        public List<SubMesh> SubMeshList
        {
            get { return subMeshList; }
        }

        private List<Matrix44f> bindPose = new List<Matrix44f>();
        public List<Matrix44f> BindPose
        {
            get { return bindPose; }
        }

        private List<uint> bonePathHashes = new List<uint>();
        public List<uint> BonePathHashes
        {
            get { return bonePathHashes; }
        }

        private uint rootBonePathHash;
        public uint RootBonePathHash
        {
            get { return rootBonePathHash; }
        }

        private bool meshCompression;
        public bool MeshCompression
        {
            get { return meshCompression; }
        }

        private bool streamCompression;
        public bool StreamCompression
        {
            get { return streamCompression; }
        }

        private bool isReadable;
        public bool IsReadable
        {
            get { return isReadable; }
        }

        private bool keepVertices;
        public bool KeepVertices
        {
            get { return keepVertices; }
        }

        private bool keepIndices;
        public bool KeepIndices
        {
            get { return keepIndices; }
        }

        private List<uint> indices = new List<uint>();
        public List<uint> Indices
        {
            get { return indices; }
        }


        private List<BoneInfluence> boneInfluenceList = new List<BoneInfluence>();
        public List<BoneInfluence> BoneInfluenceList
        {
            get { return boneInfluenceList; }
        }

        private VertexData vertexData;
        public VertexData VertexData
        {
            get { return vertexData; }
        }

        private List<StreamInfo> streamInfoList = new List<StreamInfo>();
        public List<StreamInfo> StreamInfoList
        {
            get { return streamInfoList; }
        }

        private uint dataSize;
        public uint DataSize
        {
            get { return dataSize; }
        }

        private List<Vector3f> vertexPos = new List<Vector3f>();
        public List<Vector3f> VertexPos
        {
            get { return vertexPos; }
        }

        private List<Vector3f> vertexNormal = new List<Vector3f>();
        public List<Vector3f> VertexNormal
        {
            get { return vertexNormal; }
        }

        private List<Vector4f> vertexTangent = new List<Vector4f>();
        public List<Vector4f> VertexTangent
        {
            get { return vertexTangent; }
        }

        private List<Vector2f> vertexUV = new List<Vector2f>();
        public List<Vector2f> VertexUV
        {
            get { return vertexUV; }
        }

        private List<Vector2f> vertexUV2 = new List<Vector2f>();
        public List<Vector2f> VertexUV2
        {
            get { return vertexUV2; }
        }




        public static Mesh Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            int[] version = objectInfo.UnityFileVersion;
            Mesh ret = new Mesh();
            return ret;
            int index = objectOffset + objectInfo.ByteStart;
            ret.meshName = Util.readStringAndAlign(content, objectOffset, ref index);

            if (version[0] < 3 || (version[0] == 3 && version[1] < 5)) //3.5 down
            {
                ret._bUse16BitIndices = BitConverter.ToInt32(content, index) > 0;
                index += 4;
            }

            ret.subMeshList = SubMesh.GetSubMeshList(content, objectOffset, ref index);

            ret.blendShapeData = new BlendShapeData();

            index = ret.BlendShapeData.Create(content, objectOffset, index);

            int bindposeMatrixCount = BitConverter.ToInt32(content, index);
            index += 4;

            for (int i = 0; i < bindposeMatrixCount; i++)
            {
                ret.bindPose.Add(Matrix44f.Create(content, objectOffset, ref index));
            }

            int count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                ret.bonePathHashes.Add(BitConverter.ToUInt32(content, index));
                index += 4;
            }

            ret.rootBonePathHash = BitConverter.ToUInt32(content, index);
            index += 4;

            ret.meshCompression = (content[index++] == 1);
            if (version[0] >= 4)
            {
                if (version[0] < 5)
                {
                    ret.streamCompression = (content[index++] == 1);
                }
                ret.isReadable = (content[index++] == 1);
                ret.keepVertices = (content[index++] == 1);
                ret.keepIndices = (content[index++] == 1);
            }

            index += Util.GetAlignCount(index, objectOffset);

            count = BitConverter.ToInt32(content, index);
            index += 4;

            if (ret._bUse16BitIndices)
            {
                count = count / 2;
            }
            else
            {
                count = count / 4;
            }
            //Index Buffer Size
            for (int i = 0; i < count; i++)
            {
                if (ret._bUse16BitIndices)
                {
                    ret.indices.Add(BitConverter.ToUInt16(content, index));
                    index += 2;
                }
                else
                {
                    ret.indices.Add(BitConverter.ToUInt32(content, index));
                    index += 4;
                }
            }

            index += Util.GetAlignCount(index, objectOffset);
            count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                ret.boneInfluenceList.Add(BoneInfluence.Create(content, objectOffset, ref index));
            }


          

            ret.vertexData = new VertexData();
            index=ret.vertexData.Create(content, objectOffset, index);


            count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                ret.streamInfoList.Add(StreamInfo.Create(content, objectOffset, ref index));
            }

            if (ret.meshName.Contains("constru"))
            {
                //TODO
                Console.WriteLine("a");
            }

            ret.dataSize = BitConverter.ToUInt32(content, index);
            index += 4;

            //size 48 则顺序应该是vertex (3) normal (3) tangent (4) uv (2)
            //size 56 则顺序应该是vertex (3) normal (3) tangent (4) uv (2)   uv2 (2)
            //size 24 则顺序应该是vertex (3) normal (3)
            int perVertexSize =(int) (ret.dataSize / ret.vertexData.VertexCount);
            if (perVertexSize != 48 && perVertexSize != 56 && perVertexSize!=24)
            {
                System.Windows.MessageBox.Show("未能解析格式");
                System.Environment.Exit(1);
            }

            for (int i = 0; i < ret.vertexData.VertexCount; i++)
            {
                switch (perVertexSize)
                {
                    case 48:
                        ret.vertexPos.Add(Util.ReadNextVector3f(content, ref index));
                        ret.vertexNormal.Add(Util.ReadNextVector3f(content, ref index));
                        ret.vertexTangent.Add(Util.ReadNextVector4f(content, ref index));
                        ret.vertexUV.Add(Util.ReadNextVector2f(content, ref index));
                        break;
                    case 56:
                        ret.vertexPos.Add(Util.ReadNextVector3f(content, ref index));
                        ret.vertexNormal.Add(Util.ReadNextVector3f(content, ref index));
                        ret.vertexTangent.Add(Util.ReadNextVector4f(content, ref index));
                        ret.vertexUV.Add(Util.ReadNextVector2f(content, ref index));
                        ret.vertexUV2.Add(Util.ReadNextVector2f(content, ref index));
                        break;
                    case 24:
                        ret.vertexPos.Add(Util.ReadNextVector3f(content, ref index));
                        ret.vertexNormal.Add(Util.ReadNextVector3f(content, ref index));
                        break;
                    default:
                        System.Windows.MessageBox.Show("未知顶点格式:per vertex size=" + perVertexSize);
                        break;
                }
            }
           

            if (Util.isNowParsingResourcesFiles() == false)
            {
                Console.WriteLine("Debug");
            }
            return ret;
        }
    }
}
