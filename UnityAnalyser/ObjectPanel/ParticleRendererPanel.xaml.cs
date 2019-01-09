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
    /// ParticleRendererPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ParticleRendererPanel : UserControl
    {
        private ParticleRenderer particleRenderer;

        public ParticleRendererPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(ParticleRenderer particleRenderer)
        {
            this.particleRenderer = particleRenderer;

            this.txtCameraVelocityScale.Text = particleRenderer.CameraVelocityScale.ToString();
            this.txtStretchParticles.Text = particleRenderer.StretchParticlesMode.ToString();

            this.txtLengthScale.Text = particleRenderer.LengthScale.ToString();
            this.txtVelocityScale.Text = particleRenderer.VelocityScale.ToString();

            this.txtMaxParticleSize.Text = particleRenderer.MaxParticleSize.ToString();

            this.uvAnimationPanel.SetUVAnimation(particleRenderer.UVAnimation);

            this.rendererPanel.SetRenderer(particleRenderer);
        }
    }
}
