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
using System.Windows.Shapes;

namespace UnityAnalyzer
{
    /// <summary>
    /// GameObjectTraceGraph.xaml 的交互逻辑
    /// </summary>
    public partial class GameObjectTraceGraph : Window
    {
        public GameObjectTraceGraph()
        {
            InitializeComponent();
        }

        public void SetGameObjectNode(GameObjectNode gameObjectNode)
        {
            try
            {
                GameObjectNode gameObjectNodeNew = null;
                GameObjectNode lastNewNode = null;
                while (gameObjectNode != null)
                {
                    gameObjectNodeNew = new GameObjectNode();
                    gameObjectNodeNew.Children = new List<GameObjectNode>();
                    gameObjectNodeNew.DisplayName = gameObjectNode.DisplayName;
                    gameObjectNodeNew.gameObject = gameObjectNode.gameObject;
                    gameObjectNodeNew.id = gameObjectNode.id;
                    gameObjectNodeNew.IsExpanded = true;
                    gameObjectNodeNew.IsSelected = false;
                    gameObjectNodeNew.Name = gameObjectNode.Name;
                    gameObjectNodeNew.parentId = gameObjectNode.parentId;

                    if (lastNewNode != null)
                    {
                        gameObjectNodeNew.Children.Add(lastNewNode);
                    }

                    gameObjectNode = gameObjectNode.parent;
                    lastNewNode = gameObjectNodeNew;
                }
                tvProperties.ItemsSource = new List<GameObjectNode>() { lastNewNode };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
