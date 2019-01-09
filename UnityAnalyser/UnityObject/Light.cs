using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;


namespace UnityAnalyzer
{

    public enum LIGHT_TYPE
    {
        Spot=0,
        Directional=1,
        Point,
        Area
    }

    public enum SHADOW_TYPE
    {
        NO_SHADOWS,
        HARD_SHADOWS,
        SOFT_SHADOWS
    }

    public enum RESOLUTION
    {
        Use_Quality_Setting=-1,
        Low_Resolution=0,
        Medium_Resolution=1,
        High_Resolution=2,
        Very_High_Resolution=3,
    }

    public enum RENDER_MODE
    {
        Auto=0,
        Important,
        Not_Important
    }

    public enum LIGHT_MAPPING
    {
        RealTime_Only,
        Auto,
        Baked_Only,
    }

    public class Light : Component
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        private LIGHT_TYPE lightType;
        public LIGHT_TYPE LightType
        {
            get { return lightType; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
        }

        private float intensity;
        public float Intensity
        {
            get { return intensity; }
        }

        private float range;
        public float Range
        {
            get { return range; }
        }

        private float spotAngle;
        public float SpotAngle
        {
            get { return spotAngle; }
        }


        private float cookieSize;
        public float CookieSize
        {
            get { return cookieSize; }
        }

        private SHADOW_TYPE shadowType;
        public SHADOW_TYPE ShadowType
        {
            get { return shadowType; }
        }

        private RESOLUTION resolution;
        public RESOLUTION Resolution
        {
            get { return resolution; }
        }

        private float strength;
        public float Strength
        {
            get { return strength; }
        }

        private float bias;
        public float Bias
        {
            get { return bias; }
        }

        private float softness;
        public float Softness
        {
            get { return softness; }
        }

        private float softnessFade;
        public float SoftnessFade
        {
            get { return softnessFade; }
        }


        private SerializedObjectIdentifier cookie;
        public SerializedObjectIdentifier Cookie
        {
            get { return cookie; }
        }

        private bool drawHalo;
        public bool DrawHalo
        {
            get { return drawHalo; }
        }

        private bool actuallyLightmapped;
        public bool ActuallyLightmapped
        {
            get { return actuallyLightmapped; }
        }

        private SerializedObjectIdentifier flare;
        public SerializedObjectIdentifier Flare
        {
            get { return flare; }
        }

        private RENDER_MODE renderMode;
        public RENDER_MODE RenderMode
        {
            get { return renderMode; }
        }

        private bool[] cullingMask = new bool[32];
        public bool[] CullingMask
        {
            get { return cullingMask; }
        }

        private LIGHT_MAPPING lightMapping;
        public LIGHT_MAPPING LightMapping
        {
            get { return lightMapping; }
        }

        private int shadowSamples;
        public int ShadowSamples
        {
            get { return shadowSamples; }
        }


        private float shadowRadius;
        public float ShadowRadius
        {
            get { return shadowRadius; }
        }

        private float shadowAngle;
        public float ShadowAngle
        {
            get { return shadowAngle; }
        }

        private float indirectIntensity;
        public float IndirectIntensity
        {
            get { return indirectIntensity; }
        }

        private float areaSizeWidth;
        public float AreaSizeWidth
        {
            get { return areaSizeWidth; }
        }

        private float areaSizeHeight;
        public float AreaSizeHeight
        {
            get { return areaSizeHeight; }
        }

        public static Light Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Light ret = new Light();

            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;

            index = objectOffset + objectInfo.ByteStart + 8;
            ret.isEnabled = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);


            ret.lightType = (LIGHT_TYPE)BitConverter.ToInt32(content, index);
            index += 4;

                float colorR=BitConverter.ToSingle(content, index);
            index += 4;

            float colorG = BitConverter.ToSingle(content, index);
            index += 4;

            float colorB = BitConverter.ToSingle(content, index);
            index += 4;

            float colorA = BitConverter.ToSingle(content, index);
            index += 4;

            ret.color = Color.FromArgb((int)(colorA * 255), (int)(colorR * 255), (int)(colorG * 255),(int)(colorB * 255));

            ret.intensity = BitConverter.ToSingle(content, index);
            index += 4;

            ret.range = BitConverter.ToSingle(content, index);
            index += 4;

            ret.spotAngle = BitConverter.ToSingle(content, index);
            index += 4;

            ret.cookieSize = BitConverter.ToSingle(content, index);
            index += 4;


            ret.shadowType = (SHADOW_TYPE)BitConverter.ToInt32(content, index);
            index += 4;

            ret.resolution = (RESOLUTION)BitConverter.ToInt32(content, index);
            index += 4;

            ret.strength = BitConverter.ToSingle(content, index);
            index += 4;

            ret.bias = BitConverter.ToSingle(content, index);
            index += 4;

            ret.softness = BitConverter.ToSingle(content, index);
            index += 4;

            ret.softnessFade = BitConverter.ToSingle(content, index);
            index += 4;


            int serializedFileIndex_ = BitConverter.ToInt32(content, index);
            index += 4;

            int identifierInFile_ = BitConverter.ToInt32(content, index);
            index += 4;

            ret.cookie = new SerializedObjectIdentifier(serializedFileIndex_, identifierInFile_);

            ret.drawHalo = (content[index++] != 0);
            ret.actuallyLightmapped = (content[index++] != 0);

            index += Util.GetAlignCount(index, objectOffset);


            serializedFileIndex_ = BitConverter.ToInt32(content, index);
            index += 4;

            identifierInFile_ = BitConverter.ToInt32(content, index);
            index += 4;

            ret.flare = new SerializedObjectIdentifier(serializedFileIndex_, identifierInFile_);

            ret.renderMode = (RENDER_MODE)BitConverter.ToInt32(content, index);
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

            ret.lightMapping = (LIGHT_MAPPING)BitConverter.ToInt32(content, index);
            index += 4;

            ret.shadowSamples = BitConverter.ToInt32(content, index);
            index += 4;

            ret.shadowRadius = BitConverter.ToSingle(content, index);
            index += 4;

            ret.shadowAngle = BitConverter.ToSingle(content, index);
            index += 4;


            ret.indirectIntensity = BitConverter.ToSingle(content, index);
            index += 4;

            ret.areaSizeWidth = BitConverter.ToSingle(content, index);
            index += 4;

            ret.areaSizeHeight = BitConverter.ToSingle(content, index);
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
                        LightPanel panel = new LightPanel();
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
                        LightPanel panel = new LightPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
