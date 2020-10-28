using HomDots.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace HomDots.Systems
{
    public class SyncWithInjectedObjectSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            int i = 0;
            Entities.WithNone<OffsetSyncWithTragetData>().WithoutBurst().ForEach((Transform xform, ref LocalToWorld ltw, ref Translation pos) =>
            {
                ltw.Value = xform.localToWorldMatrix;
                pos.Value = xform.position;
                i++;
            }).Run();
// k        Debug.Log(i);
        

            return inputDeps;
        }
    }
}