using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    [GenerateAuthoringComponent]
    public struct RotateOverTime : IComponentData
    {
        public float3 angularVelocity;
    }
}