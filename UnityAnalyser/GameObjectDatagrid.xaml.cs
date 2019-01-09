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
    /// GameObjectDatagrid.xaml 的交互逻辑
    /// </summary>
    public partial class GameObjectDatagrid : UserControl
    {
        public GameObjectDatagrid()
        {
            InitializeComponent();
        }

        List<UnityObject> unityObjectList = new List<UnityObject>();

        public void SetGameObject(GameObject gameObject)
        {
            unityObjectList.Add(gameObject);
            foreach (UnityObject c in gameObject.Components)
            {
                unityObjectList.Add(c);
            }
            this.dgGameObjects.ItemsSource = unityObjectList;
        }

        public void dgGameObjects_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            object item = dgGameObjects.SelectedItem;
            MainWindow.instance.ShowUnityObject(item as UnityObject);
        }
    }
}
