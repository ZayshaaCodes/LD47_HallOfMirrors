using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class SyncWithTargetSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ltwComponentDatas = GetComponentDataFromEntity<LocalToWorld>(true);
        inputDeps = Entities.WithReadOnly(ltwComponentDatas).ForEach(
            (Entity e, OffsetSyncWithTraget syncData, ref LocalToWorld ltwData) =>
            {
                ltwData.Value = math.mul(float4x4.Translate(syncData.offset), ltwComponentDatas[syncData.target].Value);
            }).Schedule(inputDeps);

        return inputDeps;
    }
}

public class GridCloneSystem : JobComponentSystem
{
    protected override void OnStartRunning()
    {
        BeginInitializationEntityCommandBufferSystem commandBufferSystem =
            World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

        var ecb = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        WorldGridData worldGridData = GetSingleton<WorldGridData>();

        var r = new Random(123);

        var total = worldGridData.counts.x * worldGridData.counts.y * worldGridData.counts.z;

        var jobHandle = Entities.WithAll<GridCloneTag>().WithNone<SyncWithInjectedObjectTag>().ForEach(
            (int nativeThreadIndex, Entity e) =>
            {
                for (int k = 0; k < worldGridData.counts.z; k++)
                for (int j = 0; j < worldGridData.counts.y; j++)
                for (int i = 0; i < worldGridData.counts.x; i++)
                {
                    var linPos        = i * j * k;
                    var indexPosition = new int3(i, j, k);
                    if (indexPosition.Equals(int3.zero))
                        continue;

                    Entity clone = ecb.Instantiate(nativeThreadIndex, e);
                    ecb.SetComponent(nativeThreadIndex, clone,
                                     new Translation() {Value = indexPosition * worldGridData.spacing});
                    // ecb.SetComponent(nativeThreadIndex, clone,
                                     // new TintColorData()
                                     // {
                                         // Value = math.unlerp(
                                             // new float4(.1f, .1f, 1, 1), new float4(.1f, 1, .1f, 1),
                                             // linPos/ (float)total - 1)
                                     // });

                    ecb.AddComponent(nativeThreadIndex, clone,
                                     new OffsetSyncWithTraget()
                                     {
                                         target = e,
                                         offset = indexPosition * worldGridData.spacing
                                     });
                }
            }).Schedule(default);

        Entities.WithAll<GridCloneTag, SyncWithInjectedObjectTag>().ForEach((int nativeThreadIndex, Entity e) =>
        {
            for (int k = 0; k < worldGridData.counts.z; k++)
            for (int j = 0; j < worldGridData.counts.y; j++)
            for (int i = 0; i < worldGridData.counts.x; i++)
            {
                var linPos = i * j * k;

                var indexPosition = new int3(i, j, k);
                if (indexPosition.Equals(int3.zero))
                    continue;

                Entity clone = ecb.Instantiate(nativeThreadIndex, e);
                ecb.RemoveComponent<SyncWithInjectedObjectTag>(nativeThreadIndex, clone);

                ecb.SetComponent(nativeThreadIndex, clone,
                                 new Translation() {Value = indexPosition * worldGridData.spacing});
                // ecb.SetComponent(nativeThreadIndex, clone,
                //                  new TintColorData()
                //                  {
                //                      Value = math.unlerp(
                //                          new float4(1, 0, 0, 1), new float4(0, 0, 1, 1),
                //                          linPos/ (float)total)
                //                  });

                ecb.AddComponent(nativeThreadIndex, clone,
                                 new OffsetSyncWithTraget()
                                 {
                                     target = e,
                                     offset = indexPosition * worldGridData.spacing
                                 });
            }
        }).Schedule(jobHandle).Complete();

        Entities.WithAll<OffsetSyncWithTraget, Transform>().WithoutBurst().ForEach((int nativeThreadIndex, Entity e) =>
        {
            ecb.RemoveComponent<Transform>(nativeThreadIndex, e);
        }).Run();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }
}