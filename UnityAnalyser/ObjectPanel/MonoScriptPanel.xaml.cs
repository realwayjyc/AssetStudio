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
    /// MonoScriptPanel.xaml 的交互逻辑
    /// </summary>
    public partial class MonoScriptPanel : UserControl
    {
        private MonoScript monoScript;
        public MonoScript MonoScript
        {
            get { return monoScript; }
        }

        public MonoScriptPanel()
        {
            InitializeComponent();
            monoScript = null;
        }

        public void SetUnityObject(MonoScript monoScript)
        {
            this.monoScript = monoScript;
            if (monoScript == null)
            {
                this.txtAssemblyname.Text = "ERROR: INVALID MONO SCRIPT";
                this.txtClassname.Text = "ERROR: INVALID MONO SCRIPT";
                this.txtNamespace.Text = "ERROR: INVALID MONO SCRIPT";
                this.txtScriptName.Text = "ERROR: INVALID MONO SCRIPT";
            }
            else
            {
                this.txtAssemblyname.Text = monoScript.ScriptAssemblyName;
                this.txtClassname.Text = monoScript.ScriptClassName;
                this.txtNamespace.Text = monoScript.ScriptNamespace;
                this.txtScriptName.Text = monoScript.ScriptName;
            }
        }
    }
}
