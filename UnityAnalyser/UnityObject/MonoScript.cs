using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class MonoScript:UnityObject
    {
        private string scriptName;
        public string ScriptName
        {
            get { return scriptName; }
        }

        private string scriptClassName;
        public string ScriptClassName
        {
            get { return scriptClassName; }
        }

        private string scriptNamespace;
        public string ScriptNamespace
        {
            get { return scriptNamespace; }
        }

        private string scriptAssemblyName;
        public string ScriptAssemblyName
        {
            get { return scriptAssemblyName; }
        }

        public static MonoScript Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            MonoScript ret = new MonoScript();
            int length = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);

            int index = objectOffset + objectInfo.ByteStart + 4;
            string scriptName = "";
            for (int i = 0; i < length; i++)
            {
                scriptName += (char)content[index + i];
            }
            index += length;
            int alignCount=Util.GetAlignCount(index, objectOffset);
            index += alignCount;

            index += 8;

            if (objectInfo.UnityFileVersion[0] == 5 && 
                objectInfo.UnityFileVersion[1] == 3)
            {
                index += 0xc;
            }

            length = BitConverter.ToInt32(content, index);
            index += 4;
            string scriptClassName = "";
            for (int i = 0; i < length; i++)
            {
                scriptClassName += (char)content[index + i];
            }
            index += length;
            alignCount = Util.GetAlignCount(index, objectOffset);
            index += alignCount;

            length = BitConverter.ToInt32(content, index);
            index += 4;
            string scriptNamespace = "";
            if (length != 0)
            {
                for (int i = 0; i < length; i++)
                {
                    scriptNamespace += (char)content[index + i];
                }
                index += length;
                alignCount = Util.GetAlignCount(index, objectOffset);
                index += alignCount;
            }

            length = BitConverter.ToInt32(content, index);
            index += 4;
            string scriptAssemblyName = "";
            for (int i = 0; i < length; i++)
            {
                scriptAssemblyName += (char)content[index + i];
            }

            ret.scriptNamespace = scriptNamespace;
            ret.scriptClassName = scriptClassName;
            ret.scriptAssemblyName = scriptAssemblyName;
            ret.scriptName = scriptName;
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
                        MonoScriptPanel panel = new MonoScriptPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }
    }
}
