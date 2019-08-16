namespace UnityAnalyzer
{
    public class ObjectReader
    {
        public ObjectInfo ObjectInfo { get; }
        public int[] Version => ObjectInfo.UnityFileVersion;

        private byte[] _content;

        private int _startIndex;

        private int _currentIndex;

        public ObjectReader(ObjectInfo objectInfo,byte[] content,int startIndex)
        {
            ObjectInfo = objectInfo;
            _content = content;
            _startIndex = startIndex;
            _currentIndex = _startIndex;
        }
    }
}
