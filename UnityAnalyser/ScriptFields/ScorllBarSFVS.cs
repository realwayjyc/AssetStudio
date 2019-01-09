using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{

    public enum Direction
    {
        Left_to_right,
        Right_to_left,
        Bottom_to_top,
        Top_to_bottom
    }
    public class ScorllBarSFVS: ScriptFieldValueSet
    {
        public ScorllBarSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            ///前半部分是相同的，所以用基类的处理函数
            GetControlInfo(scriptInfoContent, ref index);

            SerializedObjectIdentifier soi;
            UnityObject unityObject;

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
            scriptFieldValue.FieldValue = ((Direction)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Value";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Size";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Number Of Steps";
            scriptFieldValue.FieldValue = (ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            int onClickFunCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < onClickFunCount; i++)
            {
                GetEventInfo("On Value Changed ", scriptInfoContent, ref index, i);
            }
        }
    }
}
