using HomDots.Tags;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace HomDots.Components
{
    public class WrapToGridBoundsSystem : JobComponentSystem
    {
        private WorldGridData _worldGridData;

        protected override void OnStartRunning()
        {
            _worldGridData = GetSingleton<WorldGridData>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            NativeList<Entity> loopedEntitiesArr = new NativeList<Entity>(500, Allocator.TempJob);

            NativeList<Entity>.ParallelWriter loopedEntitiesPar = loopedEntitiesArr.AsParallelWriter();
            WorldGridData                     gridData          = _worldGridData;

            Entities.WithAll<SyncInjectedTransformTag>()
                    .ForEach(
                         (Entity entity, ref Translation position) =>
                         {
                             float4x4 worldToBounds = math.mul(float4x4.Translate(new float3(.5f)), float4x4.Scale(1 / gridData.spacing));
                             float4x4 boundsToWorld = math.inverse(worldToBounds);

                             float3 boundsLocalPos = math.transform(worldToBounds, position.Value);

                             float3 floor        = math.floor(boundsLocalPos);
                             float3 wrappedValue = boundsLocalPos - floor;

                             position.Value = math.transform(boundsToWorld, wrappedValue);

                             //if it looped, add it to the list
                             if (math.length(floor) > 0.01f) loopedEntitiesPar.AddNoResize(entity);
                         }).Schedule(inputDeps).Complete();

            var translationDatas = GetComponentDataFromEntity<Translation>(true);
            Job.WithoutBurst().WithReadOnly(loopedEntitiesArr).WithReadOnly(translationDatas).WithCode(() =>
            {
                for (var index = 0; index < loopedEntitiesArr.Length; index++)
                {
                    Entity         loopedEntity    = loopedEntitiesArr[index];
                    Transform      objectTransform = EntityManager.GetComponentObject<Transform>(loopedEntity);
                    ILoopBehaviour loopBehaviour   = objectTransform.gameObject.GetComponent<ILoopBehaviour>();
                    float3         newPosition     = translationDatas[loopedEntity].Value;

                    loopBehaviour?.Loop(newPosition - (float3) objectTransform.position);
                    objectTransform.position = newPosition;
                }
            }).Run();

            return inputDeps;
        }
    }
}