using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAnalyzer
{
    public enum FIT
    {
        Unconstrained=0,
        Min_Size=1,
        Preferred_Size=2,
    }

    public class ContentSizeFitterSFVS : ScriptFieldValueSet
    {
        public ContentSizeFitterSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Horizontal Fit";
            scriptFieldValue.FieldValue = ((FIT)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Vertical Fit";
            scriptFieldValue.FieldValue = ((FIT)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
