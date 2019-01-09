using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public enum EVENT_TYPE_NAME
    {
        Pointer_Enter,
        Pointer_Exit,
        Pointer_Down,
        Pointer_Up,
        Pointer_Click,
        Drag,
        Drop,
        Scroll,
        UpdateSelected,
        Selected,
        Deselect,
        Move,
        InitializePotentialDrag,
        BeginDrag,
        EndDrag,
        Submit,
        Cancel
    }
    public class EventTriggerSFVS: ScriptFieldValueSet
    {
        public EventTriggerSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            int index = 0;
            int eventCount = ReadInt4(scriptInfoContent, ref index);
            for (int i = 0; i < eventCount; i++)
            {
                EVENT_TYPE_NAME eventType = (EVENT_TYPE_NAME)ReadInt4(scriptInfoContent, ref index);
                int inner_eventCount = ReadInt4(scriptInfoContent, ref index);
                for (int j = 0; j < inner_eventCount; j++)
                {
                    GetEventInfo(eventType.ToString() + "_", scriptInfoContent, ref index, j);
                }
                //把EventTrigger的函数的string读取掉
                ReadString(scriptInfoContent, ref index);
            }
        }
    }
}
