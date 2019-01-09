using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class Sprite:UnityObject
    {
        private string spriteName;
        public string SpriteName
        {
            get { return spriteName; }
        }

        private RectangleF rect;
        public RectangleF Rect
        {
            get { return rect; }
        }

        private float offsetX;
        public float OffsetX
        {
            get { return offsetX; }
        }

        private float offsetY;
        public float OffsetY
        {
            get { return offsetY; }
        }

        private Vector4f border;
        public Vector4f Border
        {
            get { return border; }
        }

        private float pixelsToUnits;
        public float PixelsToUnits
        {
            get { return pixelsToUnits; }
        }

        private int extrude;
        public int Extrude
        {
            get { return extrude; }
        }

        private SpriteRenderData spriteRenderData;
        public SpriteRenderData SpriteRenderData
        {
            get { return spriteRenderData; }
        }


        public static Sprite Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Sprite ret = new Sprite();

            int index = objectOffset + objectInfo.ByteStart;

            ret.spriteName = Util.readStringAndAlign(content, objectOffset, ref index);

            ret.rect.X = BitConverter.ToSingle(content, index);
            index += 4;

            ret.rect.Y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.rect.Width = BitConverter.ToSingle(content, index);
            index += 4;

            ret.rect.Height = BitConverter.ToSingle(content, index);
            index += 4;

            ret.offsetX = BitConverter.ToSingle(content, index);
            index += 4;

            ret.offsetY = BitConverter.ToSingle(content, index);
            index += 4;

            ret.border.x = BitConverter.ToSingle(content, index);
            index += 4;

            ret.border.y = BitConverter.ToSingle(content, index);
            index += 4;

            ret.border.z = BitConverter.ToSingle(content, index);
            index += 4;

            ret.border.w = BitConverter.ToSingle(content, index);
            index += 4;

            ret.pixelsToUnits = BitConverter.ToSingle(content, index);
            index += 4;

            ret.extrude = BitConverter.ToInt32(content, index);
            index += 4;

            if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                //未知内容
                index += 4;
            }

            ret.spriteRenderData = SpriteRenderData.Create(ret, content, objectOffset, ref index,objectInfo);

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
                        SpritePanel panel = new SpritePanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

    }
}
