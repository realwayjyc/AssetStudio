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
using static UnityAnalyzer.AnimatorController;

namespace UnityAnalyzer
{
    public class TreeNode
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public int parentId { get; set; }
        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }
        public List<TreeNode> Children { get; set; }
        public Object objectContext { get; set; }

        public TreeNode parent { get; set; }
        public TreeNode()
        {
            Children = new List<TreeNode>();
        }
    }






    /// <summary>
    /// AnimatorControllerPanel.xaml 的交互逻辑
    /// </summary>
    public partial class AnimatorControllerPanel : UserControl
    {
        private AnimatorController animatorController;
        List<AnimationClip> animationClips1;
        List<AnimatorController.AnimControllerLayer> layers;

        public AnimatorControllerPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(AnimatorController animatorController)
        {
            this.animatorController = animatorController;
            this.txtAnimatorControllerName.Text = animatorController.AnimatorControllerName;

            layers = animatorController.Layers;
            foreach (var layer in layers)
            {
                this.LayerView.Items.Add(layer.LayerName);
            }
            



            //////////////////////////////////////////////////////////////////////////////////
            //List<AnimatorControllerState> stateList = animatorController.stateList;
            //treeNodeList = new List<TreeNode>();
            //foreach(AnimatorControllerState state in stateList)
            //{
            //    TreeNode t = new TreeNode();
            //    t.Name = state.name;
            //    t.DisplayName = state.name;
            //    t.objectContext = state;
            //    t.id = (int)state.stateNameMapKey;
            //    treeNodeList.Add(t);
            //}
            //this.tvStates.ItemsSource = treeNodeList;

            //////////////////////////////////////////////////////////////////////////////////
            //List<AnimatorControllerParam> paramList = animatorController.paramList;
            //treeNodeList = new List<TreeNode>();
            //foreach (AnimatorControllerParam param in paramList)
            //{
            //    TreeNode t = new TreeNode();
            //    t.Name = param.paramName;
            //    t.DisplayName = param.paramName;
            //    t.objectContext = param;
            //    t.id = (int)param.paramNameKey;
            //    treeNodeList.Add(t);
            //}
            //this.tvVariables.ItemsSource = treeNodeList;

            //////////////////////////////////////////////////////////////////////////////////
            List<SerializedObjectIdentifier> animationClips = animatorController.AnimationClips;
            animationClips1 = new List<AnimationClip>();
            foreach (SerializedObjectIdentifier param in animationClips)
            {
               AnimationClip animationClip=  animatorController.
                    GetUnityObjectBySerializedObjectIdentifier(param) as AnimationClip;
                if(animationClip != null)
                {
                    animationClips1.Add(animationClip);
                }
            }
            foreach(AnimationClip animationClip in animationClips1)
            {
                animationsListView.Items.Add(animationClip.Name);
            }
        }



        private void TvMouseRightDown(object sender, MouseButtonEventArgs e)
        {
            //TreeView tv=sender as TreeView;
            //AnimatorControllerLayer item = tv.SelectedItem as AnimatorControllerLayer;
            //string text = "";
            //if(item!=null)
            //{
            //    text = item.name;
            //}

            //AnimatorControllerState item2 = tv.SelectedItem as AnimatorControllerState;
            //if (item != null)
            //{
            //    text = item2.name;
            //}

            //AnimatorControllerParam item3 = tv.SelectedItem as AnimatorControllerParam;
            //if (item != null)
            //{
            //    text = item3.paramName;
            //}
                


            //try
            //{
            //    System.Windows.Clipboard.SetDataObject(text);
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void tvStates_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //TreeNode treeNode = this.tvStates.SelectedItem as TreeNode;

            //AnimatorControllerState item = treeNode.objectContext as AnimatorControllerState;
            //List<FieldValueCls> fieldValueClsList = new List<FieldValueCls>();
            //FieldValueCls nvp = new FieldValueCls();
            //nvp.FieldName="State Name";
            //nvp.FieldValue=item.name;
            //fieldValueClsList.Add(nvp);

            //nvp = new FieldValueCls();
            //nvp.FieldName="State Motion";
            //nvp.FieldValue = (item.animationClip == null ? "" : item.animationClip.Name);
            //if(item.animationClip!=null)
            //{
            //    nvp.OtherInfo = item.animationClip;
            //}
            //fieldValueClsList.Add(nvp);


            //nvp = new FieldValueCls();
            //nvp.FieldName="State Layer";
            //nvp.FieldValue=item.layer.name;
            //fieldValueClsList.Add(nvp);

            //this.generalObjectPanel.dgUnityObjectField.ItemsSource = fieldValueClsList;
        }

        private void tvVariables_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //TreeNode treeNode = this.tvVariables.SelectedItem as TreeNode;

            //AnimatorControllerParam item = treeNode.objectContext as AnimatorControllerParam;
            //List<FieldValueCls> fieldValueClsList = new List<FieldValueCls>();
            //FieldValueCls nvp = new FieldValueCls();
            //nvp.FieldName = "Param Name";
            //nvp.FieldValue = item.paramName;
            //fieldValueClsList.Add(nvp);

            //nvp = new FieldValueCls();
            //nvp.FieldName = "Param Type";
            //nvp.FieldValue = item.paramType.ToString();
            //fieldValueClsList.Add(nvp);


            //this.generalObjectPanelParam.dgUnityObjectField.ItemsSource = fieldValueClsList;

        }

        private void Button_Click_Open(object sender, RoutedEventArgs e)
        {
            if (btnOpen.DataContext as UnityObject != null)
            {
                MainWindow.instance.ShowUnityObject(btnOpen.DataContext as UnityObject);
            }
        }

        private void animationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = animationsListView.SelectedIndex;
            if(index>=0 && index< animationClips1.Count)
            {
                btnOpen.DataContext = this.animationClips1[index];
            }
        }

        private void LayerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LayerView.SelectedIndex>=0 && LayerView.SelectedIndex< layers.Count)
            {
                AnimatorController.AnimControllerLayer layer = layers[LayerView.SelectedIndex];
                this.txtWeight.Text = layer.DefaultWeight.ToString("0.0000");
                this.txtIKPass.Text = layer.IKPass.ToString();
                this.txtBlend.Text = layer.BlendMode.ToString();
                //No Pos Mask
                txtHumanPos.Text = (layer.humanPoseMask.word0 == 0xffffffff &&
                    layer.humanPoseMask.word1 == 0x07ffffff).ToString();
                int totalMask = layer.SkeletonMask.m_Data.Length;
                int weightCount = 0;
                if(totalMask==0)
                {
                    txtSkeletonMask.Text = "";
                }
                else
                {
                    MaskName.Items.Clear();
                    string copiedContent = "";
                    int index = 0;
                    foreach (var mask in layer.SkeletonMask.m_Data)
                    {
                        if(mask.m_Weight!=0)
                        {
                            weightCount++;
                            string content=StringHashLoader.GetStringByHash(mask.m_PathHash);
                            string[] fs = content.Split('/');
                            MaskName.Items.Add(index+": 0x" +mask.m_PathHash.ToString("X")+":"+fs[fs.Length - 1]);
                            copiedContent += fs[fs.Length - 1] + "\n";
                        }
                        index++;
                    }
                    MaskName.DataContext= copiedContent;
                    txtSkeletonMask.Text = weightCount.ToString() + "/" + totalMask.ToString();
                }
                StateView.DataContext = layer.states;
                StateView.Items.Clear();
                foreach (var state in layer.states)
                {
                    StateView.Items.Add(state.Name);
                }
            }
        }

        private void StateView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<State> states = StateView.DataContext as List<State>;
            int index = StateView.SelectedIndex;
            if(index>=0 && index<states.Count)
            {
                State state = states[index];
                txtStatePath.Text = state.Path;
                txtStateAnimationName.Text = state.animationClipName;
                txtStateSpeed.Text = state.speed.ToString();
                txtCycleOffset.Text=state.CycleOffset.ToString();
                txtMirror.Text=state.Mirror.ToString();
                txtFootIK.Text= state.FootIK.ToString();
                txtWriteDefault.Text=state.WriteDefault.ToString();
            }
        }

        private void MaskName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            string content = MaskName.DataContext as string;
            Clipboard.SetText(content);
            MessageBox.Show("Copied");
        }
    }
}
