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
    /// ShaderPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ShaderPanel : UserControl
    {

        private Shader shader;

        public ShaderPanel()
        {
            InitializeComponent();
        }


        public void SetUnityObject(Shader shader)
        {
            this.shader = shader;

            this.txtShaderName.Text = shader.ShaderName;
            this.txtPathName.Text = shader.PathName;
            this.txtDependenciesCount.Text = shader.Dependencies.Count.ToString();
            this.txtIsBaked.Text = shader.IsBaked.ToString();

            this.txtSourceCode.Text = shader.SourceCode;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.shader.SaveToFile();
        }
    }
}
