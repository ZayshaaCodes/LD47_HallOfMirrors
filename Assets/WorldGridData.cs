using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct WorldGridData : IComponentData
{
    public float3 spacing;
    public int3   counts;
}