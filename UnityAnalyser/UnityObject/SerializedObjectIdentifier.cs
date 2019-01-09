using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{

    


    public class SerializedObjectIdentifier
    {
        public int serializedFileIndex;
        /// <summary>
        /// 从1开始
        /// </summary>
        public int identifierInFile;

        public SerializedObjectIdentifier(int serializedFileIndex, int identifierInFile)
        {
            this.serializedFileIndex = serializedFileIndex;
            this.identifierInFile = identifierInFile;
        }

        public override bool Equals(object obj)
        {
            SerializedObjectIdentifier other = obj as SerializedObjectIdentifier;
            if(other!=null)
            {
                return serializedFileIndex == other.serializedFileIndex && identifierInFile == other.identifierInFile;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return serializedFileIndex.GetHashCode() ^ identifierInFile.GetHashCode();
        }

        public override string ToString()
        {
            return "FileID:   " + serializedFileIndex + "  ID:   0x" + identifierInFile.ToString("x");
        }
    }

    public class SerializedObjectIdentifierWithFile
    {
        public SerializedObjectIdentifier soi;
        public string fullFileName;

        public override bool Equals(object obj)
        {
            SerializedObjectIdentifierWithFile other = obj as SerializedObjectIdentifierWithFile;
            if (other != null)
            {
                return soi.Equals(other.soi) && fullFileName == other.fullFileName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return soi.GetHashCode() ^ fullFileName.GetHashCode();
        }
    }

  
}
