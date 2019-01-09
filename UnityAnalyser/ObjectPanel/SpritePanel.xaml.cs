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
    /// SpritePanel.xaml 的交互逻辑
    /// </summary>
    public partial class SpritePanel : UserControl
    {
        private Sprite sprite;
        public Sprite Sprite
        {
            get { return sprite; }
        }

        public SpritePanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(Sprite sprite)
        {
            this.sprite = sprite;
            this.txtSpriteName.Text = sprite.SpriteName;
            this.txtRectangle.Text=Util.GetRectangleString(sprite.Rect);

            this.txtOffset.Text = sprite.OffsetX.ToString()+" "+sprite.OffsetY.ToString();
            this.txtExtrude.Text = sprite.Extrude.ToString();

            this.txtBorder.Text = sprite.Border.ToString();
            this.txtPixelsToUnits.Text = sprite.PixelsToUnits.ToString();

            this.spriteRenderDataPanel.SetSpritRenderData(sprite.SpriteRenderData);
        }
    }
}
