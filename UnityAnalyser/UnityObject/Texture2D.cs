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
    public enum QFORMAT
    {
        // General formats
        Q_FORMAT_RGBA_8UI = 1,
        Q_FORMAT_RGBA_8I,
        Q_FORMAT_RGB5_A1UI,
        Q_FORMAT_RGBA_4444,
        Q_FORMAT_RGBA_16UI,
        Q_FORMAT_RGBA_16I,
        Q_FORMAT_RGBA_32UI,
        Q_FORMAT_RGBA_32I,

        Q_FORMAT_PALETTE_8_RGBA_8888,
        Q_FORMAT_PALETTE_8_RGBA_5551,
        Q_FORMAT_PALETTE_8_RGBA_4444,
        Q_FORMAT_PALETTE_4_RGBA_8888,
        Q_FORMAT_PALETTE_4_RGBA_5551,
        Q_FORMAT_PALETTE_4_RGBA_4444,
        Q_FORMAT_PALETTE_1_RGBA_8888,
        Q_FORMAT_PALETTE_8_RGB_888,
        Q_FORMAT_PALETTE_8_RGB_565,
        Q_FORMAT_PALETTE_4_RGB_888,
        Q_FORMAT_PALETTE_4_RGB_565,

        Q_FORMAT_R2_GBA10UI,
        Q_FORMAT_RGB10_A2UI,
        Q_FORMAT_RGB10_A2I,
        Q_FORMAT_RGBA_F,
        Q_FORMAT_RGBA_HF,

        Q_FORMAT_RGB9_E5,   // Last five bits are exponent bits (Read following section in GLES3 spec: "3.8.17 Shared Exponent Texture Color Conversion")
        Q_FORMAT_RGB_8UI,
        Q_FORMAT_RGB_8I,
        Q_FORMAT_RGB_565,
        Q_FORMAT_RGB_16UI,
        Q_FORMAT_RGB_16I,
        Q_FORMAT_RGB_32UI,
        Q_FORMAT_RGB_32I,

        Q_FORMAT_RGB_F,
        Q_FORMAT_RGB_HF,
        Q_FORMAT_RGB_11_11_10_F,

        Q_FORMAT_RG_F,
        Q_FORMAT_RG_HF,
        Q_FORMAT_RG_32UI,
        Q_FORMAT_RG_32I,
        Q_FORMAT_RG_16I,
        Q_FORMAT_RG_16UI,
        Q_FORMAT_RG_8I,
        Q_FORMAT_RG_8UI,
        Q_FORMAT_RG_S88,

        Q_FORMAT_R_32UI,
        Q_FORMAT_R_32I,
        Q_FORMAT_R_F,
        Q_FORMAT_R_16F,
        Q_FORMAT_R_16I,
        Q_FORMAT_R_16UI,
        Q_FORMAT_R_8I,
        Q_FORMAT_R_8UI,

        Q_FORMAT_LUMINANCE_ALPHA_88,
        Q_FORMAT_LUMINANCE_8,
        Q_FORMAT_ALPHA_8,

        Q_FORMAT_LUMINANCE_ALPHA_F,
        Q_FORMAT_LUMINANCE_F,
        Q_FORMAT_ALPHA_F,
        Q_FORMAT_LUMINANCE_ALPHA_HF,
        Q_FORMAT_LUMINANCE_HF,
        Q_FORMAT_ALPHA_HF,
        Q_FORMAT_DEPTH_16,
        Q_FORMAT_DEPTH_24,
        Q_FORMAT_DEPTH_24_STENCIL_8,
        Q_FORMAT_DEPTH_32,

        Q_FORMAT_BGR_565,
        Q_FORMAT_BGRA_8888,
        Q_FORMAT_BGRA_5551,
        Q_FORMAT_BGRX_8888,
        Q_FORMAT_BGRA_4444,
        // Compressed formats
        Q_FORMAT_ATITC_RGBA,
        Q_FORMAT_ATC_RGBA_EXPLICIT_ALPHA = Q_FORMAT_ATITC_RGBA,
        Q_FORMAT_ATITC_RGB,
        Q_FORMAT_ATC_RGB = Q_FORMAT_ATITC_RGB,
        Q_FORMAT_ATC_RGBA_INTERPOLATED_ALPHA,
        Q_FORMAT_ETC1_RGB8,
        Q_FORMAT_3DC_X,
        Q_FORMAT_3DC_XY,

        Q_FORMAT_ETC2_RGB8,
        Q_FORMAT_ETC2_RGBA8,
        Q_FORMAT_ETC2_RGB8_PUNCHTHROUGH_ALPHA1,
        Q_FORMAT_ETC2_SRGB8,
        Q_FORMAT_ETC2_SRGB8_ALPHA8,
        Q_FORMAT_ETC2_SRGB8_PUNCHTHROUGH_ALPHA1,
        Q_FORMAT_EAC_R_SIGNED,
        Q_FORMAT_EAC_R_UNSIGNED,
        Q_FORMAT_EAC_RG_SIGNED,
        Q_FORMAT_EAC_RG_UNSIGNED,

        Q_FORMAT_S3TC_DXT1_RGB,
        Q_FORMAT_S3TC_DXT1_RGBA,
        Q_FORMAT_S3TC_DXT3_RGBA,
        Q_FORMAT_S3TC_DXT5_RGBA,

        // YUV formats
        Q_FORMAT_AYUV_32,
        Q_FORMAT_I444_24,
        Q_FORMAT_YUYV_16,
        Q_FORMAT_UYVY_16,
        Q_FORMAT_I420_12,
        Q_FORMAT_YV12_12,
        Q_FORMAT_NV21_12,
        Q_FORMAT_NV12_12,

        // ASTC Format
        Q_FORMAT_ASTC_8,
        Q_FORMAT_ASTC_16,
    };

    public enum TextureFormat
    {
        Alpha8 = 1,
        ARGB4444,
        RGB24,
        RGBA32,
        ARGB32,
        RGB565 = 7,
        DXT1 = 10,
        DXT5 = 12,
        RGBA4444,
        BGRA32,
        PVRTC_RGB2 = 30,
        PVRTC_RGBA2,
        PVRTC_RGB4,
        PVRTC_RGBA4,
        ETC_RGB4,
        ATC_RGB4,
        ATC_RGBA8,
        ATF_RGB_DXT1 = 38,
        ATF_RGBA_JPG,
        ATF_RGB_JPG,
        EAC_R,
        EAC_R_SIGNED,
        EAC_RG,
        EAC_RG_SIGNED,
        ETC2_RGB,
        ETC2_RGBA1,
        ETC2_RGBA8,
        ASTC_RGB_4x4,
        ASTC_RGB_5x5,
        ASTC_RGB_6x6,
        ASTC_RGB_8x8,
        ASTC_RGB_10x10,
        ASTC_RGB_12x12,
        ASTC_RGBA_4x4,
        ASTC_RGBA_5x5,
        ASTC_RGBA_6x6,
        ASTC_RGBA_8x8,
        ASTC_RGBA_10x10,
        ASTC_RGBA_12x12,
        [Obsolete("Use PVRTC_RGB2")]
        PVRTC_2BPP_RGB = 30,
        [Obsolete("Use PVRTC_RGBA2")]
        PVRTC_2BPP_RGBA,
        [Obsolete("Use PVRTC_RGB4")]
        PVRTC_4BPP_RGB,
        [Obsolete("Use PVRTC_RGBA4")]
        PVRTC_4BPP_RGBA
    }

    public enum TextureUsageMode
    {
        TexUsageNone = 0,
        TexUsageLightmapDoubleLDR = 1,
        TexUsageLightmapRGBM = 2,
        TexUsageNormalmapDXT5nm = 3,
        TexUsageNormalmapPlain = 4,
        TexUsageAlwaysPadded = 6
    }

    public enum TextureDimension
    {
        TexDimNone = 0,
        TexDimDeprecated1D = 1,
        TexDim2D = 2,
        TexDim3D = 3,
        TexDimCUBE = 4,
        TexDimAny = 5,
        TexDimCount = 6,
        TexDimForce32Bit = 0x7FFFFFFF,
        TexDimUnknown = -1
    }

    public enum TextureFilterMode
    {
        TexFilterNearest = 0,
        TexFilterBilinear = 1,
        TexFilterTrilinear = 2,
        TexFilterCount = 3
    }

    public enum TextureWrapMode
    {
        TexWrapRepeat = 0,
        TexWrapClamp = 1,
        TexWrapCount = 2
    }

    public enum TextureColorSpace
    {
        TexColorSpaceLinear = 0,
        TexColorSpaceSRGB = 1,
        TexColorSpaceSRGBXenon = 2
    }

    public class TexutureSettings
    {
        private TextureFilterMode filterMode;
        public TextureFilterMode FilterMode
        {
            get { return filterMode; }
            set { filterMode = value; }
        }

        private int aniso;
        public int Aniso
        {
            get { return aniso; }
            set { aniso = value; }
        }

        private float mipBias;
        public float MipBias
        {
            get { return mipBias; }
            set { mipBias = value; }
        }

        private TextureWrapMode wrapMode;
        public TextureWrapMode WrapMode
        {
            get { return wrapMode; }
            set { wrapMode = value; }
        }

    }


    public static class KTXHeader
    {
        public static byte[] IDENTIFIER = { 0xAB, 0x4B, 0x54, 0x58, 0x20, 0x31, 0x31, 0xBB, 0x0D, 0x0A, 0x1A, 0x0A };
        public static byte[] ENDIANESS_LE = { 1, 2, 3, 4 };
        public static byte[] ENDIANESS_BE = { 4, 3, 2, 1 };

        // constants for glInternalFormat
        public static int GL_ETC1_RGB8_OES = 0x8D64;

        public static int GL_COMPRESSED_RGB_PVRTC_4BPPV1_IMG = 0x8C00;
        public static int GL_COMPRESSED_RGB_PVRTC_2BPPV1_IMG = 0x8C01;
        public static int GL_COMPRESSED_RGBA_PVRTC_4BPPV1_IMG = 0x8C02;
        public static int GL_COMPRESSED_RGBA_PVRTC_2BPPV1_IMG = 0x8C03;

        public static int GL_ATC_RGB_AMD = 0x8C92;
        public static int GL_ATC_RGBA_EXPLICIT_ALPHA_AMD = 0x8C93;
        public static int GL_ATC_RGBA_INTERPOLATED_ALPHA_AMD = 0x87EE;

        public static int GL_COMPRESSED_RGB8_ETC2 = 0x9274;
        public static int GL_COMPRESSED_SRGB8_ETC2 = 0x9275;
        public static int GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9276;
        public static int GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 = 0x9277;
        public static int GL_COMPRESSED_RGBA8_ETC2_EAC = 0x9278;
        public static int GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC = 0x9279;
        public static int GL_COMPRESSED_R11_EAC = 0x9270;
        public static int GL_COMPRESSED_SIGNED_R11_EAC = 0x9271;
        public static int GL_COMPRESSED_RG11_EAC = 0x9272;
        public static int GL_COMPRESSED_SIGNED_RG11_EAC = 0x9273;

        public static int GL_COMPRESSED_RED_RGTC1 = 0x8DBB;
        public static int GL_COMPRESSED_RG_RGTC2 = 0x8DBD;
        public static int GL_COMPRESSED_RGB_BPTC_UNSIGNED_FLOAT = 0x8E8F;
        public static int GL_COMPRESSED_RGBA_BPTC_UNORM = 0x8E8C;

        public static int GL_R16F = 0x822D;
        public static int GL_RG16F = 0x822F;
        public static int GL_RGBA16F = 0x881A;
        public static int GL_R32F = 0x822E;
        public static int GL_RG32F = 0x8230;
        public static int GL_RGBA32F = 0x8814;

        // constants for glBaseInternalFormat
        public static int GL_RED = 0x1903;
        public static int GL_GREEN = 0x1904;
        public static int GL_BLUE = 0x1905;
        public static int GL_ALPHA = 0x1906;
        public static int GL_RGB = 0x1907;
        public static int GL_RGBA = 0x1908;
        public static int GL_RG = 0x8227;
    }

    public class Texture2D : UnityObject
    {
        private int dwPitchOrLinearSize;
        private int dwFourCC;
        private int dwFlags2;
        private int dwRGBBitCount;
        private int dwRBitMask;
        private int dwGBitMask;
        private int dwBBitMask;
        private int dwABitMask;

        private int glBaseInternalFormat;

        //TextureConverter
        private QFORMAT q_format;

        private string textureName;
        public string TextureName
        {
            get { return textureName; }
        }
        private int textureWidth;
        public int TextureWidth
        {
            get { return textureWidth; }
        }

        private int textureHeight;
        public int TextureHeight
        {
            get { return textureHeight; }
        }

        private int imageSize;
        public int ImageSize
        {
            get { return imageSize; }
        }

        private TextureFormat format;
        public TextureFormat Format
        {
            get { return format; }
        }

        private bool mipMap;
        public bool MipMap
        {
            get { return mipMap; }
        }

        private int mipmapCount;
        public int MipmapCount
        {
            get { return mipmapCount; }
        }

        private bool isReadable;
        public bool IsReadable
        {
            get { return isReadable; }
        }

        private bool readAllowed;
        public bool ReadAllowed
        {
            get { return readAllowed; }
        }

        private int imageCount;
        public int ImageCount
        {
            get { return imageCount; }
        }

        private TextureDimension textureDimension;
        public TextureDimension TextureDimension
        {
            get { return textureDimension; }
        }

        private TexutureSettings textureSettings;
        public TexutureSettings TextureSettings
        {
            get { return textureSettings; }
        }

        private TextureUsageMode usageMode;
        public TextureUsageMode UsageMode
        {
            get { return usageMode; }
        }

        private TextureColorSpace colorSpace;
        public TextureColorSpace ColorSpace
        {
            get { return colorSpace; }
        }

        private int imageSizeXImageCount;
        public int ImageSizeXImageCount
        {
            get { return imageSizeXImageCount; }
        }

        private byte[] imageContent;
        public byte[] ImageContent
        {
            get { return imageContent; }
        }

        /// <summary>
        /// 当内容存放在resS文件中时，一下的变量用于获取resS文件中的内容
        /// </summary>
        private int resS_Offset;
        private int resS_Size;
        private string resS_Path;

        [DllImport("TextureConverterWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Ponvert(byte[] data, int dataSize, int width, int height, int type, bool fixAlpha, IntPtr image);

        public static Texture2D Create(ObjectInfo objectInfo, byte[] content, int objectOffset,ref int outIndex)
        {
            Texture2D ret = new Texture2D();
            int index = objectOffset + objectInfo.ByteStart;
            ret.textureName = Util.readStringAndAlign(content, objectOffset, ref index);

            ret.textureWidth = BitConverter.ToInt32(content, index);
            index += 4;

            ret.textureHeight = BitConverter.ToInt32(content, index);
            index += 4;

            ret.imageSize = BitConverter.ToInt32(content, index);
            index += 4;

            ret.format = (TextureFormat)BitConverter.ToInt32(content, index);
            index += 4;

            if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
            {
                ret.mipmapCount = BitConverter.ToInt32(content, index);
                index += 4;
            }
            else if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
            {
                ret.mipMap = (content[index++] == 1);
            }

           
            ret.isReadable = (content[index++] == 1);
            ret.readAllowed = (content[index++] == 1);
            index += Util.GetAlignCount(index, objectOffset);

            ret.imageCount = BitConverter.ToInt32(content, index);
            index += 4;

            ret.textureDimension = (TextureDimension)BitConverter.ToInt32(content, index);
            index += 4;

            //还要传递4*4个字节的TexutureSettings
            ret.textureSettings = new TexutureSettings();

            ret.textureSettings.FilterMode = (TextureFilterMode)BitConverter.ToInt32(content, index);
            index += 4;

            ret.textureSettings.Aniso = BitConverter.ToInt32(content, index);
            index += 4;

            ret.textureSettings.MipBias = BitConverter.ToSingle(content, index);
            index += 4;

            ret.textureSettings.WrapMode = (TextureWrapMode)BitConverter.ToInt32(content, index);
            index += 4;

            ret.usageMode = (TextureUsageMode)BitConverter.ToInt32(content, index);
            index += 4;

            ret.colorSpace = (TextureColorSpace)BitConverter.ToInt32(content, index);
            index += 4;

            ret.imageSizeXImageCount = BitConverter.ToInt32(content, index);
            index += 4;

            if (ret.imageSizeXImageCount == 0 &&
                (objectInfo.UnityFileVersion[0]==5 && objectInfo.UnityFileVersion[1]==3))
            {
                //内容存放在了相应的.resS文件中
                ret.resS_Offset=BitConverter.ToInt32(content, index);
                index += 4;
                ret.resS_Size=BitConverter.ToInt32(content, index);
                index += 4;
                ret.imageSizeXImageCount = ret.resS_Size;

                ret.imageContent = new byte[ret.imageSizeXImageCount];

                 ret.resS_Path=Util.readStringAndAlign(content, objectOffset, ref index);
                 Util.ReadFileContent(MainWindow.instance.CurrentAnalyzer.Current_folder + ret.resS_Path, ret.resS_Offset, ret.resS_Size, ret.imageContent);
            }
            else
            {
                ret.imageContent = new byte[ret.imageSizeXImageCount];
                Array.Copy(content, index, ret.imageContent, 0, ret.imageSizeXImageCount);      //名字后0x38个字节
                index += ret.imageContent.Length;
            }
            outIndex = index;
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
                        Texture2DPanel panel = new Texture2DPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public void SaveToFile(RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone)
        {
            System.Windows.Forms.SaveFileDialog open = new System.Windows.Forms.SaveFileDialog();
            open.FileName = textureName + ".png";
            open.Filter = "PNG文件（*.PNG）|*.png|所有文件(*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = open.FileName;
                Transform(filename, rotateFlipType);
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


        public void Transform(string fileName, RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone)
        {
            switch (format)
            {
                case TextureFormat.RGB24:
                    {
                        /*dwFlags2 = 0x40;
                           dwRGBBitCount = 0x18;
                           dwRBitMask = 0xFF;
                           dwGBitMask = 0xFF00;
                           dwBBitMask = 0xFF0000;
                           dwABitMask = 0x0;*/

                        //转BGRA32
                        var BGRA32 = new byte[imageContent.Length / 3 * 4];
                        for (var i = 0; i < imageContent.Length / 3; i++)
                        {
                            BGRA32[i * 4] = imageContent[i * 3 + 2];
                            BGRA32[i * 4 + 1] = imageContent[i * 3 + 1];
                            BGRA32[i * 4 + 2] = imageContent[i * 3 + 0];
                            BGRA32[i * 4 + 3] = 255;
                        }
                        //imageContent = BGRA32;

                        dwFlags2 = 0x41;
                        dwRGBBitCount = 0x20;
                        dwRBitMask = 0xFF0000;
                        dwGBitMask = 0xFF00;
                        dwBBitMask = 0xFF;
                        dwABitMask = -16777216;
                        Bitmap bitmap = BGRA32ToBitmap(BGRA32);
                        bitmap.RotateFlip(rotateFlipType);
                        bitmap.Save(fileName, ImageFormat.Png);
                    }
                    break;

                case TextureFormat.ARGB32://test pass
                    {
                        /*dwFlags2 = 0x41;
                        dwRGBBitCount = 0x20;
                        dwRBitMask = 0xFF00;
                        dwGBitMask = 0xFF0000;
                        dwBBitMask = -16777216;
                        dwABitMask = 0xFF;*/

                        //转BGRA32
                        //var BGRA32 = new byte[imageContent.Length];
                        //for (var i = 0; i < imageContent.Length; i += 4)
                        //{
                        //    BGRA32[i] = imageContent[i + 3];
                        //    BGRA32[i + 1] = imageContent[i + 2];
                        //    BGRA32[i + 2] = imageContent[i + 1];
                        //    BGRA32[i + 3] = imageContent[i + 0];
                        //}
                        //imageContent = BGRA32;
                        //dwFlags2 = 0x41;
                        //dwRGBBitCount = 0x20;
                        //dwRBitMask = 0xFF0000;
                        //dwGBitMask = 0xFF00;
                        //dwBBitMask = 0xFF;
                        //dwABitMask = -16777216;
                        //Bitmap bitmap = BGRA32ToBitmap();
                        //bitmap.Save(fileName, ImageFormat.Png);


                        for(int iter=0;iter<imageCount;iter++)
                        {
                            var BGRA32 = new byte[imageSize];
                            for (int i = iter * imageSize; i <(iter+1)* imageSize; i += 4)
                            {
                                BGRA32[i - iter * imageSize] = imageContent[i + 3];
                                BGRA32[i + 1 - iter * imageSize] = imageContent[i + 2];
                                BGRA32[i + 2 - iter * imageSize] = imageContent[i + 1];
                                BGRA32[i + 3 - iter * imageSize] = imageContent[i + 0];
                            }
                            dwFlags2 = 0x41;
                            dwRGBBitCount = 0x20;
                            dwRBitMask = 0xFF0000;
                            dwGBitMask = 0xFF00;
                            dwBBitMask = 0xFF;
                            dwABitMask = -16777216;
                            Bitmap bitmap = BGRA32ToBitmap(BGRA32);
                            string fileNameOutput = fileName;
                            if(imageCount!=1)
                            {
                                string hzm = fileNameOutput.Substring(fileNameOutput.LastIndexOf('.'));
                                string pref = fileNameOutput.Substring(0, fileNameOutput.LastIndexOf('.'));
                                fileNameOutput = pref + "_" + iter.ToString() + hzm;
                            }
                            bitmap.RotateFlip(rotateFlipType);
                            bitmap.Save(fileNameOutput, ImageFormat.Png);
                        }
                        break;
                    }
                case TextureFormat.DXT1: //test pass
                    {

                        if (this.mipMap) { dwPitchOrLinearSize = this.textureWidth * this.textureHeight / 2; }
                        dwFlags2 = 0x4;
                        dwFourCC = 0x31545844;
                        dwRGBBitCount = 0x0;
                        dwRBitMask = 0x0;
                        dwGBitMask = 0x0;
                        dwBBitMask = 0x0;
                        dwABitMask = 0x0;

                        q_format = QFORMAT.Q_FORMAT_S3TC_DXT1_RGB;

                        Bitmap bitmap  = TextureConverter();
                        bitmap.RotateFlip(rotateFlipType);
                        bitmap.Save(fileName, ImageFormat.Png);
                    }
                    break;
                case TextureFormat.DXT5: //test pass
                    {
                        if (mipMap) { dwPitchOrLinearSize = textureWidth * textureHeight / 2; }
                        dwFlags2 = 0x4;
                        dwFourCC = 0x35545844;
                        dwRGBBitCount = 0x0;
                        dwRBitMask = 0x0;
                        dwGBitMask = 0x0;
                        dwBBitMask = 0x0;
                        dwABitMask = 0x0;

                        q_format = QFORMAT.Q_FORMAT_S3TC_DXT5_RGBA;
                        Bitmap bitmap = TextureConverter();
                        bitmap.RotateFlip(rotateFlipType);
                        bitmap.Save(fileName, ImageFormat.Png);
                    }
                    break;
            }
        }
        private Bitmap TextureConverter()
        {
            var imageBuff = new byte[TextureWidth * textureHeight * 4];
            var gch = GCHandle.Alloc(imageBuff, GCHandleType.Pinned);
            var imagePtr = gch.AddrOfPinnedObject();
            var fixAlpha = glBaseInternalFormat == KTXHeader.GL_RED || glBaseInternalFormat == KTXHeader.GL_RG;
            if (!Ponvert(imageContent, this.ImageSize, TextureWidth, textureHeight, (int)q_format, fixAlpha, imagePtr))
            {
                gch.Free();
                return null;
            }
            var bitmap = new Bitmap(TextureWidth, textureHeight, TextureWidth * 4, PixelFormat.Format32bppArgb, imagePtr);
            gch.Free();
            return bitmap;
        }


        private Bitmap BGRA32ToBitmap(byte[] imageContentX)
        {
            var hObject = GCHandle.Alloc(imageContentX, GCHandleType.Pinned);
            var pObject = hObject.AddrOfPinnedObject();
            var bitmap = new Bitmap(this.textureWidth, this.textureHeight, textureWidth * 4, PixelFormat.Format32bppArgb, pObject);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            hObject.Free();
            return bitmap;
        }
    }
}
