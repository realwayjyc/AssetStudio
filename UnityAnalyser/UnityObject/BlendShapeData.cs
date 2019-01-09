using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class BlendShapeData
    {
        //TODO
        public int Create(byte[] content, int objectOffset, int index)
        {
            int verticesCount = BitConverter.ToInt32(content, index);
            index+=4;
            if (verticesCount != 0)
            {
                System.Windows.MessageBox.Show("功能未实现1");
                System.Environment.Exit(1);
            }

            int shapeCount = BitConverter.ToInt32(content, index);
            index += 4;
            if (shapeCount != 0)
            {
                System.Windows.MessageBox.Show("功能未实现2");
                System.Environment.Exit(1);
            }

            int channelCount = BitConverter.ToInt32(content, index);
            index += 4;
            if (channelCount != 0)
            {
                System.Windows.MessageBox.Show("功能未实现3");
                System.Environment.Exit(1);
            }


            int weightsCount = BitConverter.ToInt32(content, index);
            index += 4;
            if (weightsCount != 0)
            {
                System.Windows.MessageBox.Show("功能未实现4");
                System.Environment.Exit(1);
            }

            return index;
        }
    }
}
