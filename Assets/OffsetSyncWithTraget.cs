using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct OffsetSyncWithTraget : IComponentData
{
    public Entity target;
    public float3 offset;
}