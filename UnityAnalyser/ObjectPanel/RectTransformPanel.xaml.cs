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
    /// RectTransformPanel.xaml 的交互逻辑
    /// </summary>
    public partial class RectTransformPanel : UserControl
    {
        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get { return rectTransform; }
        }

        private GameObject parentGameObject;

        public RectTransformPanel()
        {
            InitializeComponent();
        }

        public void SetUnityObject(RectTransform transform)
        {
            this.rectTransform = transform;
            if(transform.AnchorMinX!=transform.AnchorMaxX)
            {
                this.txtPostionX.Text = transform.PositionX.ToString();
                this.lblPosX.Content = "Left";
                this.lblWidth.Content = "Right";

                this.txtPostionX.Text = (transform.AnchoredPosX - transform.SizeDeltaW * transform.PivotX).ToString();
                this.txtWidth.Text = (transform.SizeDeltaW * (transform.PivotX - 1) - transform.AnchoredPosX).ToString();
            }
            else
            {
                this.txtPostionX.Text = transform.PositionX.ToString();
                this.lblPosX.Content = "Pos X";
                this.lblWidth.Content = "Width";

                this.txtPostionX.Text = transform.AnchoredPosX.ToString();
                this.txtWidth.Text = transform.SizeDeltaW.ToString();
            }

            if (transform.AnchorMinY != transform.AnchorMaxY)
            {
                this.txtPostionY.Text = transform.PositionY.ToString();
                this.lblPosY.Content = "Top";
                this.lblHeight.Content = "Bottom";

                this.txtHeight.Text = (transform.AnchoredPosY - transform.SizeDeltaH * transform.PivotY).ToString();
                this.txtPostionY.Text = (transform.SizeDeltaH * (transform.PivotY - 1) - transform.AnchoredPosY).ToString();
            }
            else
            {
                this.txtPostionY.Text = transform.PositionY.ToString();
                this.lblPosY.Content = "Pos Y";
                this.lblHeight.Content = "Height";
                this.txtPostionY.Text = transform.AnchoredPosY.ToString();
                this.txtHeight.Text = transform.SizeDeltaH.ToString();
            }

            
            
            
            this.txtPostionZ.Text = transform.PositionZ.ToString();

            this.txtScaleX.Text = transform.ScaleX.ToString();
            this.txtScaleY.Text = transform.ScaleY.ToString();
            this.txtScaleZ.Text = transform.ScaleZ.ToString();

            this.txtRotationX.Text = transform.RotationX.ToString();
            this.txtRotationY.Text = transform.RotationY.ToString();
            this.txtRotationZ.Text = transform.RotationZ.ToString();

            Transform parent = transform.GetParentTransform();
            if (parent == null)
            {
                lblParentObject.Content = "NULL";
                btnOpenParentObject.Visibility = Visibility.Hidden;
            }
            else
            {
                parentGameObject = parent.GetGameObject();
                lblParentObject.Content = parentGameObject.Name;
                btnOpenParentObject.Visibility = Visibility.Visible;
            }

            this.txtDeltaW.Text = transform.SizeDeltaW.ToString();
            this.txtDeltaH.Text = transform.SizeDeltaH.ToString();

            this.txtAnchorMinX.Text = transform.AnchorMinX.ToString();
            this.txtAnchorMinY.Text = transform.AnchorMinY.ToString();

            this.txtAnchorMaxX.Text = transform.AnchorMaxX.ToString();
            this.txtAnchorMaxY.Text = transform.AnchorMaxY.ToString();

            this.txtPivotX.Text = transform.PivotX.ToString();
            this.txtPivotY.Text = transform.PivotY.ToString();

            this.txtAnchoredPositionX.Text = transform.AnchoredPosX.ToString();
            this.txtAnchoredPositionY.Text = transform.AnchoredPosY.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (parentGameObject != null)
            {
                MainWindow.instance.ShowGameObjectTabItem(parentGameObject);
            }
        }
    }
}
