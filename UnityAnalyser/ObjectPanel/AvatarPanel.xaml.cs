using System.Windows.Controls;

namespace UnityAnalyzer
{
    /// <summary>
    /// Interaction logic for AvatarPanel.xaml
    /// </summary>
    public partial class AvatarPanel
    {
        private Avatar _avatar;
        public AvatarPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(Avatar avatar)
        {
            _avatar = avatar;
            if (avatar == null) return;
            this.txtAvatarName.Text = avatar.Name;
        }
    }
}
