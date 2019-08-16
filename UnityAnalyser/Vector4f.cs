namespace UnityAnalyzer
{
    public struct Vector4F
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public override string ToString()
        {
            return X + "  " + Y + "  " + Z + "  " + W;
        }

        public Vector4F(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
