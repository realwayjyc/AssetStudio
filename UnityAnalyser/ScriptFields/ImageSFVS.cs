using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    enum IMAGE_TYPE
    {
        Simple=0,
        Sliced,
        Tiled,
        Filled
    }

    enum FILL_METHOD
    {
        Horizontal = 0,
        Vertical,
        Radial_90,
        Radial_180,
        Radial_360,
    }

    enum FILL_ORIGIN_HORIZONTAL
    {
        Left=0,
        Right
    }

    enum FILL_ORIGIN_VERTICAL
    {
        Bottom = 0,
        Top
    }

    enum FILL_ORIGIN_RADIAL_90
    {
        Bottom_Left = 0,
        Top_Left,
        Top_Right,
        Bottom_Right
    }

    enum FILL_ORIGIN_RADIAL_180
    {
        Bottom = 0,
        Left,
        Top,
        Right
    }

    enum FILL_ORIGIN_RADIAL_360
    {
        Bottom = 0,
        Right,
        Top,
        Left
    }



    public class ImageSFVS : ScriptFieldValueSet
    {
        public ImageSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;
            SerializedObjectIdentifier soi;
            UnityObject unityObject;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Material";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index, scriptRef);
            unityObject=ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject!=null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index));
            scriptFieldValueList.Add(scriptFieldValue);

            if (scriptRef.UnityFileVersion[0] == 5 && scriptRef.UnityFileVersion[1] == 3)
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
            scriptFieldValue.FieldName = "Source Image";
            soi = Util.ReadNextSerializedObjectIdentifier(scriptInfoContent, ref index,scriptRef);
            unityObject = ScriptRef.CurrentParsingScriptRef.GetUnityObjectBySerializedObjectIdentifier(soi);
            if (unityObject != null)
            {
                scriptFieldValue.FieldValue = unityObject.Name;
            }
            scriptFieldValue.OtherInfo = unityObject;
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Image Type";
            scriptFieldValue.FieldValue = ((IMAGE_TYPE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Preserve Aspect";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fill Center";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fill Method";
            int fillMethod = ReadInt4(scriptInfoContent, ref index);
            scriptFieldValue.FieldValue = ((FILL_METHOD)fillMethod).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fill Amount";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Clockwise";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fill Origin";
            int fillOrigin=ReadInt4(scriptInfoContent, ref index);
            scriptFieldValueList.Add(scriptFieldValue);

            switch((FILL_METHOD)fillMethod)
            {
                case FILL_METHOD.Horizontal:
                    scriptFieldValue.FieldValue = ((FILL_ORIGIN_HORIZONTAL)fillOrigin).ToString();
                    break;
                case FILL_METHOD.Vertical:
                    scriptFieldValue.FieldValue = ((FILL_ORIGIN_VERTICAL)fillOrigin).ToString();
                    break;
                case FILL_METHOD.Radial_90:
                    scriptFieldValue.FieldValue = ((FILL_ORIGIN_RADIAL_90)fillOrigin).ToString();
                    break;
                case FILL_METHOD.Radial_180:
                    scriptFieldValue.FieldValue = ((FILL_ORIGIN_RADIAL_180)fillOrigin).ToString();
                    break;
                case FILL_METHOD.Radial_360:
                    scriptFieldValue.FieldValue = ((FILL_ORIGIN_RADIAL_360)fillOrigin).ToString();
                    break;
            }


        }
    }
}
