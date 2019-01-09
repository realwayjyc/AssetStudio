using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnityAnalyzer
{
    public class Analyzer
    {
        /// <summary>
        /// 一个UnityGame的Folder对应一个Analyzer
        /// </summary>
        public static Dictionary<string, Analyzer> unityFolderDict = new Dictionary<string, Analyzer>();

        //需要根据Component找到GameObject
        public static Dictionary<SerializedObjectIdentifierWithFile, GameObject> componentDict = new Dictionary<SerializedObjectIdentifierWithFile, GameObject>();

        public static GameObject GetGameObjectByComponent(SerializedObjectIdentifierWithFile soiFile)
        {
            if (componentDict.ContainsKey(soiFile) == false) return null;
            return componentDict[soiFile];
        }
        public void ParseMainData(string mainDataFile)
        {
            string folder = mainDataFile.Substring(0, mainDataFile.LastIndexOf('\\') + 1);
            //if (unityFolderDict.ContainsKey(folder))
            //{
            //    return unityFolderDict[folder];
            //}
            
            this.Parse(folder);
            //unityFolderDict.Add(folder, Analyzer);
            //return Analyzer;
        }

        private TagManager tagManager;
        public TagManager TagManager
        {
            get { return tagManager; }
            set { tagManager = value; }
        }


        /// <summary>
        /// key表示该UnityFile的AliasName
        /// </summary>
        public Dictionary<string, UnityFile> unityFileDict = new Dictionary<string, UnityFile>();

        public Analyzer()
        {
        }

        private string current_folder;

        public string Current_folder
        {
            get { return current_folder; }
            set { current_folder = value; }
        }

       

        private void Parse(string folder)
        {
            current_folder = folder;

            //先解析MainData
            foreach (string file in Directory.GetFiles(folder))
            {
                string aliasname = "";
                if (file.EndsWith("mainData"))
                {
                    aliasname = file.Substring(file.LastIndexOf('\\') + 1);
                    unityFileDict.Add(aliasname, AssetsFile.ParseFile(file, aliasname, this));
                }
                else if (file.EndsWith("globalgamemanagers"))
                {
                    //Unity5的处理
                    aliasname = file.Substring(file.LastIndexOf('\\') + 1);
                    unityFileDict.Add(aliasname, AssetsFile.ParseFile(file, aliasname, this));
                }
            }

            //AssetsFile.ParseFile(@"F:\unity\project\test_unity5_project\test_unity5_Data\sharedassets0.assets", "sharedassets0.assets", this);

            //再解析Resource目录
            foreach (string file in Directory.GetFiles(folder + "Resources\\"))
            {
                string aliasname = "";
                if (file.EndsWith("unity default resources"))
                {
                    aliasname = "library/unity default resources";
                    unityFileDict.Add(aliasname, AssetsFile.ParseFile(file, aliasname, this));
                }
                else if (file.EndsWith("unity_builtin_extra"))
                {
                    aliasname = "resources/unity_builtin_extra";
                    unityFileDict.Add(aliasname, AssetsFile.ParseFile(file, aliasname, this));
                }
            }



            //最后解析主目录下的其他内容
            foreach (string file in Directory.GetFiles(folder))
            {
                string aliasname = "";
                if(file.EndsWith(".resS"))
                {
                    continue;
                }
                if (file.EndsWith(".assets") || file.Substring(file.LastIndexOf('\\') + 1).StartsWith("level"))
                {
                    aliasname = file.Substring(file.LastIndexOf('\\') + 1);
                    unityFileDict.Add(aliasname, AssetsFile.ParseFile(file, aliasname, this));
                }
            }

            MainWindow.instance.SetInfoLabelContent("文件分析完毕");
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    MainWindow.instance.lbFiles.ItemsSource = unityFileDict.Values;
                    if (unityFileDict.ContainsKey("globalgamemanagers"))
                    {
                        MainWindow.instance.ShowUnityFileItem(unityFileDict["globalgamemanagers"]);
                    }
                    else if (unityFileDict.ContainsKey("mainData"))
                    {
                        MainWindow.instance.ShowUnityFileItem(unityFileDict["mainData"]);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Unknown version");
                    }
                });
            }
        }

        public UnityFile GetUnityFileByAliasName(string aliasName)
        {
            if (unityFileDict.ContainsKey(aliasName))
            {
                return unityFileDict[aliasName];
            }
            return null;
        }
    }
}
