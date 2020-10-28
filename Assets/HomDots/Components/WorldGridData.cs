using Unity.Entities;
using Unity.Mathematics;

namespace HomDots.Components
{
    public struct WorldGridData : IComponentData
    {
        public float3 spacing;
        public int3   counts;
    }
}