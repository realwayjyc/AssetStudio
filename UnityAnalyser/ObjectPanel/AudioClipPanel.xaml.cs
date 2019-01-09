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
    /// AudioClipPanel.xaml 的交互逻辑
    /// </summary>
    public partial class AudioClipPanel : UserControl
    {
        private AudioClip audioClip;
        public AudioClipPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(AudioClip audioClip)
        {
            this.audioClip = audioClip;

            this.txtAudioClipName.Text = audioClip.AudioClipName;
            this.txtSoundFormat.Text = audioClip.Format.ToString();
            this.txtSoundType.Text = audioClip.Type.ToString();
            this.txt3Dsound.Text = audioClip.Is3dSound.ToString();
            this.txtUseHardware.Text = audioClip.IsUseHardware.ToString();
            this.txtSize.Text = "0x"+audioClip.StreamContent.Length.ToString("x");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //保存内容
            this.audioClip.SaveToFile();
        }
    }
}
