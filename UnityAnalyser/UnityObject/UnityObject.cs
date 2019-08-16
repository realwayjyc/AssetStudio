using System;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class UnityObject
    {
        public int ByteStart
        {
            get { return objectInfo.ByteStart; }
        }

        public int ByteSize
        {
            get { return objectInfo.ByteSize; }
        }

        public int TypeID
        {
            get { return objectInfo.TypeID; }
        }

        public Int16 ClassID
        {
            get { return objectInfo.ClassID; }
        }

        public ClassIDType ClassIDType
        {
            get
            {
                return objectInfo.ClassIDType;
            }
        }

        public string ClassIDTypeString
        {
            get
            {
                int intValue = (int)ClassIDType;
                if (intValue < 0)
                {
                    return "ScriptRef (" + intValue.ToString() + ")";
                }
                return (ClassIDType).ToString();
            }
        }

        public bool IsScriptRef
        {
            get
            {
                int intValue = (int)ClassIDType;
                return intValue < 0;
            }
        }

        public UInt16 IsDestroyed
        {
            get { return objectInfo.IsDestroyed; }
        }

        public int DebugLineStart
        {
            get { return objectInfo.DebugLineStart; }
        }

        public int Id
        {
            get { return objectInfo.Id; }
        }

        protected ObjectInfo objectInfo;

        public ObjectInfo ObjectInfo
        {
            get { return objectInfo; }
            set { objectInfo = value; }
        }

        public UnityFile UnityFile
        {
            get
            {
                return objectInfo.UnityFile;
            }
        }

        public int OffsetInFile
        {
            get
            {
                if (UnityFile is AssetsFile)
                {
                    return (UnityFile as AssetsFile).ObjectsOffset + ByteStart;
                }
                return 0x7FFFFFFF;
            }
        }

        /// <summary>
        /// 当在Datagrid上单击Component时，显示该控件的内容
        /// </summary>
        protected UserControl objectInfoPanel;

        public UserControl ObjectInfoPanel
        {
            get { return objectInfoPanel; }
        }

        public Boolean Active
        {
            get
            {
                if (this as GameObject != null)
                {
                    return (this as GameObject).IsActive;
                }
                else if (this as ScriptRef != null)
                {
                    return (this as ScriptRef).IsActive;
                }
                return true;
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    if (this.ClassIDType == ClassIDType.CLASS_GameObject)
                    {
                        return (this as GameObject).GName;
                    }
                    else if (this.ClassIDType <= ClassIDType.CLASS_Undefined && (this as ScriptRef).IsScriptableObject == true)
                    {
                        string objectName = (this as ScriptRef).ScriptableObjectName;
                        if (string.IsNullOrEmpty(objectName))
                        {
                            SerializedObjectIdentifier soi = (this as ScriptRef).ParentGameObjectIdentifier;
                            if (soi != null)
                            {
                                UnityObject unityObject = this.GetUnityObjectBySerializedObjectIdentifier(soi);
                                if (unityObject != null)
                                {
                                    return unityObject.Name;
                                }
                            }
                        }
                        else
                        {
                            return objectName;
                        }
                    }
                    else if (this is Component)
                    {
                        GameObject go = (this as Component).GetGameObject();
                        if (go != null)
                        {
                            return go.GName;
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_Texture2D)
                    {
                        return (this as Texture2D).TextureName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_Cubemap)
                    {
                        return (this as Cubemap).CubeMapName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_Shader)
                    {
                        return (this as Shader).ShaderName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_Material)
                    {
                        return (this as Material).MaterialName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_MonoScript)
                    {
                        return (this as MonoScript).ScriptName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_Sprite)
                    {
                        return (this as Sprite).SpriteName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_AudioClip)
                    {
                        return (this as AudioClip).AudioClipName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_AnimationClip)
                    {
                        return (this as AnimationClip).AnimationClipName;
                    }
                    else if (this.ClassIDType == ClassIDType.CLASS_AnimatorController)
                    {
                        return (this as AnimatorController).AnimatorControllerName;
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return "[ERROR]";
                }
            }
        }

        public int[] UnityFileVersion
        {
            get
            {
                return objectInfo.UnityFileVersion;
            }
        }

        ////////////////////////////////////  以下为GamgObject独有 //////////////////////////////////////

        public Int16 Tag
        {
            get
            {
                if (this.ClassIDType == ClassIDType.CLASS_GameObject)
                {
                    return (this as GameObject).GTag;
                }
                return -1;
            }
        }

        public int Layer
        {
            get
            {
                if (this.ClassIDType == ClassIDType.CLASS_GameObject)
                {
                    return (int)((this as GameObject).GLayer);
                }
                return -1;
            }
        }

        public int ParentID
        {
            get
            {
                try
                {
                    if (this.ClassIDType == ClassIDType.CLASS_GameObject)
                    {
                        int index = 0;
                        GameObject parent = (this as GameObject).GetParentGameObject(ref index);
                        if (parent != null)
                        {
                            return parent.Id;
                        }
                    }
                    return -1;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        public string ParentName
        {
            get
            {
                try
                {
                    if (this.ClassIDType == ClassIDType.CLASS_GameObject)
                    {
                        int index = -1;
                        GameObject parent = (this as GameObject).GetParentGameObject(ref index);
                        if (parent != null)
                        {
                            return parent.Name;
                        }
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public static UnityObject CreateUnityObject(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            ObjectReader objectReader = new ObjectReader(objectInfo, content, objectOffset);

            UnityObject ret = null;
            if ((int)objectInfo.ClassIDType < 0)
            {
                ret = ScriptRef.Create(objectInfo, content, objectOffset);
            }
            else
            {
                switch (objectInfo.ClassIDType)
                {
                    case ClassIDType.CLASS_GameObject:
                        ret = GameObject.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Transform:
                        ret = Transform.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_MonoScript:
                        ret = MonoScript.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Camera:
                        ret = Camera.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_TagManager:
                        ret = TagManager.Create(objectInfo, content, objectOffset);
                        objectInfo.UnityFile.Analyzer.TagManager = ret as TagManager;
                        break;

                    case ClassIDType.CLASS_Canvas:
                        ret = Canvas.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_RectTransform:
                        ret = RectTransform.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_CanvasRenderer:
                        ret = CanvasRenderer.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Texture2D:
                        {
                            int index = 0;
                            ret = Texture2D.Create(objectInfo, content, objectOffset, ref index);
                        }
                        break;

                    case ClassIDType.CLASS_BoxCollider:
                        {
                            ret = BoxCollider.Create(objectInfo, content, objectOffset);
                            break;
                        }
                    case ClassIDType.CLASS_Rigidbody:
                        {
                            ret = RigidBody.Create(objectInfo, content, objectOffset);
                            break;
                        }
                    case ClassIDType.CLASS_Cubemap:
                        {
                            ret = Cubemap.Create(objectInfo, content, objectOffset);
                        }
                        break;

                    case ClassIDType.CLASS_Shader:
                        ret = Shader.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Material:
                        ret = Material.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Sprite:
                        ret = Sprite.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_SpriteRenderer:
                        ret = SpriteRenderer.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_ParticleAnimator:
                        ret = ParticleAnimator.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_EllipsoidParticleEmitter:
                        ret = EllipsoidParticleEmitter.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_ParticleRenderer:
                        ret = ParticleRenderer.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Mesh:
                        ret = Mesh.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_PlayerSettings:
                        ret = PlayerSettings.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_AudioSource:
                        ret = AudioSource.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_AudioClip:
                        ret = AudioClip.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Animator:
                        ret = Animator.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_AnimationClip:
                        ret = AnimationClip.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_AnimatorController:
                        ret = AnimatorController.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_MeshRenderer:
                        ret = MeshRenderer.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_SkinnedMeshRenderer:
                        ret = SkinnedMeshRenderer.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_MeshFilter:
                        ret = MeshFilter.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_Light:
                        ret = Light.Create(objectInfo, content, objectOffset);
                        break;

                    case ClassIDType.CLASS_RenderSettings:
                        ret = RenderSettings.Create(objectInfo, content, objectOffset);
                        break;

                    default:
                        ret = new UnityObject();
                        break;
                }
            }
            if (ret == null)
            {
                ret = new UnityObject();
            }
            if (ret != null)
            {
                ret.objectInfo = objectInfo;
            }
            return ret;
        }

        public UnityObject GetUnityObjectBySerializedObjectIdentifier(SerializedObjectIdentifier identifier)
        {
            UnityFile searchFile = null;
            if (identifier.serializedFileIndex == 0)
            {
                //从UnityObject所在的文件读取
                searchFile = this.UnityFile;
            }
            else
            {
                searchFile = (this.UnityFile as AssetsFile).GetSerializedUnityFileByFileIndex(identifier.serializedFileIndex);
            }
            if (searchFile == null) return null;
            return (searchFile as AssetsFile).GetUnityObjectByIdentifier(identifier.identifierInFile);
        }

        public virtual UserControl CreateObjectInfoPanel()
        {
            return null;
        }
    }
}