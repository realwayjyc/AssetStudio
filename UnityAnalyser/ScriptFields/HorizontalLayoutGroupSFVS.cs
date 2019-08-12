using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAnalyzer
{
      public class HorizontalLayoutGroupSFVS : ScriptFieldValueSet
    {
        public HorizontalLayoutGroupSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            int index = 0;
            ScriptFieldValue scriptFieldValue = null;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Padding Left";
            scriptFieldValue.FieldValue = ReadInt4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Padding Right";
            scriptFieldValue.FieldValue = ReadInt4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Padding Top";
            scriptFieldValue.FieldValue = ReadInt4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Padding Bottom";
            scriptFieldValue.FieldValue = ReadInt4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Child Alignment";
            scriptFieldValue.FieldValue = ((CHILD_ALIGNMENT)ReadInt4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Spacing";
            scriptFieldValue.FieldValue = ReadSingle4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Child Force Expand Width";
            scriptFieldValue.FieldValue = ReadBoolean4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Child Force Expand Height";
            scriptFieldValue.FieldValue = ReadBoolean4(scriptInfoContent, ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

        }
    }
}
