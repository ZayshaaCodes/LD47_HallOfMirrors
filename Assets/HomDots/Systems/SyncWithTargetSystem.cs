using HomDots.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace HomDots.Systems
{
    public class SyncWithTargetSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var ltwComponentDatas = GetComponentDataFromEntity<LocalToWorld>(true);
            inputDeps = Entities.WithReadOnly(ltwComponentDatas).ForEach(
                (Entity e, OffsetSyncWithTragetData syncData, ref LocalToWorld ltwData) =>
                {
                    ltwData.Value = math.mul(float4x4.Translate(syncData.offset), ltwComponentDatas[syncData.target].Value);
                }).Schedule(inputDeps);

            return inputDeps;
        }
    }
}