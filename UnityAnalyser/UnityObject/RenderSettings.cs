using System;
using System.Drawing;
using System.Windows.Controls;
using UnityAnalyzer.ObjectPanel;

namespace UnityAnalyzer
{
    public class RenderSettings : UnityObject
    {
        public enum FOG_MODE
        {
            Linear = 1,
            Exponential = 2,
            Exp_2 = 3
        }

        public bool Fog { get; set; }
        public Color FogColor { get; set; }
        public FOG_MODE FogMode { get; set; }
        public double FogDensity { get; set; }
        public double LinearFogStart { get; set; }
        public double LinearFogEnd { get; set; }
        public Color AmbientLight { get; set; }
        public SerializedObjectIdentifier SkyBoxMaterial { get; set; }
        public double HaloStrength { get; set; }
        public double FlareStrength { get; set; }
        public double FlareFadeSpeed { get; set; }
        public SerializedObjectIdentifier HaloTexture { get; set; }
        public SerializedObjectIdentifier SpotCookie { get; set; }

        public static RenderSettings Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            RenderSettings ret = new RenderSettings();
            int index = objectOffset + objectInfo.ByteStart;

            ret.Fog = (1==BitConverter.ToInt32(content, index));
            index += 4;


            float colorR = BitConverter.ToSingle(content, index);
            index += 4;

            float colorG = BitConverter.ToSingle(content, index);
            index += 4;

            float colorB = BitConverter.ToSingle(content, index);
            index += 4;

            float colorA = BitConverter.ToSingle(content, index);
            index += 4;

            ret.FogColor = Color.FromArgb((int)(colorA * 255), (int)(colorR * 255), (int)(colorG * 255), (int)(colorB * 255));

            ret.FogMode =(FOG_MODE) BitConverter.ToInt32(content, index);
            index += 4;

            ret.FogDensity= BitConverter.ToSingle(content, index);
            index += 4;

            ret.LinearFogStart = BitConverter.ToSingle(content, index);
            index += 4;

            ret.LinearFogEnd = BitConverter.ToSingle(content, index);
            index += 4;

            colorR = BitConverter.ToSingle(content, index);
            index += 4;

            colorG = BitConverter.ToSingle(content, index);
            index += 4;

            colorB = BitConverter.ToSingle(content, index);
            index += 4;

            colorA = BitConverter.ToSingle(content, index);
            index += 4;

            ret.AmbientLight = Color.FromArgb((int)(colorA * 255), (int)(colorR * 255), (int)(colorG * 255), (int)(colorB * 255));

            ret.SkyBoxMaterial = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            ret.HaloStrength = BitConverter.ToSingle(content, index);
            index += 4;

            ret.FlareStrength = BitConverter.ToSingle(content, index);
            index += 4;

            ret.FlareFadeSpeed = BitConverter.ToSingle(content, index);
            index += 4;

            ret.HaloTexture = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            ret.SpotCookie = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

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
                        RenderSettingsPanel panel = new RenderSettingsPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public Material GetSkyBoxMaterial()
        {
            Material ret = this.GetUnityObjectBySerializedObjectIdentifier(this.SkyBoxMaterial) as Material;
            return ret;
        }

        public Texture2D GetHaloTexture()
        {
            Texture2D ret = this.GetUnityObjectBySerializedObjectIdentifier(this.HaloTexture) as Texture2D;
            return ret;
        }

        public Texture2D GetSpotCookie()
        {
            Texture2D ret = this.GetUnityObjectBySerializedObjectIdentifier(this.SpotCookie) as Texture2D;
            return ret;
        }
    }
}