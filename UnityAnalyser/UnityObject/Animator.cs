using System;
using System.Windows.Controls;

namespace UnityAnalyzer
{

    public enum CullingMode
    {
        AlwaysAnimate,
        BasedOnRenderers
    }

    public enum UpdateMode
    {
        Normal,
        AnimatePhysics,
        UnscaledTime
    }

    public class Animator : Component
    {

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        private SerializedObjectIdentifier avatar;
        public SerializedObjectIdentifier Avatar
        {
            get { return avatar; }
        }

        private SerializedObjectIdentifier controller;
        public SerializedObjectIdentifier Controller
        {
            get { return controller; }
        }

        private CullingMode cullingMode;
        public CullingMode CullingMode
        {
            get { return cullingMode; }
        }

        private UpdateMode updateMode;
        public UpdateMode UpdateMode
        {
            get { return updateMode; }
        }

        private bool applyRootMotion;
        public bool ApplyRootMotion
        {
            get { return applyRootMotion; }
        }

        private bool hasTransformHierarchy;
        public bool HasTransformHierarchy
        {
            get { return hasTransformHierarchy; }
        }

        private bool allowConstantClipSamplingOptimization;
        public bool AllowConstantClipSamplingOptimization
        {
            get { return allowConstantClipSamplingOptimization; }
        }


         public override UserControl CreateGameObjectComponentInfoControl()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AnimatorPanel panel = new AnimatorPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public AnimatorController GetAnimatorController()
         {
             return this.GetUnityObjectBySerializedObjectIdentifier(this.controller) as AnimatorController;
         }

        public static Animator Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            Animator ret = new Animator();


            int serializedFileIndex = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart);
            int identifier = BitConverter.ToInt32(content, objectOffset + objectInfo.ByteStart + 4);
            ret.parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            int index = 0;

            index = objectOffset + objectInfo.ByteStart + 8;
            ret.isEnabled = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);

            int serializedFileIndex_ = BitConverter.ToInt32(content, index);
            index += 4;

            int identifierInFile_ = BitConverter.ToInt32(content, index);
            index += 4;

            ret.avatar = new SerializedObjectIdentifier(serializedFileIndex_, identifierInFile_);

            serializedFileIndex_ = BitConverter.ToInt32(content, index);
            index += 4;

            identifierInFile_ = BitConverter.ToInt32(content, index);
            index += 4;

            ret.controller = new SerializedObjectIdentifier(serializedFileIndex_, identifierInFile_);

            ret.cullingMode =(CullingMode) BitConverter.ToInt32(content, index);
            index += 4;


            ret.updateMode = (UpdateMode)BitConverter.ToInt32(content, index);
            index += 4;


           ret.applyRootMotion=(content[index++]!=0);
           index += Util.GetAlignCount(index, objectOffset);

           ret.hasTransformHierarchy = (content[index++] != 0);
           ret.allowConstantClipSamplingOptimization = (content[index++] != 0);



            return ret;
        }
    }
}
