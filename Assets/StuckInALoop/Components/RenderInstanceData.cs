using Unity.Entities;
using Unity.Mathematics;

namespace StuckInALoop.Components
{
    public struct RenderInstanceData : IComponentData
    {
        public Entity source;
        public float3 offset;
    }
}