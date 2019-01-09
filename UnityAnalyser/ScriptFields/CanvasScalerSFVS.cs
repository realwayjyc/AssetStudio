using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public enum UI_SCALE_MODE
    {
        CONSTANT_PIXEL_SIZE = 0,
        SCALE_WITH_SCREEN_SIZE = 1,
        CONSTANT_PHYSICAL_SIZE = 2
    }

    public enum PHYSICAL_UNIT
    {
        CENTIMETERS = 0,
        MILLIMETERS = 1,
        INCHES = 2,
        POINTS = 3,
        PICAS = 4
    }

    public enum MATCH_MODE
    {
        MATCH_WIDTH_OR_HEIGHT = 0,
        EXPAND = 1,
        SHRINK = 2
    }

    public class CanvasScalerSFVS : ScriptFieldValueSet
    {
        public CanvasScalerSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Ui Scale Mode";
            scriptFieldValue.FieldValue = ((UI_SCALE_MODE)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Reference Pixels Per Unit";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Scale Factor";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Reference Resoulution X";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent,ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Reference Resoulution Y";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Screen Match Mode";
            scriptFieldValue.FieldValue = ((MATCH_MODE)ReadInt4(scriptInfoContent,ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Match value";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent,ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Physics Unit";
            scriptFieldValue.FieldValue = ((PHYSICAL_UNIT)ReadInt4(scriptInfoContent,ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Fallback Screen DPI";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent,ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Default Sprite Screen DPI";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent,ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
