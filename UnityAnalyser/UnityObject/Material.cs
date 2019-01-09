using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public struct MaterialTagString
    {
        public string tag;
        public string value;
    }
    public class Material : UnityObject
    {
        private string materialName;
        public string MaterialName
        {
            get { return materialName; }
        }

        private SerializedObjectIdentifier shader;
        public SerializedObjectIdentifier Shader
        {
            get { return shader; }
        }


        private List<string> keyWordList=new List<string>();
        public List<string> KeyWordList
        {
            get { return keyWordList; }
        }

        private int customRenderQueue;
        public int CustomRenderQueue
        {
            get { return customRenderQueue; }
        }

        private UnityPropertySheet unityPropertySheet;
        public UnityPropertySheet UnityPropertySheet
        {
            get { return unityPropertySheet; }
        }

        private int lightMapFlags;
        public int LightMapFlags
        {
            get { return lightMapFlags; }
        }

        private List<MaterialTagString> tagStringList=new List<MaterialTagString>();

        public static Material Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Material ret = new Material();
            int index = objectOffset + objectInfo.ByteStart;

            ret.materialName=Util.readStringAndAlign(content, objectOffset, ref index);

            int serializedFileIndex= BitConverter.ToInt32(content, index);
            index += 4;
            int idinfile = BitConverter.ToInt32(content, index);
            index += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);

            ret.shader = new SerializedObjectIdentifier(serializedFileIndex, idinfile);

            if (objectInfo.UnityFileVersion[0]==5 &&
                objectInfo.UnityFileVersion[1] == 3)
            {
                string keyword = Util.readStringAndAlign(content, objectOffset, ref index);
                ret.keyWordList.Add(keyword);
            }
            else if (objectInfo.UnityFileVersion[0] == 4 &&
                objectInfo.UnityFileVersion[1] == 6)
            {
                int keywordsCount = BitConverter.ToInt32(content, index);
                index += 4;

                for (int i = 0; i < keywordsCount; i++)
                {
                    string keyword = Util.readStringAndAlign(content, objectOffset, ref index);
                    ret.keyWordList.Add(keyword);
                }
            }

            if (objectInfo.UnityFileVersion[0] == 5 &&
                objectInfo.UnityFileVersion[1] == 3)
            {
                ret.lightMapFlags = BitConverter.ToInt32(content, index);
                index += 4;
            }

            ret.customRenderQueue = BitConverter.ToInt32(content, index);
            index += 4;

            if (objectInfo.UnityFileVersion[0] == 5 &&
                objectInfo.UnityFileVersion[1] == 3)
            {
                int tagStringCount = BitConverter.ToInt32(content, index);
                index += 4;

                for(int i=0;i<tagStringCount;i++)
                {
                    MaterialTagString m = new MaterialTagString();
                    m.tag = Util.readStringAndAlign(content, objectOffset, ref index);
                    m.value = Util.readStringAndAlign(content, objectOffset, ref index);
                    ret.tagStringList.Add(m);
                }

            }

            UnityPropertySheet unityPropertySheet = new UnityPropertySheet(ret);

            index=unityPropertySheet.readFromContent(content, objectOffset, index,objectInfo);
            ret.unityPropertySheet = unityPropertySheet;
            return ret;
        }


        public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MaterialPanel panel = new MaterialPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public Shader GetShader()
        {
            Shader ret=this.GetUnityObjectBySerializedObjectIdentifier(this.shader) as Shader;
            return ret;
        }
    }
}
