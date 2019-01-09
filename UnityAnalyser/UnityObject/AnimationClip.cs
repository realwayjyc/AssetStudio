using System;
using System.Windows.Controls;


namespace UnityAnalyzer
{
    public class AnimationClip : UnityObject
    {
        private string animationClipName;
        public string AnimationClipName
        {
            get { return animationClipName; }
        }

         public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AnimationClipPanel panel = new AnimationClipPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

         public static AnimationClip Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
         {
             AnimationClip ret = new AnimationClip();
             int index = objectOffset + objectInfo.ByteStart;

             ret.animationClipName = Util.readStringAndAlign(content, objectOffset, ref index);

             return ret;

         }
    }
}
