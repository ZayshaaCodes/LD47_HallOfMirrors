using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace StuckInALoop.Systems
{
    public class StaticRenderChunkData
    {
        public          int             count;
        public readonly List<Matrix4x4> spriteInstances;
        public readonly Matrix4x4[][]   transformBlocks;

        public StaticRenderChunkData()
        {
            spriteInstances = new List<Matrix4x4>();
            transformBlocks = new Matrix4x4[32][];
            for (var i = 0; i < transformBlocks.Length; i++)
                transformBlocks[i] = new Matrix4x4[1023];
            count = 0;
        }

        public void SetRenderData(float3[] offsets, int offsetCount)
        {
            var c = 0;
            for (var i = 0; i < offsetCount; i++)
                foreach (var mtx in spriteInstances)
                {
                    transformBlocks[c / 1023][c % 1023] = Matrix4x4.Translate(offsets[i]) * mtx;
                    c++;
                }

            count = c;
        }
        public void SetRenderData(NativeArray<float3> offsets, int offsetCount)
        {
            var c = 0;
            for (var i = 0; i < offsetCount; i++)
                foreach (var mtx in spriteInstances)
                {
                    transformBlocks[c / 1023][c % 1023] = Matrix4x4.Translate(offsets[i]) * mtx;
                    c++;
                }

            count = c;
        }
    }
}