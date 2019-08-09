using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Windows.Controls.Primitives;


namespace UnityAnalyzer
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;

        private Analyzer currentAnalyzer;
        public Analyzer CurrentAnalyzer
        {
            get { return currentAnalyzer; }
            set { currentAnalyzer = value; }
        }

        /// <summary>
        /// 已经显示过的UnityObject
        /// </summary>
        private List<UnityObject> displayedUnityObjectList = new List<UnityObject>();
        private int currentIndex;

        private Dictionary<GameObject, TabItem> openedGameObjectPanelDict = new Dictionary<GameObject, TabItem>();

        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            lbFiles.DisplayMemberPath = "AliasFileName";
            System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
            binding.Path = new PropertyPath("SelectedValue");
            lbFiles.SetBinding(Selector.SelectedValueProperty, binding);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "maindata|maindata||*.assets|*.assets||所有文件|*.*";
            openFileDialog.Filter = "maindata|maindata|globalgamemanagers|globalgamemanagers|*.unity3d|*.unity3d";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] fields = openFileDialog.FileName.Split('\\');
                if (fields[fields.Length - 1].ToLower().StartsWith("maindata") ||
                    fields[fields.Length - 1].ToLower().StartsWith("globalgamemanagers"))
                {
                    Thread thread = new Thread(ParseMainDataThread);
                    thread.Start(openFileDialog.FileName);
                }
            }
        }

        private void Button_Click_Convert(object sender, RoutedEventArgs e)
        {
            //ConvertFile(@"F:\unity\project\HS_SOURCE\honeyselect\____AnonType0_2.cs");
            //ConvertFile(@"F:\unity\project\HS_SOURCE\honeyselect\____AnonType3_1.cs");

            //ConvertFile(@"F:\unity\project\HS_SOURCE\honeyselect\Config\VoiceSystem.cs");
            //ConvertFile(@"F:\test.cs");
            //ConvertFile(@"F:\unity\project\HS_SOURCE\honeyselect\BaseLoader.cs");
            //return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "maindata|maindata||*.assets|*.assets||所有文件|*.*";
            openFileDialog.Filter = "cs|*.cs";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] fields = openFileDialog.FileName.Split('\\');
                string filename = fields[fields.Length - 1];
                Thread thread = new Thread(ConvertFileThread);
                thread.Start(openFileDialog.FileName.Replace(filename, ""));
            }
        }

        private void ConvertFileThread(object o)
        {
            ConvertFolder(o as string);
            System.Windows.Forms.MessageBox.Show("转换完毕");
        }

        private void ConvertFolder(string folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".cs"))
                {
                    ConvertFile(file);
                }
            }
            foreach (string subFolder in Directory.GetDirectories(folder))
            {
                ConvertFolder(subFolder);
            }
        }

        private void ParseMainDataThread(object o)
        {
            currentAnalyzer = new Analyzer();
            currentAnalyzer.ParseMainData(o as String);

        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = e.Source as ListBoxItem;
            UnityFile unityFile = item.Content as UnityFile;
            if (unityFile != null)
            {
                ShowUnityFileItem(unityFile);
            }
        }

        public void ShowUnityFileItem(UnityFile unityFile)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    for (int i = 0; i < lbFiles.Items.Count; i++)
                    {
                        if (lbFiles.Items[i] == unityFile)
                        {
                            lbFiles.SelectedIndex = i;
                            break;
                        }
                    }
                    if (unityFile is AssetsFile)
                    {
                        this.dgMainData.ItemsSource = (unityFile as AssetsFile).ObjectList;
                        this.dgMainData.SelectedIndex = 0;
                        this.tbItemFirst.Header = unityFile.AliasFileName;

                        tvProperties.ItemsSource = (unityFile as AssetsFile).GameObjTreeList;
                    }
                    SwitchToTabIndex(0);
                });
            }
        }

        private void dgMainData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMainData.SelectedItem != null)
            {
                ShowUnityObject(dgMainData.SelectedItem as UnityObject);
            }
        }

        public void ShowUnityObject(UnityObject unityObject, bool isOuterCall = true)
        {
            if (unityObject == null) return;

            #region 修改导航功能
            if (isOuterCall == true)
            {
                int index = displayedUnityObjectList.IndexOf(unityObject);
                if (index != -1)
                {
                    displayedUnityObjectList.RemoveAt(index);
                }
                displayedUnityObjectList.Add(unityObject);
                currentIndex = displayedUnityObjectList.Count - 1;
            }
            else
            {
                int index = displayedUnityObjectList.IndexOf(unityObject);
                if (index == -1)
                {
                    return;
                }
                displayedUnityObjectList.RemoveAt(index);
                displayedUnityObjectList.Add(unityObject);
                currentIndex = displayedUnityObjectList.Count - 1;
            }

            btnPrev.Visibility = Visibility.Visible;
            btnNext.Visibility = Visibility.Visible;
            if (currentIndex == 0)
            {
                btnPrev.Visibility = Visibility.Hidden;
            }
            if (currentIndex == displayedUnityObjectList.Count - 1)
            {
                btnNext.Visibility = Visibility.Hidden;
            }
            #endregion

            this.componentInfoGrid.Children.Clear();
            if (unityObject is GameObject)
            {
                GameObject gameObject = unityObject as GameObject;
                if (gameObject.GameObjectPanel == null)
                {
                    gameObject.CreateGameObjectPanel();
                }
                gameObject.GameObjectPanel.VerticalAlignment = VerticalAlignment.Stretch;
                gameObject.GameObjectPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                this.componentInfoGrid.Children.Add(gameObject.GameObjectPanel);
            }
            else
            {
                System.Windows.Controls.UserControl userControl = unityObject.CreateObjectInfoPanel();
                if (userControl != null)
                {
                    userControl.VerticalAlignment = VerticalAlignment.Stretch;
                    userControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    this.componentInfoGrid.Children.Add(userControl);
                }
            }
        }

        public void SetInfoLabelContent(string content)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    this.lblInfo.Content = content;
                });
            }
        }

        private void dgMainData_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.DataGrid datagrid = sender as System.Windows.Controls.DataGrid;
            Point aP = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;
            while (target != null)
            {
                if (target is DataGridRow)
                {
                    break;
                }
                target = VisualTreeHelper.GetParent(target);
            }
            DataGridRow row = target as DataGridRow;
            if (row != null)
            {
                GameObject gameObject = row.Item as GameObject;
                if (gameObject != null)
                {
                    ShowGameObjectTabItem(gameObject);
                }
            }
        }

        public void ShowGameObjectTabItem(GameObject gameObject)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (openedGameObjectPanelDict.ContainsKey(gameObject) == true)
                    {
                        TabItem tabItem2 = openedGameObjectPanelDict[gameObject];
                        SwitchToTabIndex(this.tbCtrl.Items.IndexOf(tabItem2));
                        return;
                    }
                    TabItem tabItem = new TabItem();
                    this.tbCtrl.Items.Add(tabItem);
                    tabItem.MouseDoubleClick += tabItem_MouseDoubleClick;
                    tabItem.Header = gameObject.Name.Replace("_", "__");
                    tabItem.DataContext = gameObject;

                    GameObjectDatagrid gameObjectDataGrid = null;
                    if (tabItem.Content != null)
                    {
                        gameObjectDataGrid = tabItem.Content as GameObjectDatagrid;
                    }
                    if (gameObjectDataGrid == null)
                    {
                        gameObjectDataGrid = new GameObjectDatagrid();
                        gameObjectDataGrid.VerticalAlignment = VerticalAlignment.Stretch;
                        gameObjectDataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                        tabItem.Content = gameObjectDataGrid;
                    }
                    gameObjectDataGrid.SetGameObject(gameObject);
                    SwitchToTabIndex(this.tbCtrl.Items.IndexOf(tabItem));

                    openedGameObjectPanelDict.Add(gameObject, tabItem);
                });
            }

        }

        void tabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabItem ti = sender as TabItem;

            if (tbCtrl.Items.IndexOf(ti) == 0)
            {
                //关闭其他所有的
                while (tbCtrl.Items.Count >= 2)
                {
                    CloseTab(tbCtrl.Items[1] as TabItem);
                }

            }
            else
            {
                //关闭当前的
                CloseTab(ti);
            }
        }

        private void CloseTab(TabItem tabItem)
        {
            tabItem.Content = null;
            GameObject g = tabItem.DataContext as GameObject;
            openedGameObjectPanelDict.Remove(g);
            tbCtrl.Items.Remove(tabItem);
        }

        private void SwitchToTabIndex(int index)
        {
            Thread t = new Thread(delegate()
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        tbCtrl.SelectedIndex = index;
                    });
                }
            });
            t.Start();
        }


        public void TvMouseRightDown(object sender, MouseButtonEventArgs e)
        {
            GameObjectNode item = this.tvProperties.SelectedItem as GameObjectNode;
            try
            {
                System.Windows.Clipboard.SetDataObject(item.DisplayName);
            }
            catch(Exception ex)
            {

            }
        }

        private void tbCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tbCtrl.SelectedItem is TabItem)
            {
                TabItem tbItem = tbCtrl.SelectedItem as TabItem;
                if (tbItem.Content is GameObjectDatagrid)
                {
                    GameObjectDatagrid godg = tbItem.Content as GameObjectDatagrid;
                    if (godg.dgGameObjects.SelectedIndex == -1)
                    {
                        godg.dgGameObjects.SelectedIndex = 0;
                    }
                    else
                    {
                        godg.dgGameObjects_SelectionChanged_1(null, null);
                    }
                }
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            ShowUnityObject(displayedUnityObjectList[currentIndex - 1], false);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ShowUnityObject(displayedUnityObjectList[currentIndex + 1], false);
        }

        private void tvProperties_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            GameObjectNode item = this.tvProperties.SelectedItem as GameObjectNode;
            if (item == null)
            {
                return;
            }
            GameObject gameObject = item.gameObject;
            if (gameObject != null)
            {
                ShowGameObjectTabItem(gameObject);
            }
        }

        private void cbGameObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string text = cbGameObject.Text;
                if (text.StartsWith("0x") == false)
                {
                    text = "0x" + text;
                }
                int idInFile = 0;
                try
                {
                    idInFile = Convert.ToInt32(text, 16);
                    ShowGameObjectHierarchyTree(idInFile);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void ShowGameObjectHierarchyTree(int idInFile)
        {
            GameObjectNode searchNode = null;
            foreach (GameObjectNode g in MainWindow.instance.tvProperties.ItemsSource)
            {
                searchNode = SearchTreeForObject(idInFile, g);
                if (searchNode != null)
                {
                    break;
                }
            }
            if (searchNode == null) return;
            //已经发现了需要找的节点
            GameObjectTraceGraph wnd = new GameObjectTraceGraph();
            double dWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double dHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            wnd.WindowStartupLocation = WindowStartupLocation.Manual;
            wnd.Left = (int)dWidth - wnd.Width - 50;
            wnd.Top = (dHeight - wnd.Height) / 2;
            wnd.SetGameObjectNode(searchNode);
            wnd.Show();
        }


        private static GameObjectNode SearchTreeForObject(int idInFile, GameObjectNode treeViewItem)
        {
            if (treeViewItem.gameObject != null && treeViewItem.gameObject.ObjectInfo.Id == idInFile)
            {
                return treeViewItem;
            }
            foreach (GameObjectNode g in treeViewItem.Children)
            {
                GameObjectNode ret = SearchTreeForObject(idInFile, g);
                if (ret != null)
                {
                    return ret;
                }
            }
            return null;
        }

        private void ConvertFile(string fileName)
        {
            FileStream fileStream = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            StreamReader streamReader = new StreamReader(fileStream);
            string content = streamReader.ReadToEnd();
            streamReader.Close();

            Dictionary<string, string> toConvertSymbol = new Dictionary<string, string>();
            foreach (string line in content.Split('\n'))
            {
                List<String> symbols = ParseLine(line.Replace("\r", ""));
                foreach (string symbol in symbols)
                {
                    if (toConvertSymbol.ContainsKey(symbol) == false)
                    {
                        string symbolConverted = symbol.Replace("<", "_J_").Replace(">", "_C_").Replace("$", "_Y_").Replace("`", "_X_");
                        toConvertSymbol.Add(symbol, symbolConverted);
                    }
                }
            }
            fileStream.Close();


            if (toConvertSymbol.Keys.Count > 0)
            {
                fileStream = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Write);
                foreach (String key in toConvertSymbol.Keys)
                {
                    content = content.Replace(key, toConvertSymbol[key]);
                }
                StreamWriter sw = new StreamWriter(fileStream);
                sw.Write(content);
                sw.Close();
                fileStream.Close();
                MainWindow.instance.SetInfoLabelContent("转换文件:" + fileName);
            }
        }

        private string RemoveKeyword(string line)
        {
            line = line.Trim();
            bool bFound = true;
            while (bFound)
            {
                bFound = false;
                if (line.StartsWith("private"))
                {
                    line = line.Substring("private".Length);
                    bFound = true;
                }
                else if (line.StartsWith("public"))
                {
                    line = line.Substring("public".Length);
                    bFound = true;
                }
                else if (line.StartsWith("protected"))
                {
                    line = line.Substring("protected".Length);
                    bFound = true;
                }
                else if (line.StartsWith("internal"))
                {
                    line = line.Substring("internal".Length);
                    bFound = true;
                }
                else if (line.StartsWith("readonly"))
                {
                    line = line.Substring("readonly".Length);
                    bFound = true;
                }
                else if (line.StartsWith("static"))
                {
                    line = line.Substring("static".Length);
                    bFound = true;
                }
                else if (line.StartsWith("ref"))
                {
                    line = line.Substring("ref".Length);
                    bFound = true;
                }
                else if (line.StartsWith("out"))
                {
                    line = line.Substring("out".Length);
                    bFound = true;
                }
                line = line.Trim();
            }
            return line;
        }

        private List<string> ParseLine(string line)
        {
            List<string> ret = new List<string>();
            if (!(line.Contains("internal ") ||
                line.Contains("private ") ||
                line.Contains("protected ") ||
                line.Contains("public ")))
            {
                return ret;
            }

            line = line.Replace(";", "");

            if ((line.Contains("(") || line.Contains(")"))
                && (line.Contains("=") == false || (line.IndexOf("=") > line.IndexOf("("))))
            {
                //表示函数  ，例如int myFunction(int a,ref int b,int c=0)

                string lineLight = RemoveKeyword(line);
                //先找返回类型
                char[] charArray = lineLight.ToCharArray();
                string returnType = "";
                string funcName = "";
                int innerCount = 0;
                for (int i = 0; i < charArray.Length; i++)
                {
                    if (charArray[i] == '<')
                    {
                        innerCount++;
                    }
                    else if (charArray[i] == '>')
                    {
                        innerCount--;
                    }
                    if (charArray[i] == ' ' && innerCount == 0)
                    {
                        returnType = lineLight.Substring(0, i);
                        funcName = lineLight.Substring(i);
                        break;
                    }
                }

                returnType = returnType.Trim(); //如上例中的int
                funcName = funcName.Trim();     //如上例中的myFunction(int a,ref int b,int c=0)
                ret.AddRange(ProcessTypeString(returnType));
                ret.AddRange(ProcessFunctionDefString(funcName));
                return ret;
            }
            //////////////////////////////////////////////
            if (line.Contains(" class "))
            {
                //表示类
                string className = line.Substring(line.IndexOf(" class ") + 7);
                if (className.Contains(":"))
                {
                    className = className.Substring(0, className.IndexOf(":")).Trim();
                }
                if (className.EndsWith(">"))
                {
                    int index = className.IndexOf(",");
                    if (index != -1)
                    {
                        int leftBrace = GetLeftBrace(className, index);
                        int rightBrace = GetRightBrace(className, index);
                        if (leftBrace != -1 && rightBrace != -1)
                        {
                            string classNameLite = className.Substring(0, leftBrace);
                            if (classNameLite.Contains("<") && classNameLite.Contains(">"))
                            {
                                ret.Add(classNameLite);
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Error");
                        }
                    }
                    else
                    {
                        int leftBrace = GetLeftBrace(className, className.Length - 2);
                        string classNameLite = className.Substring(0, leftBrace);
                        if (classNameLite.Contains("<") && classNameLite.Contains(">"))
                        {
                            ret.Add(classNameLite);
                        }
                    }
                }
                else if (className.StartsWith("<"))
                {
                    //类处理
                    ret.Add(className);
                }
                return ret;
            }

            //////////////////////////////////////////////
            //变量处理
            //先去掉 private,public ,protected,internal,readonly,static等关键字
            if (line.Contains("="))
            {
                string[] eFields = line.Split('=');
                ret.AddRange(ProcessVariablesDefString(eFields[0]));
            }
            else
            {
                ret.AddRange(ProcessVariablesDefString(line));
            }


            return ret;
        }

        /// <summary>
        /// 处理函数定义字符串，例如myFunction(int a,ref int b=0)，无返回值 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<string> ProcessFunctionDefString(string line)
        {
            List<string> ret = new List<string>();
            char[] charArray = line.ToCharArray();
            int innerCount = 0;
            int endIndex = charArray.Length - 1;
            string functionName = "";
            for (int i = charArray.Length - 2; i >= 0; i--)
            {
                if (charArray[i] == '>' || charArray[i] ==']')
                {
                    innerCount += 1;
                }
                else if (charArray[i] == '<' || charArray[i] == '[')
                {
                    innerCount -= 1;
                }
                else if (charArray[i] == ',' && innerCount == 0)
                {
                    string param = line.Substring(i + 1, endIndex - i - 1);
                    ret.AddRange(ProcessFunctionParam(param));
                    endIndex = i;
                }
                else if (charArray[i] == '(')
                {
                    string param = line.Substring(i + 1, endIndex - i - 1);
                    ret.AddRange(ProcessFunctionParam(param));

                    functionName = line.Substring(0,i).Trim();
                    ret.AddRange(ProcessTypeString(functionName));
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 处理函数参数，例如int x(int a,ref int b=0)中的"int a"和"ref int b=0"
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<string> ProcessFunctionParam(string line)
        {
            List<string> ret = new List<string>();
            string param = line.Split('=')[0];      //去除"int b=0"中的"=0"
            param = RemoveKeyword(param.Trim());
            ret.AddRange(ProcessVariablesDefString(param));
            return ret;
        }



        /// <summary>
        /// 处理变量定义，例如int a;
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<string> ProcessVariablesDefString(string line)
        {
            List<string> ret = new List<string>();
            string lineLight = RemoveKeyword(line);
            int lastIndex = lineLight.LastIndexOf(' ');
            if (lastIndex == -1) return ret;
            string variableName = lineLight.Substring(lastIndex + 1);
            ret.AddRange(ProcessVariableName(variableName));

            //开始处理类型
            string typeName = lineLight.Substring(0, lastIndex);
            List<string> retList = ProcessTypeString(typeName);
            if (retList != null && retList.Count != 0)
            {
                ret.AddRange(retList);
            }
            return ret;
        }

        /// <summary>
        /// 处理变量名，例如int a中的a
        /// </summary>
        /// <param name="typeString"></param>
        /// <returns></returns>
        private List<string> ProcessVariableName(string typeString)
        {
            List<string> retList = new List<string>();
            typeString = typeString.Trim();
            if (typeString.Contains("<") || typeString.Contains(">") ||
                typeString.Contains("$") || typeString.Contains("`"))
            {
                retList.Add(typeString);
            }
            return retList;
        }

        /// <summary>
        /// 处理单个类型，这个类型中肯定没有泛型，例如int ,string,List,Dictionary等，肯定不会有List<int>
        /// </summary>
        /// <param name="typeString"></param>
        /// <returns></returns>
        private List<string> ProcessSingleType(string typeString)
        {
            List<string> retList = new List<string>();
            if (typeString.StartsWith("<"))
            {
                retList.Add(typeString.Trim());
            }
            else if (typeString.Contains("`"))
            {
                retList.Add(typeString.Trim());
            }
            else if (typeString.Contains("$"))
            {
                retList.Add(typeString.Trim());
            }
            return retList;
        }


        /// <summary>
        /// 处理复合类型定义，例如Dictionary<string,List<int>>，也可能仅仅是int
        /// 注意，该函数还能够用来处理函数名
        /// </summary>
        /// <param name="typeString"></param>
        /// <returns></returns>
        private List<string> ProcessTypeString(string typeString)
        {
            List<string> retList = new List<string>();

            typeString = typeString.Trim();

            if (typeString.EndsWith("]"))
            {
                int indexT = typeString.LastIndexOf("[");
                typeString = typeString.Substring(0, indexT);
            }

            typeString = typeString.Trim();
            if (typeString.EndsWith(">"))
            {
                char[] charArray = typeString.ToCharArray();
                //往前找到第一个,和<符号
                int endIndex = charArray.Length - 1;
                int innerCount = 0;
                for (int i = charArray.Length - 2; i >= 0; i--)
                {
                    if (charArray[i] == '>')
                    {
                        innerCount++;
                    }
                    else if (charArray[i] == '<')
                    {
                        if (innerCount > 0)
                        {
                            innerCount--;
                        }
                        else
                        {
                            string subType = typeString.Substring(i + 1, endIndex - i - 1);
                            retList.AddRange(ProcessTypeString(subType));
                            //截取前面的
                            string preSubType = typeString.Substring(0, i);
                            retList.AddRange(ProcessTypeString(preSubType));
                            return retList;
                        }
                    }
                    else if (charArray[i] == ',')
                    {
                        if (innerCount == 0)
                        {
                            string subType = typeString.Substring(i + 1, endIndex - i - 1);
                            retList.AddRange(ProcessTypeString(subType));
                            endIndex = i;
                        }
                    }
                }
            }
            else
            {
                retList.AddRange(ProcessSingleType(typeString));
            }
            return retList;
        }



        private int GetLeftBrace(string line, int index)
        {
            int rCount = 0;
            char[] charArray = line.ToCharArray();
            int iter = index;
            while (iter >= 0)
            {
                if (charArray[iter] == '>')
                {
                    rCount++;
                }
                else if (charArray[iter] == '<')
                {
                    if (rCount == 0)
                    {
                        return iter;
                    }
                    rCount--;
                }
                iter--;
            }
            return -1;
        }

        private int GetRightBrace(string line, int index)
        {
            int rCount = 0;
            char[] charArray = line.ToCharArray();
            int iter = index;
            while (iter < charArray.Length)
            {
                if (charArray[iter] == '<')
                {
                    rCount++;
                }
                else if (charArray[iter] == '>')
                {
                    if (rCount == 0)
                    {
                        return iter;
                    }
                    rCount--;
                }
                iter++;
            }
            return -1;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
