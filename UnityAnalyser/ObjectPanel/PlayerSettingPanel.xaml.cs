using System.Windows.Controls;

namespace UnityAnalyzer
{
    /// <summary>
    /// PlayerSettingPanel.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerSettingPanel : UserControl
    {
        private PlayerSettings playerSettings;
        public PlayerSettings PlayerSettings
        {
            get { return playerSettings; }
        }

        public PlayerSettingPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(PlayerSettings playerSettings)
        {
            this.playerSettings = playerSettings;
            this.txtColorSpace.Text = playerSettings.activeColorSpace == 0 ? "Gamma" : "Linear";
            this.txtRenderPath.Text = playerSettings.renderingPath.ToString();
        }
    }
}
