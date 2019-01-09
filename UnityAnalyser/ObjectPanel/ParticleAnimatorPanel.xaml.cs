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
    /// ParticleAnimatorPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ParticleAnimatorPanel : UserControl
    {
        private ParticleAnimator particleAnimator;

        public ParticleAnimatorPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(ParticleAnimator particleAnimator)
        {
            this.particleAnimator = particleAnimator;
            this.txtDoesAnimatorColor.Text = particleAnimator.DoesAnimatorColor.ToString();
            
            this.txtColorAnimation0.Text = Util.GetColorRgbaString(particleAnimator.ColorAnimation[0]);
            this.txtColorAnimation1.Text = Util.GetColorRgbaString(particleAnimator.ColorAnimation[1]);
            this.txtColorAnimation2.Text = Util.GetColorRgbaString(particleAnimator.ColorAnimation[2]);
            this.txtColorAnimation3.Text = Util.GetColorRgbaString(particleAnimator.ColorAnimation[3]);
            this.txtColorAnimation4.Text = Util.GetColorRgbaString(particleAnimator.ColorAnimation[4]);

            this.txtWorldRotationAxis.Text = particleAnimator.WorldRotationAxis.ToString();
            this.txtLocalRotationAxis.Text = particleAnimator.LocalRotationAxis.ToString();

            this.txtSizeGrow.Text = particleAnimator.SizeGrow.ToString();
            this.txtDamping.Text = particleAnimator.Damping.ToString();

            this.txtRndForce.Text = particleAnimator.RndForce.ToString();
            this.txtForce.Text = particleAnimator.Force.ToString();

            this.txtStopSimulation.Text = particleAnimator.StopSimulation.ToString();
            this.txtAutoDestruct.Text = particleAnimator.Autodestruct.ToString();
        }
    }
}
