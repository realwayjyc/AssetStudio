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
    public class Cubemap : UnityObject
    {
        private Texture2D texture2D;
        public Texture2D Texture2D
        {
            get { return texture2D; }
        }

        public string CubeMapName
        {
            get
            {
                return texture2D.TextureName;
            }
        }

        private List<SerializedObjectIdentifier> sourceTexturesList;
        public List<SerializedObjectIdentifier> SourceTexturesList
        {
            get { return sourceTexturesList; }
        }

        public static Cubemap Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Cubemap ret = new Cubemap();
            int index=0;
            ret.texture2D = Texture2D.Create(objectInfo, content, objectOffset, ref index);
            ret.sourceTexturesList=new List<SerializedObjectIdentifier>();
            int count = BitConverter.ToInt32(content, index);
            index += 4;
            for(int i=0;i<count;i++)
            {
                ret.sourceTexturesList.Add(
                    Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo));
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
                        CubemapPanel panel = new CubemapPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }
    }
}
