using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnityAnalyzer
{

    public class GameObjectNode
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public int parentId { get; set; }
        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }
        public List<GameObjectNode> Children { get; set; }
        public GameObject gameObject{get; set;}

        public String ItemColor
        {
            get
            {
                if(gameObject.IsActive==true)
                {
                    return "Black";
                }
                return "DarkRed";
            }
        }

        public GameObjectNode parent { get; set; }
        public GameObjectNode()
        {
            Children = new List<GameObjectNode>();
        }
    }

    public class AssetsFile:UnityFile
    {
        public static string nowParsingFile = "";

        /// <summary>
        /// 存放objects的起始偏移地址
        /// </summary>
        protected int objectsOffset = -1;
        public int ObjectsOffset
        {
            get { return objectsOffset; }
        }

        /// <summary>
        /// Object的信息
        /// </summary>
        protected List<ObjectInfo> objectInfoList = new List<ObjectInfo>();
        public List<ObjectInfo> ObjectInfoList
        {
            get { return objectInfoList; }
        }

        /// <summary>
        /// 所有的Object
        /// </summary>
        protected List<UnityObject> objectList = new List<UnityObject>();
        public List<UnityObject> ObjectList
        {
            get { return objectList; }
        }

        /// <summary>
        /// 根据每个ClassIDType来索引其对象
        /// </summary>
        protected Dictionary<ClassIDType, List<UnityObject>> classTypeDict = new Dictionary<ClassIDType, List<UnityObject>>();
        public Dictionary<ClassIDType, List<UnityObject>> ClassTypeDict
        {
            get { return classTypeDict; }
        }

        /// <summary>
        /// 该UnityFile引用到的外部的UnityFiles
        /// </summary>
        protected List<String> serializedUnityFiles = new List<String>();
        public List<String> SerializedUnityFiles
        {
            get { return serializedUnityFiles; }
        }

        /// <summary>
        /// 用于表示树的各个GameObj的节点
        /// </summary>
        private List<GameObjectNode> gameObjTreeList = new List<GameObjectNode>();
        public List<GameObjectNode> GameObjTreeList
        {
            get { return gameObjTreeList; }
        }

        private Dictionary<int, GameObjectNode> gameObjDict = new Dictionary<int, GameObjectNode>();

        protected AssetsFile(string fullname, string aliasname, Analyzer Analyzer):base(fullname,aliasname,Analyzer)
        {
        }

        private static int[] GetFileVersion(byte[] content)
        {
            int[] version = new int[3];
            version[0] = content[0x14] - 48;
            version[1] = content[0x16] - 48;
            version[2] = content[0x18] - 48;
            return version;
        }

        public static AssetsFile ParseFile(string fileName,string aliasName,Analyzer Analyzer)
        {
            nowParsingFile = aliasName;

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] content = new byte[fs.Length];
            fs.Read(content, 0, (int)fs.Length);
            fs.Close();

            int[] version=GetFileVersion(content);

            AssetsFile ret = new AssetsFile(fileName, aliasName, Analyzer);
            ret.unityFileVersion = version;
            ret.objectsOffset = (content[12] << 24) + (content[13] << 16) + (content[14] << 8) + content[15];

            int index = 0;
            int totalObjectsCount = 0;

            if (ret.unityFileVersion[0] == 4 && ret.unityFileVersion[1] == 6)
            {
                totalObjectsCount = BitConverter.ToInt32(content, 0x28);
                ret.serializedUnityFiles.AddRange(ret.GetAssetsFiles(content, 0x30 + 0x14 * totalObjectsCount + 0x15));
            }
            else if (ret.unityFileVersion[0] == 5 && ret.unityFileVersion[1] == 3)
            {
                int baseCount = BitConverter.ToInt32(content, 0x21);
                index = 0x21 + 4;
                for (int i = 0; i < baseCount; i++)
                {
                    int class_id = BitConverter.ToInt32(content, index);
                    index += 4;
                    index += 16;
                    if(class_id<0)
                    {
                        index += 16;
                    }
                }
                totalObjectsCount = BitConverter.ToInt32(content, index);
                index += 4;
            }
            else
            {
                throw new Exception("不支持的版本:" + ret.VersionString);
            }
            

            MainWindow.instance.SetInfoLabelContent("分析文件:" + aliasName + "0/" + totalObjectsCount);

            int offset = 0;
            //从0x30开始依次读取ObjectInfo
            for (int i = 0; i < totalObjectsCount; i++)
            {
                Int64 pathId = 0;
                if (ret.unityFileVersion[0] == 4 && ret.unityFileVersion[1] == 6)
                {
                    offset = 0x30 + i * 0x14;
                }
                else if (ret.unityFileVersion[0] == 5 && ret.unityFileVersion[1] == 3)
                {
                    index += Util.GetAlignCount(index, 0);
                    pathId=BitConverter.ToInt64(content, index);
                    index += 8;
                    offset = index;
                }
                ObjectInfo objInfo = new ObjectInfo();
                objInfo.ByteStart = BitConverter.ToInt32(content, offset); index += 4;
                objInfo.ByteSize = BitConverter.ToInt32(content, offset + 4); index += 4;
                objInfo.TypeID = BitConverter.ToInt32(content, offset + 8); index += 4;
                objInfo.ClassID = BitConverter.ToInt16(content, offset + 12); index += 4;
                objInfo.IsDestroyed = BitConverter.ToUInt16(content, offset + 14); index += 2;
                objInfo.DebugLineStart = BitConverter.ToInt16(content, offset + 16); index += 2;
                objInfo.Id = 1 + i;
                objInfo.UnityFile = ret;

                if (ret.unityFileVersion[0] == 5 && ret.unityFileVersion[1] == 3)
                {
                    objInfo.DebugLineStart =(int) pathId;
                }

                ret.objectInfoList.Add(objInfo);
            }

            if (ret.unityFileVersion[0] == 5 && ret.unityFileVersion[1] == 3)
            {
                ret.serializedUnityFiles.AddRange(ret.GetAssetsFiles(content, index));
            }

            for (int i = 0; i < totalObjectsCount; i++)
            {
                ObjectInfo objInfo = ret.objectInfoList[i];
                UnityObject unityObject = UnityObject.CreateUnityObject(objInfo, content, ret.objectsOffset);
                ret.objectList.Add(unityObject);

                ///////////////////////////////////////////////////////////////////////////
                if (ret.classTypeDict.ContainsKey(objInfo.ClassIDType) == false)
                {
                    List<UnityObject> temp = new List<UnityObject>();
                    temp.Add(unityObject);
                    ret.classTypeDict.Add(objInfo.ClassIDType, temp);
                }
                else
                {
                    ret.classTypeDict[objInfo.ClassIDType].Add(unityObject);
                }

                if (unityObject as GameObject != null)
                {
                    //如果是GameObject，加入到dict中去
                    ret.CreateGameObjectNode(unityObject as GameObject);
                }

                MainWindow.instance.SetInfoLabelContent("分析文件:" + aliasName + ": " + (i + 1).ToString() + "/" + totalObjectsCount);
            }

            //形成GameObject的树
            ret.FormGameObjectTree();

            ret.Log();
            return ret;
        }


        public void CreateGameObjectNode(GameObject aGameObject)
        {

            GameObjectNode gameObjectNode = new GameObjectNode() {
                DisplayName = aGameObject.Name,
                Name = aGameObject.GName,
                id = aGameObject.Id,
                parentId = aGameObject.ParentID,
                IsExpanded = false,
                IsSelected=false,
                gameObject=aGameObject,
            };
            gameObjDict.Add(aGameObject.Id, gameObjectNode);
        }


        private void FormGameObjectTree()
        {
            //GameObjectNode root = new GameObjectNode()
            //{
            //    DisplayName = "root",
            //    Name = "tvroot",
            //    id = 0,
            //    parentId = -1,
            //    IsExpanded = false,
            //    gameObject = null,
            //};
            //gameObjTreeList.Add(root);

            foreach (int id in gameObjDict.Keys)
            {
                GameObjectNode gameObjectNode = gameObjDict[id];
                GameObject gameObject = gameObjectNode.gameObject;
                int index = -1;
                GameObject parent = gameObject.GetParentGameObject(ref index);

                if (parent != null)
                {
                    GameObjectNode parentGameObjectNode = gameObjDict[parent.Id];
                    if (index >= parentGameObjectNode.Children.Count)
                    {
                        parentGameObjectNode.Children.Add(gameObjectNode);
                    }
                    else
                    {
                        parentGameObjectNode.Children.Insert(index, gameObjectNode);
                    }
                    gameObjectNode.parent = parentGameObjectNode;
                }
                else
                {
                    gameObjTreeList.Add(gameObjectNode);
                }
            }
        }

        private List<String> GetAssetsFiles(byte[] content,int indexStart)
        {
            int index = indexStart;
            List<String> assetsFiles = new List<String>();
            string aliasname = "";
            while (index < content.Length && index<this.objectsOffset)
            {
                if (content[index] != 0)
                {
                    aliasname += (char)content[index];
                    index++;
                }
                else if (aliasname == "")
                {
                    index++;
                }
                else
                {
                    if (aliasname.EndsWith(".assets") || aliasname == "library/unity default resources" ||
                        aliasname == "resources/unity_builtin_extra")
                    {
                        assetsFiles.Add(aliasname);
                    }
                    aliasname = "";
                    index += 0x16;
                }
            }
            return assetsFiles;
        }

        private static int SearchNextValidCharacter(byte[] content, int index)
        {
            int ret = index;
            while (ret < content.Length)
            {
                if ((content[ret] >= 'a' && content[ret] <= 'z') ||
                    (content[ret] >= 'A' && content[ret] <= 'Z') || content[ret] == '/' || content[ret] == '_'
                    || content[ret] == ' ')
                {
                    return ret;
                }
                ret++;
            }
            return -1;
        }

        private string GetAliasFileName(byte[] content, int startIndex)
        {
            string path = this.FullFileName.Substring(0, this.FullFileName.LastIndexOf('\\') + 1);
            string fileName = "";
            for (int i = startIndex; content[i] != 0; i++)
            {
                fileName += (char)content[i];
            }
            string ret = fileName;
            if (fileName.StartsWith("library"))
            {
                fileName = fileName.Replace("library", "Resources");
            }
            if (System.IO.File.Exists(path + fileName))
            {
                return ret;
            }
            return null;
        }

        public void Log()
        {
            //for (int i = 0; i < objectList.Count; i++)
            //{
            //    Console.WriteLine(i.ToString("x") + " " + objectList[i].ObjectInfo.ToString());
            //}
            //Console.WriteLine("");
            //for (int i = 0; i < objectList.Count; i++)
            //{
            //    if (objectList[i].ObjectInfo.ClassIDType == ClassIDType.CLASS_GameObject)
            //    {
            //        GameObject gameObject = objectList[i] as GameObject;
            //        Console.WriteLine(gameObject.ToString());
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializedFileIndex">序号，从1开始</param>
        /// <returns></returns>
        public UnityFile GetSerializedUnityFileByFileIndex(int serializedFileIndex)
        {
            if (serializedFileIndex <= 0 || serializedFileIndex > this.serializedUnityFiles.Count) return null;
            string aliasFileName= this.serializedUnityFiles[serializedFileIndex - 1];
            return Analyzer.GetUnityFileByAliasName(aliasFileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifierInFile">从1开始</param>
        /// <returns></returns>
        public UnityObject GetUnityObjectByIdentifier(int identifierInFile)
        {
            //对于library/unity default resources和resources/unity_builtin_extra这两个文件区分处理
            if (this.aliasFileName.Equals("library/unity default resources") ||
                this.aliasFileName.Equals("resources/unity_builtin_extra"))
            {
                foreach (UnityObject uObject in objectList)
                {
                    if (identifierInFile==uObject.DebugLineStart)
                    {
                        return uObject;
                    }
                }
                return null;
            }
            else
            {
                if (identifierInFile <= 0 || identifierInFile > objectList.Count) return null;
                return objectList[identifierInFile - 1];
            }
        }

        /// <summary>
        /// index从1开始，如果是0，则表示自己
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override UnityFile GetSerializedUnityFileByIndex(int index)
        {
            if (index == 0) return this;
            if (index < 0 || index > serializedUnityFiles.Count) return null;

            return this.Analyzer.GetUnityFileByAliasName(serializedUnityFiles[index - 1]);
        }
    }
}
