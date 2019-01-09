using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    enum NAVIGATION_MODE
    {
        None=0,
        Horizontal,
        Vertical,
        Automatic,
        Explicit
    }

    enum TRANSITION_MODE
    {
        None=0,
        Color_Tint,
        Sprite_Swap,
        Animation
    }

    enum DIRECTION_MODE
    {
        Left_To_Right=0,
        Right_To_Left,
        Bottom_To_Top,
        Top_To_Bottom
    }

    enum VALUECHANGED_MODE
    {
        Off=0,
        Editor_And_Runtime,
        Runtime_Only
    }

    public class ValueChangeFun
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

        public ValueChangeFun(string n,string v)
        {
            name = n;
            value = v;
        }
    }

    public class SliderSFVS: ScriptFieldValueSet
    {
        public SliderSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            ///前半部分是相同的，所以用基类的处理函数
            GetControlInfo(scriptInfoContent, ref index);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fill Rect";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Handle Rect";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Direction";
            scriptFieldValue.FieldValue = ((DIRECTION_MODE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Min value";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Max value";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Whole Number";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Value";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            int valueChangeFunCount = ReadInt4(scriptInfoContent, ref index);
            for(int i=0;i<valueChangeFunCount;i++)
            {
                GetEventInfo("On Value Changed ", scriptInfoContent, ref index, i);
            }
        }
    }
}
