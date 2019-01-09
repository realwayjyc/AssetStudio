using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    /// <summary>
    /// Transform的数值是针对其父Transform的数值
    /// </summary>
    public class Transform : Component
    {
        ///////////////////////////////  POSITION   /////////////////////////
        protected float positionX;
        public float PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        protected float positionY;
        public float PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }

        protected float positionZ;
        public float PositionZ
        {
            get { return positionZ; }
            set { positionZ = value; }
        }

        ///////////////////////////////  ROTATION   /////////////////////////
        protected float rotationX;
        public float RotationX
        {
            get { return rotationX; }
            set { rotationX = value; }
        }

        protected float rotationY;
        public float RotationY
        {
            get { return rotationY; }
            set { rotationY = value; }
        }

        protected float rotationZ;
        public float RotationZ
        {
            get { return rotationZ; }
            set { rotationZ = value; }
        }

        ///////////////////////////////  SCALE   /////////////////////////
        protected float scaleX;
        public float ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }

        protected float scaleY;
        public float ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }

        protected float scaleZ;
        public float ScaleZ
        {
            get { return scaleZ; }
            set { scaleZ = value; }
        }

        /// <summary>
        /// 其实是子GameObject的Transform
        /// </summary>
        protected List<SerializedObjectIdentifier> childTransformsIdentifier;
        public List<SerializedObjectIdentifier> ChildTransformsIdentifier
        {
            get { return childTransformsIdentifier; }
        }

        /// <summary>
        /// 父GameObject的Transform
        /// </summary>
        protected SerializedObjectIdentifier parentTransformIdentifier;
        public SerializedObjectIdentifier ParentTransformIdentifier
        {
            get { return parentTransformIdentifier; }
        }

        public List<Transform> ChildTransforms
        {
            get
            {
                List<Transform> childTransform = new List<Transform>();
                foreach (SerializedObjectIdentifier so in childTransformsIdentifier)
                {
                    AssetsFile unityFile = this.UnityFile.GetSerializedUnityFileByIndex(so.serializedFileIndex) as AssetsFile;
                    Transform t=unityFile.GetUnityObjectByIdentifier(so.identifierInFile) as Transform;
                    childTransform.Add(t);
                }
                return childTransform;
            }
        }

        protected int rootOrder;
        public int RootOrder
        {
            get { return rootOrder; }
        }



        public static Transform Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Transform ret = new Transform();

            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int iterIndex = 8 + Util.GetSerializedFileIndexIdRange(objectInfo);
            float qX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;
            float qY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;
            float qZ = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;
            float qW = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            float xAngle = 0;
            float yAngle = 0;
            float zAngle = 0;

            Util.Quaternion2Euler(qX, qY, qZ, qW, out xAngle, out yAngle, out zAngle);

            ret.rotationX = xAngle;
            ret.rotationY = yAngle;
            ret.rotationZ = zAngle;

            ret.positionX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.positionY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.positionZ = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.scaleX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;


            ret.scaleY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.scaleZ = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.childTransformsIdentifier = new List<SerializedObjectIdentifier>();
            int childCount = BitConverter.ToInt32(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;
            for (int i = 0; i < childCount; i++)
            {
                int _serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + iterIndex);
                iterIndex += 4;
                int _identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + iterIndex);
                iterIndex += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);
                ret.childTransformsIdentifier.Add(new SerializedObjectIdentifier(_serializedFileIndex, _identifier));
            }

            serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + iterIndex);
            iterIndex += 4;
            identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + iterIndex);
            iterIndex += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);

            ret.parentTransformIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            ret.rootOrder = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + iterIndex);
            iterIndex += 4;
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
                        TransformPanel panel = new TransformPanel();
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
                        TransformPanel panel = new TransformPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }

        public Transform GetParentTransform()
        {
            AssetsFile unityFile = this.UnityFile.GetSerializedUnityFileByIndex(parentTransformIdentifier.serializedFileIndex) as AssetsFile;
            return unityFile.GetUnityObjectByIdentifier(parentTransformIdentifier.identifierInFile) as Transform;
        }
    }
}
