using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAnalyzer
{
    public class ObjectInfo
    {
        private UnityFile unityFile;
        public UnityFile UnityFile
        {
            get { return unityFile; }
            set { unityFile = value; }
        }

        private int byteStart;
        public int ByteStart
        {
            get { return byteStart; }
            set { byteStart = value; }
        }

        private int byteSize;
        public int ByteSize
        {
            get { return byteSize; }
            set { byteSize = value; }
        }

        private int typeID;
        public int TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        private Int16 classID;
        public Int16 ClassID
        {
            get { return classID; }
            set { classID = value; }
        }

        public ClassIDType ClassIDType
        {
            get
            {
                return (ClassIDType)typeID;
            }
        }

        private UInt16 isDestroyed;
        public UInt16 IsDestroyed
        {
            get { return isDestroyed; }
            set { isDestroyed = value; }
        }

        private int debugLineStart;
        public int DebugLineStart
        {
            get { return debugLineStart; }
            set { debugLineStart = value; }
        }

        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Unity 5里面的PathID;
        /// </summary>
        private long pathId;
        public long PathId
        {
            get { return pathId; }
            set { pathId = value; }
        }

        /// <summary>
        /// 获得该ObjectInfo所在的文件的版本号
        /// </summary>
        public int[] UnityFileVersion
        {
            get
            {
                return unityFile.UnityFileVersion;
            }
        }
        public override string ToString()
        {
            return byteStart.ToString("x") + "  \t" + byteSize.ToString("x") + "  \t" + ClassIDType.ToString()+" debugLineStart:0x"+this.debugLineStart.ToString("X");
        }

        public UnityObject GetUnityObjectBySerializedObjectIdentifier(SerializedObjectIdentifier identifier)
        {
            UnityFile searchFile = null;
            if (identifier.serializedFileIndex == 0)
            {
                //从UnityObject所在的文件读取
                searchFile = this.UnityFile;
            }
            else
            {
                searchFile = (this.UnityFile as AssetsFile).GetSerializedUnityFileByFileIndex(identifier.serializedFileIndex);
            }
            if (searchFile == null) return null;
            return (searchFile as AssetsFile).GetUnityObjectByIdentifier(identifier.identifierInFile);
        }
    }
}
