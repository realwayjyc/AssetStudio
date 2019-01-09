using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace UnityAnalyzer
{

    public enum FMOD_SOUND_FORMAT
    {
        FMOD_SOUND_FORMAT_NONE  = 0,
        FMOD_SOUND_FORMAT_PCM8  = 1,
        FMOD_SOUND_FORMAT_PCM16  = 2,
        FMOD_SOUND_FORMAT_PCM24  = 3,
        FMOD_SOUND_FORMAT_PCM32  = 4,
        FMOD_SOUND_FORMAT_PCMFLOAT  = 5,
        FMOD_SOUND_FORMAT_GCADPCM  = 6,
        FMOD_SOUND_FORMAT_IMAADPCM  = 7,
        FMOD_SOUND_FORMAT_VAG  = 8,
        FMOD_SOUND_FORMAT_HEVAG  = 9,
        FMOD_SOUND_FORMAT_XMA  = 10,
        FMOD_SOUND_FORMAT_MPEG  = 11,
        FMOD_SOUND_FORMAT_CELT  = 12,
        FMOD_SOUND_FORMAT_AT9  = 13,
        FMOD_SOUND_FORMAT_XWMA  = 14,
        FMOD_SOUND_FORMAT_VORBIS  = 15,
        FMOD_SOUND_FORMAT_MAX  = 16,
        FMOD_SOUND_FORMAT_FORCEINT = 0x10000
    }

    public enum FMOD_SOUND_TYPE
    {
        FMOD_SOUND_TYPE_UNKNOWN  = 0,
        FMOD_SOUND_TYPE_ACC  = 1,
        FMOD_SOUND_TYPE_AIFF  = 2,
        FMOD_SOUND_TYPE_ASF  = 3,
        FMOD_SOUND_TYPE_AT3  = 4,
        FMOD_SOUND_TYPE_CDDA  = 5,
        FMOD_SOUND_TYPE_DLS  = 6,
        FMOD_SOUND_TYPE_FLAC  = 7,
        FMOD_SOUND_TYPE_FSB  = 8,
        FMOD_SOUND_TYPE_GCADPCM  = 9,
        FMOD_SOUND_TYPE_IT  = 0xA,
        FMOD_SOUND_TYPE_MIDI  = 0xB,
        FMOD_SOUND_TYPE_MOD  = 0xC,
        FMOD_SOUND_TYPE_MPEG  = 0xD,
        FMOD_SOUND_TYPE_OGGVORBIS  = 0xE,
        FMOD_SOUND_TYPE_PLAYLIST  = 0xF,
        FMOD_SOUND_TYPE_RAW  = 0x10,
        FMOD_SOUND_TYPE_S3M  = 0x11,
        FMOD_SOUND_TYPE_SF2  = 0x12,
        FMOD_SOUND_TYPE_USER  = 0x13,
        FMOD_SOUND_TYPE_WAV  = 0x14,
        FMOD_SOUND_TYPE_XM  = 0x15,
        FMOD_SOUND_TYPE_XMA  = 0x16,
        FMOD_SOUND_TYPE_VAG  = 0x17,
        FMOD_SOUND_TYPE_AUDIOQUEUE  = 0x18,
        FMOD_SOUND_TYPE_XWMA  = 0x19,
        FMOD_SOUND_TYPE_BCWAV  = 0x1A,
        FMOD_SOUND_TYPE_AT9  = 0x1B,
        FMOD_SOUND_TYPE_VORBIS  = 0x1C,
        FMOD_SOUND_TYPE_MEDIA_FOUNDATION  = 0x1D,
        FMOD_SOUND_TYPE_MAX  = 0x1E,
        FMOD_SOUND_TYPE_FORCEINT  = 0x10000
    }

    public class AudioClip :UnityObject
    {
        private string audioClipName;
        public string AudioClipName
        {
            get { return audioClipName; }
        }

        private FMOD_SOUND_FORMAT format;
        public FMOD_SOUND_FORMAT Format
        {
            get { return format; }
        }

        private FMOD_SOUND_TYPE type;
        public FMOD_SOUND_TYPE Type
        {
            get { return type; }
        }



        private Boolean is3dSound;
        public Boolean Is3dSound
        {
            get { return is3dSound; }
        }

        private Boolean isUseHardware;
        public Boolean IsUseHardware
        {
            get { return isUseHardware; }
        }

        private int streamCount;

        public int StreamCount
        {
            get { return streamCount; }
        }

        private byte[] streamContent;
        public byte[] StreamContent
        {
            get { return streamContent; }
            set { streamContent = value; }
        }


        private string extension;
        public string Extension
        {
            get { return extension; }
        }

        private string infoText;
        public string InfoText
        {
            get { return infoText; }
            set { infoText = value; }
        }

        public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AudioClipPanel panel = new AudioClipPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public static AudioClip Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            AudioClip ret = new AudioClip();
            int index = objectOffset + objectInfo.ByteStart;

            ret.audioClipName = Util.readStringAndAlign(content, objectOffset, ref index);

            ret.format = (FMOD_SOUND_FORMAT)BitConverter.ToInt32(content, index);
            index += 4;

            ret.type = (FMOD_SOUND_TYPE)BitConverter.ToInt32(content, index);
            index += 4;

            ret.is3dSound = !(content[index++] == 0);

            ret.isUseHardware = !(content[index++] == 0);
            index += Util.GetAlignCount(index, objectOffset);

            ret.streamCount = BitConverter.ToInt32(content, index);
            index += 4;

            int streamSize = BitConverter.ToInt32(content, index);
            index += 4;


            ret.streamContent = new byte[streamSize];
            Array.Copy(content, index, ret.streamContent, 0, streamSize);

            index += streamSize;

            ret.infoText = "Compression format: ";
            switch ((int)ret.type)
            {
                case 2:
                    ret.extension = ".aif";
                    ret.InfoText += "AIFF";
                    break;
                case 13:
                    ret.extension = ".mp3";
                    ret.InfoText += "MP3";
                    break;
                case 14:
                    ret.extension = ".ogg";
                    ret.InfoText += "Ogg Vorbis";
                    break;
                case 20:
                    ret.extension = ".wav";
                    ret.InfoText += "WAV";
                    break;
                case 22: //xbox encoding
                    ret.extension = ".wav";
                    ret.InfoText += "Xbox360 WAV";
                    break;
            }

            if (ret.extension == null)
            {
                ret.extension = ".AudioClip";
                ret.InfoText += "Unknown";
            }
            ret.InfoText += "\n3D: " + ret.is3dSound;

            return ret;
        }

        public void SaveToFile()
        {
            System.Windows.Forms.SaveFileDialog open = new System.Windows.Forms.SaveFileDialog();
            open.FileName = audioClipName + extension;
            open.Filter = extension + "文件（*" + extension + "）|*" + extension + "|所有文件(*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = open.FileName;
                Transform(filename);
                if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show(
                    "已经保存到了文件:" + filename + "，是否需要打开目录",
                    "",
                    System.Windows.Forms.MessageBoxButtons.OKCancel))
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                    psi.Arguments = "/e,/select," + filename;
                    System.Diagnostics.Process.Start(psi);
                }
            }
        }

        public void Transform(string filename)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            fileStream.Write(streamContent, 0, streamContent.Length);
            fileStream.Close();
        }
    }
}
