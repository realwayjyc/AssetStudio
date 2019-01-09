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
    /// <summary>
    /// LightPanel.xaml 的交互逻辑
    /// </summary>
    public partial class LightPanel : UserControl
    {
        private Light light;
        public Light Light
        {
            get { return light; }
        }

        private List<CullingMaskItem> cullingMaskList = new List<CullingMaskItem>();

        public LightPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(Light light)
        {
            this.light = light;
            this.txtEnabled.Text = light.IsEnabled.ToString();

            for (int i = 0; i < 32; i++)
            {
                CullingMaskItem ci = new CullingMaskItem();
                ci.LayerName = light.UnityFile.Analyzer.TagManager.Layers[i];
                ci.IsChecked = light.CullingMask[i];
                cullingMaskList.Add(ci);
            }
            this.txtLightType.Text = light.LightType.ToString();
            this.txtColor.Text = Util.GetColorRgbaString(light.Color);
            this.txtIntensity.Text = light.Intensity.ToString();
            this.txtRange.Text = light.Range.ToString();
            this.txtSpotAngle.Text = light.SpotAngle.ToString();
            this.txtCookieSize.Text = light.CookieSize.ToString();
            this.txtShadowType.Text = light.ShadowType.ToString();
            this.txtResolution.Text = light.Resolution.ToString();
            this.txtStrength.Text = light.Strength.ToString();
            this.txtBias.Text = light.Bias.ToString();
            this.txtSoftness.Text = light.Softness.ToString();
            this.txtDrawHalo.Text = light.DrawHalo.ToString();
            this.txtActuallyLightmapped.Text = light.ActuallyLightmapped.ToString();
            this.txtRenderMode.Text = light.RenderMode.ToString();
            this.txtLightMapping.Text = light.LightMapping.ToString();
            this.txtshadowSamples.Text = light.ShadowSamples.ToString();
            this.txtShadowRadius.Text = light.ShadowRadius.ToString();
            this.txtShadowAngle.Text = light.ShadowAngle.ToString();
            this.txtIndirectIntensity.Text = light.IndirectIntensity.ToString();
            this.txtAreaSizeWidth.Text = light.AreaSizeWidth.ToString();
            this.txtAreaSizeHeight.Text = light.AreaSizeHeight.ToString();
            this.txtSoftnessFade.Text = light.SoftnessFade.ToString();


            this.txtCookie.Text = light.Cookie.ToString();
            this.txtFlare.Text = light.Flare.ToString();


            //this.txtClearFlag.Text = camera.ClearFlags.ToString();
            //this.txtColor.Text = Util.GetColorRgbaString(camera.BackgroundColor);
            //this.txtNormalizedViewport.Text = camera.NormalizedViewportRect.Left + "  " + camera.NormalizedViewportRect.Top + "  " + camera.NormalizedViewportRect.Width + "  " + camera.NormalizedViewportRect.Height;
            //this.txtNearClip.Text = camera.NearClip.ToString();
            //this.txtFarClip.Text = camera.FarClip.ToString();
            //this.txtProjection.Text = (camera.IsOrthographic ? "Orthographic" : "Perspective").ToString();
            //this.txtOrthographicSize.Text = camera.OrthographicSize.ToString();
            //this.txtDepth.Text = camera.Depth.ToString();
            //this.txtFieldOfView.Text = camera.FieldOfView.ToString();


           

            //this.txtRenderingPath.Text = camera.RenderingPath.ToString();
            //if (camera.RenderTextureIdentifier.serializedFileIndex == 0 && camera.RenderTextureIdentifier.identifierInFile == 0)
            //{
            //    this.txtRenderingTexture.Text = "     ";
            //}
            //else
            //{
            //    this.txtRenderingTexture.Text = camera.RenderTextureIdentifier.ToString();
            //}

            //txtTargetDisplay.Text = camera.TargetDisplay.ToString();
            //txtHDR.Text = camera.Hdr.ToString();

            //txtOcclusionCulling.Text = camera.OcclusionCulling.ToString();
            //txtStereoConvergence.Text = camera.StereoConvergence.ToString();
            //txtStereoSeparation.Text = camera.StereoSeparation.ToString();

        }

        private void txtCullingMask_Click(object sender, RoutedEventArgs e)
        {
            CullingMaskWnd wnd = new CullingMaskWnd();
            wnd.dgCullingMask.ItemsSource = cullingMaskList;
            wnd.ShowDialog();

        }
    }
}
