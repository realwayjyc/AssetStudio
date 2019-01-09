using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class EventSystemSFVS : ScriptFieldValueSet
    {
        public EventSystemSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "First Selected";
            SerializedObjectIdentifier soi = Util.ReadNextSerializedObjectIdentifier(
                                scriptInfoContent, ref index, scriptRef);
            UnityObject unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Send Navigation Events";
            scriptFieldValue.FieldValue = ReadBoolean4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Drag Threshold";
            scriptFieldValue.FieldValue = ReadInt4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
