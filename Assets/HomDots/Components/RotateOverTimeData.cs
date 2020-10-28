using Unity.Entities;
using Unity.Mathematics;

namespace HomDots.Components
{
    [GenerateAuthoringComponent]
    public struct RotateOverTimeData : IComponentData
    {
        public float3 angularVelocity;
    }
}