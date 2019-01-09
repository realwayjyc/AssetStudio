using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class Matrix44f
    {
        public float _11;
        public float _12;
        public float _13;
        public float _14;

        public float _21;
        public float _22;
        public float _23;
        public float _24;

        public float _31;
        public float _32;
        public float _33;
        public float _34;

        public float _41;
        public float _42;
        public float _43;
        public float _44;

        public float itemAt(int row,int col)
        {
            if (row == 1 && col == 1) return _11;
            if (row == 1 && col == 2) return _12;
            if (row == 1 && col == 3) return _13;
            if (row == 1 && col == 4) return _14;

            if (row == 2 && col == 1) return _21;
            if (row == 2 && col == 2) return _22;
            if (row == 2 && col == 3) return _23;
            if (row == 2 && col == 4) return _24;

            if (row == 3 && col == 1) return _31;
            if (row == 3 && col == 2) return _32;
            if (row == 3 && col == 3) return _33;
            if (row == 3 && col == 4) return _34;

            if (row == 4 && col == 1) return _41;
            if (row == 4 && col == 2) return _42;
            if (row == 4 && col == 3) return _43;
            if (row == 4 && col == 4) return _44;
            return float.NaN;
        }

        public static Matrix44f Create(byte[] content, int objectOffset, ref int index)
        {
            Matrix44f ret = new Matrix44f();
            ret._11 = BitConverter.ToSingle(content, index); index += 4;
            ret._12 = BitConverter.ToSingle(content, index); index += 4;
            ret._13 = BitConverter.ToSingle(content, index); index += 4;
            ret._14 = BitConverter.ToSingle(content, index); index += 4;

            ret._21 = BitConverter.ToSingle(content, index); index += 4;
            ret._22 = BitConverter.ToSingle(content, index); index += 4;
            ret._23 = BitConverter.ToSingle(content, index); index += 4;
            ret._24 = BitConverter.ToSingle(content, index); index += 4;

            ret._31 = BitConverter.ToSingle(content, index); index += 4;
            ret._32 = BitConverter.ToSingle(content, index); index += 4;
            ret._33 = BitConverter.ToSingle(content, index); index += 4;
            ret._34 = BitConverter.ToSingle(content, index); index += 4;

            ret._41 = BitConverter.ToSingle(content, index); index += 4;
            ret._42 = BitConverter.ToSingle(content, index); index += 4;
            ret._43 = BitConverter.ToSingle(content, index); index += 4;
            ret._44 = BitConverter.ToSingle(content, index); index += 4;

            return ret;
        }
    }
}
