using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class Layer
    {
        private string name;
        public string Name
        {
          get { return name; }
          set { name = value; }
        }

        private int id;
        public int Id
        {
          get { return id; }
          set { id = value; }
        }
    }
    public class MaskedLayers
    {
        public bool IsNone
        {
            get
            {
                return layer_list.Count == 0;
            }
        }

        public bool IsEverything
        {
            get
            {
                return layer_list.Count == 32;
            }
        }

        private List<Layer> layer_list;
        public List<Layer> Layer_list
        {
            get { return layer_list; }
            set { layer_list = value; }
        }


        private MaskedLayers()
        {
            layer_list = new List<Layer>();
        }

        private void ParseMaskValue(UInt32  maskValue)
        {
            Analyzer Analyzer = MainWindow.instance.CurrentAnalyzer;
            List<string> all_layers = Analyzer.TagManager.Layers;

            for(int i=0;i<32;i++)
            {
                UInt32 m =(UInt32) 1 << i;
                if((m & maskValue)!=0)
                {
                    Layer layer = new Layer();
                    layer.Id = i;
                    layer.Name = all_layers[i];
                    layer_list.Add(layer);
                }
            }
        }

        public static MaskedLayers Parse(UInt32 maskValue)
        {
            MaskedLayers maskedLayers = new MaskedLayers();
            maskedLayers.ParseMaskValue(maskValue);
            return maskedLayers;
        }
    }
}
