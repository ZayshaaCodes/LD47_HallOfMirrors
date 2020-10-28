using Unity.Entities;
using Unity.Mathematics;

namespace HomDots.Components
{
    [GenerateAuthoringComponent]
    public struct OffsetSyncWithTragetData : IComponentData
    {
        public Entity target;
        public float3 offset;
    }
}