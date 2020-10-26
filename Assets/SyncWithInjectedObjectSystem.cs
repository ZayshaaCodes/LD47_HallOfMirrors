using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class SyncWithInjectedObjectSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        int i = 0;
        Entities.WithNone<OffsetSyncWithTraget>().WithoutBurst().ForEach((Transform xform, ref LocalToWorld ltw) =>
        {
            ltw.Value = xform.localToWorldMatrix;
            i++;
        }).Run();
        Debug.Log(i);
        

        return inputDeps;
    }
}