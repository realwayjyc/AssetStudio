using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public enum CANVAS_RENDERMODE
    {
        ScreenSpaceOverlay  = 0,
        ScreenSpaceCamera  = 1,
        WorldSpace =2
    }

    public enum TARGET_DISPLAY
    {
        DISPLAY_1,
        DISPLAY_2,
        DISPLAY_3,
        DISPLAY_4,
        DISPLAY_5,
        DISPLAY_6,
        DISPLAY_7,
        DISPLAY_8,
    }
    public class Canvas : Component
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        private CANVAS_RENDERMODE renderMode;
        public CANVAS_RENDERMODE RenderMode
        {
            get { return renderMode; }
        }

        private SerializedObjectIdentifier eventCameraIdentifier;
        public SerializedObjectIdentifier EventCameraIdentifier
        {
            get { return eventCameraIdentifier; }
       }

        private float planeDistance;
        public float PlaneDistance
        {
            get { return planeDistance; }
        }

        private bool pixelPerfect;
        public bool PixelPerfect
        {
            get { return pixelPerfect; }
        }

        private bool receivesEvents;
        public bool ReceivesEvents
        {
            get { return receivesEvents; }
        }

        private bool overrideSorting;
        public bool OverrideSorting
        {
            get { return overrideSorting; }
        }

        private bool overridePixelPerfect;
        public bool OverridePixelPerfect
        {
            get { return overridePixelPerfect; }
        }

        private uint sortingLayerID;
        public uint SortingLayerID
        {
            get { return sortingLayerID; }
        }

        private short sortingOrder;
        public short SortingOrder
        {
            get { return sortingOrder; }
        }

        private TARGET_DISPLAY targetDisplay;
        public TARGET_DISPLAY TargetDisplay
        {
            get { return targetDisplay; }
        }

        public static Canvas Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Canvas ret = new Canvas();
            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;
            index = objectOffset + objectInfo.ByteStart + 8 + Util.GetSerializedFileIndexIdRange(objectInfo);
            ret.isEnabled = (content[index++] == 1);
            index += Util.GetAlignCount(index, objectOffset);

            ret.renderMode = (CANVAS_RENDERMODE)BitConverter.ToInt32(content, index);
            index += 4;

            int serializedFileIndex_ = BitConverter.ToInt32(content, index);
            index += 4;
            int identifierInFile_ = BitConverter.ToInt32(content, index);
            index += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);

            ret.eventCameraIdentifier = new SerializedObjectIdentifier(serializedFileIndex_, identifierInFile_);


            ret.planeDistance = BitConverter.ToSingle(content, index);
            index += 4;

            ret.pixelPerfect = (content[index++] == 1);
            ret.receivesEvents = (content[index++] == 1);
            ret.overrideSorting = (content[index++] == 1);
            ret.overridePixelPerfect = (content[index++] == 1);

            if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                //未知4字节
                index += 4;
            }

            ret.sortingLayerID = BitConverter.ToUInt32(content, index);
            index += 4;

            ret.sortingOrder = BitConverter.ToInt16(content, index);
            index += 2;

            ret.targetDisplay =(TARGET_DISPLAY) BitConverter.ToInt16(content, index);


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
                        CanvasPanel panel = new CanvasPanel();
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
                        CanvasPanel panel = new CanvasPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }

        public Camera GetEventCamera()
        {
            if (eventCameraIdentifier.identifierInFile == 0 && eventCameraIdentifier.serializedFileIndex == 0)
            {
                return null;
            }
            AssetsFile assetsFile=this.UnityFile.GetSerializedUnityFileByIndex(eventCameraIdentifier.serializedFileIndex) as AssetsFile;
            if (assetsFile != null)
            {
                UnityObject un= assetsFile.GetUnityObjectByIdentifier(eventCameraIdentifier.identifierInFile);
                if (un is Camera)
                {
                    return un as Camera;
                }
            }
            throw new Exception("Error camera type");
        }

        public string GetSortingLayerName()
        {
           SortingLayerEntry entry= this.UnityFile.Analyzer.TagManager.GetSortingLayerEntryByUniqueID(this.SortingLayerID);
           if (entry == null)
           {
               return "NULL";
           }
           return entry.Name;
        }
    }
}
