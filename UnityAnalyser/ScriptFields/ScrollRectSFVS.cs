using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{

    public enum MovementType
    {
        Unrestricted,
        Elastic,
        Clamped
    }

    public enum VISIBILITY
    {
        Permant,
        AutoHide,
        AutoHideAndExpandViewport
    }

    public class ScrollRectSFVS : ScriptFieldValueSet
    {
        public ScrollRectSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Content";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Horizontal";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Vertical";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Movement Type";
            scriptFieldValue.FieldValue = ((MovementType)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Elasticity";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Inertia";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Deceleration Rate";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Scroll Sensitivity";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
            {
                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Viewport";
                soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
                unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
                if (unityObject != null)
                {
                    scriptFieldValue.FieldValue = unityObject.Name;
                }
                scriptFieldValue.OtherInfo = unityObject;
                scriptFieldValueList.Add(scriptFieldValue);
            }


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Horizontal Scroll Bar";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index,scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Vertical Scroll Bar";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index,scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
            {
                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "HScrollBar Visibility";
                scriptFieldValue.FieldValue = ((VISIBILITY)ReadInt4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);

                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "VScrollBar Visibility";
                scriptFieldValue.FieldValue = ((VISIBILITY)ReadInt4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);

                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "HScrollBar Spacing";
                scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);

                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "VScrollBar Spacing";
                scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
                scriptFieldValueList.Add(scriptFieldValue);
            }




            int onClickFunCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < onClickFunCount; i++)
            {
                GetEventInfo("On Value Changed ", scriptInfoContent, ref index, i);
            }
        }
    }
}
