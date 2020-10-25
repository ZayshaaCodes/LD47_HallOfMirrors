using StuckInALoop.Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace StuckInALoop.Systems
{
    public class SyncTransformsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithoutBurst().ForEach(
                (Transform xform, SyncTransforms st, ref LocalToWorld ltw, ref Rotation rot, ref Translation pos) =>
                {
                    ltw.Value = xform.localToWorldMatrix;
                    rot.Value = xform.rotation;
                    pos.Value = xform.position;
                }).Run();
        }
    }
}