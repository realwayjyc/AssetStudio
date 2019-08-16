using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public enum CAST_SHADOWS
    {
        OFF = 0,
        ON = 1,
        Two_Sided = 2,
        Shadows_Only = 3
    }

    public enum ReflectionProbes
    {
        Off=0,
        Blend_Probes,
        Blend_Probes_And_SkyBox,
        Simple
    }

    public class Renderer:Component
    {
        protected bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        protected CAST_SHADOWS castShadows;
        public CAST_SHADOWS CastShadows
        {
            get { return castShadows; }
        }

        protected bool receiveShadows;
        public bool ReceiveShadows
        {
            get { return receiveShadows; }
        }

        protected short lightMapIndex;
        public short LightMapIndex
        {
            get { return lightMapIndex; }
        }

        protected short lightMapIndexDynamic;
        public short LightMapIndexDynamic
        {
            get { return lightMapIndexDynamic; }
        }

        protected Vector4F lightMapST;
        public Vector4F LightMapST
        {
            get { return lightMapST; }
        }

        protected Vector4F lightMapSTDynamic;
        public Vector4F LightMapSTDynamic
        {
            get { return lightMapSTDynamic; }
        }

        protected List<SerializedObjectIdentifier> materials;
        public List<SerializedObjectIdentifier> Materials
        {
            get { return materials; }
        }

        protected List<uint> subsetIndices=new List<uint>();
        public List<uint> SubsetIndices
        {
            get { return subsetIndices; }
        }

        protected SerializedObjectIdentifier staticBatchRootTransform;
        public SerializedObjectIdentifier StaticBatchRootTransform
        {
            get { return staticBatchRootTransform; }
        }

        protected bool lightProbe;
        public bool LightProbe
        {
            get { return lightProbe; }
        }

        protected ReflectionProbes reflectionProbes;
        public ReflectionProbes ReflectionProbes
        {
            get { return reflectionProbes; }
        }

        protected SerializedObjectIdentifier lightProbeAnchorTransform;
        public SerializedObjectIdentifier LightProbeAnchorTransform
        {
            get { return lightProbeAnchorTransform; }
        }

        protected uint sortingLayerID;
        public uint SortingLayerID
        {
            get { return sortingLayerID; }
        }

        protected short sortingOrder;
        public short SortingOrder
        {
            get { return sortingOrder; }
        }

        public int CreateRenderer(ObjectInfo objectInfo, byte[] content, int objectOffset,int index)
        {
            int serializedFileIndex = BitConverter.ToInt32(content, index);
            index += 4;
            int identifier = BitConverter.ToInt32(content,index);
            index += 4;
            parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);
            index += Util.GetSerializedFileIndexIdRange(objectInfo);

            if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
            {
                isEnabled = (content[index++] == 1);
                castShadows = (CAST_SHADOWS)(content[index++]);
                receiveShadows = (content[index++] == 1);
                lightMapIndex = content[index++];
            }
            else if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                isEnabled = (BitConverter.ToInt32(content, index) != 0);
                index += 4;

                castShadows = (CAST_SHADOWS)(content[index++]);
                receiveShadows = (content[index++] == 1);

                index += Util.GetAlignCount(index, objectOffset);

                lightMapIndex = BitConverter.ToInt16(content, index); index += 2;
                lightMapIndexDynamic = BitConverter.ToInt16(content, index); index += 2;
            }

            lightMapST=new Vector4F();
            lightMapST.X = BitConverter.ToSingle(content, index); index += 4;
            lightMapST.Y = BitConverter.ToSingle(content, index); index += 4;
            lightMapST.Z = BitConverter.ToSingle(content, index); index += 4;
            lightMapST.W = BitConverter.ToSingle(content, index); index += 4;

            if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1]==3)
            {
                lightMapSTDynamic.X = BitConverter.ToSingle(content, index); index += 4;
                lightMapSTDynamic.Y = BitConverter.ToSingle(content, index); index += 4;
                lightMapSTDynamic.Z = BitConverter.ToSingle(content, index); index += 4;
                lightMapSTDynamic.W = BitConverter.ToSingle(content, index); index += 4;
            }


            materials = Util.ReadSerializedObjectIdentifierList(content, ref index, objectInfo);

            if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                //StaticBatchInfo
                index += 4;
            }
            else if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
            {
                int count = BitConverter.ToInt32(content, index); index += 4;
                for (int i = 0; i < count; i++)
                {
                    subsetIndices.Add(BitConverter.ToUInt32(content, index));
                    index += 4;
                }
            }
            staticBatchRootTransform = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);


            lightProbe = (content[index++]==1);
            index+=Util.GetAlignCount(index, objectOffset);

            if (objectInfo.UnityFileVersion[0] >= 5)
            {
                this.reflectionProbes = (ReflectionProbes)BitConverter.ToUInt32(content, index);
                index += 4;
            }





            lightProbeAnchorTransform = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            sortingLayerID = BitConverter.ToUInt32(content, index);
            index += 4;

            sortingOrder = BitConverter.ToInt16(content, index);
            index += 2;
            index += Util.GetAlignCount(index, objectOffset);


            return index;
        }

        public Transform GetStaticBatchRoot()
        {
            return this.GetUnityObjectBySerializedObjectIdentifier(staticBatchRootTransform) as Transform;
        }

        public Transform GetLightProbeAnchor()
        {
            return this.GetUnityObjectBySerializedObjectIdentifier(lightProbeAnchorTransform) as Transform;
        }

        public SortingLayerEntry GetSortingLayer()
        {
            return this.UnityFile.Analyzer.TagManager.GetSortingLayerEntryByUniqueID(this.SortingLayerID);
        }

        /// <summary>
        /// 可能是Material ，也可能是Shader
        /// </summary>
        /// <returns></returns>
        public List<UnityObject> GetMaterials()
        {
            List<UnityObject> ret = new List<UnityObject>();
            foreach (SerializedObjectIdentifier si in materials)
            {
                UnityObject unityObject=GetUnityObjectBySerializedObjectIdentifier(si);
                if (unityObject != null)
                {
                    ret.Add(unityObject);
                }
            }
            return ret;
        }
    }
}
