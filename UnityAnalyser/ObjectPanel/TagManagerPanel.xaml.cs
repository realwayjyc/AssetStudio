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
    /// TagManagerPanel.xaml 的交互逻辑
    /// </summary>
    public partial class TagManagerPanel : UserControl
    {
        public class TagItem
        {
            private string id;
            public string Id
            {
                get { return id; }
                set { id = value; }
            }

            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            private string uniqueID;
            public string UniqueID
            {
                get { return uniqueID; }
                set { uniqueID = value; }
            }

            private string userID;
            public string UserID
            {
                get { return userID; }
                set { userID = value; }
            }

        }

        private TagManager tagManager;
        public TagManager TagManager
        {
            get { return tagManager; }
        }

        private List<TagItem> tagList=new List<TagItem>();
        private List<TagItem> layerList=new List<TagItem>();
        private List<TagItem> sortingLayerList=new List<TagItem>();
        


        public TagManagerPanel()
        {
            InitializeComponent();
            tagManager = null;
        }

        public void SetUnityObject(TagManager tagManger)
        {
            this.tagManager = tagManger;
           
            tagList.Clear();
            layerList.Clear();
            sortingLayerList.Clear();

            for(int i=0;i<tagManager.Tags.Count;i++)
            {
                TagItem ti=new TagItem();
                ti.Id="Element "+i;
                ti.Name=tagManager.Tags[i];
                tagList.Add(ti);
            }
            this.dgTag.ItemsSource = tagList;

            /////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < tagManager.Layers.Count; i++)
            {
                TagItem ti = new TagItem();
                if (i < 8)
                {
                    ti.Id = "Builtin Layer " + i;
                }
                else
                {
                    ti.Id = "User Layer " + i;
                }
                ti.Name = tagManager.Layers[i];
                layerList.Add(ti);
            }
            this.dgLayer.ItemsSource = layerList;

            /////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < tagManager.SortingLayers.Count; i++)
            {
                TagItem ti = new TagItem();
                ti.Id = "Layer " + i;
                ti.Name = tagManager.SortingLayers[i].Name;
                ti.UserID = tagManager.SortingLayers[i].UserID.ToString();
                ti.UniqueID = "0x"+tagManager.SortingLayers[i].UniqueID.ToString("X");
                sortingLayerList.Add(ti);
            }
            this.dgSortingLayer.ItemsSource = sortingLayerList;
        }

        private void dgTag_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = GetElementFromPoint((ItemsControl)sender, e.GetPosition((ItemsControl)sender));
            TagItem tagItem = item as TagItem;
            if(tagItem!=null)
            {
                try
                {
                    Clipboard.SetDataObject(tagItem.Name);
                }
                catch(Exception ex)
                {

                }
            }
        }

        private object GetElementFromPoint(ItemsControl itemsControl, Point point)
        {
            UIElement element = itemsControl.InputHitTest(point) as UIElement;
            while (element != null)
            {
                if (element == itemsControl)
                    return null;
                object item = itemsControl.ItemContainerGenerator.ItemFromContainer(element);
                if (!item.Equals(DependencyProperty.UnsetValue))
                    return item;
                element = (UIElement)VisualTreeHelper.GetParent(element);
            }
            return null;
        }

        private void dgLayer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = GetElementFromPoint((ItemsControl)sender, e.GetPosition((ItemsControl)sender));
            TagItem tagItem = item as TagItem;
            if (tagItem != null)
            {
                try
                {
                    Clipboard.SetDataObject(tagItem.Name);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void dgSortingLayer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            object item = GetElementFromPoint((ItemsControl)sender, e.GetPosition((ItemsControl)sender));
            TagItem tagItem = item as TagItem;
            if (tagItem != null)
            {
                try
                {
                    Clipboard.SetDataObject(tagItem.Name);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
