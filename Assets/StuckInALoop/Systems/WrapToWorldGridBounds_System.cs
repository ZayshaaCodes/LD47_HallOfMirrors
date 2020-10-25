using StuckInALoop.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace StuckInALoop.Systems
{
    [ExecuteAlways] [UpdateAfter(typeof(PresentationSystemGroup))]
    public class WrapToWorldGridBoundsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var grid = GetSingleton<WorldGridData>();

            var moveDeltas = new NativeHashMap<Entity, float3>(100, Allocator.TempJob);
            var writer     = moveDeltas.AsParallelWriter();

            // Return;
            Entities.ForEach((Entity e, WrappingComponentTag wrap, ref Translation position) =>
            {
                //inverse lerp position to 0-1 range.
                float3 curPos = position.Value;

                var halfBoundsSize = grid.spacing / 2;
                var boundsPos      = math.unlerp(-halfBoundsSize, halfBoundsSize, curPos) + new float3(1);

                var minus = (int3) boundsPos;

                //lerp back to bounds position
                var newPos = math.lerp(-halfBoundsSize, halfBoundsSize, boundsPos - minus);
                position.Value = newPos;

                var delta = newPos - curPos;

                if (math.length(delta) > .01f)
                    writer.TryAdd(e, delta);
            }).Schedule(default).Complete();

            // Move and update any enteties that have gone out of bounds
            Entities.WithoutBurst().ForEach((Entity e, WrappingComponentTag wrapComp, Transform xform) =>
            {
                if (!moveDeltas.ContainsKey(e)) return;
                
                Vector3 moveDelta = moveDeltas[e];
                xform.position = xform.position + moveDelta;

                var loopBehaviour = xform.GetComponent<ILoopBehaviour>();
                if (loopBehaviour != null)
                {
                    loopBehaviour.Loop(moveDelta);
                }
            }).Run();
        }
    }
}