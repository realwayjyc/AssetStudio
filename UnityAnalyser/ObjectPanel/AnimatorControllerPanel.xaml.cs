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

        public AnimatorControllerPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(AnimatorController animatorController)
        {
            //this.animatorController = animatorController;
            //this.txtAnimatorControllerName.Text = animatorController.AnimatorControllerName;

            //List<AnimatorControllerLayer> layerList = animatorController.layerList;
            //List<TreeNode> treeNodeList = new List<TreeNode>();
            //foreach(AnimatorControllerLayer layer in layerList)
            //{
            //    TreeNode t = new TreeNode();
            //    t.Name = layer.name;
            //    t.DisplayName = layer.name;
            //    t.objectContext = layer;
            //    t.id = (int)layer.layerNameMapKey;
            //    treeNodeList.Add(t);
            //}
            //this.tvLayers.ItemsSource = treeNodeList;

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

        }

        private void tvLayers_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeNode treeNode = this.tvLayers.SelectedItem as TreeNode;
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
    }
}
