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
using System.Drawing;

namespace UnityAnalyzer
{
    /// <summary>
    /// Texture2DPanel.xaml 的交互逻辑
    /// </summary>
    public partial class Texture2DPanel : UserControl
    {
        private Texture2D texture2D;
        public Texture2D Texture2D
        {
            get { return texture2D; }
        }

        public Texture2DPanel()
        {
            InitializeComponent();
        }

        private RotateFlipType rotationFlipType = RotateFlipType.RotateNoneFlipNone;
        public RotateFlipType RotationFlipType
        {
            get { return rotationFlipType; }
            set { rotationFlipType = value; }
        }

        public void SetUnityObject(Texture2D texture2D)
        {
            this.texture2D = texture2D;

            this.txtTexutreName.Text = texture2D.TextureName;
            this.txtImageSize.Text = "0x"+texture2D.ImageSize.ToString("X");

            this.txtTextureWidth.Text = texture2D.TextureWidth.ToString();
            this.txtTextureHeight.Text = texture2D.TextureHeight.ToString();

            txtTextureFormat.Text = texture2D.Format.ToString();
            txtMipMap.Text = texture2D.MipMap.ToString();

            txtIsReadable.Text = texture2D.IsReadable.ToString();
            txtReadAllowed.Text = texture2D.ReadAllowed.ToString();

            txtImageCount.Text = texture2D.ImageCount.ToString();
            txtTexutreDimesion.Text = texture2D.TextureDimension.ToString();

            txtFilterMode.Text = texture2D.TextureSettings.FilterMode.ToString();
            txtAniso.Text = texture2D.TextureSettings.Aniso.ToString();

            txtMipBias.Text = texture2D.TextureSettings.MipBias.ToString();
            txtWrapMode.Text = texture2D.TextureSettings.WrapMode.ToString();

            txtTextureUsageMode.Text = texture2D.UsageMode.ToString();
            txtTextureColorSpace.Text = texture2D.ColorSpace.ToString();

            txtImageSizexiamgecount.Text = "0x" + texture2D.ImageSizeXImageCount.ToString("X");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //保存内容
            this.texture2D.SaveToFile(rotationFlipType);
        }
    }
}
