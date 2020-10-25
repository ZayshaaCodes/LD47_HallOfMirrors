using Unity.Entities;
using Unity.Mathematics;

namespace StuckInALoop.Components
{
    public struct WorldGridData : IComponentData
    {
        public float3 spacing;
    }
}