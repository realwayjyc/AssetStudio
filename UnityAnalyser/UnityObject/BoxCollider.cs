using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnityAnalyzer
{
    public class BoxCollider:Component
    {
        public SerializedObjectIdentifier physicalMaterial;
        public Boolean isTrigger;
        public float sizeX;
        public float sizeY;
        public float sizeZ;

        public float centerX;
        public float centerY;
        public float centerZ;
        public static BoxCollider Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            BoxCollider ret = new BoxCollider();
            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;
            index = objectOffset + objectInfo.ByteStart + 8 + Util.GetSerializedFileIndexIdRange(objectInfo);

            ret.physicalMaterial=Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            index++;
            ret.isTrigger = (content[index] != 0);

            int alignCount = Util.GetAlignCount(index, objectOffset);
            index += alignCount;

            ret.sizeX = BitConverter.ToSingle(content, index); index += 4;
            ret.sizeY = BitConverter.ToSingle(content, index); index += 4;
            ret.sizeZ = BitConverter.ToSingle(content, index); index += 4;

            ret.centerX = BitConverter.ToSingle(content, index); index += 4;
            ret.centerY = BitConverter.ToSingle(content, index); index += 4;
            ret.centerZ = BitConverter.ToSingle(content, index); index += 4;

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
                        GeneralObjectPanel panel = new GeneralObjectPanel();
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
                        GeneralObjectPanel panel = new GeneralObjectPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
