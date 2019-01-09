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
    /// TransformPanel.xaml 的交互逻辑
    /// </summary>
    public partial class TransformPanel : UserControl
    {
        private Transform transform;
        public Transform Transform
        {
            get { return transform; }
        }

        private GameObject parentGameObject;

        public TransformPanel()
        {
            InitializeComponent();
            parentGameObject = null;
        }

        public void SetUnityObject(Transform transform)
        {
            this.transform = transform;
            this.txtPostionX.Text = transform.PositionX.ToString();
            this.txtPostionY.Text = transform.PositionY.ToString();
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
