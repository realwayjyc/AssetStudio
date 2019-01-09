using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public enum FONT_STYLE
    {
        Normal=0,
        Bold,
        Italic,
        Bold_Italic
    }

    public enum ALIGNMENT
    {
        LEFT_TOP=0,
        LEFT_CENTER=3,
        LEFT_BOTTOM=6,
        CENTER_TOP=1,
        CENTER_CENTER=4,
        CENTER_BOTTOM=7,
        RIGHT_TOP = 2,
        RIGHT_CENTER = 5,
        RIGHT_BOTTOM = 8
    }

    public enum HORIZONTAL_OVERFLOW
    {
        Wrap=0,
        Overflow
    }

    public enum VERTICAL_OVERFLOW
    {
        Truncate = 0,
        Overflow
    }

    public class TextSVFS : ScriptFieldValueSet
    {
        public TextSVFS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Material";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index));
            scriptFieldValueList.Add(scriptFieldValue);

            if (scriptRef.UnityFileVersion[0] == 5 &&
                scriptRef.UnityFileVersion[1] == 3)
            {
                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Raycast Target";
                scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);

                int eventCount = ReadInt4(scriptInfoContent, ref index);
                for (int i = 0; i < eventCount; i++)
                {
                    GetEventInfo("Cull State Change ", scriptInfoContent, ref index, i);
                }

                ReadString(scriptInfoContent, ref index);
            }

            




            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Font";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Font Size";
            scriptFieldValue.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

             scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Font Style";
            scriptFieldValue.FieldValue = ((FONT_STYLE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Best Fit";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Min Size";
            scriptFieldValue.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Max Size";
            scriptFieldValue.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Alignment";
            scriptFieldValue.FieldValue = ((ALIGNMENT)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
            {
                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Align By Geoemetry";
                scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);
            }



            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Rich Text";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Horizontal Overflow";
            scriptFieldValue.FieldValue = ((HORIZONTAL_OVERFLOW)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Vertical Overflow";
            scriptFieldValue.FieldValue = ((VERTICAL_OVERFLOW)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Line Spacing";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Text";
            scriptFieldValue.FieldValue = (ReadString(scriptInfoContent, ref index)).ToString();
            scriptFieldValue.OtherInfo = scriptFieldValue.FieldValue;
            scriptFieldValueList.Add(scriptFieldValue);
            
        }
    }
}
