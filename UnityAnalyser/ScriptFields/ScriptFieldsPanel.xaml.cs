﻿using System;
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
    /// ScriptFieldsPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ScriptFieldsPanel : UserControl
    {
        public ScriptFieldsPanel()
        {
            InitializeComponent();
        }

        public void HandleValues(ScriptFieldValueSet scriptFiledValueSet)
        {
            if (scriptFiledValueSet == null)
            {
                dgScriptField.Visibility = Visibility.Hidden;
                lblInfo.Visibility = Visibility.Visible;
                lblInfo.Content = "No Fields Implemented";
            }
            else
            {
                if (scriptFiledValueSet.MonoScript.ScriptNamespace.Contains("UnityEngine")==false)
                {
                    scriptFiledValueSet.Sort();
                }
                dgScriptField.Visibility = Visibility.Visible;
                lblInfo.Visibility = Visibility.Hidden;
                this.dgScriptField.ItemsSource = scriptFiledValueSet.ScriptFieldValueList;
            }
        }
        private void dgMainData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgScriptField.SelectedItem != null)
            {
                ScriptFieldValue scriptFieldValue = dgScriptField.SelectedItem as ScriptFieldValue;
                if (scriptFieldValue.OtherInfo != null)
                {
                    extraInfoGrid.Visibility = Visibility.Visible;
                    extraInfoGrid2.Children.Clear();
                    ScriptFieldMultiValue multiValue = scriptFieldValue.OtherInfo as ScriptFieldMultiValue;
                    if (multiValue != null)
                    {
                        SetValue(multiValue);
                        extraInfoGrid2.Children.Add(dgMultiValue);
                        return;
                    }

                    UnityObject unityObject = scriptFieldValue.OtherInfo as UnityObject;
                    if (unityObject != null)
                    {
                        SetValue(unityObject);
                        extraInfoGrid2.Children.Add(unityObjectGrid);
                        return;
                    }

                    string content = scriptFieldValue.OtherInfo as String;
                    if (content != null)
                    {
                        SetValue(content);
                        extraInfoGrid2.Children.Add(txtString);
                        return;
                    }
                }
                else
                {
                    //隐藏最右边的内容
                    extraInfoGrid.Visibility = Visibility.Visible;
                    string content = scriptFieldValue.FieldValue;
                    extraInfoGrid2.Children.Clear();
                    SetValue(content);
                    extraInfoGrid2.Children.Add(txtString);
                }
            }
        }

        private void SetValue(String textContent)
        {
            txtString.Text = textContent;
        }

        private void SetValue(ScriptFieldMultiValue multiValue)
        {
            dgMultiValue.Columns.Clear();
            dgMultiValue.ItemsSource = null;
            foreach (string columnName in multiValue.columnNameList)
            {
                DataGridTextColumn dgcolumn = new DataGridTextColumn();
                dgcolumn.Header = columnName;
                if (dgMultiValue.Columns.Count==1)
                {
                    dgcolumn.Width = 300;

                }
                else
                {
                    dgcolumn.Width = 200;
                }
                dgcolumn.Binding = new Binding(columnName);
                dgMultiValue.Columns.Add(dgcolumn);
            }
            dgMultiValue.ItemsSource = multiValue.contentList;
        }

        private void SetValue(UnityObject unityObject)
        {
            this.lblObjectType.Content = unityObject.GetType().ToString();
            this.txtSerializedFileName.Text = unityObject.ObjectInfo.UnityFile.AliasFileName;
            this.txtId.Text = "0x" + unityObject.ObjectInfo.Id.ToString("x");
            this.txtDebugLineStart.Text = "0x" + unityObject.DebugLineStart.ToString("x");

            this.btnOpen.Content="Open Object";
            this.btnOpen2.Visibility = Visibility.Hidden;
            this.btnSave.Content = "Save Content";

            if (unityObject as GameObject!=null)
            {
                GameObject gameObject = unityObject as GameObject;
                if (gameObject != null)
                {
                    this.txtGameObjSerializedFileName.Text = gameObject.ObjectInfo.UnityFile.AliasFileName;
                    this.txtGameObjId.Text = "0x" + gameObject.ObjectInfo.Id.ToString("x");
                    this.txtGameObjDebugLineStart.Text = "0x" + gameObject.ObjectInfo.DebugLineStart.ToString("x");
                    this.btnOpen2.Visibility = Visibility.Visible;
                    this.btnOpen2.DataContext = gameObject;
                }
            }
            else if (unityObject as UnityAnalyzer.Sprite != null)
            {
                btnSave.Visibility = Visibility.Visible;
                Sprite sprite = unityObject as UnityAnalyzer.Sprite;
                SpriteRenderData reander_data = sprite.SpriteRenderData;
                if (reander_data != null)
                {
                    Texture2D texture2D = sprite.GetUnityObjectBySerializedObjectIdentifier(reander_data.Texture2D) as Texture2D;
                    btnSave.DataContext = texture2D;
                }
                btnOpen.DataContext = unityObject;
            }
            else if (unityObject as UnityAnalyzer.Texture2D != null)
            {
                btnSave.Visibility = Visibility.Visible;
                btnSave.DataContext = unityObject as Texture2D;
            }
            else if (unityObject as Component != null)
            {
                btnOpen.Visibility = Visibility.Visible;
                btnSave.Visibility = Visibility.Hidden;
                btnOpen.DataContext = unityObject as Component;

                SerializedObjectIdentifierWithFile soif = new SerializedObjectIdentifierWithFile();
                soif.soi = new SerializedObjectIdentifier(0, unityObject.ObjectInfo.Id);
                soif.fullFileName = unityObject.ObjectInfo.UnityFile.FullFileName;

                GameObject gameObject = Analyzer.GetGameObjectByComponent(soif);
                if (gameObject != null)
                {
                    this.txtGameObjSerializedFileName.Text = soif.fullFileName;
                    this.txtGameObjId.Text = "0x" + soif.soi.identifierInFile.ToString("x");
                    this.txtGameObjDebugLineStart.Text = "0x" + gameObject.ObjectInfo.DebugLineStart.ToString("x");
                    this.btnOpen2.Visibility = Visibility.Visible;
                    this.btnOpen2.DataContext = gameObject;
                }
            }
            else if(unityObject.ObjectInfo.UnityFile.AliasFileName.Contains("sharedassets"))
            {
                this.btnOpen.Content = "Open Resource";
                this.btnSave.Content = "Save Resource";

                this.btnSave.DataContext = unityObject;
                this.btnOpen.DataContext = unityObject; ;

            }
            else
            {
                btnSave.Visibility = Visibility.Hidden;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //保存内容
            Texture2D texture2D = btnSave.DataContext as Texture2D;
            if (texture2D!=null)
            {
                texture2D.SaveToFile();
                return;
            }

            Shader shader = btnSave.DataContext as Shader;
            if (shader != null)
            {
                shader.SaveToFile();
                return;
            }
        }

        private void Button_Click_Open(object sender, RoutedEventArgs e)
        {
            //保存内容
            if (btnOpen.DataContext as UnityObject != null)
            {
                MainWindow.instance.ShowUnityObject(btnOpen.DataContext as UnityObject);
            }
        }

        private void Button_Click_OpenGO(object sender, RoutedEventArgs e)
        {
            //保存内容
            if (btnOpen2.DataContext as GameObject != null)
            {
                MainWindow.instance.ShowGameObjectTabItem(btnOpen2.DataContext as GameObject);
            }
        }

        private void Button_Click_ShowInTree(object sender, RoutedEventArgs e)
        {
            //保存内容
            if (btnOpen2.DataContext as GameObject != null)
            {
                GameObject gameObject = btnOpen2.DataContext as GameObject;
                MainWindow.ShowGameObjectHierarchyTree(gameObject.Id);
            }
        }

        private void dgMultiValue_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<Object> m = dgMultiValue.ItemsSource as List<Object>;

            int id = 0;
            string file = "";

            for (int i = 0; i < m.Count; i++)
            {
                if ((m[i] as ValueChangeFun).Name == "GameObject_Id")
                {
                    id = Convert.ToInt32((m[i] as ValueChangeFun).Value, 16);
                }
                else if ((m[i] as ValueChangeFun).Name == "GameObject_File")
                {
                    file = (m[i] as ValueChangeFun).Value;
                }
            }

            MainWindow.ShowGameObjectHierarchyTree(id);
           // Analyzer Analyzer = null;
           // foreach(Analyzer a in Analyzer.unityFolderDict.Values)
           // {
           //     Analyzer = a;
           //     break;
           // }
           //// file = Analyzer.Current_folder + file;

           // UnityFile unityFile = Analyzer.unityFileDict[file];

           // UnityObject unityObject= (unityFile as AssetsFile).GetUnityObjectByIdentifier(id);
           // if(unityObject as GameObject!=null)
           // {
                
           // }

        }
    }
}
