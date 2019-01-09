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
    /// SpriteRenderDataPanel.xaml 的交互逻辑
    /// </summary>
    public partial class SpriteRenderDataPanel : UserControl
    {
        private SpriteRenderData spriteRenderData;

        private List<Vetex3fDgItem> vertexList = new List<Vetex3fDgItem>();
        private List<IndexDgItem> indexList = new List<IndexDgItem>();

        public SpriteRenderDataPanel()
        {
            InitializeComponent();
        }

        public void SetSpritRenderData(SpriteRenderData spriteRenderData)
        {
            this.spriteRenderData = spriteRenderData;
            Texture2D t=spriteRenderData.GetTexture2D();
            if (t == null)
            {
                this.txtTexture2D.Text = "[ERROR:NULL]";
            }
            else
            {
                this.txtTexture2D.Text = t.TextureName;
            }

            this.txtRectangle.Text=Util.GetRectangleString(spriteRenderData.TextureRect);

            this.txtTextureOffset.Text = spriteRenderData.TextureOffsetY.ToString() + "  " + spriteRenderData.TextureOffsetY.ToString();
            this.txtSetting.Text = spriteRenderData.Setting.ToString();

            this.txtUVTransform.Text = spriteRenderData.UvTransform.ToString();

            for (int i=0;i<spriteRenderData.Vertices.Count;i++)
            {
                vertexList.Add(new Vetex3fDgItem(spriteRenderData.Vertices[i], i));
            }

            this.dgVertex.ItemsSource = vertexList;

            for (int i = 0; i < spriteRenderData.Indices.Count; i++)
            {
                indexList.Add(new IndexDgItem(i,spriteRenderData.Indices[i]));
            }
            this.dgIndex.ItemsSource = indexList;

            this.lblVertexCount.Content = "Vertex list count=" + vertexList.Count;
            this.lblIndexCount.Content = "Index list count=" + indexList.Count;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.instance.ShowUnityObject(spriteRenderData.GetTexture2D());
        }

        private void dgVertex_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           
        }
    }
}
