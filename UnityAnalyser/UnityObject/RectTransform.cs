using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class RectTransform:Transform
    {
        private float anchorMinX;
        public float AnchorMinX
        {
            get { return anchorMinX; }
        }

        private float anchorMinY;
        public float AnchorMinY
        {
            get { return anchorMinY; }
        }

        private float anchorMaxX;
        public float AnchorMaxX
        {
            get { return anchorMaxX; }
        }

        private float anchorMaxY;
        public float AnchorMaxY
        {
            get { return anchorMaxY; }
        }

        private float anchoredPosX;
        public float AnchoredPosX
        {
            get { return anchoredPosX; }
        }

        private float anchoredPosY;
        public float AnchoredPosY
        {
            get { return anchoredPosY; }
        }

        private float sizeDeltaW;
        public float SizeDeltaW
        {
            get { return sizeDeltaW; }
        }

        private float sizeDeltaH;
        public float SizeDeltaH
        {
            get { return sizeDeltaH; }
        }

        private float pivotX;
        public float PivotX
        {
            get { return pivotX; }
        }

        private float pivotY;
        public float PivotY
        {
            get { return pivotY; }
        }
        
        public new static RectTransform Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            RectTransform ret = new RectTransform();
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

            ret.anchorMinX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.anchorMinY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.anchorMaxX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.anchorMaxY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.anchoredPosX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.anchoredPosY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.sizeDeltaW = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.sizeDeltaH = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.pivotX = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
            iterIndex += 4;

            ret.pivotY = BitConverter.ToSingle(content, iterIndex + objectOffset + objectInfo.ByteStart);
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
                        RectTransformPanel panel = new RectTransformPanel();
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
                        RectTransformPanel panel = new RectTransformPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
