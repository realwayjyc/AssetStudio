using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    enum BLOCKING_OBJECTS
    {
        NONE = 0,
        TWO_D = 1,
        THREE_D = 2,
        ALL = 3
    }

    public class GraphicRayCasterSFVS : ScriptFieldValueSet
    {
        public GraphicRayCasterSFVS(byte[] scriptInfoContent,ScriptRef scriptRef)
            : base(scriptRef)
        {
            ScriptFieldValue scriptFieldValue = null;
            int index = 0;

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Ignore Reversed Graphics";
            scriptFieldValue.FieldValue = ReadBoolean4(scriptInfoContent,ref index).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Blocking Objects";
            scriptFieldValue.FieldValue = ((BLOCKING_OBJECTS)ReadInt4(scriptInfoContent,ref index)).ToString();
            scriptFieldValueList.Add(scriptFieldValue);

            MaskedLayers maskedLayers = ReadMaskedLayers4(scriptInfoContent,ref index);
            scriptFieldValue = new ScriptFieldValue();
            scriptFieldValue.FieldName = "Blocking Mask";
            if (maskedLayers.IsNone)
            {
                scriptFieldValue.FieldValue = "NONE (0)";
            }
            else if (maskedLayers.IsEverything)
            {
                scriptFieldValue.FieldValue = "Everything (" + maskedLayers.Layer_list.Count+ ")";
            }
            else
            {
                scriptFieldValue.FieldValue = "Count=" + maskedLayers.Layer_list.Count;
            }

            ScriptFieldMultiValue multiValue = new ScriptFieldMultiValue();
            multiValue.AddColumns(new string[] { "Name","Id" });
            List<Layer> layersList = maskedLayers.Layer_list;
            foreach(Layer layer in layersList)
            {
                multiValue.AddValue(layer);
            }
            scriptFieldValue.OtherInfo = multiValue;

            scriptFieldValueList.Add(scriptFieldValue);
        }
    }
}
