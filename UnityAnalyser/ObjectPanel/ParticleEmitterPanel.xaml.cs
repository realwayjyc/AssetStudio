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
    /// ParticleEmitterPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ParticleEmitterPanel : UserControl
    {
        private ParticleEmitter particleEmitter;

        public ParticleEmitterPanel()
        {
            InitializeComponent();
        }

        public void SetParticleEmitter(ParticleEmitter particleEmitter)
        {
            this.particleEmitter = particleEmitter;
            this.txtIsEnabled.Text = particleEmitter.IsEnabled.ToString();
            this.txtEmit.Text = particleEmitter.Emit.ToString();

            this.txtMinSize.Text = particleEmitter.MinSize.ToString();
            this.txtMaxSize.Text = particleEmitter.MaxSize.ToString();

            this.txtMinEnergy.Text = particleEmitter.MinEnergy.ToString();
            this.txtMaxEnergy.Text = particleEmitter.MaxEnergy.ToString();

            this.txtMinEmission.Text = particleEmitter.MinEmission.ToString();
            this.txtMaxEmision.Text = particleEmitter.MaxEmission.ToString();

            this.txtWorldVelocity.Text = particleEmitter.WorldVelocity.ToString();
            this.txtLocalVelocity.Text = particleEmitter.LocalVelocity.ToString();

            this.txtRndVelocity.Text = particleEmitter.RndVelocity.ToString();
            this.txtTangentVelocity.Text = particleEmitter.TangentVelocity.ToString();

            this.txtEmitterVelocityScale.Text = particleEmitter.EmitterVelocityScale.ToString();
            this.txtAngularVelocity.Text = particleEmitter.AngularVelocity.ToString();

            this.txtRndAngularVelocity.Text = particleEmitter.RndAngularVelocity.ToString();
            this.txtRndInitialRotations.Text = particleEmitter.RndInitialRotations.ToString();

            this.txtUseWorldSpace.Text = particleEmitter.UseWorldSpace.ToString();
            this.txtOneShot.Text = particleEmitter.OneShot.ToString();
        }
    }
}
