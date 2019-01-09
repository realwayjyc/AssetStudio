using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public enum CLEAR_FLAGS
    {
        SKY_BOX=1,
        SOLID_COLOR=2,
        DEPTH_ONLY=3,
        DONT_CLEAR=4
    }

    public enum RENDERING_PATH
    {
        USE_PLAYER_SETTING=-1,
        VERTEX_LIT___LEGACY_VERTEX_LIT= 0,
        FORWARD=1,
        DEFERRED_LIGHTING___LEGACY_DEFERRED_LIGHT_PREPASS= 2,
        DEFERRED=3,
    }


    public class Camera:Component
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        private CLEAR_FLAGS clearFlags;
        public CLEAR_FLAGS ClearFlags
        {
            get { return clearFlags; }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        private Color backgroundColor;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private RectangleF normalizedViewportRect;
        public RectangleF NormalizedViewportRect
        {
            get { return normalizedViewportRect; }
        }

        private float nearClip;
        public float NearClip
        {
            get { return nearClip; }
        }

        private float farClip;
        public float FarClip
        {
            get { return farClip; }
        }

        private float fieldOfView;
        public float FieldOfView
        {
            get { return fieldOfView; }
        }

        private bool isOrthographic;
        public bool IsOrthographic
        {
            get { return isOrthographic; }
        }

        private float orthographicSize;
        public float OrthographicSize
        {
            get { return orthographicSize; }
        }

        private float depth;
        public float Depth
        {
            get { return depth; }
        }

        private bool[] cullingMask=new bool[32];
        public bool[] CullingMask
        {
            get { return cullingMask; }
        }

        private RENDERING_PATH renderingPath;
        public RENDERING_PATH RenderingPath
        {
            get { return renderingPath; }
        }


        private SerializedObjectIdentifier renderTextureIdentifier;
        public SerializedObjectIdentifier RenderTextureIdentifier
        {
            get { return renderTextureIdentifier; }
        }

        private TARGET_DISPLAY targetDisplay;
        public TARGET_DISPLAY TargetDisplay
        {
            get { return targetDisplay; }
        }

        private bool hdr;
        public bool Hdr
        {
            get { return hdr; }
        }

        private bool occlusionCulling;
        public bool OcclusionCulling
        {
            get { return occlusionCulling; }
        }

        private float stereoConvergence;
        public float StereoConvergence
        {
            get { return stereoConvergence; }
        }


        private float stereoSeparation;
        public float StereoSeparation
        {
            get { return stereoSeparation; }
        }

        public static Camera Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Camera ret = new Camera();

            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;

            index=objectOffset + objectInfo.ByteStart + 8+Util.GetSerializedFileIndexIdRange(objectInfo);
            ret.isEnabled = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);

            ret.clearFlags=(CLEAR_FLAGS)BitConverter.ToInt32(content, index);
            index += 4;

            float colorR=BitConverter.ToSingle(content, index);
            index += 4;

            float colorG = BitConverter.ToSingle(content, index);
            index += 4;

            float colorB = BitConverter.ToSingle(content, index);
            index += 4;

            float colorA = BitConverter.ToSingle(content, index);
            index += 4;

            ret.backgroundColor = Color.FromArgb((int)(colorA * 255), (int)(colorR * 255), (int)(colorG * 255),(int)(colorB * 255));

            ret.normalizedViewportRect.X = BitConverter.ToSingle(content, index);
            index += 4;

            ret.normalizedViewportRect.Y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.normalizedViewportRect.Width = BitConverter.ToSingle(content, index);
            index += 4;

            ret.normalizedViewportRect.Height = BitConverter.ToSingle(content, index);
            index += 4;

            ret.nearClip = BitConverter.ToSingle(content, index);
            index += 4;

            ret.farClip = BitConverter.ToSingle(content, index);
            index += 4;

            ret.fieldOfView = BitConverter.ToSingle(content, index);
            index += 4;

            ret.isOrthographic=(content[index++]==1);
            index += Util.GetAlignCount(index, objectOffset);

            ret.orthographicSize = BitConverter.ToSingle(content, index);
            index += 4;

            ret.depth = BitConverter.ToSingle(content, index);
            index += 4;

            int cullingMask = BitConverter.ToInt32(content, index);
            index += 4;

            for (int i = 0; i < 32; i++)
            {
                if ((cullingMask & (1 << i)) != 0)
                {
                    ret.cullingMask[i] = true;
                }
                else
                {
                    ret.cullingMask[i] = false;
                }
            }

            ret.renderingPath =(RENDERING_PATH) BitConverter.ToInt32(content, index);
            index += 4;

            int serializedFileIndex_ = BitConverter.ToInt32(content, index);
            index += 4;

            int identifierInFile_ = BitConverter.ToInt32(content, index);
            index += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);

            ret.renderTextureIdentifier = new SerializedObjectIdentifier(serializedFileIndex_, identifierInFile_);

            ret.targetDisplay =(TARGET_DISPLAY) BitConverter.ToInt32(content, index);
            index += 4;

            ret.hdr = (content[index++] == 1);
            ret.occlusionCulling = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);

            ret.stereoConvergence = BitConverter.ToSingle(content, index);
            index += 4;

            ret.stereoSeparation = BitConverter.ToSingle(content, index);
            index += 4;
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
                        CameraPanel panel = new CameraPanel();
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
                        CameraPanel panel = new CameraPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
