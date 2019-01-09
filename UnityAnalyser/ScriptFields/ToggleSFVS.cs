using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    enum TOGGLE_TRANSITION
    {
        None=0,
        Fade
    }
    public class ToggleSFVS : ScriptFieldValueSet
    {
        public ToggleSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            ///前半部分是相同的，所以用基类的处理函数
            GetControlInfo(scriptInfoContent, ref index);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Toggle Transition";
            scriptFieldValue.FieldValue = ((TOGGLE_TRANSITION)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Graphics";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Toggle Group";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            int onClickFunCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < onClickFunCount; i++)
            {
                GetEventInfo("On Value Changed ",scriptInfoContent, ref index, i);
            }

            ReadString(scriptInfoContent, ref index);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Is On";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
