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

    public enum Interpolate
    {
        None=0,
        Interpolate,
        Extrapolate,
    }

    public enum CollisionDetection
    {
        Discrete=0,
        Continuous,
        ContinuousDynamic
    }

    public class RigidBody:Component
    {
        public float mass;
        public float drag;
        public float angularDrag;
        public bool useGravity;
        public bool isKinematic;
        public Interpolate interpolate;
        public CollisionDetection collisionDetection;

        public Boolean freezePositionX;
        public Boolean freezePositionY;
        public Boolean freezePositionZ;

        public Boolean freezeRotationX;
        public Boolean freezeRotationY;
        public Boolean freezeRotationZ;


        public static RigidBody Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            RigidBody ret = new RigidBody();
            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;
            index = objectOffset + objectInfo.ByteStart + 8 + Util.GetSerializedFileIndexIdRange(objectInfo);

            ret.mass = BitConverter.ToSingle(content, index);
            index += 4;

            ret.drag = BitConverter.ToSingle(content, index);
            index += 4;

            ret.angularDrag = BitConverter.ToSingle(content, index);
            index += 4;

            ret.useGravity = (content[index] != 0);
            index++;

            ret.isKinematic = (content[index] != 0);
            index++;

            ret.interpolate = (Interpolate)(content[index]);
            index++;

            int alignCount = Util.GetAlignCount(index, objectOffset);
            index += alignCount;

            int value = BitConverter.ToInt32(content, index);
            index += 4;

            string tag = Convert.ToString(value, 2);
            while (tag.Length<8)
            {
                tag = "0" + tag;
            }
            char[] array = tag.ToCharArray();
            Array.Reverse(array);

            ret.freezePositionX = (array[1] == '1');
            ret.freezePositionY = (array[2] == '1');
            ret.freezePositionZ = (array[3] == '1');

            ret.freezeRotationX = (array[4] == '1');
            ret.freezeRotationY = (array[5] == '1');
            ret.freezeRotationZ = (array[6] == '1');


            ret.collisionDetection = (CollisionDetection)(BitConverter.ToInt32(content,index));
            index+=4;

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
