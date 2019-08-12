using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class SkinnedMeshRenderer : Renderer
    {
        /// <summary>
        /// Mesh
        /// </summary>
        public SerializedObjectIdentifier Mesh { get; set; }

        //Transform
        public List<SerializedObjectIdentifier> Bones;

        public List<float> BlendShapeWeights;

        public static SkinnedMeshRenderer Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            SkinnedMeshRenderer ret = new SkinnedMeshRenderer();
            int index = objectOffset + objectInfo.ByteStart;
            index = ret.CreateRenderer(objectInfo, content, objectOffset, index);

            int m_Quality = BitConverter.ToInt32(content, index);
            index += 4;

            bool m_UpdateWhenOffscreen = (content[index++] == 1);
            bool m_SkinNormals = (content[index++] == 1);
            index += Util.GetAlignCount(index, objectOffset);


            ret.Mesh = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            ret.Bones= Util.ReadSerializedObjectIdentifierList(content, ref index, objectInfo);

            int weightsCount= BitConverter.ToInt32(content, index);
            index += 4;

            ret.BlendShapeWeights = new List<float>();
            for (int i = 0; i < weightsCount; i++)
            {
                float weight= BitConverter.ToSingle(content, index);
                index += 4;

                ret.BlendShapeWeights.Add(weight);
            }
            return ret;
        }

        public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SkinnedMeshRendererPanel panel = new SkinnedMeshRendererPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public override UserControl CreateGameObjectComponentInfoControl()
        {
            if (gameObjectComponentInfoControl == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SkinnedMeshRendererPanel panel = new SkinnedMeshRendererPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
