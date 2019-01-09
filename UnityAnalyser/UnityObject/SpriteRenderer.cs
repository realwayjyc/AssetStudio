using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Drawing;

namespace UnityAnalyzer
{
    public class SpriteRenderer : Renderer
    {
        private SerializedObjectIdentifier sprite;
        public SerializedObjectIdentifier Sprite
        {
            get { return sprite; }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
        }

        private bool flipX;
        public bool FlipX
        {
            get { return flipX; }
        }

        private bool flipY;
        public bool FlipY
        {
            get { return flipY; }
        }


        public static SpriteRenderer Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            SpriteRenderer ret = new SpriteRenderer();
            int index=objectOffset + objectInfo.ByteStart;
            index=ret.CreateRenderer(objectInfo, content, objectOffset, index);

            ret.sprite = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            float colorR = BitConverter.ToSingle(content, index);
            index += 4;

            float colorG = BitConverter.ToSingle(content, index);
            index += 4;

            float colorB = BitConverter.ToSingle(content, index);
            index += 4;

            float colorA = BitConverter.ToSingle(content, index);
            index += 4;

            ret.color = Color.FromArgb((int)(colorA * 255), (int)(colorR * 255), (int)(colorG * 255), (int)(colorB * 255));

            if (objectInfo.UnityFileVersion[0] == 5 &&
                objectInfo.UnityFileVersion[1] == 3)
            {
                ret.flipX = (content[index++] != 0);
                ret.flipY = (content[index++] != 0);
            }
            return ret;
        }

        public Sprite GetSprite()
        {
            return this.GetUnityObjectBySerializedObjectIdentifier(sprite) as Sprite;
        }

        public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SpriteRendererPanel panel = new SpriteRendererPanel();
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
                        SpriteRendererPanel panel = new SpriteRendererPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
