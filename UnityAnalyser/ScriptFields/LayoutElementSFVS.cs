using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityAnalyzer
{
    public class LayoutElementSFVS : ScriptFieldValueSet
    {
        public LayoutElementSFVS(byte[] scriptInfoContent, ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Ignore Layout";
            scriptFieldValue.FieldValue = (ReadBoolean4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Min Width";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
            if (scriptFieldValue.FieldValue=="-1")
            {
                scriptFieldValue.FieldValue = "False";
            }

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Min Height";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
            if (scriptFieldValue.FieldValue == "-1")
            {
                scriptFieldValue.FieldValue = "False";
            }

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Preferred Width";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
            if (scriptFieldValue.FieldValue == "-1")
            {
                scriptFieldValue.FieldValue = "False";
            }

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Preferred Height";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
            if (scriptFieldValue.FieldValue == "-1")
            {
                scriptFieldValue.FieldValue = "False";
            }

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Flexible Width";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
            if (scriptFieldValue.FieldValue == "-1")
            {
                scriptFieldValue.FieldValue = "False";
            }


            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Flexible Height";
            scriptFieldValue.FieldValue = (ReadSingle4(scriptInfoContent, ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);
            if (scriptFieldValue.FieldValue == "-1")
            {
                scriptFieldValue.FieldValue = "False";
            }
        }
    }
}
