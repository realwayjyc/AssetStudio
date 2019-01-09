using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    enum CONTENT_TYPE
    {
        Standard,
        AutoCorrected,
        Integer_Number,
        Decimal_Number,
        Alphanumeric,
        Name,
        Email_Address,
        Password,
        Pin,
        Custom
    }

    enum Input_Type
    {
        Standard,
        Auto_Correct,
        Password
    }

    enum Keyboard_Type
    {
        Default,
        AsciiCapable,
        NumbersAndPunctuation,
        URL,
        Number_Pad,
        Phone_Pad,
        Number_Phone_pad,
        Email_Address
    }

    enum Line_Type
    {
        Single_Line,
        Multi_Line_Submit,
        Multi_Line_NewLine
    }

    enum Character_Validation
    {
        None,
        Integer,
        Decimal,
        Alphanumeric,
        Name,
        Email_Address
    }

    public class InputFieldSFVS: ScriptFieldValueSet
    {
        public InputFieldSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            ///前半部分是相同的，所以用基类的处理函数
            GetControlInfo(scriptInfoContent, ref index);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Text Component";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Place Holder";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Content Type";
            scriptFieldValue.FieldValue = ((CONTENT_TYPE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Input Type";
            scriptFieldValue.FieldValue = ((Input_Type)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
            {
                index += 4;
            }


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "keyboard Type";
            scriptFieldValue.FieldValue = ((Keyboard_Type)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Line Type";
            scriptFieldValue.FieldValue = ((Line_Type)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Hide Mobile Input";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Character Validation";
            scriptFieldValue.FieldValue = ((Character_Validation)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Character Limit";
            scriptFieldValue.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            int endEditFunCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < endEditFunCount; i++)
            {
                GetEventInfo("End Edit ",scriptInfoContent, ref index, i);
            }

            ReadString(scriptInfoContent, ref index);

            int onValueChangeFunCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < onValueChangeFunCount; i++)
            {
                GetEventInfo("On Value Change ", scriptInfoContent, ref index, i);
            }
            ReadString(scriptInfoContent, ref index);


            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
            {

                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Caret Color";
                scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index));
                scriptFieldValueList.Add(scriptFieldValue);


                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Custom Caret Color";
                scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);
            }


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Selection Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index));
            scriptFieldValueList.Add(scriptFieldValue);

           


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Text";
            scriptFieldValue.FieldValue = (ReadString(scriptInfoContent, ref index));
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Caret Blink Rate";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index).ToString());
            scriptFieldValueList.Add(scriptFieldValue);


            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
            {

                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Caret Width";
                scriptFieldValue.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);

                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Read Only";
                scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);
            }
        }
    }
}
