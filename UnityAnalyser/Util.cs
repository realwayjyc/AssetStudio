﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace UnityAnalyzer
{
    public class Util
    {
        public static int GetAlignCount(int index, int startOffset)
        {
            int diff = index - startOffset;
            return (int)(((diff + 3) & 0xFFFFFFFC) - diff);
        }

        public static void Quaternion2Euler(float x, float y, float z, float w,
            out float xAngle, out float yAngle, out float zAngle)
        {
 
            zAngle = (float)(180 * Math.Atan2(2 * (w * z + y * x), 1 - 2 * (x * x + z * z)) / Math.PI);
            xAngle = (float)(180 * Math.Asin(2 * (w * x - z * y)) / Math.PI);
            yAngle = (float)(180 * Math.Atan2(2 * (w * y + x * z), 1 - 2 * (x * x + y * y)) / Math.PI);

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

        public static Vector3f ReadNextVector3f(byte[] content, ref int index)
        {
            Vector3f ret=new Vector3f();
            ret.x=BitConverter.ToSingle(content, index);
            index += 4;

            ret.y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.z = BitConverter.ToSingle(content, index);
            index += 4;

            return ret;
        }

        public static Vector2f ReadNextVector2f(byte[] content, ref int index)
        {
            Vector2f ret = new Vector2f();
            ret.x = BitConverter.ToSingle(content, index);
            index += 4;

            ret.y = BitConverter.ToSingle(content, index);
            index += 4;

            return ret;
        }

        public static Vector4f ReadNextVector4f(byte[] content, ref int index)
        {
            Vector4f ret = new Vector4f();
            ret.x = BitConverter.ToSingle(content, index);
            index += 4;

            ret.y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.z = BitConverter.ToSingle(content, index);
            index += 4;

            ret.w = BitConverter.ToSingle(content, index);
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
