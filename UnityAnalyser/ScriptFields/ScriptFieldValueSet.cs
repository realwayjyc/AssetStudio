using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace UnityAnalyzer
{
    public enum AnimationCurveMode
    {
        PingPong=0,
        Loop=1,
        Clamp=2
    }

    public class NameValuePair
    {
         private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private object otherInfo;
        public object OtherInfo
        {
            get { return otherInfo; }
            set { otherInfo = value; }
        }


        public NameValuePair(string n, string v)
        {
            name = n;
            value = v;
        }
    }
    /// <summary>
    /// 表示一个脚本文件的类中一个变量及其值，例如float scaleFactor=1.0f
    /// </summary>
    public class ScriptFieldValue
    {
        /// <summary>
        /// 变量名称:比如scaleFactor
        /// </summary>
        private string fieldName;
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 变量的值，比如1.0f
        /// </summary>
        private string fieldValue;
        public string FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        /// <summary>
        /// 变量的值的其他信息，待定
        /// </summary>
        private object otherInfo;
        public object OtherInfo
        {
            get { return otherInfo; }
            set { otherInfo = value; }
        }
    }


    public class ScriptFieldValueSet
    {
        protected List<ScriptFieldValue> scriptFieldValueList;
        public List<ScriptFieldValue> ScriptFieldValueList
        {
            get { return scriptFieldValueList; }
            set { scriptFieldValueList = value; }
        }

        protected MonoScript monoScript;
        public MonoScript MonoScript
        {
            get { return monoScript; }
            set { monoScript = value; }
        }

        protected ScriptRef scriptRef;
        public ScriptRef ScriptRef
        {
            get { return scriptRef; }
            set { scriptRef = value; }
        }


        protected ScriptFieldValueSet(ScriptRef scriptRef)
        {
            this.scriptRef = scriptRef;
            scriptFieldValueList = new List<ScriptFieldValue>();
        }

        private static int SortTestObj2Compare(ScriptFieldValue obj1, ScriptFieldValue obj2)
        {
            if ((obj1 == null) && (obj2 == null))
            {
                return 0;
            }
            else if ((obj1 != null) && (obj2 == null))
            {
                return 1;
            }
            else if ((obj1 == null) && (obj2 != null))
            {
                return -1;
            }
            return obj1.FieldName.CompareTo(obj2.FieldName);
        }

        public void Sort()
        {
            //将 scriptFieldValueList中的字段排序
            scriptFieldValueList.Sort(SortTestObj2Compare);

        }

        public static ScriptFieldValueSet Parse(MonoScript monoScript,byte[] scriptInfoContent,ScriptRef scriptRef)
        {
            ScriptFieldValueSet scriptFieldValueSet = null;
            

            if (monoScript.ScriptAssemblyName == "UnityEngine.UI.dll")
            {
                switch (monoScript.Name)
                {
                    case "CanvasScaler":
                        scriptFieldValueSet = new CanvasScalerSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "GraphicRaycaster":
                        scriptFieldValueSet = new GraphicRayCasterSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Image":
                        scriptFieldValueSet = new ImageSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "EventSystem":
                        scriptFieldValueSet = new EventSystemSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "StandaloneInputModule":
                        scriptFieldValueSet = new StandaloneInputModuleSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "TouchInputModule":
                        scriptFieldValueSet = new TouchInputModuleSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Slider":
                        scriptFieldValueSet = new SliderSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Button":
                        scriptFieldValueSet = new ButtonSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "VerticalLayoutGroup":
                        scriptFieldValueSet = new VerticalLayoutGroupSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "HorizontalLayoutGroup":
                        scriptFieldValueSet = new HorizontalLayoutGroupSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "ContentSizeFitter":
                        scriptFieldValueSet = new ContentSizeFitterSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "LayoutElement":
                        scriptFieldValueSet = new LayoutElementSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "InputField":
                        scriptFieldValueSet = new InputFieldSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "ScrollRect":
                        scriptFieldValueSet = new ScrollRectSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Scrollbar":
                        scriptFieldValueSet = new ScorllBarSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Mask":
                        scriptFieldValueSet = new MaskSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "EventTrigger":
                        scriptFieldValueSet = new EventTriggerSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Toggle":
                        scriptFieldValueSet = new ToggleSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "ToggleGroup":
                        scriptFieldValueSet = new ToggleGroupSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Text":
                        scriptFieldValueSet = new TextSVFS(scriptInfoContent, scriptRef);
                        break;
                    case "Shadow":
                        scriptFieldValueSet = new ShadowSFVS(scriptInfoContent, scriptRef);
                        break;
                    case "Outline":
                        scriptFieldValueSet = new OutlineSFVS(scriptInfoContent, scriptRef);
                        break;
                }
            }
            else
            {
                string folderName = MainWindow.instance.CurrentAnalyzer.Current_folder;
                string sourceDll = folderName + "Managed/" + monoScript.ScriptAssemblyName;
                scriptFieldValueSet = GetScriptFieldValueSet(scriptInfoContent, sourceDll, monoScript.ScriptClassName, monoScript.ScriptNamespace, scriptRef);
            }
            if (scriptFieldValueSet!=null)
            {
                scriptFieldValueSet.monoScript = monoScript;
            }
            return scriptFieldValueSet;
        }

        public static ScriptFieldValueSet GetScriptFieldValueSet(byte[] scriptInfoContent,string assemblyName, string scriptClassName, string scriptNamespace,ScriptRef scriptRef)
        {
            ScriptFieldValueSet scriptFieldValueSet = new ScriptFieldValueSet(scriptRef);
            if (File.Exists(assemblyName) == false) return scriptFieldValueSet;
            try
            {
                Assembly ass = Assembly.LoadFrom(assemblyName);
                string classFullName = scriptClassName;
                if (scriptNamespace != "")
                {
                    classFullName = scriptNamespace + "." + scriptClassName;
                }

                Type myType = ass.GetType(classFullName);   //参数必须是类的全名
                List<FieldInfo> fieldInfoList = new List<FieldInfo>();


                List<Type> typeList = new List<Type>();
                while(myType!=null)
                {
                    typeList.Insert(0,myType);
                    myType = myType.BaseType;
                }

                foreach(Type t in typeList)
                {
                    if(t.FullName=="UnityEngine.Object")
                    {
                        continue;
                    }

                    FieldInfo[] myFields2 = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly );

                    foreach(FieldInfo fieldInfo in myFields2)
                    {
                        if (fieldInfo.IsInitOnly) continue;
                        if (fieldInfo.IsPublic==false)
                        {
                            var attribs=fieldInfo.GetCustomAttributes(typeof(UnityEngine.SerializeField));
                            if (attribs.ToArray<object>().Length != 0)
                            {
                                fieldInfoList.Add(fieldInfo);
                            }
                        }
                        else
                        {
                            var attribs = fieldInfo.GetCustomAttributes(typeof(System.NonSerializedAttribute));
                            if (attribs.ToArray<object>().Length == 0)
                            {
                                fieldInfoList.Add(fieldInfo);
                            }
                        }
                    }

                    //FieldInfo[] myFields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                    //FieldInfo[] fis = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                    //List<FieldInfo> privateFields=new List<FieldInfo>();
                    //foreach(FieldInfo fff in fis)
                    //{
                    //    if (fff != null && fff.GetCustomAttributes(typeof(UnityEngine.SerializeField)) != null)
                    //    {
                    //        privateFields.Add(fff);
                    //    }
                    //}
                    

                    //fieldInfoList.AddRange(privateFields);
                }

                //解析该类中的各个public 变量及其类型
                scriptFieldValueSet.ParseFieldInfos(fieldInfoList.ToArray(), scriptInfoContent);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return scriptFieldValueSet;
        }


        public void ParseFieldInfos(FieldInfo[] myFields, byte[] scriptInfoContent)
        {
            int index = 0;
            foreach (FieldInfo fieldInfo in myFields)
            {
                List<ScriptFieldValue> singleFieldList = ParseTypeAndName(fieldInfo.FieldType,fieldInfo.Name, scriptInfoContent, ref index);
                foreach (ScriptFieldValue s in singleFieldList)
                {
                    scriptFieldValueList.Add(s);
                }
            }
        }

        private Boolean IsSerializable_My(Type t)
        {
            if(t.FullName=="UnityEngine.Vector4")
            {
                return true;
            }
            if (t.FullName == "UnityEngine.Vector3")
            {
                return true;
            }
            if (t.FullName == "UnityEngine.Vector2")
            {
                return true;
            }
            if (t.FullName == "UnityEngine.Quaternion")
            {
                return true;
            }
            return false;
        }

        public List<ScriptFieldValue> ParseTypeAndName(Type fieldType, string fieldVarName, byte[] scriptInfoContent, ref int index)
        {
            List<ScriptFieldValue> ret = new List<ScriptFieldValue>();
            string fieldTypeFullName = fieldType.FullName;
            if (fieldTypeFullName.EndsWith("[]") || fieldTypeFullName.StartsWith("System.Collections.Generic.List"))
            {
                //处理数组的情况
                string typeToRead="";
                Type arrayInnerType=null;
                if(fieldTypeFullName.EndsWith("[]"))
                {
                    typeToRead = fieldTypeFullName.Replace("[]", "");
                    arrayInnerType = fieldType.Assembly.GetType(typeToRead);
                }
                else
                {
                    typeToRead = fieldTypeFullName.Substring(fieldTypeFullName.IndexOf("[[") + 2);
                    string assemblyName = typeToRead.Split(',')[1].Trim()+".dll";
                    typeToRead = typeToRead.Substring(0, typeToRead.IndexOf(","));
                    if(assemblyName=="mscorlib.dll")
                    {
                        arrayInnerType = Type.GetType(typeToRead);
                    }
                    else
                    {
                        string folderName = MainWindow.instance.CurrentAnalyzer.Current_folder;
                        string sourceDll = folderName + "Managed/" + assemblyName;
                        Assembly ass = Assembly.LoadFrom(sourceDll);
                        arrayInnerType = ass.GetType(typeToRead);
                    }

                }
                
                if (arrayInnerType!=null && IsSerializable_My(arrayInnerType)==false 
                    &&  IsSerializable(arrayInnerType) == false)
                {
                    ScriptFieldValue ss = new ScriptFieldValue();
                    ss.FieldName = fieldVarName;
                    ss.FieldValue = "不可序列化:" + arrayInnerType.FullName;
                    ss.OtherInfo = 9999;
                    ret.Add(ss);
                    return ret;
                }

                int arrayCount = ReadInt4(scriptInfoContent, ref index);
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = "Count=" + arrayCount.ToString();
                ret.Add(s);
                for(int i=0;i<arrayCount;i++)
                {
                    List<ScriptFieldValue> temp = ParseTypeAndName(arrayInnerType, fieldVarName + "[" + i.ToString() + "]", scriptInfoContent, ref index);
                    foreach(ScriptFieldValue s2 in temp)
                    {
                        ret.Add(s2);
                    }
                }
                return ret;
            }
            #region "非数组"
            //处理非数组的情况
            if (fieldType.BaseType.FullName == "System.Enum")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = Enum.GetName(fieldType, ReadInt4(scriptInfoContent, ref index));
                ret.Add(s);
            }
            else if (fieldType.FullName == "System.String")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadString(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "System.Single")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadFloat4(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "System.Int32")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "System.Boolean")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "System.Byte")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadByte4(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "System.Double")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadDouble8(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.Color")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadColor(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (IsUnityObject(fieldType))
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                SerializedObjectIdentifier soi = Util.ReadNextSerializedObjectIdentifier(
                    scriptInfoContent, ref index, ScriptRef);
                UnityObject unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
                if (unityObject != null)
                {
                    s.FieldValue = unityObject.Name;
                }
                s.OtherInfo = unityObject;
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.AnimationCurve")
            {
                ScriptFieldValue scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = fieldVarName;

                ScriptFieldMultiValue multiValue = new ScriptFieldMultiValue();
                multiValue.AddColumns(new string[] { "Name", "Value" });


                int arrayCount = ReadInt4(scriptInfoContent, ref index);
                ValueChangeFun v;
                for (int i = 0; i < arrayCount; i++)
                {
                    float x = ReadSingle4(scriptInfoContent, ref index);
                    float y = ReadSingle4(scriptInfoContent, ref index);
                    float unknown1 = ReadSingle4(scriptInfoContent, ref index);
                    float unknown2 = ReadSingle4(scriptInfoContent, ref index);

                    v = new ValueChangeFun("Point[" + (i).ToString() + "].x", x.ToString());
                    multiValue.AddValue(v);

                    v = new ValueChangeFun("Point[" + (i).ToString() + "].y", y.ToString());
                    multiValue.AddValue(v);
                }

                AnimationCurveMode startMode = (AnimationCurveMode)ReadInt4(scriptInfoContent, ref index);
                AnimationCurveMode endMode = (AnimationCurveMode)ReadInt4(scriptInfoContent, ref index);

                v = new ValueChangeFun("Start Point Mode: ", startMode.ToString());
                multiValue.AddValue(v);

                v = new ValueChangeFun("End Point Mode: ", endMode.ToString());
                multiValue.AddValue(v);

                scriptFieldValue.FieldValue = "Point Count=" + arrayCount.ToString();
                scriptFieldValue.OtherInfo = multiValue;

                ret.Add(scriptFieldValue);
            }
            else if (fieldType.FullName == "UnityEngine.Vector3")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadVector3(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.Vector2")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadVector2(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.Vector4")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadVector4(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.Quaternion")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName;
                s.FieldValue = (ReadVector4(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.Bounds")
            {
                ScriptFieldValue s = new ScriptFieldValue();
                s.FieldName = fieldVarName + ".Center";
                s.FieldValue = (ReadVector3(scriptInfoContent, ref index)).ToString();
                ret.Add(s);

                s = new ScriptFieldValue();
                s.FieldName = fieldVarName + ".Extents";
                s.FieldValue = (ReadVector3(scriptInfoContent, ref index)).ToString();
                ret.Add(s);
            }
            else if (fieldType.FullName == "UnityEngine.LayerMask")
            {
                MaskedLayers maskedLayers = ReadMaskedLayers4(scriptInfoContent, ref index);
                ScriptFieldValue scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = fieldVarName;
                if (maskedLayers.IsNone)
                {
                    scriptFieldValue.FieldValue = "NONE (0)";
                }
                else if (maskedLayers.IsEverything)
                {
                    scriptFieldValue.FieldValue = "Everything (" + maskedLayers.Layer_list.Count + ")";
                }
                else
                {
                    scriptFieldValue.FieldValue = "Count=" + maskedLayers.Layer_list.Count;
                }

                ScriptFieldMultiValue multiValue = new ScriptFieldMultiValue();
                multiValue.AddColumns(new string[] { "Name", "Id" });
                List<Layer> layersList = maskedLayers.Layer_list;
                foreach (Layer layer in layersList)
                {
                    multiValue.AddValue(layer);
                }
                scriptFieldValue.OtherInfo = multiValue;

                scriptFieldValueList.Add(scriptFieldValue);
            }
            else if (IsUnityEvent(fieldType))
            {
                //处理从UnityEvent<T>派生出来的类型
                int eventCount = ReadInt4(scriptInfoContent, ref index);
                for(int i=0;i< eventCount;i++)
                {
                    this.GetEventInfo(fieldType.Name, scriptInfoContent, ref index, i);
                }
            }
            else if (fieldType.FullName.StartsWith("System.")
                || fieldType.FullName.StartsWith("Unity.")
                || fieldType.FullName.StartsWith("UnityEngine."))
            {
                //System.Windows.Forms.MessageBox.Show("未知类型:" + fieldType.FullName);
            }
            else
            #endregion
            {
                //开始判断自定义类的类型
                //先判断是否是Serializable
                if (IsSerializable(fieldType) == false)
                {
                    //不可序列化的类型
                    ScriptFieldValue ss = new ScriptFieldValue();
                    ss.FieldName = fieldVarName;
                    ss.FieldValue = "不可序列化:" + fieldType.FullName;
                    ss.OtherInfo = 9999;
                    ret.Add(ss);
                }
                else
                {
                    //可以序列化
                    FieldInfo[] myFields2 = fieldType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    List<FieldInfo> fieldInfoList = new List<FieldInfo>();
                    foreach (FieldInfo fieldInfo in myFields2)
                    {
                        if (fieldInfo.IsInitOnly) continue;     //排除Readonly
                        if (fieldInfo.Attributes.ToString().Contains("Private"))
                        {
                            var attribs = fieldInfo.GetCustomAttributes(typeof(UnityEngine.SerializeField));
                            if (attribs.ToArray<object>().Length != 0)
                            {
                                fieldInfoList.Add(fieldInfo);
                            }
                        }
                        else
                        {
                            var attribs = fieldInfo.GetCustomAttributes(typeof(System.NonSerializedAttribute));
                            if (attribs.ToArray<object>().Length == 0)
                            {
                                fieldInfoList.Add(fieldInfo);
                            }
                        }
                    }

                    FieldInfo[] myFields = fieldInfoList.ToArray();
                    foreach (FieldInfo f in myFields)
                    {
                        List<ScriptFieldValue> sss = ParseTypeAndName(f.FieldType, fieldVarName + "." + f.Name, scriptInfoContent, ref index);
                        ret.AddRange(sss);
                    }
                }
            }
            return ret;
        }

        public bool IsUnityObject(Type t)
        {
            if(t==null) return false;
            while(t.BaseType!=null)
            {
                if(t.BaseType.FullName=="UnityEngine.Object")
                {
                    return true;
                }
                t = t.BaseType;
            }
            return false;
        }

        public bool IsSerializable(Type t)
        {
            if (t == null) return false;
            if (IsUnityObject(t)) return true;
            if (t.BaseType.FullName == "System.Enum") return true;
            if (t.BaseType.FullName == "System.String" || t.BaseType.FullName == "System.Single" || t.BaseType.FullName == "System.Int32"
                || t.BaseType.FullName == "System.Boolean" || t.BaseType.FullName == "System.Byte" ||
                t.BaseType.FullName == "System.Double" || t.BaseType.FullName == "UnityEngine.Color")
            {
                return true;
            }
            return t.Attributes.ToString().Contains("Serializable");
        }

        protected bool IsUnityEvent(Type t)
        {
            while (t != null)
            {
                if (t.Name.Contains("UnityEvent"))
                {
                    return true;
                }
                t = t.BaseType;
            }
            return false;
        }

        /// <summary>
        /// 对于Button,Slider等控件的处理,有一部分内容是相同
        /// </summary>
        /// <param name="scriptInfoContent"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        protected void GetControlInfo(byte[] scriptInfoContent, ref int index)
        {
            ScriptFieldValue scriptFieldValue = null;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Navigation";
            scriptFieldValue.FieldValue = ((NAVIGATION_MODE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Select On Top";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Select On Down";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Select On Left";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Select On Right";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Transition";
            scriptFieldValue.FieldValue = ((TRANSITION_MODE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Normal Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Highlighted Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Pressed Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Disabled Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Color Multiplier";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fade Duration";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Highlighted Sprite";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Pressed Sprite";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Disabled Sprite";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Normal Trigger";
            scriptFieldValue.FieldValue = (ReadString(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Highlighted Trigger";
            scriptFieldValue.FieldValue = (ReadString(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Pressed Trigger";
            scriptFieldValue.FieldValue = (ReadString(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Disabled Trigger";
            scriptFieldValue.FieldValue = (ReadString(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Interactable";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Target Graphic";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);
        }

        /// <summary>
        /// 对于Button,Slider等控件，事件处理函数的读取都步骤是相同的
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="scriptInfoContent"></param>
        /// <param name="index"></param>
        /// <param name="id"></param>
        protected void GetEventInfo(string eventName,
            byte[] scriptInfoContent, ref int index, int id)
        {
            ScriptFieldValue scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = eventName + (id + 1).ToString();

            ScriptFieldMultiValue multiValue = new ScriptFieldMultiValue();
            multiValue.AddColumns(new string[] { "Name", "Value" });

            SerializedObjectIdentifier soi;
            UnityObject unityObject;
            SerializedObjectIdentifierWithFile soif=null;
            string className = "";

            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);

            if (unityObject!=null)
            {
                soif = new SerializedObjectIdentifierWithFile();
                soif.soi = soi;
                soif.fullFileName = unityObject.ObjectInfo.UnityFile.FullFileName;
            }

            GameObject gameObject = null;
            if (unityObject as ScriptRef != null)
            {
                ScriptRef scriptRefTransfer = unityObject as ScriptRef;
                MonoScript monoScriptThis = scriptRefTransfer.GetMonoScriptRef();
                className = monoScriptThis.ScriptClassName;
                gameObject = Analyzer.GetGameObjectByComponent(soif);
            }
            else if (unityObject as GameObject != null)
            {
                gameObject = unityObject as GameObject;
                className = unityObject.ClassIDTypeString;
            }
            else if(soif!=null)
            {
                gameObject = Analyzer.GetGameObjectByComponent(soif);
                className = unityObject.ClassIDTypeString;
            }

            ValueChangeFun v = null;
            if(gameObject!=null)
            {
                v = new ValueChangeFun("GameObject", gameObject.Name);
                multiValue.AddValue(v);

                v = new ValueChangeFun("GameObject_Id", "0x" + gameObject.Id.ToString("x"));
                multiValue.AddValue(v);

                v = new ValueChangeFun("GameObject_File", gameObject.ObjectInfo.UnityFile.AliasFileName);
                multiValue.AddValue(v);
            }

            string functioNname = ReadString(scriptInfoContent, ref index);

             int dynamic = ReadInt4(scriptInfoContent, ref index);
             v = new ValueChangeFun("Is Dynamic", (dynamic == 0).ToString() + (dynamic == 0 ? "-------" : ""));
             multiValue.AddValue(v);





            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);

            string assemblyName = ReadString(scriptInfoContent, ref index);
            string functionNameChanged = className + "." + functioNname;
            v = new ValueChangeFun("Function", functionNameChanged);
            multiValue.AddValue(v);

            int intV = ReadInt4(scriptInfoContent, ref index);
            float flt = ReadSingle4(scriptInfoContent, ref index);
            string str = ReadString(scriptInfoContent, ref index);
            Boolean b = ReadBoolean4(scriptInfoContent, ref index);

            string m = ((VALUECHANGED_MODE)ReadInt4(scriptInfoContent, ref index)).ToString();

            v = new ValueChangeFun("Runtime Combobox", m);
            multiValue.AddValue(v);



            //对于On event的函数中，有数值存在的情况，需要将数值都显示出来，然后根据函数的参数或者返回值的类型来挑选数值
            string value_unity_object = "";
            string value_unity_object_id = "";
            string value_unity_object_in_file = "";
            if (unityObject != null)
            {
                value_unity_object = unityObject.Name;
                value_unity_object_id = "0x" + unityObject.Id.ToString("x");
                value_unity_object_in_file = unityObject.ObjectInfo.UnityFile.AliasFileName;
            }

            v = new ValueChangeFun("", "");
            multiValue.AddValue(v);


            v = new ValueChangeFun("Value_UnityObject", value_unity_object);
            multiValue.AddValue(v);

            v = new ValueChangeFun("Value_UnityObject_Id", value_unity_object_id);
            multiValue.AddValue(v);

            v = new ValueChangeFun("Value_UnityObject_InFile", value_unity_object_in_file);
            multiValue.AddValue(v);


            ///////////////////
            v = new ValueChangeFun("Value_Int", intV.ToString());
            multiValue.AddValue(v);

            v = new ValueChangeFun("Value_Float", flt.ToString());
            multiValue.AddValue(v);

            v = new ValueChangeFun("Value_String", str);
            multiValue.AddValue(v);

            v = new ValueChangeFun("Value_Boolean", b.ToString());
            multiValue.AddValue(v);


            scriptFieldValue.FieldValue = functionNameChanged;
            scriptFieldValue.OtherInfo = multiValue;
            scriptFieldValueList.Add(scriptFieldValue);
        }


#region Read

        public static float ReadFloat4(byte[] content, ref int index)
        {
            float value = BitConverter.ToSingle(content, index);
            index += 4;
            return value;
        }

        public static byte ReadByte4(byte[] content, ref int index)
        {
            byte value = content[index];
            index += 4;
            return value;
        }
        public static int ReadInt4(byte[] content,ref int index)
        {
            int value=BitConverter.ToInt32(content, index);
            index += 4;
            return value;
        }

        public static float ReadSingle4(byte[] content, ref int index)
        {
            float value = BitConverter.ToSingle(content, index);
            index += 4;
            return value;
        }

        public static Boolean ReadBoolean4(byte[] content, ref int index)
        {
            int value = BitConverter.ToInt32(content, index);
            index += 4;
            return !(value==0);
        }

        public static String ReadString(byte[] content, ref int index)
        {
            return Util.readStringAndAlign(content, 0, ref index);
        }

        public static MaskedLayers ReadMaskedLayers4(byte[] content, ref int index)
        {
            UInt32 value = BitConverter.ToUInt32(content, index);
            index += 4;
            return MaskedLayers.Parse(value);
        }

        public static double ReadDouble8(byte[] content, ref int index)
        {
            double value = BitConverter.ToDouble(content, index);
            index += 8;
            return value;
        }

        public static string ReadVector3(byte[] content, ref int index)
        {
            float x = ReadSingle4(content, ref index);
            float y = ReadSingle4(content, ref index);
            float z = ReadSingle4(content, ref index);
            return "XYZ: " + x.ToString() + "   " + y.ToString() + "   " + z.ToString();
        }

        public static string ReadVector2(byte[] content, ref int index)
        {
            float x = ReadSingle4(content, ref index);
            float y = ReadSingle4(content, ref index);
            return "XY: " + x.ToString() + "   " + y.ToString();
        }

        public static string ReadVector4(byte[] content, ref int index)
        {
            float x = ReadSingle4(content, ref index);
            float y = ReadSingle4(content, ref index);
            float z = ReadSingle4(content, ref index);
            float w = ReadSingle4(content, ref index);
            return "XYZW: " + x.ToString() + "   " + y.ToString() + "   " + z.ToString() + "   " + w.ToString();
        }

        public static String ReadColor(byte[] content, ref int index)
        {
            float r = ReadSingle4(content, ref index);
            float g = ReadSingle4(content, ref index);
            float b = ReadSingle4(content, ref index);
            float a = ReadSingle4(content, ref index);
            return "RGBA: "+(r * 255).ToString() + "  " + (g * 255).ToString() + "  " + (b * 255).ToString() + "  " + (a * 255).ToString();
        }
#endregion
    }
}
