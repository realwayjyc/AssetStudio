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
    /// AnimatorPanel.xaml 的交互逻辑
    /// </summary>
    public partial class AnimatorPanel : UserControl
    {
        private Animator animator;
        public AnimatorPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(Animator animator)
        {
            this.animator = animator;

            this.txtEnabled.Text = animator.IsEnabled.ToString();

            if(animator.Avatar.serializedFileIndex==0 && animator.Avatar.identifierInFile==0)
            {
                this.txtAvatar.Text = "[NULL]";
            }
            else
            {
                this.txtAvatar.Text = "0x" + animator.Avatar.identifierInFile.ToString("x") + 
                    " " + "0x" + animator.Avatar.serializedFileIndex.ToString("x");
            }

            if (animator.Controller.serializedFileIndex == 0 && animator.Controller.identifierInFile == 0)
            {
                this.txtController.Text = "[NULL]";
                this.btnAnimator.DataContext = null;
            }
            else
            {
                AnimatorController c = animator.GetAnimatorController();
                if(c!=null)
                {
                    this.txtController.Text = c.Name;
                }
                else
                {
                    this.txtController.Text = "[ERROR]";
                }
                this.btnAnimator.DataContext = c;
            }

            this.txtAllowConstantClipSamplingOptimization.Text = animator.AllowConstantClipSamplingOptimization.ToString();
            this.txtApplyRootMotion.Text = animator.ApplyRootMotion.ToString();
            this.txtCullingMode.Text = animator.CullingMode.ToString();
            this.txtHasTransformHierarchy.Text = animator.HasTransformHierarchy.ToString();
            this.txtUpdateMode.Text = animator.UpdateMode.ToString();


            //this.txtAudioClipName.Text = audioClip.AudioClipName;
            //this.txtSoundFormat.Text = audioClip.Format.ToString();
            //this.txtSoundType.Text = audioClip.Type.ToString();
            //this.txt3Dsound.Text = audioClip.Is3dSound.ToString();
            //this.txtUseHardware.Text = audioClip.IsUseHardware.ToString();
            //this.txtSize.Text = "0x" + audioClip.StreamContent.Length.ToString("x");
        }

        private void Button_ClickAvatar(object sender, RoutedEventArgs e)
        {

        }

        private void Button_ClickController(object sender, RoutedEventArgs e)
        {
           
            AnimatorController c = this.btnAnimator.DataContext as AnimatorController;
            if(c!=null)
            {
                MainWindow.instance.ShowUnityObject(c);
            }
        }
    }
}
