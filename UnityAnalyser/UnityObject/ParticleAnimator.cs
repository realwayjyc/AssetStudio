using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Drawing;

namespace UnityAnalyzer
{
    public class ParticleAnimator:Component
    {
        private bool doesAnimatorColor;
        public bool DoesAnimatorColor
        {
            get { return doesAnimatorColor; }
        }

        private Color[] colorAnimation = new Color[5];
        public Color[] ColorAnimation
        {
            get { return colorAnimation; }
        }

        private Vector3F worldRotationAxis;
        public Vector3F WorldRotationAxis
        {
            get { return worldRotationAxis; }
        }

        private Vector3F localRotationAxis;
        public Vector3F LocalRotationAxis
        {
            get { return localRotationAxis; }
        }

        private float sizeGrow;
        public float SizeGrow
        {
            get { return sizeGrow; }
        }

        private Vector3F rndForce;
        public Vector3F RndForce
        {
            get { return rndForce; }
        }

        private Vector3F force;
        public Vector3F Force
        {
            get { return force; }
        }

        private float damping;
        public float Damping
        {
            get { return damping; }
        }

        private bool stopSimulation;
        public bool StopSimulation
        {
            get { return stopSimulation; }
        }

        private bool autodestruct;
        public bool Autodestruct
        {
            get { return autodestruct; }
        }

        public static ParticleAnimator Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            ParticleAnimator ret = new ParticleAnimator();

            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;

            index = objectOffset + objectInfo.ByteStart + 8;

            ret.doesAnimatorColor = (content[index] == 1);
            index += 1;
            index += Util.GetAlignCount(index, objectOffset);

            for (int i = 0; i < 5; i++)
            {
                ret.colorAnimation[i] = Color.FromArgb(content[index], content[index + 1], content[index + 2], content[index+3]);
                index += 4;
            }

            ret.worldRotationAxis = Util.ReadNextVector3f(content, ref index);
            ret.localRotationAxis = Util.ReadNextVector3f(content, ref index);

            ret.sizeGrow = BitConverter.ToSingle(content, index);
            index += 4;

            ret.rndForce = Util.ReadNextVector3f(content, ref index);
            ret.force = Util.ReadNextVector3f(content, ref index);

            ret.damping = BitConverter.ToSingle(content, index);
            index += 4;

            ret.stopSimulation = (content[index] == 1);
            index++;

            ret.autodestruct = (content[index] == 1);
            index++;


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
                        ParticleAnimatorPanel panel = new ParticleAnimatorPanel();
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
                        ParticleAnimatorPanel panel = new ParticleAnimatorPanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }
    }
}
