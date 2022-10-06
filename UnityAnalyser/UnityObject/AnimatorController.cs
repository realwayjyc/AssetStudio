using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace UnityAnalyzer
{

    //public class AnimatorControllerLayer
    //{
    //    public string name;
    //    public uint layerNameMapKey;
    //}

    //public class AnimatorControllerState
    //{
    //    public string name;
    //    public uint stateNameMapKey;
    //    public AnimatorControllerLayer layer;
    //    public AnimationClip animationClip;

    //    public uint motionMapKey;
    //    public string motionName;
    //}

    //public enum AnimatorControllerParamType
    //{
    //    Float=1,
    //    Int=3,
    //    Bool=4,
    //    Trigger=9
    //}

    //public class AnimatorControllerParam
    //{
    //    public string paramName;
    //    public uint paramNameKey;
    //    public AnimatorControllerParamType paramType;
    //}

    public class HumanPoseMask
    {
        public uint word0;
        public uint word1;
        public uint word2;

        public HumanPoseMask(ObjectInfo objectInfo, byte[] content, int objectOffset,ref int index)
        {
            //var version = reader.version;
            word0 = BitConverter.ToUInt32(content, index);
            index += 4;
            word1 = BitConverter.ToUInt32(content, index);
            index += 4;

            //if (version[0] > 5 || (version[0] == 5 && version[1] >= 2)) //5.2 and up
            //{
            //    word2 = reader.ReadUInt32();
            //}
        }
    }

    public class SkeletonMaskElement
    {
        public uint m_PathHash;
        public float m_Weight;

        public SkeletonMaskElement(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            m_PathHash = BitConverter.ToUInt32(content, index);
            index += 4;

            m_Weight = BitConverter.ToSingle(content, index);
            index += 4;
        }
    }

    public class SkeletonMask
    {
        public SkeletonMaskElement[] m_Data;
        public SkeletonMask(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int numElements = BitConverter.ToInt32(content, index);
            index += 4;

            m_Data = new SkeletonMaskElement[numElements];
            for (int i = 0; i < numElements; i++)
            {
                m_Data[i] = new SkeletonMaskElement(objectInfo,content, objectOffset,ref index);
            }
        }
    }

    public class LayerConstant
    {
        public uint m_StateMachineIndex;
        public uint m_StateMachineMotionSetIndex;
        public HumanPoseMask m_BodyMask;
        public SkeletonMask m_SkeletonMask;
        public uint m_Binding;
        public int m_LayerBlendingMode;
        public float m_DefaultWeight;
        public bool m_IKPass;
        public bool m_SyncedLayerAffectsTiming;

        public LayerConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;

            m_StateMachineIndex = BitConverter.ToUInt32(content, index);
            index += 4;

            m_StateMachineMotionSetIndex = BitConverter.ToUInt32(content, index);
            index += 4;

            m_BodyMask = new HumanPoseMask(objectInfo, content, objectOffset, ref index);

            m_SkeletonMask = new SkeletonMask(objectInfo, content, objectOffset, ref index);

            m_Binding = BitConverter.ToUInt32(content, index);
            index += 4;

            m_LayerBlendingMode = BitConverter.ToInt32(content, index);
            index += 4;

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 2)) //4.2 and up
            {
                m_DefaultWeight = BitConverter.ToSingle(content, index);
                index += 4;
            }
            m_IKPass = content[index++] == 1;
            if (version[0] > 4 || (version[0] == 4 && version[1] >= 2)) //4.2 and up
            {
                m_SyncedLayerAffectsTiming = content[index++] == 1;
            }
            index += Util.GetAlignCount(index, objectOffset);
        }
    }

    public class ConditionConstant
    {
        public uint m_ConditionMode;
        public uint m_EventID;
        public float m_EventThreshold;
        public float m_ExitTime;

        public ConditionConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            m_ConditionMode = BitConverter.ToUInt32(content, index);
            index += 4;

            m_EventID = BitConverter.ToUInt32(content, index);
            index += 4;

            m_EventThreshold = BitConverter.ToSingle(content, index);
            index += 4;

            m_ExitTime = BitConverter.ToSingle(content, index);
            index += 4;
        }
    }

    public class TransitionConstant
    {
        public ConditionConstant[] m_ConditionConstantArray;
        public uint m_DestinationState;
        public uint m_FullPathID;
        public uint m_ID;
        public uint m_UserID;
        public float m_TransitionDuration;
        public float m_TransitionOffset;
        public float m_ExitTime;
        public bool m_HasExitTime;
        public bool m_HasFixedDuration;
        public int m_InterruptionSource;
        public bool m_OrderedInterruption;
        public bool m_Atomic;
        public bool m_CanTransitionToSelf;

        public TransitionConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;

            int numConditions = BitConverter.ToInt32(content, index);
            index += 4;

            m_ConditionConstantArray = new ConditionConstant[numConditions];
            for (int i = 0; i < numConditions; i++)
            {
                m_ConditionConstantArray[i] = new ConditionConstant(objectInfo,content,objectOffset,ref index);
            }

            m_DestinationState = BitConverter.ToUInt32(content, index);
            index += 4;

            if (version[0] >= 5) //5.0 and up
            {
                m_FullPathID = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            m_ID = BitConverter.ToUInt32(content, index);
            index += 4;

            m_UserID = BitConverter.ToUInt32(content, index);
            index += 4;

            m_TransitionDuration = BitConverter.ToSingle(content, index);
            index += 4;

            m_TransitionOffset = BitConverter.ToSingle(content, index);
            index += 4;

            if (version[0] >= 5) //5.0 and up
            {
                m_ExitTime = BitConverter.ToSingle(content, index);
                index += 4;

                m_HasExitTime = content[index++] == 1;
                m_HasFixedDuration = content[index++] == 1;
                index += Util.GetAlignCount(index, objectOffset);

                m_InterruptionSource = BitConverter.ToInt32(content, index);
                m_OrderedInterruption = content[index++] == 1;
            }
            else
            {
                m_Atomic = content[index++] == 1;
            }

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 5)) //4.5 and up
            {
                m_CanTransitionToSelf = content[index++] == 1;
            }

            index += Util.GetAlignCount(index, objectOffset);
        }
    }

    public class LeafInfoConstant
    {
        public uint[] m_IDArray;
        public uint m_IndexOffset;

        public LeafInfoConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int count= BitConverter.ToInt32(content, index);
            index += 4;

            m_IDArray = new uint[count];
            for (int i = 0; i < count; i++)
            {
                m_IDArray[i] = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            m_IndexOffset = BitConverter.ToUInt32(content, index);
            index += 4;
        }
    }

    public class Blend1dDataConstant // wrong labeled
    {
        public float[] m_ChildThresholdArray;

        public Blend1dDataConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int count = BitConverter.ToInt32(content, index);
            index += 4;

            m_ChildThresholdArray = new float[count];
            for(int i=0;i<count;i++)
            {
                m_ChildThresholdArray[i]= BitConverter.ToSingle(content, index);
                index += 4;
            }
        }
    }

    public class MotionNeighborList
    {
        public uint[] m_NeighborArray;

        public MotionNeighborList(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int count = BitConverter.ToInt32(content, index);
            index += 4;

            m_NeighborArray = new uint[count];
           
            for(int i=0;i<index;i++)
            {
                m_NeighborArray[i]= BitConverter.ToUInt32(content, index);
                index += 4;
            }
        }
    }

    public class Blend2dDataConstant
    {
        public Vector2F[] m_ChildPositionArray;
        public float[] m_ChildMagnitudeArray;
        public Vector2F[] m_ChildPairVectorArray;
        public float[] m_ChildPairAvgMagInvArray;
        public MotionNeighborList[] m_ChildNeighborListArray;

        public Blend2dDataConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int count = BitConverter.ToInt32(content, index);
            index += 4;
            m_ChildPositionArray = new Vector2F[count];

            for (int i=0;i<count;i++)
            {
                m_ChildPositionArray[i] = new Vector2F();
                m_ChildPositionArray[i].X= BitConverter.ToSingle(content, index);
                index += 4;

                m_ChildPositionArray[i].Y = BitConverter.ToSingle(content, index);
                index += 4;
            }

            count = BitConverter.ToInt32(content, index);
            index += 4;
            m_ChildMagnitudeArray = new float[count];
            for(int i=0;i< count;i++)
            {
                m_ChildMagnitudeArray[i]= BitConverter.ToSingle(content, index);
                index += 4;
            }

            count = BitConverter.ToInt32(content, index);
            index += 4;
            m_ChildPairVectorArray = new Vector2F[count];
            for (int i = 0; i < count; i++)
            {
                m_ChildPairVectorArray[i] = new Vector2F();
                m_ChildPairVectorArray[i].X = BitConverter.ToSingle(content, index);
                index += 4;

                m_ChildPairVectorArray[i].Y = BitConverter.ToSingle(content, index);
                index += 4;
            }


            count = BitConverter.ToInt32(content, index);
            index += 4;
            m_ChildPairAvgMagInvArray = new float[count];
            for (int i = 0; i < count; i++)
            {
                m_ChildPairAvgMagInvArray[i] = BitConverter.ToSingle(content, index);
                index += 4;
            }


            int numNeighbours = BitConverter.ToInt32(content, index);
            index += 4;

            m_ChildNeighborListArray = new MotionNeighborList[numNeighbours];
            for (int i = 0; i < numNeighbours; i++)
            {
                m_ChildNeighborListArray[i] = new MotionNeighborList(objectInfo, content, objectOffset, ref index);
            }
        }
    }

    public class BlendDirectDataConstant
    {
        public uint[] m_ChildBlendEventIDArray;
        public bool m_NormalizedBlendValues;

        public BlendDirectDataConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int count = BitConverter.ToInt32(content, index);
            index += 4;

            m_ChildBlendEventIDArray = new uint[count];
            for(int i=0;i<count;i++)
            {
                m_ChildBlendEventIDArray[i]= BitConverter.ToUInt32(content, index);
                index += 4;
            }


            m_NormalizedBlendValues = content[index++] == 1;
            index += Util.GetAlignCount(index, objectOffset);
        }
    }

    public class BlendTreeNodeConstant
    {
        public uint m_BlendType;
        public uint m_BlendEventID;
        public uint m_BlendEventYID;
        public uint[] m_ChildIndices;
        public float[] m_ChildThresholdArray;
        public Blend1dDataConstant m_Blend1dData;
        public Blend2dDataConstant m_Blend2dData;
        public BlendDirectDataConstant m_BlendDirectData;
        public uint m_ClipID;
        public uint m_ClipIndex;
        public float m_Duration;
        public float m_CycleOffset;
        public bool m_Mirror;

        public BlendTreeNodeConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 1)) //4.1 and up
            {
                m_BlendType = BitConverter.ToUInt32(content, index);
                index += 4;
            }
            m_BlendEventID = BitConverter.ToUInt32(content, index);
            index += 4;

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 1)) //4.1 and up
            {
                m_BlendEventYID = BitConverter.ToUInt32(content, index);
                index += 4;
            }
            int count= BitConverter.ToInt32(content, index);
            index += 4;

            m_ChildIndices = new uint[count];
            for(int i=0;i<count;i++)
            {
                m_ChildIndices[i]= BitConverter.ToUInt32(content, index);
                index += 4;
            }


            if (version[0] < 4 || (version[0] == 4 && version[1] < 1)) //4.1 down
            {
                count = BitConverter.ToInt32(content, index);
                index += 4;

                m_ChildThresholdArray = new float[count];
                for (int i = 0; i < count; i++)
                {
                    m_ChildThresholdArray[i] = BitConverter.ToSingle(content, index);
                    index += 4;
                }
            }

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 1)) //4.1 and up
            {
                m_Blend1dData = new Blend1dDataConstant(objectInfo,content,objectOffset,ref index);
                m_Blend2dData = new Blend2dDataConstant(objectInfo, content, objectOffset, ref index);
            }

            if (version[0] >= 5) //5.0 and up
            {
                m_BlendDirectData = new BlendDirectDataConstant(objectInfo, content, objectOffset, ref index);
            }

            m_ClipID = BitConverter.ToUInt32(content, index);
            index += 4;

            if (version[0] == 4 && version[1] >= 5) //4.5 - 5.0
            {
                m_ClipIndex = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            m_Duration = BitConverter.ToSingle(content, index);
            index += 4;

            if (version[0] > 4
                || (version[0] == 4 && version[1] > 1)
                || (version[0] == 4 && version[1] == 1 && version[2] >= 3)) //4.1.3 and up
            {
                m_CycleOffset = BitConverter.ToSingle(content, index);
                index += 4;
                m_Mirror = content[index++] == 1;
                index += Util.GetAlignCount(index, objectOffset);
            }
        }
    }

    public class ValueConstant
    {
        public uint m_ID;
        public uint m_TypeID;
        public uint m_Type;
        public uint m_Index;

        public ValueConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;
            m_ID = BitConverter.ToUInt32(content, index);
            index += 4;

            if (version[0] < 5 || (version[0] == 5 && version[1] < 5))//5.5 down
            {
                m_TypeID = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            m_Type = BitConverter.ToUInt32(content, index);
            index += 4;

            m_Index = BitConverter.ToUInt32(content, index);
            index += 4;
        }
    }

    public class ValueArrayConstant
    {
        public ValueConstant[] m_ValueArray;

        public ValueArrayConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int numVals = BitConverter.ToInt32(content, index);
            index += 4;

            m_ValueArray = new ValueConstant[numVals];
            for (int i = 0; i < numVals; i++)
            {
                m_ValueArray[i] = new ValueConstant(objectInfo, content, objectOffset, ref index);
            }
        }
    }

    public class BlendTreeConstant
    {
        public BlendTreeNodeConstant[] m_NodeArray;
        public ValueArrayConstant m_BlendEventArrayConstant;

        public BlendTreeConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;

            int numNodes = BitConverter.ToInt32(content, index);
            index += 4;

            m_NodeArray = new BlendTreeNodeConstant[numNodes];
            for (int i = 0; i < numNodes; i++)
            {
                m_NodeArray[i] = new BlendTreeNodeConstant(objectInfo, content, objectOffset, ref index);
            }

            if (version[0] < 4 || (version[0] == 4 && version[1] < 5)) //4.5 down
            {
                m_BlendEventArrayConstant = new ValueArrayConstant(objectInfo, content, objectOffset, ref index);
            }
        }
    }

    public class StateConstant
    {
        public TransitionConstant[] m_TransitionConstantArray;
        public int[] m_BlendTreeConstantIndexArray;
        public LeafInfoConstant[] m_LeafInfoArray;
        public BlendTreeConstant[] m_BlendTreeConstantArray;
        public uint m_NameID;
        public uint m_PathID;
        public uint m_FullPathID;
        public uint m_TagID;
        public uint m_SpeedParamID;
        public uint m_MirrorParamID;
        public uint m_CycleOffsetParamID;
        public float m_Speed;
        public float m_CycleOffset;
        public bool m_IKOnFeet;
        public bool m_WriteDefaultValues;
        public bool m_Loop;
        public bool m_Mirror;

        public StateConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;

            int numTransistions = BitConverter.ToInt32(content, index);
            index += 4;

            m_TransitionConstantArray = new TransitionConstant[numTransistions];
            for (int i = 0; i < numTransistions; i++)
            {
                m_TransitionConstantArray[i] = new TransitionConstant(objectInfo,content,objectOffset,ref index);
            }

            int count= BitConverter.ToInt32(content, index);
            index += 4;
            m_BlendTreeConstantIndexArray = new int[count];
            for (int i=0;i<count;i++)
            {
                m_BlendTreeConstantIndexArray[i] = BitConverter.ToInt32(content, index);
                index += 4;
            }

            if (version[0] < 5 || (version[0] == 5 && version[1] < 2)) //5.2 down
            {
                int numInfos = BitConverter.ToInt32(content, index);
                index += 4;

                m_LeafInfoArray = new LeafInfoConstant[numInfos];
                for (int i = 0; i < numInfos; i++)
                {
                    m_LeafInfoArray[i] = new LeafInfoConstant(objectInfo, content, objectOffset, ref index);
                }
            }

            int numBlends = BitConverter.ToInt32(content, index);
            index += 4;
            m_BlendTreeConstantArray = new BlendTreeConstant[numBlends];
            for (int i = 0; i < numBlends; i++)
            {
                m_BlendTreeConstantArray[i] = new BlendTreeConstant(objectInfo, content, objectOffset, ref index);
            }

            m_NameID = BitConverter.ToUInt32(content, index);
            index += 4;

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 3)) //4.3 and up
            {
                m_PathID = BitConverter.ToUInt32(content, index);
                index += 4;
            }
            if (version[0] >= 5) //5.0 and up
            {
                m_FullPathID = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            m_TagID = BitConverter.ToUInt32(content, index);
            index += 4;
            if (version[0] > 5 || (version[0] == 5 && version[1] >= 1)) //5.1 and up
            {
                m_SpeedParamID = BitConverter.ToUInt32(content, index);
                index += 4;

                m_MirrorParamID = BitConverter.ToUInt32(content, index);
                index += 4;

                m_CycleOffsetParamID = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2)) //2017.2 and up
            {
                var m_TimeParamID = BitConverter.ToUInt32(content, index);
                index += 4;
            }

            m_Speed = BitConverter.ToSingle(content, index);
            index += 4;

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 1)) //4.1 and up
            {
                m_CycleOffset = BitConverter.ToSingle(content, index);
                index += 4;
            }
            m_IKOnFeet = content[index++] == 1;
            if (version[0] >= 5) //5.0 and up
            {
                m_WriteDefaultValues = content[index++] == 1;
            }

            m_Loop = content[index++] == 1;
            if (version[0] > 4 || (version[0] == 4 && version[1] >= 1)) //4.1 and up
            {
                m_Mirror = content[index++] == 1;
            }

            index += Util.GetAlignCount(index, objectOffset);
        }
    }

    public class SelectorTransitionConstant
    {
        public uint m_Destination;
        public ConditionConstant[] m_ConditionConstantArray;

        public SelectorTransitionConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            m_Destination = BitConverter.ToUInt32(content, index);
            index += 4;

            int numConditions = BitConverter.ToInt32(content, index);
            index += 4;

            m_ConditionConstantArray = new ConditionConstant[numConditions];
            for (int i = 0; i < numConditions; i++)
            {
                m_ConditionConstantArray[i] = new ConditionConstant(objectInfo, content, objectOffset, ref index);
            }
        }
    }

    public class SelectorStateConstant
    {
        public SelectorTransitionConstant[] m_TransitionConstantArray;
        public uint m_FullPathID;
        public bool m_isEntry;

        public SelectorStateConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int numTransitions = BitConverter.ToInt32(content, index);
            index += 4;

            m_TransitionConstantArray = new SelectorTransitionConstant[numTransitions];
            for (int i = 0; i < numTransitions; i++)
            {
                m_TransitionConstantArray[i] = new SelectorTransitionConstant(objectInfo, content, objectOffset, ref index);
            }

            m_FullPathID = BitConverter.ToUInt32(content, index);
            index += 4;

            m_isEntry = content[index++] == 1;
            index += Util.GetAlignCount(index, objectOffset);
        }
    }

    public class StateMachineConstant
    {
        public StateConstant[] m_StateConstantArray;
        public TransitionConstant[] m_AnyStateTransitionConstantArray;
        public SelectorStateConstant[] m_SelectorStateConstantArray;
        public uint m_DefaultState;
        public uint m_MotionSetCount;

        public StateMachineConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            try
            {
                var version = objectInfo.UnityFileVersion;

                int numStates = BitConverter.ToInt32(content, index);
                index += 4;

                m_StateConstantArray = new StateConstant[numStates];
                for (int i = 0; i < numStates; i++)
                {
                    m_StateConstantArray[i] = new StateConstant(objectInfo, content, objectOffset, ref index);
                }

                int numAnyStates = BitConverter.ToInt32(content, index);
                index += 4;

                m_AnyStateTransitionConstantArray = new TransitionConstant[numAnyStates];
                for (int i = 0; i < numAnyStates; i++)
                {
                    m_AnyStateTransitionConstantArray[i] = new TransitionConstant(objectInfo, content, objectOffset, ref index);
                }

                if (version[0] >= 5) //5.0 and up
                {
                    int numSelectors = BitConverter.ToInt32(content, index);
                    index += 4;
                    m_SelectorStateConstantArray = new SelectorStateConstant[numSelectors];
                    for (int i = 0; i < numSelectors; i++)
                    {
                        m_SelectorStateConstantArray[i] = new SelectorStateConstant(objectInfo, content, objectOffset, ref index);
                    }
                }

                m_DefaultState = BitConverter.ToUInt32(content, index);
                index += 4;

                m_MotionSetCount = BitConverter.ToUInt32(content, index);
                index += 4;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }

    public class ValueArray
    {
        public bool[] m_BoolValues;
        public int[] m_IntValues;
        public float[] m_FloatValues;
        public Vector4F[] m_VectorValues;
        public Vector3F[] m_PositionValues;
        public Vector4F[] m_QuaternionValues;
        public Vector3F[] m_ScaleValues;

        public ValueArray(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            var version = objectInfo.UnityFileVersion;

            int count = 0;
            if (version[0] < 5 || (version[0] == 5 && version[1] < 5)) //5.5 down
            {
                count= BitConverter.ToInt32(content, index);
                index += 4;
                m_BoolValues = new bool[count];
                for(int i=0;i<count;i++)
                {
                    m_BoolValues[i] = content[index++] == 1;
                }
                index += Util.GetAlignCount(index, objectOffset);


                count = BitConverter.ToInt32(content, index);
                index += 4;
                m_IntValues = new int[count];
                for (int i = 0; i < count; i++)
                {
                    m_IntValues[i] = BitConverter.ToInt32(content, index);
                    index += 4;
                }


                count = BitConverter.ToInt32(content, index);
                index += 4;
                m_FloatValues = new float[count];
                for (int i = 0; i < count; i++)
                {
                    m_FloatValues[i] = BitConverter.ToSingle(content, index);
                    index += 4;
                }
            }

            if (version[0] < 4 || (version[0] == 4 && version[1] < 3)) //4.3 down
            {
                count = BitConverter.ToInt32(content, index);
                index += 4;
                m_VectorValues = new Vector4F[count];
                for(int i=0;i< count;i++)
                {
                    m_VectorValues[i] = new Vector4F();
                    m_VectorValues[i].X= BitConverter.ToSingle(content, index);
                    index += 4;
                    m_VectorValues[i].Y = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_VectorValues[i].Z = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_VectorValues[i].W = BitConverter.ToSingle(content, index);
                    index += 4;
                }
            }
            else
            {
                int numPosValues = BitConverter.ToInt32(content, index);
                index += 4;

                m_PositionValues = new Vector3F[numPosValues];
                for (int i = 0; i < numPosValues; i++)
                {
                    m_PositionValues[i] = new Vector3F();

                    m_PositionValues[i].X = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_PositionValues[i].Y = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_PositionValues[i].Z = BitConverter.ToSingle(content, index);
                    index += 4;

                    if (!(version[0] > 5 || (version[0] == 5 && version[1] >= 4)))
                    {
                        //5.4 and up
                        BitConverter.ToSingle(content, index);
                        index += 4;
                    }
                }

                count = BitConverter.ToInt32(content, index);
                index += 4;
                m_QuaternionValues = new Vector4F[count];
                for (int i = 0; i < count; i++)
                {
                    m_QuaternionValues[i] = new Vector4F();
                    m_QuaternionValues[i].X = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_QuaternionValues[i].Y = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_QuaternionValues[i].Z = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_QuaternionValues[i].W = BitConverter.ToSingle(content, index);
                    index += 4;
                }


                int numScaleValues = BitConverter.ToInt32(content, index);
                index += 4;
                m_ScaleValues = new Vector3F[numScaleValues];
                for (int i = 0; i < numScaleValues; i++)
                {
                    m_ScaleValues[i] = new Vector3F();

                    m_ScaleValues[i].X = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_ScaleValues[i].Y = BitConverter.ToSingle(content, index);
                    index += 4;
                    m_ScaleValues[i].Z = BitConverter.ToSingle(content, index);
                    index += 4;

                    if (!(version[0] > 5 || (version[0] == 5 && version[1] >= 4)))
                    {
                        //5.4 and up
                        BitConverter.ToSingle(content, index);
                        index += 4;
                    }
                }

                if (version[0] > 5 || (version[0] == 5 && version[1] >= 5)) //5.5 and up
                {
                    count = BitConverter.ToInt32(content, index);
                    index += 4;
                    m_FloatValues = new float[count];
                    for (int i=0;i<count;i++)
                    {
                        m_FloatValues[i]= BitConverter.ToSingle(content, index);
                        index += 4;
                    }

                    count = BitConverter.ToInt32(content, index);
                    index += 4;
                    m_IntValues = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        m_IntValues[i] = BitConverter.ToInt32(content, index);
                        index += 4;
                    }

                    count = BitConverter.ToInt32(content, index);
                    index += 4;
                    m_BoolValues = new bool[count];
                    for (int i = 0; i < count; i++)
                    {
                        m_BoolValues[i] = content[index++] == 1;
                        index += 4;
                    }
                    index += Util.GetAlignCount(index, objectOffset);
                }
            }
        }
    }

    public class ControllerConstant
    {
        public LayerConstant[] m_LayerArray;
        public StateMachineConstant[] m_StateMachineArray;
        public ValueArrayConstant m_Values;
        public ValueArray m_DefaultValues;

        public ControllerConstant(ObjectInfo objectInfo, byte[] content, int objectOffset, ref int index)
        {
            int numLayers = BitConverter.ToInt32(content, index);
            index += 4;

            m_LayerArray = new LayerConstant[numLayers];
            for (int i = 0; i < numLayers; i++)
            {
                m_LayerArray[i] = new LayerConstant(objectInfo,content,objectOffset,ref index);
            }

            int numStates = BitConverter.ToInt32(content, index);
            index += 4;
            m_StateMachineArray = new StateMachineConstant[numStates];
            for (int i = 0; i < numStates; i++)
            {
                m_StateMachineArray[i] = new StateMachineConstant(objectInfo, content, objectOffset, ref index);
            }

            m_Values = new ValueArrayConstant(objectInfo, content, objectOffset, ref index);
            m_DefaultValues = new ValueArray(objectInfo, content, objectOffset, ref index);
        }
    }

    public class AnimatorController : UnityObject
    {
        public enum BlendMode
        {
            Override=0,
            Additive=1,
        }

        public class State
        {
            public string Name;
            public string Path;
            public string animationClipName;
            public float speed;
            public float CycleOffset;
            public bool FootIK;
            public bool Loop;
            public bool Mirror;
            public bool WriteDefault;
        }


        public class AnimControllerLayer
        {
            public bool IKPass;
            public float DefaultWeight;
            public string LayerName;
            public BlendMode BlendMode;
            public HumanPoseMask humanPoseMask;
            public SkeletonMask SkeletonMask;
            public List<State> states;
        }

        public List<AnimControllerLayer> Layers;

        private string animatorControllerName;
        public string AnimatorControllerName
        {
            get { return animatorControllerName; }
        }

        public List<SerializedObjectIdentifier> AnimationClips { get; private set; }
        public ControllerConstant ControllerConstant { get; private set; }

        public KeyValuePair<uint, string>[] TOS { get; private set; }

        public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AnimatorControllerPanel panel = new AnimatorControllerPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public static AnimatorController Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            AnimatorController ret = new AnimatorController();
            int index = objectOffset + objectInfo.ByteStart;

            ret.animatorControllerName = Util.readStringAndAlign(content, objectOffset, ref index);

            int controllerSize= BitConverter.ToInt32(content, index);
            index += 4;

            try
            {
                ret.ControllerConstant = new ControllerConstant(objectInfo, content, objectOffset, ref index);

                int tosSize = BitConverter.ToInt32(content, index);
                index += 4;

                ret.TOS = new KeyValuePair<uint, string>[tosSize];
                for (int i = 0; i < tosSize; i++)
                {
                    uint v = BitConverter.ToUInt32(content, index);
                    index += 4;

                    string s = Util.readStringAndAlign(content, objectOffset, ref index);
                    ret.TOS[i] = new KeyValuePair<uint, string>(v, s);
                }
                Dictionary<uint, string> nameDict = new Dictionary<uint, string>();
                foreach(KeyValuePair<uint, string> pair in ret.TOS)
                {
                    nameDict.Add(pair.Key, pair.Value);
                }

                int numClips = BitConverter.ToInt32(content, index);
                index += 4;
                ret.AnimationClips = new List<SerializedObjectIdentifier>();
                for (int i = 0; i < numClips; i++)
                {
                    ret.AnimationClips.Add(Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo));
                }
                if(ret.animatorControllerName=="BODY_F")
                {
                    Console.WriteLine("A");
                }
                ret.Layers = new List<AnimControllerLayer>();
                for(int i=0;i < ret.ControllerConstant.m_LayerArray.Length;i++)
                {
                    var layerConstant = ret.ControllerConstant.m_LayerArray[i];
                    AnimControllerLayer layer = new AnimControllerLayer();
                    layer.IKPass = layerConstant.m_IKPass;
                    layer.DefaultWeight = layerConstant.m_DefaultWeight;
                    layer.LayerName = nameDict[layerConstant.m_Binding];
                    layer.BlendMode =(BlendMode)layerConstant.m_LayerBlendingMode;
                    layer.humanPoseMask = layerConstant.m_BodyMask;
                    layer.SkeletonMask=layerConstant.m_SkeletonMask;
                    layer.states = new List<State>();
                    var layerState = ret.ControllerConstant.m_StateMachineArray[i];
                    foreach(var stateConstant in layerState.m_StateConstantArray)
                    {
                        State state = new State();
                        state.Name = nameDict[stateConstant.m_NameID];
                        state.Path = nameDict[stateConstant.m_PathID];
                        state.speed = stateConstant.m_Speed;
                        state.CycleOffset=stateConstant.m_CycleOffset;
                        state.Mirror=stateConstant.m_Mirror;
                        state.FootIK = stateConstant.m_IKOnFeet;
                        state.WriteDefault = stateConstant.m_WriteDefaultValues;
                        try
                        {
                            state.animationClipName = nameDict[stateConstant.m_LeafInfoArray[0].m_IDArray[0]];
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        layer.states.Add(state);
                    }
                    ret.Layers.Add(layer);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ret;
        }
    }
}
