using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace UnityAnalyzer
{
    /// <summary>
    /// CubemapPanel.xaml 的交互逻辑
    /// </summary>
    public partial class CubemapPanel : UserControl
    {
        public CubemapPanel()
        {
            InitializeComponent();
        }

        private Cubemap cubeMap;
        public Cubemap Cubemap
        {
            get { return cubeMap; }
        }

        public void SetUnityObject(Cubemap cubeMap)
        {
            this.cubeMap = cubeMap;
            if(texture2dPanel.Children.Count==0)
            {
                Texture2DPanel p = cubeMap.Texture2D.CreateObjectInfoPanel() as Texture2DPanel;
                p.RotationFlipType = RotateFlipType.RotateNoneFlipXY;
                texture2dPanel.Children.Add(p);
            }
        }
    }
}
