using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Color = System.Drawing.Color;

namespace UnityAnalyzer
{
    public class Util
    {
        private static float episilon = 0.000001f;
        public static int GetAlignCount(int index, int startOffset)
        {
            int diff = index - startOffset;
            return (int)(((diff + 3) & 0xFFFFFFFC) - diff);
        }

        private static float RoundValue(float value)
        {
            if (value>-episilon &&  value < episilon)
            {
                return 0.0f;
            }

            int intV = (int) value;
            if ((value - (float) intV) >= -episilon && (value - (float) intV) <= episilon)
            {
                return (float) intV;
            }

            return value;
        }

        public static void Quaternion2Euler(float x, float y, float z, float w,
            out float xAngle, out float yAngle, out float zAngle)
        {
            float z1 = 2 * (w * z + y * x);
            float z2 = 1 - 2 * (x * x + z * z);

            float x1 = 2 * (w * x - z * y);

            float y1 = 2 * (w * y + x * z);
            float y2 = 1 - 2 * (x * x + y * y);

            z1 = RoundValue(z1);
            z2 = RoundValue(z2);
            x1 = RoundValue(x1);

            y1 = RoundValue(y1);
            y2 = RoundValue(y2);

            zAngle = (float)(180 * Math.Atan2(z1,z2) / Math.PI);
            xAngle = (float)(180 * Math.Asin(x1) / Math.PI);
            yAngle = (float)(180 * Math.Atan2(y1, y2) / Math.PI);

            if (xAngle < 0)
            {
                xAngle += 360;
            }
            if (yAngle < 0)
            {
                yAngle += 360;
            }
            if (zAngle < 0)
            {
                zAngle += 360;
            }
        }

        public static string readStringAndAlign(byte[] content, int objectOffset,ref int index)
        {
            int stringcount = BitConverter.ToInt32(content, index);
            index += 4;

            string ret = ASCIIEncoding.UTF8.GetString(content,index,stringcount);
            index += stringcount;
            index += Util.GetAlignCount(index, objectOffset);
            return ret;
        }

        public static string GetRectangleString(RectangleF rect)
        {
            return rect.X.ToString() + "  " + rect.Y.ToString() + "  " + rect.Width.ToString() + "  " + rect.Height.ToString();
        }

        public static string GetColorRgbaString(Color c)
        {
            return c.R + "   " + c.G + "   " + c.B + "   " + c.A;
        }

        /// <summary>
        /// 从index开始读取一系列SerializedObjectIdentifier
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static List<SerializedObjectIdentifier> ReadSerializedObjectIdentifierList(byte[] content, ref int index, ObjectInfo objectInfo)
        {
            int count = BitConverter.ToInt32(content, index);
            index += 4;

            List<SerializedObjectIdentifier> ret = new List<SerializedObjectIdentifier>();
            for (int i = 0; i < count; i++)
            {
                ret.Add(ReadNextSerializedObjectIdentifier(content, ref index, objectInfo));
            }
            return ret;
        }

        public static List<SerializedObjectIdentifier> ReadSerializedObjectIdentifierList(byte[] content, ref int index, UnityObject unityObject)
        {
            return ReadSerializedObjectIdentifierList(content, ref index, unityObject.ObjectInfo);
        }

        public static SerializedObjectIdentifier ReadNextSerializedObjectIdentifier(byte[] content, ref int index,ObjectInfo objectInfo)
        {
            int fileid = BitConverter.ToInt32(content, index); index += 4;
            int idinfile = BitConverter.ToInt32(content, index); index += 4;
            index += Util.GetSerializedFileIndexIdRange(objectInfo);
            return new SerializedObjectIdentifier(fileid, idinfile);
        }

        public static SerializedObjectIdentifier ReadNextSerializedObjectIdentifier(byte[] content, ref int index, UnityObject unityObject)
        {
            return ReadNextSerializedObjectIdentifier(content, ref index, unityObject.ObjectInfo);
        }

        public static Vector3F ReadNextVector3f(byte[] content, ref int index)
        {
            Vector3F ret=new Vector3F();
            ret.X=BitConverter.ToSingle(content, index);
            index += 4;

            ret.Y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.Z = BitConverter.ToSingle(content, index);
            index += 4;

            return ret;
        }

        public static Vector2F ReadNextVector2f(byte[] content, ref int index)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            Vector2F ret = new Vector2F();
            ret.X = BitConverter.ToSingle(content, index);
            index += 4;

            ret.Y = BitConverter.ToSingle(content, index);
            index += 4;

            return ret;
        }

        public static Vector4F ReadNextVector4f(byte[] content, ref int index)
        {
            Vector4F ret = new Vector4F();
            ret.X = BitConverter.ToSingle(content, index);
            index += 4;

            ret.Y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.Z = BitConverter.ToSingle(content, index);
            index += 4;

            ret.W = BitConverter.ToSingle(content, index);
            index += 4;


            return ret;
        }

        public static bool isNowParsingResourcesFiles()
        {
            return AssetsFile.nowParsingFile.Equals("library/unity default resources") ||
                AssetsFile.nowParsingFile.Equals("resources/unity_builtin_extra");
        }

        /// <summary>
        /// //5.0以上的版本identifier是8位的，所以一个SerializedObjectIdentifier是12位，
        /// //此处需要多加上4位
        /// </summary>
        /// <returns></returns>
        public static int GetSerializedFileIndexIdRange(ObjectInfo objectInfo)
        {
            if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                return 4;
            }
            else if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
            {
                return 0;
            }
            return 0;
        }

        public static int GetSerializedFileIndexIdRange(UnityObject unityObject)
        {
            if (unityObject == null) return 0;
            return GetSerializedFileIndexIdRange(unityObject.ObjectInfo);
        }

        public static bool ReadFileContent(string filename,int offset,int size,byte[] content)
        {
            if(File.Exists(filename)==false)
            {
                return false;
            }
            FileStream file_stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            file_stream.Position = offset;
            file_stream.Read(content, 0, size);
            file_stream.Close();
            return true;
        }

        public static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.0000001) return true;
            return false;
        }
    }
}
