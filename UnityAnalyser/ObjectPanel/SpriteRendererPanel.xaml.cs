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
    /// SpriteRendererPanel.xaml 的交互逻辑
    /// </summary>
    public partial class SpriteRendererPanel : UserControl
    {
        private SpriteRenderer spriteRenderer;

        public SpriteRendererPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;

            Sprite sprite=spriteRenderer.GetSprite();
            if (sprite == null)
            {
                this.txtSprite.Text = "[NULL]";
                this.btnOpenSprite.IsEnabled = false;
            }
            else
            {
                this.txtSprite.Text = sprite.SpriteName;
                this.btnOpenSprite.IsEnabled = true;
            }

            this.txtColor.Text = spriteRenderer.Color.R + "   " + spriteRenderer.Color.G + "   " + spriteRenderer.Color.B + "   " + spriteRenderer.Color.A;

            this.txtFlipX.Text = spriteRenderer.FlipX.ToString();
            this.txtFlipY.Text = spriteRenderer.FlipY.ToString();


            this.rendererPanel.SetRenderer(spriteRenderer);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.ShowUnityObject(spriteRenderer.GetSprite());
        }
    }
}
