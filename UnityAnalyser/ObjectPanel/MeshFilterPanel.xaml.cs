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
    /// MeshFilterPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MeshFilterPanel : UserControl
    {
        private MeshFilter meshFilter;
        public MeshFilterPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(MeshFilter meshFilter)
        {
            this.meshFilter = meshFilter;
            UnityObject o = meshFilter.GetMesh();
            this.txtMeshInFile.Text = o.UnityFile.AliasFileName;
            this.txtMeshID.Text = "0x"+o.Id.ToString("x");
            this.txtMeshDLS.Text = "0x" + o.DebugLineStart.ToString("x");

            this.txtMeshNameDef.Text = "";
            if(o.DebugLineStart==0x27e1)
            {
                this.txtMeshNameDef.Text = "Plane";
            }
        }


    }
}
