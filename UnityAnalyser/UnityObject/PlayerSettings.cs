using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class PlayerSettings : UnityObject
    {
        public bool androidProfiler;
        public int defaultScreenOrientation;
        public int targetDevice;
        public int targetGlesGraphics;
        public int targetIOSGraphics;
        public int targetResolution;
        public int accelerometerFrequency;
        public string companyName;
        public string productName;
        public SerializedObjectIdentifier defaultCursor;
        public Vector2F cursorHotSpot;
        public int defaultScreenWidth;
        public int defaultScreenHeight;
        public int defaultWebScreenWidth;
        public int defaultWebScreenHeight;
        public RENDERING_PATH renderingPath;
        public RENDERING_PATH mobileRenderingPath;
        public int activeColorSpace;

        public bool mtRendering;
        public bool mobileMTRendering;
        public bool useDirextX11;
        public bool stereoscopic3D;

        public int iosShowActivityIndicatorOnLoading;
        public int androidShowActivityIndicatorOnLoading;
        public int displayResolutionDialog;

        public bool uiAutoRotateToPortrait;
        public bool uiAutoRotateToPortraitUpsideDown;
        public bool uiAutoRotateToLandscapeRight;
        public bool uiAutoRotateToLandscapeLeft;
        public bool uiUseAnimatedAutoRotation;
        public bool uiUse32BitDisplayBuffer;
        public bool uiUse24BitDepthBuffer;

        public bool defaultIsFullScreen;
        public bool defaultIsNativeResolution;
        public bool runInBackground;
        public bool captureSingleScreen;
        public bool overrideIPodMusic;
        public bool prepareIOSForRecording;
        public bool enableHWStatistics;
        public bool usePlayerLog;
        public bool stripPhysics;
        public bool forceSingleInstance;
        public bool resizableWindow;
        public bool useMacAppStoreValidation;

        public bool gpuSkinning;
        public bool xboxPIXTextureCapture;
        public bool xboxEnableAvatar;
        public bool xboxEnableKinect;
        public bool xboxEnableKinectAutoTracking;
        public bool xboxEnableFitness;
        public bool visibleInBackground;

        public int macFullscreenMode;
        public int d3d9FullscreenMode;
        public bool d3d11ForceExclusiveMode;

        public int xboxSpeechDB;
        public bool xboxEnableHeadOrientation;
        public bool xboxEnableGuest;

        public int videoMemoryForVertexBuffers;

        public bool aspectRatio_4v3;
        public bool aspectRatio_5v4;
        public bool aspectRatio_16v10;
        public bool aspectRatio_16v9;
        public bool aspectRatio_Others;

        public string iPhoneBundleIdentifier;


        public bool metroEnableIndependentInputSource;
        public bool metroEnableLowLatencyPresentationAPI;

        public static PlayerSettings Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            if (!(objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
                && !(objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3))
            {
                //暂时不解析
                return null;
            }
          
            int index = objectOffset + objectInfo.ByteStart;
            PlayerSettings ret = new PlayerSettings();
            //if ((objectInfo.UnityFileVersion[0] == 5))
            //{

            //  ret.activeColorSpace = BitConverter.ToInt32(content, index + 0x80);
            //    ret.renderingPath = (RENDERING_PATH)BitConverter.ToInt32(content, index + 0x80 - 8);
            //    return ret;
            //}
            int initIndex = index;
            if ((objectInfo.UnityFileVersion[0] != 5))
            {
                ret.androidProfiler = (content[index++] == 1);
                index += Util.GetAlignCount(index, objectOffset);
            }

            ret.defaultScreenOrientation = BitConverter.ToInt32(content, index);
            index += 4;

            ret.targetDevice = BitConverter.ToInt32(content, index);
            index += 4;

            ret.targetGlesGraphics = BitConverter.ToInt32(content, index);
            index += 4;

            ret.targetIOSGraphics = BitConverter.ToInt32(content, index);
            index += 4;

            if (ret.targetIOSGraphics==9000)
            {
                ret.targetIOSGraphics = 4;
            }

            ret.targetResolution = BitConverter.ToInt32(content, index);
            index += 4;
            if (ret.targetResolution==1)
            {
                ret.targetResolution = 6;
            }
            else if (ret.targetResolution==2)
            {
                ret.targetResolution = 0;
            }

            if ((objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6
                && (objectInfo.UnityFileVersion[2] == 7|| objectInfo.UnityFileVersion[2] == 4)))
            {
                ret.accelerometerFrequency = BitConverter.ToInt32(content, index);
                index += 4;
            }


            ret.companyName = Util.readStringAndAlign(content, objectOffset, ref index);
            ret.productName = Util.readStringAndAlign(content, objectOffset, ref index);
            if ((objectInfo.UnityFileVersion[0] == 5))
            {
                ret.androidProfiler = (content[index++] == 1);
                index += Util.GetAlignCount(index, objectOffset);
            }

            int serializedFileIndex = BitConverter.ToInt32(content, index);
            index += 4;
            int identifier = BitConverter.ToInt32(content, index);
            index += 4;

            ret.defaultCursor = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            ret.cursorHotSpot = new Vector2F();
            ret.cursorHotSpot.X = BitConverter.ToSingle(content, index);
            index += 4;

            ret.cursorHotSpot.Y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.defaultScreenWidth = BitConverter.ToInt32(content, index);
            index += 4;

            ret.defaultScreenHeight = BitConverter.ToInt32(content, index);
            index += 4;

            ret.defaultWebScreenWidth = BitConverter.ToInt32(content, index);
            index += 4;

            ret.defaultWebScreenHeight = BitConverter.ToInt32(content, index);
            index += 4;

            if ((objectInfo.UnityFileVersion[0] == 5))
            {
                index += 0x10;
            }
            ret.renderingPath = (RENDERING_PATH)BitConverter.ToInt32(content, index);
            index += 4;

            ret.mobileRenderingPath = (RENDERING_PATH)BitConverter.ToInt32(content, index);
            index += 4;

            ret.activeColorSpace = BitConverter.ToInt32(content, index);
            index += 4;
            if ((objectInfo.UnityFileVersion[0] == 5))
            {
                return ret;
            }

            ret.mtRendering = (1==content[index++]);
            ret.mobileMTRendering = (1 == content[index++]);
            ret.useDirextX11 = (1 == content[index++]);
            ret.stereoscopic3D = (1 == content[index++]);

            index += Util.GetAlignCount(index, objectOffset);

            ret.iosShowActivityIndicatorOnLoading = BitConverter.ToInt32(content, index);
            index += 4;

            ret.androidShowActivityIndicatorOnLoading = BitConverter.ToInt32(content, index);
            index += 4;

            ret.displayResolutionDialog = BitConverter.ToInt32(content, index);
            index += 4;

            ret.uiAutoRotateToPortrait = (1 == content[index++]);
            ret.uiAutoRotateToPortraitUpsideDown = (1 == content[index++]);
            ret.uiAutoRotateToLandscapeRight = (1 == content[index++]);
            ret.uiAutoRotateToLandscapeLeft = (1 == content[index++]);
            ret.uiUseAnimatedAutoRotation = (1 == content[index++]);
            ret.uiUse32BitDisplayBuffer = (1 == content[index++]);
            ret.uiUse24BitDepthBuffer = (1 == content[index++]);
            index += Util.GetAlignCount(index, objectOffset);

            ret.defaultIsFullScreen = (1 == content[index++]);
            ret.defaultIsNativeResolution = (1 == content[index++]);
            ret.runInBackground = (1 == content[index++]);
            ret.captureSingleScreen = (1 == content[index++]);

            ret.overrideIPodMusic = (1 == content[index++]);
            ret.prepareIOSForRecording = (1 == content[index++]);
            ret.enableHWStatistics = (1 == content[index++]);
            ret.usePlayerLog = (1 == content[index++]);
            ret.stripPhysics = (1 == content[index++]);
            ret.forceSingleInstance = (1 == content[index++]);
            ret.resizableWindow = (1 == content[index++]);
            ret.useMacAppStoreValidation = (1 == content[index++]);
            ret.gpuSkinning = (1 == content[index++]);
            ret.xboxPIXTextureCapture = (1 == content[index++]);
            ret.xboxEnableAvatar = (1 == content[index++]);
            ret.xboxEnableKinect = (1 == content[index++]);
            ret.xboxEnableKinectAutoTracking = (1 == content[index++]);
            ret.xboxEnableFitness = (1 == content[index++]);
            ret.visibleInBackground = (1 == content[index++]);
            index += Util.GetAlignCount(index, objectOffset);

            ret.macFullscreenMode=BitConverter.ToInt32(content, index);
            index += 4;

            ret.d3d9FullscreenMode = BitConverter.ToInt32(content, index);
            index += 4;

            ret.d3d11ForceExclusiveMode = (1 == content[index++]);
            index += Util.GetAlignCount(index, objectOffset);

            ret.xboxSpeechDB = BitConverter.ToInt32(content, index);
            index += 4;

            ret.xboxEnableHeadOrientation = (1 == content[index++]);
            ret.xboxEnableGuest = (1 == content[index++]);
            index += Util.GetAlignCount(index, objectOffset);

            ret.videoMemoryForVertexBuffers = BitConverter.ToInt32(content, index);
            index += 4;

            ret.aspectRatio_4v3 = (1 == content[index++]);
            ret.aspectRatio_5v4 = (1 == content[index++]);
            ret.aspectRatio_16v10 = (1 == content[index++]);
            ret.aspectRatio_16v9 = (1 == content[index++]);
            ret.aspectRatio_Others = (1 == content[index++]);

            index += Util.GetAlignCount(index, objectOffset);

            ret.iPhoneBundleIdentifier = Util.readStringAndAlign(content, objectOffset, ref index);

            ret.metroEnableIndependentInputSource = (1 == content[index++]);
            ret.metroEnableLowLatencyPresentationAPI = (1 == content[index++]);
            index += Util.GetAlignCount(index, objectOffset);

            if (index-initIndex!= objectInfo.ByteSize)
            {
                System.Windows.Forms.MessageBox.Show("Player setting parse error");
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
                        PlayerSettingPanel panel = new PlayerSettingPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }
    }
}
