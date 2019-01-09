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
    /// RendererPanel.xaml 的交互逻辑
    /// </summary>
    public partial class RendererPanel : UserControl
    {
        private Renderer renderer;
        public RendererPanel()
        {
            InitializeComponent();
        }

        public void SetRenderer(Renderer renderer)
        {
            this.renderer=renderer;
            this.txtEnabled.Text = renderer.IsEnabled.ToString();
            this.txtCastShadows.Text = renderer.CastShadows.ToString();

            this.txtReceiveShadows.Text = renderer.ReceiveShadows.ToString();
            this.txtLightmapIndex.Text = "0x"+renderer.LightMapIndex.ToString("X");

            this.txtlightmapST.Text=renderer.LightMapST.ToString();

            Transform t=renderer.GetStaticBatchRoot();
            if (t == null)
            {
                this.txtStaticBatchRoot.Text = "";
                btnSpriteBatchRoot.Visibility = Visibility.Hidden;
            }
            else
            {
                this.txtStaticBatchRoot.Text = t.GetGameObject().GName;
                btnSpriteBatchRoot.Visibility = Visibility.Visible;
            }

            this.txtLightProbe.Text = renderer.LightProbe.ToString();

            t = renderer.GetLightProbeAnchor();
            if (t == null)
            {
                this.txtLightProbeAnchor.Text = "";
                btnLightProbeAnchor.Visibility = Visibility.Hidden;
            }
            else
            {
                this.txtLightProbeAnchor.Text = t.GetGameObject().GName;
                btnLightProbeAnchor.Visibility = Visibility.Visible;
            }


            SortingLayerEntry sle=renderer.GetSortingLayer();
            if(sle!=null)
            {
                this.txtSortingLayer.Text = sle.Name;
            }

            this.txtSortingOrder.Text = renderer.SortingOrder.ToString();
            
            lblMaterial.Content="Material count="+renderer.Materials.Count.ToString();

            this.lbMaterial.ItemsSource = renderer.GetMaterials();

            lblSubsetIndex.Content = "Subset index count=" + renderer.SubsetIndices.Count.ToString();
            this.lbSubsetIndex.ItemsSource = renderer.SubsetIndices;

            this.txtReflectionProbes.Text = renderer.ReflectionProbes.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Transform t = renderer.GetStaticBatchRoot();
            MainWindow.instance.ShowUnityObject(t.GetGameObject());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Transform t = renderer.GetLightProbeAnchor();
            MainWindow.instance.ShowUnityObject(t.GetGameObject());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UnityObject m = this.lbMaterial.SelectedItem as UnityObject;
            if (m != null)
            {
                MainWindow.instance.ShowUnityObject(m);
            }
        }
    }
}
