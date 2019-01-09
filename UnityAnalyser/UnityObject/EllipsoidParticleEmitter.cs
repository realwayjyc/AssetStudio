using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class EllipsoidParticleEmitter : ParticleEmitter
    {
        private Vector3f ellipsoid;
        public Vector3f Ellipsoid
        {
            get { return ellipsoid; }
        }

        private float minEmitterRange;
        public float MinEmitterRange
        {
            get { return minEmitterRange; }
        }


        public static EllipsoidParticleEmitter Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            EllipsoidParticleEmitter ret = new EllipsoidParticleEmitter();
            int index = objectOffset + objectInfo.ByteStart;
            index = ret.CreateParticleEmitter(objectInfo, content, objectOffset, index);
            index += Util.GetAlignCount(index, objectOffset);

            ret.ellipsoid = Util.ReadNextVector3f(content, ref index);

            ret.minEmitterRange = BitConverter.ToSingle(content, index);

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
                        EllipsoidParticleEmitterPanel panel = new EllipsoidParticleEmitterPanel();
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
                        EllipsoidParticleEmitterPanel panel = new EllipsoidParticleEmitterPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
