using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class ButtonSFVS: ScriptFieldValueSet
    {
        public ButtonSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            int index = 0;

            ///前半部分是相同的，所以用基类的处理函数
            GetControlInfo(scriptInfoContent, ref index);

            int onClickFunCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < onClickFunCount; i++)
            {
                GetEventInfo("On Click ",scriptInfoContent, ref index, i);
            }
        }
    }
}
