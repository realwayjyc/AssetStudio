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
    /// AudioSourcePanel.xaml 的交互逻辑
    /// </summary>
    public partial class AudioSourcePanel : UserControl
    {
        private AudioSource audioSource;
        public AudioSource AudioSource
        {
            get { return audioSource; }
        }
        public AudioSourcePanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            this.txtEnabled.Text = audioSource.IsEnabled.ToString();

            AudioClip audioClip=audioSource.GetAudioClip();
            if (audioClip != null)
            {
                this.txtAudioClip.Text = audioClip.AudioClipName;
            }

            this.txtPlayOnAwake.Text = audioSource.PlayOnAwake.ToString();
            this.txtVolumn.Text = audioSource.Volumn.ToString();
            this.txtPitch.Text = audioSource.Pitch.ToString();
            this.txtLoop.Text = audioSource.Loop.ToString();
            this.txtMute.Text = audioSource.Mute.ToString();
            this.txtPriority.Text = audioSource.Priority.ToString();
            this.txtDopplerLevel.Text = audioSource.DopplerLevel.ToString();
            this.txtMinDistance.Text = audioSource.MinDistance.ToString();
            this.txtMaxDistance.Text = audioSource.MaxDistance.ToString();
            this.txtPan2D.Text = audioSource.Pan2D.ToString();
            this.txtRollOffMode.Text = audioSource.RollOffMode.ToString();
            this.txtBypassEffects.Text = audioSource.BypassEffects.ToString();
            this.txtBypassListenerEffects.Text = audioSource.BypassListenerEffects.ToString();
            this.txtBypassReverbZones.Text = audioSource.BypassReverbZones.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             AudioClip audioClip=audioSource.GetAudioClip();
            if (audioClip != null)
            {
                MainWindow.instance.ShowUnityObject(audioClip);
            }
        }
    }
}
