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

    public class AnimatorControllerLayer
    {
        public string name;
        public uint layerNameMapKey;
    }

    public class AnimatorControllerState
    {
        public string name;
        public uint stateNameMapKey;
        public AnimatorControllerLayer layer;
        public AnimationClip animationClip;

        public uint motionMapKey;
        public string motionName;
    }

    public enum AnimatorControllerParamType
    {
        Float=1,
        Int=3,
        Bool=4,
        Trigger=9
    }

    public class AnimatorControllerParam
    {
        public string paramName;
        public uint paramNameKey;
        public AnimatorControllerParamType paramType;
    }

    public class AnimatorController : UnityObject
    {
        private string animatorControllerName;
        public string AnimatorControllerName
        {
            get { return animatorControllerName; }
        }

        public List<AnimatorControllerLayer> layerList = new List<AnimatorControllerLayer>();

        public List<AnimatorControllerState> stateList = new List<AnimatorControllerState>();

        public List<AnimatorControllerParam> paramList = new List<AnimatorControllerParam>();

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

            index += 4;

            //读取层数
            int layerCount = BitConverter.ToInt32(content, index);
            index += 4;

            for (int i = 0; i < layerCount;i++ )
            {
                AnimatorControllerLayer layer = new AnimatorControllerLayer();
                index += 0x14;
                layer.layerNameMapKey = BitConverter.ToUInt32(content, index);
                index += 4;
                index += 0xC;

                ret.layerList.Add(layer);
            }

            index += 4;
            //先读第一层的State数
            int stateCount = 0;

            for (int i = 0; i < layerCount; i++)
            {
                stateCount = BitConverter.ToInt32(content, index);
                index += 4;

                AnimatorControllerLayer layer = ret.layerList[i];
                //TODO：有Motion和无Motion是不同的大小
                for(int j=0;j<stateCount;j++)
                {
                    AnimatorControllerState state = new AnimatorControllerState();
                    state.layer = layer;

                    int stag = BitConverter.ToInt32(content, index + 8);
                    if(stag==0)
                    {
                        //表示有Motion，一个State的大小为0x78
                        state.motionMapKey = BitConverter.ToUInt32(content, index+0x14);
                        index += 0x14 + 0x50 - 4;

                        state.stateNameMapKey = BitConverter.ToUInt32(content, index);
                        index += 4;

                        index += 0x60 - 0x50 + 4;
                    }
                    else
                    {
                        //表示无Motion     一个State 的大小为0x34
                        index += 0x1c;
                        state.stateNameMapKey = BitConverter.ToUInt32(content, index);
                        index += 4;

                        index += 0x14;

                    }

                    ret.stateList.Add(state);
                }

                index += 0xC;
            }

            //开始读取参数
            int paramCount = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < paramCount;i++ )
            {
                //1,3,4,9分别表示Float int bool trigger
                AnimatorControllerParam p = new AnimatorControllerParam();
                ret.paramList.Add(p);

                p.paramNameKey = BitConverter.ToUInt32(content, index);
                index += 4;

                index += 4;
                p.paramType =(AnimatorControllerParamType) BitConverter.ToInt32(content, index);
                index += 4;

                index += 4;
           }

            
            int pos = index;

            while (true)
            {
                if (content[pos] == 0x12 &&
                    content[pos + 1] == 0 &&
                    content[pos + 2] == 0 &&
                    content[pos + 3] == 0 &&
                    content[pos + 4] == 0x53 &&
                    content[pos + 5] == 0x74 &&
                    content[pos + 6] == 0x61 &&
                    content[pos + 7] == 0x74)
                {
                    break;
                }
                pos++;
                if (pos >= content.Length)
                {
                    throw new Exception("Parsing AnimatorController ERROR");
                }
            }
            index = pos - 8 - 0x18;

            pos = index;

            while (true)
            {
                bool bFound = true;
                for (int k = 0; k < 16; k++)
                {
                    if (content[pos + k] != 0)
                    {
                        bFound = false;
                        break;
                    }
                }
                if (bFound == false)
                {
                    pos--;
                    continue;
                }
                if (content[pos + 16] != 0)
                {
                    index = pos + 16;
                    break;
                }
                else
                {
                    pos--;
                    continue;
                }
            }




            //int valCount=0;
            //while(true)
            //{
            //    valCount = BitConverter.ToInt32(content, index);
            //    index += 4;
            //    if(valCount==0)
            //    {
            //        index -= 8;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            int valCount = BitConverter.ToInt32(content, index);
            index += 4;

            Dictionary<uint, string> valMap = new Dictionary<uint, string>();
            for (int i = 0; i < valCount;i++ )
            {
                uint key = BitConverter.ToUInt32(content, index);
                index += 4;
                string valstring = Util.readStringAndAlign(content, objectOffset, ref index);
                valMap.Add(key, valstring);
            }

            int clipCount = BitConverter.ToInt32(content, index);
            index += 4;
            List<SerializedObjectIdentifier> seoList = new List<SerializedObjectIdentifier>();
            for (int i = 0; i < clipCount;i++ )
            {
                seoList.Add(Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo));
            }

            foreach(AnimatorControllerLayer l in ret.layerList)
            {
                l.name = valMap[l.layerNameMapKey];
            }

            int x=0;
            foreach (AnimatorControllerState l in ret.stateList)
            {
                if (valMap.ContainsKey(l.stateNameMapKey)==false)
                {
                    l.name = "";
                }
                else
                {
                    l.name = valMap[l.stateNameMapKey];
                }
                if (l.motionMapKey != 0 && valMap.ContainsKey(l.motionMapKey))
                {
                    l.motionName = valMap[l.motionMapKey];
                    l.animationClip = objectInfo.GetUnityObjectBySerializedObjectIdentifier(seoList[x++]) as AnimationClip;
                }
            }

            foreach (AnimatorControllerParam l in ret.paramList)
            {
                if (valMap.ContainsKey(l.paramNameKey) == true)
                {
                    l.paramName = valMap[l.paramNameKey];
                }
            }
            return ret;
        }
    }
}
