using HomDots.Components;
using HomDots.Tags;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace HomDots.Systems
{
    public class GridCloneSystem : JobComponentSystem
    {
        protected override void OnStartRunning()
        {
            BeginInitializationEntityCommandBufferSystem commandBufferSystem =
                World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

            var ecb = commandBufferSystem.CreateCommandBuffer().AsParallelWriter();

            WorldGridData worldGridData = GetSingleton<WorldGridData>();

            var left = -worldGridData.counts / 2;

            var jobHandle = Entities.WithAll<GridCloneTag>().WithNone<SyncInjectedTransformTag>().ForEach(
                (int nativeThreadIndex, Entity e) =>
                {
                    for (int k = 0; k < worldGridData.counts.z; k++)
                    for (int j = 0; j < worldGridData.counts.y; j++)
                    for (int i = 0; i < worldGridData.counts.x; i++)
                    {
                        var indexPosition = new int3(i, j, k);
                        if (indexPosition.Equals(int3.zero))
                            continue;

                        var worldPos = (left + indexPosition) * worldGridData.spacing;

                        Entity clone = ecb.Instantiate(nativeThreadIndex, e);
                        ecb.SetComponent(nativeThreadIndex, clone,
                                         new Translation() {Value = worldPos});

                        ecb.AddComponent(nativeThreadIndex, clone,
                                         new OffsetSyncWithTragetData()
                                         {
                                             target = e,
                                             offset = worldPos
                                         });
                    }
                }).Schedule(default);

            Entities.WithAll<GridCloneTag, SyncInjectedTransformTag>().ForEach((int nativeThreadIndex, Entity e) =>
            {
                for (int k = 0; k < worldGridData.counts.z; k++)
                for (int j = 0; j < worldGridData.counts.y; j++)
                for (int i = 0; i < worldGridData.counts.x; i++)
                {
                    var indexPosition = new int3(i, j, k);
                    if (indexPosition.Equals(int3.zero))
                        continue;

                    Entity clone = ecb.Instantiate(nativeThreadIndex, e);
                    ecb.RemoveComponent<SyncInjectedTransformTag>(nativeThreadIndex, clone);

                    var worldPos = (left + indexPosition) * worldGridData.spacing;
                    ecb.SetComponent(nativeThreadIndex, clone,
                                     new Translation() {Value = worldPos});

                    ecb.AddComponent(nativeThreadIndex, clone,
                                     new OffsetSyncWithTragetData()
                                     {
                                         target = e,
                                         offset = worldPos
                                     });
                }
            }).Schedule(jobHandle).Complete();

            Entities.WithAll<OffsetSyncWithTragetData, Transform>().WithoutBurst().ForEach(
                (int nativeThreadIndex, Entity e) =>
                {
                    ecb.RemoveComponent<Transform>(nativeThreadIndex, e);
                }).Run();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            return inputDeps;
        }
    }
}