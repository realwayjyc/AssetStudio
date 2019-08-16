using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAnalyzer
{
    public class ParticleEmitter:Component
    {
        protected bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
        }

        protected bool emit;
        public bool Emit
        {
            get { return emit; }
        }

        protected float minSize;
        public float MinSize
        {
            get { return minSize; }
        }


        protected float maxSize;
        public float MaxSize
        {
            get { return maxSize; }
        }


        protected float minEnergy;
        public float MinEnergy
        {
            get { return minEnergy; }
        }

        protected float maxEnergy;
        public float MaxEnergy
        {
            get { return maxEnergy; }
        }

        protected float minEmission;
        public float MinEmission
        {
            get { return minEmission; }
            set { minEmission = value; }
        }

        protected float maxEmission;
        public float MaxEmission
        {
            get { return maxEmission; }
            set { maxEmission = value; }
        }

        protected Vector3F worldVelocity;
        public Vector3F WorldVelocity
        {
            get { return worldVelocity; }
        }

        protected Vector3F localVelocity;
        public Vector3F LocalVelocity
        {
            get { return localVelocity; }
        }

        protected Vector3F rndVelocity;
        public Vector3F RndVelocity
        {
            get { return rndVelocity; }
        }

        protected float emitterVelocityScale;
        public float EmitterVelocityScale
        {
            get { return emitterVelocityScale; }
        }

        protected Vector3F tangentVelocity;
        public Vector3F TangentVelocity
        {
            get { return tangentVelocity; }
        }

        protected float angularVelocity;
        public float AngularVelocity
        {
            get { return angularVelocity; }
        }

        protected float rndAngularVelocity;
        public float RndAngularVelocity
        {
            get { return rndAngularVelocity; }
        }

        protected bool rndInitialRotations;
        public bool RndInitialRotations
        {
            get { return rndInitialRotations; }
        }

        protected bool useWorldSpace;
        public bool UseWorldSpace
        {
            get { return useWorldSpace; }
            set { useWorldSpace = value; }
        }

        private bool oneShot;
        public bool OneShot
        {
            get { return oneShot; }
        }

        protected int CreateParticleEmitter(ObjectInfo objectInfo, byte[] content, int objectOffset, int index)
        {
            int serializedFileIndex = BitConverter.ToInt32(content, index);
            index += 4;
            int identifier = BitConverter.ToInt32(content, index);
            index += 4;
            parentGameObjectIdentifier = new SerializedObjectIdentifier(serializedFileIndex, identifier);

            isEnabled = (content[index++] == 1);
            emit = (content[index++] == 1);

            index += Util.GetAlignCount(index, objectOffset);


            minSize = BitConverter.ToSingle(content, index); index += 4;
            maxSize = BitConverter.ToSingle(content, index); index += 4;

            minEnergy = BitConverter.ToSingle(content, index); index += 4;
            maxEnergy = BitConverter.ToSingle(content, index); index += 4;

            minEmission = BitConverter.ToSingle(content, index); index += 4;
            MaxEmission = BitConverter.ToSingle(content, index); index += 4;


            worldVelocity = Util.ReadNextVector3f(content, ref index);
            localVelocity = Util.ReadNextVector3f(content, ref index);
            rndVelocity = Util.ReadNextVector3f(content, ref index);

            emitterVelocityScale = BitConverter.ToSingle(content, index); index += 4;

            tangentVelocity = Util.ReadNextVector3f(content, ref index);

            angularVelocity = BitConverter.ToSingle(content, index); index += 4;

            rndAngularVelocity = BitConverter.ToSingle(content, index); index += 4;

            rndInitialRotations = (content[index++] == 1);
            useWorldSpace = (content[index++] == 1);
            oneShot = (content[index++] == 1);
            return index;
        }
    }
}
