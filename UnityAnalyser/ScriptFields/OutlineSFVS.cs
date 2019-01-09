using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class OutlineSFVS : ScriptFieldValueSet
    {
        public OutlineSFVS(byte[] scriptInfoContent,ScriptRef  scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Effect Color";
            scriptFieldValue.FieldValue = (ReadColor(scriptInfoContent, ref index));
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Effect Distance X";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Effect Distance Y";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Use Graphic Alpha";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);


        }
    }
}
