namespace UnityAnalyzer
{
    public struct Vector3F
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override string ToString()
        {
            return X + "  " + Y + "  " + Z;
        }

        public Vector3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3F(float x, float y, float z,float w)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
