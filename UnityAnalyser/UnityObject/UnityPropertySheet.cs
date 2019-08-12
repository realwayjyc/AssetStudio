using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{

    public class UnityPropertySheet
    {
        #region inner class
        public class UnityTexEnv
        {
            private string poprertyName;
            public string PoprertyName
            {
                get { return poprertyName; }
                set { poprertyName = value; }
            }

            private UnityPropertySheet parent;
            public UnityPropertySheet Parent
            {
                get { return parent; }
            }


            private float scaleX;
            public float ScaleX
            {
                get { return scaleX; }
                set { scaleX = value; }
            }

            private float scaleY;
            public float ScaleY
            {
                get { return scaleY; }
                set { scaleY = value; }
            }

            private float offsetX;
            public float OffsetX
            {
                get { return offsetX; }
                set { offsetX = value; }
            }

            private float offsetY;
            public float OffsetY
            {
                get { return offsetY; }
                set { offsetY = value; }
            }

            private SerializedObjectIdentifier texture;
            public SerializedObjectIdentifier Texture
            {
                get { return texture; }
                set { texture = value; }
            }

            public string TextureName
            {
                get
                {
                    Texture2D texture2d = GetTexture2D();
                    if (texture2d != null)
                    {
                        return texture2d.TextureName;
                    }
                    return "";
                }
            }

            public UnityTexEnv(UnityPropertySheet parent)
            {
                this.parent = parent;
            }

            public Texture2D GetTexture2D()
            {
                try
                {
                    if (parent != null)
                    {
                        Texture2D texture2D = parent.parent.GetUnityObjectBySerializedObjectIdentifier(texture) as Texture2D;
                        return texture2D;
                    }
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        };

        public class UnityFloatProperty
        {
            private string poprertyName;
            public string PoprertyName
            {
                get { return poprertyName; }
                set { poprertyName = value; }
            }

            private float proertyValue;
            public float ProertyValue
            {
                get { return proertyValue; }
                set { proertyValue = value; }
            }
        }

        public class UnityColorProperty
        {
            private string poprertyName;
            public string PoprertyName
            {
                get { return poprertyName; }
                set { poprertyName = value; }
            }

            private float r;
            public float RValue
            {
                get { return r; }
                set { r = value; }
            }

            private float g;
            public float GValue
            {
                get { return g; }
                set { g = value; }
            }

            private float b;
            public float BValue
            {
                get { return b; }
                set { b = value; }
            }

            private float a;

            public float AValue
            {
                get { return a; }
                set { a = value; }
            }
        }


        #endregion

        private UnityObject parent;
        public UnityObject Parent
        {
            get { return parent; }
        }

        private Dictionary<string, UnityTexEnv> namedUnityTexEnvPropertyDict = new Dictionary<string, UnityTexEnv>();
        public Dictionary<string, UnityTexEnv> NamedUnityTexEnvPropertyDict
        {
            get { return namedUnityTexEnvPropertyDict; }
        }

        private Dictionary<string, UnityFloatProperty> namedFloatPropertyDict = new Dictionary<string, UnityFloatProperty>();
        public Dictionary<string, UnityFloatProperty> NamedFloatPropertyDict
        {
            get { return namedFloatPropertyDict; }
        }

        private Dictionary<string, UnityColorProperty> namedColorPropertyDict = new Dictionary<string, UnityColorProperty>();
        public Dictionary<string, UnityColorProperty> NamedColorPropertyDict
        {
            get { return namedColorPropertyDict; }
        }


        public UnityPropertySheet(UnityObject parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// 从content的index位置处读取信息，存储到this中
        /// </summary>
        /// <param name="content"></param>
        /// <param name="objectOffset"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int readFromContent(byte[] content, int objectOffset, int index,ObjectInfo objectInfo)
        {
            //先读取namedUnityTexEnvPropertyDict
            int count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                string name=Util.readStringAndAlign(content,objectOffset,ref index);
                UnityTexEnv unityTexEnv = new UnityTexEnv(this);
                unityTexEnv.PoprertyName = name;
                int serializedFileIndex=BitConverter.ToInt32(content,index);
                index+=4;
                int idinfile=BitConverter.ToInt32(content,index);
                index += 4 + Util.GetSerializedFileIndexIdRange(objectInfo);

                unityTexEnv.Texture = new SerializedObjectIdentifier(serializedFileIndex, idinfile);

                unityTexEnv.ScaleX=BitConverter.ToSingle(content,index);
                index+=4;

                unityTexEnv.ScaleY = BitConverter.ToSingle(content, index);
                index += 4;

                unityTexEnv.OffsetX = BitConverter.ToSingle(content, index);
                index += 4;

                unityTexEnv.OffsetY = BitConverter.ToSingle(content, index);
                index += 4;

                this.namedUnityTexEnvPropertyDict.Add(name, unityTexEnv);
            }

            //再读取namedFloatPropertyDict
            count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                string name = Util.readStringAndAlign(content, objectOffset, ref index);
                float value = BitConverter.ToSingle(content, index);
                index += 4;
                UnityFloatProperty unityFloatProperty = new UnityFloatProperty();
                unityFloatProperty.PoprertyName = name;
                unityFloatProperty.ProertyValue = value;
                this.namedFloatPropertyDict.Add(name, unityFloatProperty);
            }

            //最后读取namedColorPropertyDict
            count = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < count; i++)
            {
                string name = Util.readStringAndAlign(content, objectOffset, ref index);

                UnityColorProperty unityColorProperty = new UnityColorProperty();
                unityColorProperty.PoprertyName = name;

                unityColorProperty.RValue = BitConverter.ToSingle(content, index);
                index += 4;
                unityColorProperty.GValue = BitConverter.ToSingle(content, index);
                index += 4;
                unityColorProperty.BValue = BitConverter.ToSingle(content, index);
                index += 4;
                unityColorProperty.AValue = BitConverter.ToSingle(content, index);
                index += 4;

                unityColorProperty.RValue *= 255;
                unityColorProperty.GValue *= 255;
                unityColorProperty.BValue *= 255;
                unityColorProperty.AValue *= 255;

                this.namedColorPropertyDict.Add(name, unityColorProperty);
            }

            return index;
        }
    }
}
