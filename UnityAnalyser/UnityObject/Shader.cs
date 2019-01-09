using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;

namespace UnityAnalyzer
{
    public class Shader:UnityObject
    {
        private string shaderName;
        public string ShaderName
        {
            get { return shaderName; }
        }

        private string sourceCode;
        public string SourceCode
        {
            get { return sourceCode; }
        }

        private string pathName;
        public string PathName
        {
            get { return pathName; }
        }

        private List<SerializedObjectIdentifier> dependencies;
        public List<SerializedObjectIdentifier> Dependencies
        {
            get { return dependencies; }
        }

        private bool isBaked;
        public bool IsBaked
        {
            get { return isBaked; }
        }

        public static Shader Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Shader ret = new Shader();

            int index = objectOffset + objectInfo.ByteStart;
            ret.shaderName=Util.readStringAndAlign(content, objectOffset,ref index);

            ret.sourceCode = Util.readStringAndAlign(content, objectOffset, ref index);
            ret.pathName = Util.readStringAndAlign(content, objectOffset, ref index);

            ret.dependencies = new List<SerializedObjectIdentifier>();
            if (objectInfo.UnityFileVersion[0] == 4 &&
                objectInfo.UnityFileVersion[1] == 6)
            {
                int dependencisCount = BitConverter.ToInt32(content, index);
                index += 4;
                for (int i = 0; i < dependencisCount; i++)
                {
                    int serializedFileIndex = BitConverter.ToInt32(content, index);
                    index += 4;

                    int idinFile = BitConverter.ToInt32(content, index);
                    index += 4;

                    ret.dependencies.Add(new SerializedObjectIdentifier(serializedFileIndex, idinFile));
                }
                ret.isBaked = (content[index] == 1);
            }
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
                        ShaderPanel panel = new ShaderPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public void SaveToFile()
        {
            System.Windows.Forms.SaveFileDialog open = new System.Windows.Forms.SaveFileDialog();
            open.FileName = shaderName + ".shader";
            open.Filter = "shader文件（*.shader）|*.shader|所有文件(*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = open.FileName;
                Transform(filename);
                if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show(
                    "已经保存到了文件:" + filename + "，是否需要打开目录",
                    "",
                    System.Windows.Forms.MessageBoxButtons.OKCancel))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = "/e,/select," + filename;
                    System.Diagnostics.Process.Start(psi);
                }
            }
        }

        public void Transform(string fileName)
        {
            FileStream file_stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(file_stream);
            streamWriter.Write(this.sourceCode);
            streamWriter.Close();
            file_stream.Close();
        }
    }
}
