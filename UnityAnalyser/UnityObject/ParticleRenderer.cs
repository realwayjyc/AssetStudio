using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public enum STRETCH_PARTICLES_MODE
    {
        Billboard=0,
        Sorted_billboard=2,
        Stretched=3,
        Horizontal_Billboard=4,
        Vertical_Billboard = 5
    }

    public class ParticleRenderer:Renderer
    {
        private float cameraVelocityScale;
        public float CameraVelocityScale
        {
            get { return cameraVelocityScale; }
        }

        private STRETCH_PARTICLES_MODE stretchParticlesMode;
        public STRETCH_PARTICLES_MODE StretchParticlesMode
        {
            get { return stretchParticlesMode; }
        }

        private float lengthScale;
        public float LengthScale
        {
            get { return lengthScale; }
        }

        private float velocityScale;
        public float VelocityScale
        {
            get { return velocityScale; }
        }

        private float maxParticleSize;
        public float MaxParticleSize
        {
            get { return maxParticleSize; }
        }

        private UVAnimation uvAnimation;
        public UVAnimation UVAnimation
        {
            get { return uvAnimation; }
        }

        public static ParticleRenderer Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            ParticleRenderer ret = new ParticleRenderer();
            int index = objectOffset + objectInfo.ByteStart;
            index = ret.CreateRenderer(objectInfo, content, objectOffset, index);

            ret.cameraVelocityScale = BitConverter.ToSingle(content, index);
            index += 4;

            ret.stretchParticlesMode =(STRETCH_PARTICLES_MODE) BitConverter.ToInt32(content, index);
            index += 4;


            ret.lengthScale = BitConverter.ToSingle(content, index);
            index += 4;

            ret.velocityScale = BitConverter.ToSingle(content, index);
            index += 4;

            ret.maxParticleSize = BitConverter.ToSingle(content, index);
            index += 4;

            ret.uvAnimation = new UVAnimation();
            ret.uvAnimation.CreateUVAnimation(objectInfo, content, objectOffset, index);
            
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
                        ParticleRendererPanel panel = new ParticleRendererPanel();
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
                        ParticleRendererPanel panel = new ParticleRendererPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
