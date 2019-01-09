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
    /// GameObjectPanel.xaml 的交互逻辑
    /// </summary>
    public partial class GameObjectPanel : UserControl
    {
        private GameObject gameObject;
        public GameObject GameObject
        {
            get { return gameObject; }
        }

        private List<UserControl> componentPanels;

        public GameObjectPanel()
        {
            InitializeComponent();
            componentPanels = new List<UserControl>();
        }

        public void SetGameObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.tbCtrlComponents.Items.Clear();
            foreach (UnityObject unityObject in this.gameObject.Components)
            {
                TabItem ti = new TabItem();
                Component component = unityObject as Component;
                ti.Header = unityObject.ClassIDTypeString.Replace("_", "__");
                if (component != null)
                {
                    UserControl userControl=component.CreateGameObjectComponentInfoControl();
                    if (userControl != null)
                    {
                        userControl.VerticalAlignment = VerticalAlignment.Stretch;
                        userControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                        ti.Content = userControl;
                    }
                }
                this.tbCtrlComponents.Items.Add(ti);
            }
        }
    }
}
