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
    /// ScriptRegPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptRefPanel : UserControl
    {
        private ScriptRef scriptRef;
        public ScriptRef ScriptRef
        {
            get { return scriptRef; }
        }

        /// <summary>
        /// 该scriptRef所指向的monoScript的Panel
        /// </summary>
        private MonoScriptPanel refMonoScriptPanel;

        public ScriptRefPanel()
        {
            InitializeComponent();
            scriptRef = null;
            refMonoScriptPanel = null;
        }

        public void SetUnityObject(ScriptRef scriptRef)
        {
            this.scriptRef = scriptRef;
            this.txtScriptInfoFile.Text = scriptRef.ScriptInfoFile.AliasFileName;
            this.txtScriptId.Text = "0x" + scriptRef.ScriptIdentifier.ToString("x");
            this.lblActive.Content = scriptRef.IsActive ? "" : "Active=FALSE";

            if (refMonoScriptPanel == null)
            {
                refMonoScriptPanel = new MonoScriptPanel();
            }
            gridMonoScriptPanel.Children.Clear();
            MonoScript monoScript = scriptRef.GetMonoScriptRef();
            scriptRef.HandleScriptRefInfoByMonoScript(monoScript);
            refMonoScriptPanel.SetUnityObject(monoScript);

            refMonoScriptPanel.VerticalAlignment = VerticalAlignment.Stretch;
            refMonoScriptPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            gridMonoScriptPanel.Children.Add(refMonoScriptPanel);

            //设置Script附加信息的控件，例如CanvasScaler的Script有Scale Factor，Ui scale mode等信息
            UserControl usercontrol = scriptRef.ScriptFieldsPanel;
            if (usercontrol!=null)
            {
                if (usercontrol.Parent!=null)
                {
                    Grid grid = usercontrol.Parent as Grid;
                    grid.Children.Remove(usercontrol);
                }
                scriptRefInfoPanel.Children.Add(usercontrol);
            }
        }
    }
}
