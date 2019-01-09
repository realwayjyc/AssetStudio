using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class MaskSFVS : ScriptFieldValueSet
    {
        public MaskSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Show Mask Graphic";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
