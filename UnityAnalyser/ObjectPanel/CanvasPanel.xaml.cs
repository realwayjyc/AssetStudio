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
    /// CanvasPanel.xaml 的交互逻辑
    /// </summary>
    public partial class CanvasPanel : UserControl
    {
        public CanvasPanel()
        {
            InitializeComponent();
        }
        private Camera eventCamera;
        public void SetUnityObject(Canvas canvas)
        {
            txtEnabled.Text = canvas.IsEnabled.ToString();
            txtRenderMode.Text = canvas.RenderMode.ToString();
            
            eventCamera=canvas.GetEventCamera();
            if (eventCamera != null)
            {
                txtEventCamera.Text = eventCamera.GetGameObject().Name;
                btnOpenEventCamera.Visibility = Visibility.Visible;
            }
            else
            {
                txtEventCamera.Text = "NULL";
                btnOpenEventCamera.Visibility = Visibility.Hidden;
            }

            txtPlaneDistance.Text = canvas.PlaneDistance.ToString();
            txtPixelPerfect.Text = canvas.PixelPerfect.ToString();
            txtReceiveEvents.Text = canvas.ReceivesEvents.ToString();

            txtOverrideSorting.Text = canvas.OverrideSorting.ToString();
            txtOverridePixelPerfect.Text = canvas.OverridePixelPerfect.ToString();

            txtSortingLayer.Text = canvas.GetSortingLayerName();
            txtSortingOrder.Text = canvas.SortingOrder.ToString();

            this.txtTargetDisplay.Text = canvas.TargetDisplay.ToString();
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            if (eventCamera != null)
            {
                MainWindow.instance.ShowGameObjectTabItem(eventCamera.GetGameObject());
            }
        }
    }
}
