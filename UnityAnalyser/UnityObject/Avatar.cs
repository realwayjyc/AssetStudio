using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class Node
    {
        public int ParentId { get; }
        public int AxesId { get; }

        public Node(ObjectReader reader)
        {
            ParentId = reader.ReadInt32();
            AxesId = reader.ReadInt32();
        }
    }

    public class Limit
    {
        public object Min { get; }
        public object Max { get; }

        public Limit(ObjectReader reader)
        {
            var version = reader.Version;
            if (version[0] > 5 || (version[0] == 5 && version[1] >= 4))//5.4 and up
            {
                Min = reader.ReadVector3();
                Max = reader.ReadVector3();
            }
            else
            {
                Min = reader.ReadVector4();
                Max = reader.ReadVector4();
            }
        }
    }

    public class Axes
    {
        public Vector4F PreQ { get; }
        public Vector4F PostQ { get; }
        public object Sgn { get; }
        public Limit Limit { get; }
        public float Length { get; }
        public uint Type { get; }

        public Axes(ObjectReader reader)
        {
            var version = reader.Version;
            PreQ = reader.ReadVector4();
            PostQ = reader.ReadVector4();
            if (version[0] > 5 || (version[0] == 5 && version[1] >= 4)) //5.4 and up
            {
                Sgn = reader.ReadVector3();
            }
            else
            {
                Sgn = reader.ReadVector4();
            }
            Limit = new Limit(reader);
            Length = reader.ReadSingle();
            Type = reader.ReadUInt32();
        }
    }

    public class Skeleton
    {
        public Node[] Node;
        public uint[] Id;
        public Axes[] AxesArray;


        public Skeleton(ObjectReader reader)
        {
            int numNodes = reader.ReadInt32();
            Node = new Node[numNodes];
            for (int i = 0; i < numNodes; i++)
            {
                Node[i] = new Node(reader);
            }

            Id = reader.ReadUInt32Array();

            int numAxes = reader.ReadInt32();
            AxesArray = new Axes[numAxes];
            for (int i = 0; i < numAxes; i++)
            {
                AxesArray[i] = new Axes(reader);
            }
        }
    }

    public class xform
    {
        public Vector3F T { get; }
        public Quaternion Q { get; }
        public Vector3F S { get; }

        public xform(ObjectReader reader)
        {
            var version = reader.Version;
            T = version[0] > 5 || (version[0] == 5 && version[1] >= 4) ? reader.ReadVector3() : reader.ReadVector4AsVector3();//5.4 and up
            Q = reader.ReadQuaternion();
            S = version[0] > 5 || (version[0] == 5 && version[1] >= 4) ? reader.ReadVector3() : reader.ReadVector4AsVector3();//5.4 and up
        }
    }

    public class SkeletonPose
    {
        public xform[] X { get; }

        public SkeletonPose(ObjectReader reader)
        {
            int num = reader.ReadInt32();
            X = new xform[num];
            for (int i = 0; i < num; i++)
            {
                X[i] = new xform(reader);
            }
        }
    }

    public class Hand
    {
        public int[] HandBoneIndex { get; }

        public Hand(ObjectReader reader)
        {
            HandBoneIndex = reader.ReadInt32Array();
        }
    }

    public class Handle
    {
        public xform X { get; }
        public uint ParentHumanIndex { get; }
        public uint Id { get; }

        public Handle(ObjectReader reader)
        {
            X = new xform(reader);
            ParentHumanIndex = reader.ReadUInt32();
            Id = reader.ReadUInt32();
        }
    }

    public class Collider
    {
        public xform X { get; }
        public uint Type { get; }
        public uint XMotionType { get; }
        public uint YMotionType { get; }
        public uint ZMotionType { get; }
        public float MinLimitX { get; }
        public float MaxLimitX { get; }
        public float MaxLimitY { get; }
        public float MaxLimitZ { get; }

        public Collider(ObjectReader reader)
        {
            X = new xform(reader);
            Type = reader.ReadUInt32();
            XMotionType = reader.ReadUInt32();
            YMotionType = reader.ReadUInt32();
            ZMotionType = reader.ReadUInt32();
            MinLimitX = reader.ReadSingle();
            MaxLimitX = reader.ReadSingle();
            MaxLimitY = reader.ReadSingle();
            MaxLimitZ = reader.ReadSingle();
        }
    }

    public class Human
    {
        public xform RootX { get; }
        public Skeleton Skeleton { get; }
        public SkeletonPose SkeletonPose { get; }
        public Hand LeftHand { get; }
        public Hand RightHand { get; }
        public Handle[] Handles { get; }
        public Collider[] ColliderArray { get; }
        public int[] HumanBoneIndex { get; }
        public float[] HumanBoneMass { get; }
        public int[] ColliderIndex { get; }
        public float Scale { get; }
        public float ArmTwist { get; }
        public float ForeArmTwist { get; }
        public float UpperLegTwist { get; }
        public float LegTwist { get; }
        public float ArmStretch { get; }
        public float LegStretch { get; }
        public float FeetSpacing { get; }
        public bool HasLeftHand { get; }
        public bool HasRightHand { get; }
        public bool HasTDoF { get; }

        public Human(ObjectReader reader)
        {
            var version = reader.Version;
            RootX = new xform(reader);
            Skeleton = new Skeleton(reader);
            SkeletonPose = new SkeletonPose(reader);
            LeftHand = new Hand(reader);
            RightHand = new Hand(reader);

            if (version[0] < 2018 || (version[0] == 2018 && version[1] < 2)) //2018.2 down
            {
                int numHandles = reader.ReadInt32();
                Handles = new Handle[numHandles];
                for (int i = 0; i < numHandles; i++)
                {
                    Handles[i] = new Handle(reader);
                }

                int numCollider = reader.ReadInt32();
                ColliderArray = new Collider[numCollider];
                for (int i = 0; i < numCollider; i++)
                {
                    ColliderArray[i] = new Collider(reader);
                }
            }

            HumanBoneIndex = reader.ReadInt32Array();

            HumanBoneMass = reader.ReadSingleArray();

            if (version[0] < 2018 || (version[0] == 2018 && version[1] < 2)) //2018.2 down
            {
                ColliderIndex = reader.ReadInt32Array();
            }

            Scale = reader.ReadSingle();
            ArmTwist = reader.ReadSingle();
            ForeArmTwist = reader.ReadSingle();
            UpperLegTwist = reader.ReadSingle();
            LegTwist = reader.ReadSingle();
            ArmStretch = reader.ReadSingle();
            LegStretch = reader.ReadSingle();
            FeetSpacing = reader.ReadSingle();
            HasLeftHand = reader.ReadBoolean();
            HasRightHand = reader.ReadBoolean();
            if (version[0] > 5 || (version[0] == 5 && version[1] >= 2)) //5.2 and up
            {
                HasTDoF = reader.ReadBoolean();
            }
            reader.AlignStream();
        }
    }

    public class AvatarConstant
    {
        public Skeleton AvatarSkeleton { get; }
        public SkeletonPose AvatarSkeletonPose { get; }
        public SkeletonPose DefaultPose { get; }
        public uint[] SkeletonNameIdArray { get; }
        public Human Human { get; }
        public int[] HumanSkeletonIndexArray { get; }
        public int[] HumanSkeletonReverseIndexArray { get; }
        public int RootMotionBoneIndex { get; }
        public xform RootMotionBoneX { get; }
        public Skeleton RootMotionSkeleton { get; }
        public SkeletonPose RootMotionSkeletonPose { get; }
        public int[] RootMotionSkeletonIndexArray { get; }

        public AvatarConstant(ObjectReader reader)
        {
            var version = reader.Version;
            AvatarSkeleton = new Skeleton(reader);
            AvatarSkeletonPose = new SkeletonPose(reader);

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 3)) //4.3 and up
            {
                DefaultPose = new SkeletonPose(reader);

                SkeletonNameIdArray = reader.ReadUInt32Array();
            }

            Human = new Human(reader);

            HumanSkeletonIndexArray = reader.ReadInt32Array();

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 3)) //4.3 and up
            {
                HumanSkeletonReverseIndexArray = reader.ReadInt32Array();
            }

            RootMotionBoneIndex = reader.ReadInt32();
            RootMotionBoneX = new xform(reader);

            if (version[0] > 4 || (version[0] == 4 && version[1] >= 3)) //4.3 and up
            {
                RootMotionSkeleton = new Skeleton(reader);
                RootMotionSkeletonPose = new SkeletonPose(reader);
                RootMotionSkeletonIndexArray = reader.ReadInt32Array();
            }
        }
    }

    public class Avatar:UnityObject
    {
        public string AvatarName { get; private set; }

        public uint AvatarSize { get; private set; }
        public AvatarConstant AvatarConstant { get; private set; }
        public KeyValuePair<uint, string>[] Tos { get; private set; }

        public static Avatar Create(ObjectReader reader)
        {
            Avatar ret = new Avatar
            {
                AvatarName = reader.ReadAlignedString(),
                AvatarSize = reader.ReadUInt32(),
                AvatarConstant = new AvatarConstant(reader)
            };

            int num = reader.ReadInt32();
            ret.Tos = new KeyValuePair<uint, string>[num];
            for (int i = 0; i < num; i++)
            {
                ret.Tos[i] = new KeyValuePair<uint, string>(reader.ReadUInt32(), reader.ReadAlignedString());
            }
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
                        AvatarPanel panel = new AvatarPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }
    }
}
