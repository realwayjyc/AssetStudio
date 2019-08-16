namespace UnityAnalyzer
{
    public struct Vector2F
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override string ToString()
        {
            return X+"  "+Y;
        }

        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
