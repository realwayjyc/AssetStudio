using System;
using System.Text;

namespace UnityAnalyzer
{
    public class ObjectReader
    {
        public ObjectInfo ObjectInfo { get; }
        public int[] Version => ObjectInfo.UnityFileVersion;

        private readonly byte[] _content;

        private int _currentIndex;

        public ObjectReader(ObjectInfo objectInfo,byte[] content,int startIndex)
        {
            ObjectInfo = objectInfo;
            _content = content;
            _currentIndex = startIndex;
        }

        public int ReadInt32()
        {
            _currentIndex += 4;
            return BitConverter.ToInt32(_content, _currentIndex - 4);
        }

        public uint ReadUInt32()
        {
            _currentIndex += 4;
            return BitConverter.ToUInt32(_content, _currentIndex - 4);
        }

        public float ReadSingle()
        {
            _currentIndex += 4;
            return BitConverter.ToSingle(_content, _currentIndex - 4);
        }

        public bool ReadBoolean()
        {
            _currentIndex += 1;
            return _content[_currentIndex - 1] == 1;
        }

        public Vector3F ReadVector3()
        {
            return new Vector3F(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector4F ReadVector4()
        {
            return new Vector4F(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector3F ReadVector4AsVector3()
        {
            return new Vector3F(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        private static T[] ReadArray<T>(Func<T> del, int length)
        {
            var array = new T[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = del();
            }
            return array;
        }

        public uint[] ReadUInt32Array()
        {
            return ReadArray(ReadUInt32, ReadInt32());
        }

        public int[] ReadInt32Array()
        {
            return ReadArray(ReadInt32, ReadInt32());
        }

        public float[] ReadSingleArray()
        {
            return ReadArray(ReadSingle, ReadInt32());
        }

        public Quaternion ReadQuaternion()
        {
            return new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public void AlignStream()
        {
            AlignStream(4);
        }

        public void AlignStream(int alignment)
        {
            int pos = _currentIndex;
            int mod = pos % alignment;
            if (mod != 0)
            {
                _currentIndex += alignment - mod;
            }
        }

        public string ReadAlignedString()
        {
            int length = ReadInt32();
            if (length == 0) return "";
            if (length < 0 || length > _content.Length - _currentIndex) throw new Exception("ReadAlignedString error");
            string result = Encoding.UTF8.GetString(_content, _currentIndex, length);
            _currentIndex += length;
            AlignStream(4);
            return result;
        }
    }
}
