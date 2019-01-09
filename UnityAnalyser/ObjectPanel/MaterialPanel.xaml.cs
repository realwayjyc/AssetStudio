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
    /// MaterialPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialPanel : UserControl
    {
        private Material material;
        public Material Material
        {
            get { return material; }
        }


        public MaterialPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(Material material)
        {
            this.material = material;
            this.txtMaterialName.Text = this.material.Name;

            Shader shader = material.GetShader();
            if (shader != null)
            {
                this.txtShader.Text = shader.ShaderName;
            }

            string keywordlist="";
            foreach(string keyword in this.material.KeyWordList)
            {
                keywordlist+=keyword+";";
            }
            this.txtKeywordsList.Text = keywordlist;
            this.txtCustomRenderQueue.Text = "0x"+this.material.CustomRenderQueue.ToString("X");

            this.unityPropertySheetPanel.SetUnityPropertySheet(material.UnityPropertySheet);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Shader shader=  this.material.GetShader();
            if (shader != null)
            {
                MainWindow.instance.ShowUnityObject(shader);
            }
        }
    }
}
