using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class SyncWithInjectedObjectSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.WithoutBurst().ForEach((Transform xform, OffsetSyncWithTraget offsetSyncData, ref LocalToWorld ltw ) =>
        {
            ltw.Value = Matrix4x4.Translate(offsetSyncData.offset) * xform.localToWorldMatrix;
        }).Run();
        

        return inputDeps;
    }
}