using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components
{
    [GenerateAuthoringComponent]
    public struct RotateOverTimeData : IComponentData
    {
        public float3 angularVelocity;
    }
}