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
    /// UnityPropertySheetPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UnityPropertySheetPanel : UserControl
    {
        private UnityPropertySheet unityPropertySheet;

        public UnityPropertySheetPanel()
        {
            InitializeComponent();
        }

        public void SetUnityPropertySheet(UnityPropertySheet unityPropertySheet)
        {
            this.unityPropertySheet = unityPropertySheet;

            this.dgUnityTexEnv.ItemsSource = unityPropertySheet.NamedUnityTexEnvPropertyDict.Values;

            this.dgFloatProperty.ItemsSource = unityPropertySheet.NamedFloatPropertyDict.Values;

            this.dgColor4fProperty.ItemsSource = unityPropertySheet.NamedColorPropertyDict.Values;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UnityPropertySheet.UnityTexEnv obj = ((FrameworkElement)sender).DataContext as UnityPropertySheet.UnityTexEnv;
            if (obj == null) return;
            Texture2D texture2D=obj.GetTexture2D();
            if (texture2D != null)
            {
                MainWindow.instance.ShowUnityObject(texture2D);
            }
        }
    }
}
