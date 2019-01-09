using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public enum ROLL_OFF_MODE
    {
        Logarithimic=0,
        Linear=1,
        Custom=2

    }
    public class AudioSource : Component
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        private SerializedObjectIdentifier audioClip;
        public SerializedObjectIdentifier AudioClip
        {
            get { return audioClip; }
        }

        private bool playOnAwake;
        public bool PlayOnAwake
        {
            get { return playOnAwake; }
        }

        private float volumn;
        public float Volumn
        {
            get { return volumn; }
        }

        private float pitch;

        public float Pitch
        {
            get { return pitch; }
        }

        private bool loop;
        public bool Loop
        {
            get { return loop; }
        }

        private bool mute;
        public bool Mute
        {
            get { return mute; }
        }

        private int priority;
        public int Priority
        {
            get { return priority; }
        }

        private float dopplerLevel;
        public float DopplerLevel
        {
            get { return dopplerLevel; }
        }

        private float minDistance;

        public float MinDistance
        {
            get { return minDistance; }
        }

        private float maxDistance;
        public float MaxDistance
        {
            get { return maxDistance; }
        }

        private float pan2D;
        public float Pan2D
        {
            get { return pan2D; }
        }

        private ROLL_OFF_MODE rollOffMode;
        public ROLL_OFF_MODE RollOffMode
        {
            get { return rollOffMode; }
        }

        private bool bypassEffects;
        public bool BypassEffects
        {
            get { return bypassEffects; }
        }

        private bool bypassListenerEffects;
        public bool BypassListenerEffects
        {
            get { return bypassListenerEffects; }
        }

        private bool bypassReverbZones;
        public bool BypassReverbZones
        {
            get { return bypassReverbZones; }
        }

        public static AudioSource Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            AudioSource ret = new AudioSource();
            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;

            index = objectOffset + objectInfo.ByteStart + 8;
            ret.isEnabled = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);

            ret.audioClip = Util.ReadNextSerializedObjectIdentifier(content, ref index, objectInfo);

            ret.playOnAwake = (content[index++] == 1);
            index += Util.GetAlignCount(index, objectOffset);

            ret.volumn = BitConverter.ToSingle(content, index);
            index += 4;

            ret.pitch = BitConverter.ToSingle(content, index);
            index += 4;

            ret.loop = (content[index++] == 1);
            ret.mute = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);

            ret.priority = BitConverter.ToInt32(content, index);
            index += 4;

            ret.dopplerLevel = BitConverter.ToSingle(content, index);
            index += 4;

            ret.minDistance = BitConverter.ToSingle(content, index);
            index += 4;

            ret.maxDistance = BitConverter.ToSingle(content, index);
            index += 4;

            ret.pan2D = BitConverter.ToSingle(content, index);
            index += 4;


            ret.rollOffMode = (ROLL_OFF_MODE)BitConverter.ToInt32(content, index);
            index += 4;

            ret.bypassEffects = (content[index++] == 1);
            ret.bypassListenerEffects = (content[index++] == 1);
            ret.bypassReverbZones = (content[index++] == 1);
            index += Util.GetAlignCount(index, objectOffset);

            //不往下读取了

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
                        AudioSourcePanel panel = new AudioSourcePanel();
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
                        AudioSourcePanel panel = new AudioSourcePanel();
                        gameObjectComponentInfoControl = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return gameObjectComponentInfoControl;
        }

        public AudioClip GetAudioClip()
        {
            AudioClip ret = this.GetUnityObjectBySerializedObjectIdentifier(this.audioClip) as AudioClip;
            return ret;
        }

      
    }
}
