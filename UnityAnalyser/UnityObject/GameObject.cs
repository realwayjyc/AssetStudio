using System;
using System.Collections.Generic;


namespace UnityAnalyzer
{
    public class GameObject:UnityObject
    {
        private List<SerializedObjectIdentifier> componentIdentifierList = new List<SerializedObjectIdentifier>();
        public List<SerializedObjectIdentifier> ComponentIdentifierList
        {
            get { return componentIdentifierList; }
        }

        private uint layer;
        public uint GLayer
        {
            get { return layer; }
        }

        private string name;
        public string GName
        {
            get { return name; }
        }

        private Int16 tag;
        public Int16 GTag
        {
            get { return tag; }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
        }

        private GameObjectPanel gameObjectPanel;
        public GameObjectPanel GameObjectPanel
        {
            get { return gameObjectPanel; }
            set { gameObjectPanel = value; }
        }

       

        public List<UnityObject> Components
        {
            get
            {
                List<UnityObject> componentList = new List<UnityObject>();
                foreach (SerializedObjectIdentifier componentIdentifier in componentIdentifierList)
                {
                    UnityObject unityObject = GetUnityObjectBySerializedObjectIdentifier(componentIdentifier);
                    componentList.Add(unityObject);
                }
                return componentList;
            }
        }

        public Transform GetTransformComponent()
        {
            foreach (UnityObject o in Components)
            {
                if (o is Transform)
                {
                    return o as Transform;
                }
            }
            return null;
        }

        public static GameObject Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            GameObject ret = new GameObject();
            int componentCount = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            for (int i = 0; i < componentCount; i++)
            {
                int serializedFileIndex = 0;
                int idInFile = 0;

                if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
                {
                    serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4 + i * 12 + 4);
                    idInFile = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4 + i * 12 + 8);
                }
                else if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
                {
                    serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4 + i * 16 + 4);
                    idInFile =(int) BitConverter.ToInt64(content, objectOffset + objectInfo.ByteStart + 4 + i * 16 + 8);
                }
 
                SerializedObjectIdentifier c = new SerializedObjectIdentifier(serializedFileIndex,idInFile);
                //objectOffset + objectInfo.ByteStart+4 + i * 12开始的4个字节未保存
                ret.componentIdentifierList.Add(c);

                //GameObject的Component的serializedFileIndex肯定为0


                SerializedObjectIdentifierWithFile cf = new SerializedObjectIdentifierWithFile();
                cf.soi = c;
                cf.fullFileName = objectInfo.UnityFile.FullFileName;

                if(Analyzer.componentDict.ContainsKey(cf)==false)
                {
                    Analyzer.componentDict.Add(cf, ret);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("SerializedObjectIdentifier 重复:" + c, "Error");
                }
            }

            int componentInfoSize = 0;
            if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
            {
                componentInfoSize = 12;
            }
            else if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                componentInfoSize = 16;
            }

            int index = objectOffset + objectInfo.ByteStart + 4 + componentCount * componentInfoSize;
            //4个字节的m_Layer
            ret.layer = BitConverter.ToUInt32(content, index); index += 4;
            //4个字节的名字的长度
            int stringLength = BitConverter.ToInt32(content, index); index += 4;
            ret.name = "";
            for (int i = 0; i < stringLength; i++)
            {
                ret.name += (char)content[index + i];
            }
            //仅用来区分同一个Scene下的各个不同的同名GameObject
            ret.name += "_" + objectInfo.Id;

            index += stringLength;
            index+=Util.GetAlignCount(index, objectOffset);
            ret.tag = BitConverter.ToInt16(content, index); index += 2;
            ret.isActive = (content[index++] == 1);
            return ret;
        }

        public override string ToString()
        {
            string ret = "[name="+this.name+"][tag="+this.tag+"][isActive="+this.isActive+"]\n";
            foreach (SerializedObjectIdentifier c in componentIdentifierList)
            {
                ret += "componentInfo:" + c.serializedFileIndex.ToString("x") + " " + c.identifierInFile.ToString("x") + "\n";
            }
            return ret;
        }

        public void CreateGameObjectPanel()
        {
            if (gameObjectPanel == null)
            {
                gameObjectPanel = new GameObjectPanel();
                gameObjectPanel.SetGameObject(this);
            }
        }

        /// <summary>
        /// index表示该gameobject是第几个子节点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GameObject GetParentGameObject(ref int index)
        {
            Transform transform = GetTransformComponent();
            if (transform == null)
            {
                return null;
            }
            if (transform.GetParentTransform() != null)
            {
                Transform parentTransform=transform.GetParentTransform();
                index = parentTransform.ChildTransforms.IndexOf(transform);
                return parentTransform.GetGameObject();
            }
            return null;
        }

        
    }
}
