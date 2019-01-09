using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAnalyzer
{

    public enum CHILD_ALIGNMENT
    {
        Upper_Left=0,
        Upper_Center,
        Upper_Right,
        Middle_Left ,
        Middle_Center,
        Middle_Right,
        Lower_Left,
        Lower_Center,
        Lower_Right,
    }

    public class VerticalLayoutGroupSFVS : ScriptFieldValueSet
    {
        public VerticalLayoutGroupSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
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
