using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnityAnalyzer
{
    public class UnityFile
    {
        protected int[] unityFileVersion;
        public int[] UnityFileVersion
        {
            get { return unityFileVersion; }
        }

        public string VersionString
        {
            get
            {
                return unityFileVersion[0].ToString() + "." + unityFileVersion[1].ToString() + "." + unityFileVersion[2].ToString();
            }
        }

        protected Analyzer analyzer;
        public Analyzer Analyzer
        {
            get { return analyzer; }
            set { analyzer = value; }
        }

        /// <summary>
        /// Unity文件的类型
        /// </summary>
        protected UnityFileType unityFileType;
        public UnityFileType UnityFileType
        {
            get { return unityFileType; }
        }

        /// <summary>
        /// 完整的文件名，包括路径
        /// </summary>
        protected string fullFileName;
        public string FullFileName
        {
            get { return fullFileName; }
            set { fullFileName = value; }
        }

        /// <summary>
        /// 不包括路径的文件名，但可能是别名，例如library/unity default resources。
        /// </summary>
        protected string aliasFileName;
        public string AliasFileName
        {
            get { return aliasFileName; }
            set { aliasFileName = value; }
        }

        protected UnityFile(string fullname, string aliasname, Analyzer Analyzer)
        {
            this.fullFileName = fullname;
            this.aliasFileName = aliasname;
            this.Analyzer = Analyzer;
        }

        /// <summary>
        /// index从1开始，如果是0，则表示自己
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual UnityFile GetSerializedUnityFileByIndex(int index)
        {
            return this;
        }
    }
}
