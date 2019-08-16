using System.Windows;

namespace UnityAnalyzer
{
    /// <summary>
    /// AnimatorPanel.xaml 的交互逻辑
    /// </summary>
    public partial class AnimatorPanel
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
                this.btnAavatar.DataContext = null;
            }
            else
            {
                Avatar avatar = animator.GetAvatar();
                if (avatar != null)
                {
                    this.txtAvatar.Text = avatar.Name;
                }
                else
                {
                    this.txtAvatar.Text = "[ERROR]";
                }
                this.btnAavatar.DataContext = avatar;
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
            Avatar avatar = this.btnAavatar.DataContext as Avatar;
            if (avatar != null)
            {
                MainWindow.instance.ShowUnityObject(avatar);
            }
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
