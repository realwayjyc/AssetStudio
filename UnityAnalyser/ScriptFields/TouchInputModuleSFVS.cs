using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class TouchInputModuleSFVS: ScriptFieldValueSet
    {
        public TouchInputModuleSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Allow Activation On Standalone";
            scriptFieldValue.FieldValue = ReadBoolean4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
