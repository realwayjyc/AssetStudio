using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class StandaloneInputModuleSFVS : ScriptFieldValueSet
    {
        public StandaloneInputModuleSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Horizontal Axis";
            scriptFieldValue.FieldValue = ReadString(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Vertical Axis";
            scriptFieldValue.FieldValue = ReadString(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Submit Button";
            scriptFieldValue.FieldValue = ReadString(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Cancel Button";
            scriptFieldValue.FieldValue = ReadString(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Input Action Per Second";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


            if (scriptRef.UnityFileVersion[0]==5 && 
                scriptRef.UnityFileVersion[1]==3)
            {
                scriptFieldValue = new ScriptFieldValue();
                scriptFieldValue.FieldName = "Repeat Delay";
                scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
                scriptFieldValueList.Add(scriptFieldValue);
            }

            scriptFieldValue = new ScriptFieldValue();
            if (scriptRef.UnityFileVersion[0] == 5 &&
                scriptRef.UnityFileVersion[1] == 3)
            {
                scriptFieldValue.FieldName = "Force Modle Active";
            }
            else if (scriptRef.UnityFileVersion[0] == 4 &&
                scriptRef.UnityFileVersion[1] == 6)
            {
                scriptFieldValue.FieldName = "Allow Activation On Mobile";
            }
            scriptFieldValue.FieldValue = ReadBoolean4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
