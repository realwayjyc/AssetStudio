using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class MeshFilter : Component
    {
        protected bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        protected SerializedObjectIdentifier mesh;

        public SerializedObjectIdentifier Mesh
        {
            get { return mesh; }
        }



        public static MeshFilter Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            MeshFilter ret = new MeshFilter();
            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 8;
            serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + index);
            identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4 + index);

            ret.mesh = new SerializedObjectIdentifier(serializedFileIndex, identifier);
            return ret;
        }

        public UnityObject GetMesh()
        {
            UnityObject ret = this.GetUnityObjectBySerializedObjectIdentifier(this.mesh);
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
                        MeshFilterPanel panel = new MeshFilterPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public override UserControl CreateGameObjectComponentInfoControl()
        {
            if (gameObjectComponentInfoControl == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MeshFilterPanel panel = new MeshFilterPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
