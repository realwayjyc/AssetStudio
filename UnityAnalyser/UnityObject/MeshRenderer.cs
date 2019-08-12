using System;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class MeshRenderer : Renderer
    {
        public static MeshRenderer Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            MeshRenderer ret = new MeshRenderer();
            int index = objectOffset + objectInfo.ByteStart;
            index = ret.CreateRenderer(objectInfo, content, objectOffset, index);

            return ret;


            //int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            //int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            //ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            //int index = 0;
            //index = objectOffset + objectInfo.ByteStart + 8+Util.GetSerializedFileIndexIdRange();

            //if (MainWindow.instance.CurrentAnalyzer.fileVersion == "4.6.7f1")
            //{
            //    ret.isEnabled = (content[index++] == 1);
            //    ret.castShadows = (content[index++] == 1);
            //    ret.receiveShadows = (content[index++] == 1);
            //    ret.lightMapIndex = content[index++];
            //}
            //else if (MainWindow.instance.CurrentAnalyzer.fileVersion == "5.3.5f1")
            //{
            //    ret.isEnabled = (BitConverter.ToInt32(content, index)!=0);
            //    index += 4;
            //}
            

            //ret.lightMapST.x = BitConverter.ToSingle(content, index); index += 4;
            //ret.lightMapST.y = BitConverter.ToSingle(content, index); index += 4;
            //ret.lightMapST.z = BitConverter.ToSingle(content, index); index += 4;
            //ret.lightMapST.w = BitConverter.ToSingle(content, index); index += 4;


            //ret.materials = Util.ReadSerializedObjectIdentifierList(content, ref index);
            //int count = BitConverter.ToInt32(content, index); index += 4;
            //for (int i = 0; i < count; i++)
            //{
            //    ret.subsetIndices.Add(BitConverter.ToUInt32(content, index));
            //    index += 4;
            //}

            //ret.staticBatchRootTransform = Util.ReadNextSerializedObjectIdentifier(content, ref index);

            //ret.lightProbe = (content[index++] == 1);
            //index += Util.GetAlignCount(index, objectOffset);

            //ret.lightProbeAnchorTransform = Util.ReadNextSerializedObjectIdentifier(content, ref index);

            //ret.sortingLayerID = BitConverter.ToUInt32(content, index);
            //index += 4;

            //ret.sortingOrder = BitConverter.ToInt16(content, index);
            //index += 2;
            //index += Util.GetAlignCount(index, objectOffset);



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
                        MeshRendererPanel panel = new MeshRendererPanel();
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
                        MeshRendererPanel panel = new MeshRendererPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
