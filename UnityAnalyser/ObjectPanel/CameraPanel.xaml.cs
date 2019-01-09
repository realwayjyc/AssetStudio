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
    /// CameraPanel.xaml 的交互逻辑
    /// </summary>
    public partial class CameraPanel : UserControl
    {
        private Camera camera;
        public Camera Camera
        {
            get { return camera; }
        }

        public CameraPanel()
        {
            InitializeComponent();
        }

        private List<CullingMaskItem> cullingMaskList = new List<CullingMaskItem>();

        public void SetUnityObject(Camera camera)
        {
            this.camera = camera;
            this.txtEnabled.Text = camera.IsEnabled.ToString();
            this.txtClearFlag.Text = camera.ClearFlags.ToString();
            this.txtColor.Text = Util.GetColorRgbaString(camera.BackgroundColor);
            this.txtNormalizedViewport.Text = camera.NormalizedViewportRect.Left + "  " + camera.NormalizedViewportRect.Top + "  " + camera.NormalizedViewportRect.Width + "  " + camera.NormalizedViewportRect.Height;
            this.txtNearClip.Text = camera.NearClip.ToString();
            this.txtFarClip.Text = camera.FarClip.ToString();
            this.txtProjection.Text = (camera.IsOrthographic?"Orthographic":"Perspective").ToString();
            this.txtOrthographicSize.Text = camera.OrthographicSize.ToString();
            this.txtDepth.Text = camera.Depth.ToString();
            this.txtFieldOfView.Text = camera.FieldOfView.ToString();

            
            for (int i = 0; i < 32; i++)
            {
                CullingMaskItem ci = new CullingMaskItem();
                ci.LayerName = camera.UnityFile.Analyzer.TagManager.Layers[i];
                ci.IsChecked = camera.CullingMask[i];
                cullingMaskList.Add(ci);
            }

            this.txtRenderingPath.Text = camera.RenderingPath.ToString();
            if (camera.RenderTextureIdentifier.serializedFileIndex == 0 && camera.RenderTextureIdentifier.identifierInFile == 0)
            {
                this.txtRenderingTexture.Text = "     ";
            }
            else
            {
                this.txtRenderingTexture.Text = camera.RenderTextureIdentifier.ToString();
            }

            txtTargetDisplay.Text = camera.TargetDisplay.ToString();
            txtHDR.Text = camera.Hdr.ToString();

            txtOcclusionCulling.Text = camera.OcclusionCulling.ToString();
            txtStereoConvergence.Text = camera.StereoConvergence.ToString();
            txtStereoSeparation.Text = camera.StereoSeparation.ToString();

        }

        private void txtCullingMask_Click(object sender, RoutedEventArgs e)
        {
            CullingMaskWnd wnd = new CullingMaskWnd();
            wnd.dgCullingMask.ItemsSource = cullingMaskList;
            wnd.ShowDialog();
           
        }

    }

    public class CullingMaskItem
    {
        private string layerName;

        public string LayerName
        {
            get { return layerName; }
            set { layerName = value; }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

    }
}
