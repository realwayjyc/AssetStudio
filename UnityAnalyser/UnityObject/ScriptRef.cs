using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;


namespace UnityAnalyzer
{
    /// <summary>
    /// ClassType小于0的类型，一般是指向其他文件中一段脚本的索引
    /// </summary>
    public class ScriptRef : Component
    {
        /// <summary>
        /// 当前正在解析的ScriptRef
        /// </summary>
        private static ScriptRef currentParsingScriptRef = null;

        public static ScriptRef CurrentParsingScriptRef
        {
            get { return ScriptRef.currentParsingScriptRef; }
        }

        /// <summary>
        /// 这段脚本的信息在哪个Asset文件中
        /// </summary>
        private int scriptInfoFileIndex;
        public int ScriptInfoFileIndex
        {
            get { return scriptInfoFileIndex; }
        }

        public AssetsFile ScriptInfoFile
        {
            get
            {
                return this.UnityFile.GetSerializedUnityFileByIndex(scriptInfoFileIndex) as AssetsFile;
            }
        }

        /// <summary>
        /// 这段脚本的信息在Asset文件中的Identifier
        /// </summary>
        private int scriptIdentifier;
        public int ScriptIdentifier
        {
            get { return scriptIdentifier; }
            set { scriptIdentifier = value; }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
        }

        //对于CanvasScaler,因为它有其他一些信息，这些信息先存放在scriptInfoContent数组中，
        //然后根据Script的内容来决定如何解析这些内容
        private byte[] scriptInfoContent;
        public byte[] ScriptInfoContent
        {
            get { return scriptInfoContent; }
            set { scriptInfoContent = value; }
        }

        /// <summary>
        /// 该脚本类在编辑器中可以设置的变量名，及其值
        /// </summary>
        private ScriptFieldValueSet scriptFiledValueSet;
        public ScriptFieldValueSet ScriptFiledValueSet
        {
            get { return scriptFiledValueSet; }
            set { scriptFiledValueSet = value; }
        }

        /// <summary>
        /// 显示该脚本类各个变量名及其值的列表控件
        /// </summary>
        private ScriptFieldsPanel scriptFieldsPanel;

        public ScriptFieldsPanel ScriptFieldsPanel
        {
            get { return scriptFieldsPanel; }
            set { scriptFieldsPanel = value; }
        }

        private bool isScriptableObject = false;
        public bool IsScriptableObject
        {
            get { return isScriptableObject; }
        }

        private string scriptableObjectName;
        public string ScriptableObjectName
        {
            get { return scriptableObjectName; }
        }

        public static ScriptRef Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            ScriptRef ret = new ScriptRef();

            if(objectInfo.UnityFile.AliasFileName.EndsWith(".assets"))
            {
                //这个ScriptRef是一个ScriptableObject;
                ret.isScriptableObject = true;
            }

            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int iterIndex = 8 + Util.GetSerializedFileIndexIdRange(objectInfo);
            ret.isActive = (BitConverter.ToInt32(content, iterIndex + objectOffset + objectInfo.ByteStart) == 1);
            iterIndex += 4;

            ret.scriptInfoFileIndex = BitConverter.ToInt32(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.scriptIdentifier = BitConverter.ToInt32(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);

            if (ret.isScriptableObject==false)
            {
                iterIndex += 4;
            }
            else
            {
                int outIndex=iterIndex + objectOffset + objectInfo.ByteStart;
                string name = Util.readStringAndAlign(content, objectOffset, ref outIndex);
                ret.scriptableObjectName = name;
                iterIndex = outIndex - (objectOffset + objectInfo.ByteStart);
            }

            ret.scriptInfoContent = new byte[objectInfo.ByteSize-iterIndex];
            Array.Copy(content, iterIndex + objectOffset + objectInfo.ByteStart, ret.scriptInfoContent, 0, objectInfo.ByteSize - iterIndex);

            ret.scriptFiledValueSet = null;

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
                        ScriptRefPanel panel = new ScriptRefPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        /// <summary>
        /// 得到该ScriptRef所指向的MonoScript
        /// </summary>
        /// <returns></returns>
        public MonoScript GetMonoScriptRef()
        {
            return ScriptInfoFile.GetUnityObjectByIdentifier(scriptIdentifier) as MonoScript;
        }

        public override UserControl CreateGameObjectComponentInfoControl()
        {
            if (gameObjectComponentInfoControl == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        ScriptRefPanel panel = new ScriptRefPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }


        public void HandleScriptRefInfoByMonoScript(MonoScript monoScript)
        {
            if (this.scriptFiledValueSet != null) return;

            currentParsingScriptRef = this;
            //解析该脚本类中的变量，并从scriptInfoContent中获得这些变量的值，存入scriptFiledValueSet
            this.scriptFiledValueSet = ScriptFieldValueSet.Parse(monoScript, scriptInfoContent,this);

            if(scriptFieldsPanel==null)
            {
                scriptFieldsPanel = new ScriptFieldsPanel();
            }
            scriptFieldsPanel.HandleValues(this.scriptFiledValueSet);
        }
    }
}
