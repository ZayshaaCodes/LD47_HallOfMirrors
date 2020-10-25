using System.Collections.Generic;
using StuckInALoop.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace StuckInALoop.Systems
{
    [ExecuteAlways]
    public class InstancedEntityRenderSystem : JobComponentSystem
    {
        private Dictionary<SharedRenderDefinition, StaticRenderChunkData> _staticRenderDatas =
            new Dictionary<SharedRenderDefinition, StaticRenderChunkData>();

        private EndInitializationEntityCommandBufferSystem _sys;

        private EntityArchetype     _renderEntArch;
        private NativeArray<float3> _pointsInBounds;

        protected override void OnCreate()
        {
            _sys = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
            //_renderEntArch = EntityManager.CreateArchetype(typeof(RenderInstanceData), typeof(SharedRenderInstanceData));
        }

        protected override void OnStartRunning()
        {
            var ecb = _sys.CreateCommandBuffer();

            _expandedViewMatrix    = new ExpandedViewMatrix(15);
            _grid.WorldSpaceBounds = _expandedViewMatrix.WorldBounds;

            float4x4 viewMtx        = _expandedViewMatrix.ViewMatrix;
            float4x4 inverseViewMtx = math.inverse(viewMtx);

            _pointsInBounds = new NativeArray<float3>(_grid.points.Length, Allocator.Persistent);

            var inboundCount = _grid.GetPointsInViewFrustrum(inverseViewMtx, ref _pointsInBounds);


            _dynamicTransformBuffer = new Matrix4x4[32][];
            for (var i = 0; i < _dynamicTransformBuffer.Length; i++)
                _dynamicTransformBuffer[i] = new Matrix4x4[1023];

            Entities.WithoutBurst().WithReadOnly(_pointsInBounds).ForEach((Entity e, SimpleSprite2 ss) =>
            {
                if (!ss.isStatic)
                {
                    var sharedRenderInstanceData = new SharedRenderInstanceData()
                    {
                        material      = ss.srd.material,
                        mesh          = ss.srd.mesh,
                        texture       = ss.srd.texture,
                        propertyBlock = ss.srd.PropBlock
                    };
                    ecb.AddSharedComponent(e, sharedRenderInstanceData);

                    for (var i = 0; i < inboundCount; i++)
                    {
                        var p = _pointsInBounds[i];

                        var pe = ecb.CreateEntity();
                        ecb.AddComponent(pe, new RenderInstanceData {offset = p, source = pe});
                        ecb.AddSharedComponent(pe, sharedRenderInstanceData);
                    }
                }
                else
                {
                    if (!_staticRenderDatas.ContainsKey(ss.srd))
                        _staticRenderDatas.Add(ss.srd, new StaticRenderChunkData());
                    _staticRenderDatas[ss.srd].spriteInstances.Add(ss.transform.localToWorldMatrix);
                }
            }).Run();

            foreach (var pair in _staticRenderDatas) pair.Value.SetRenderData(_pointsInBounds, inboundCount);
        }

        private Matrix4x4[] rBuffer = new Matrix4x4[1023];
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            NativeMultiHashMap<int, Matrix4x4> blockData =
                new NativeMultiHashMap<int, Matrix4x4>(50_000, Allocator.TempJob);
            var writer = blockData.AsParallelWriter();

            var pointsInBounts = new NativeArray<float3>(_pointsInBounds.Length, Allocator.TempJob);
            _pointsInBounds.CopyTo(pointsInBounts);

            RenderStaticData();
            
            var transformData = GetComponentDataFromEntity<LocalToWorld>(true);
            inputDeps = Entities.WithReadOnly(pointsInBounts).WithReadOnly(transformData).ForEach((RenderInstanceData instanceData) =>
            {
                for (var i = 0; i < pointsInBounts.Length; i++)
                {
                    var point = pointsInBounts[i];

                    float4x4 instanceTransform = transformData[instanceData.source].Value;
                    writer.Add(instanceData.source.Index,
                               math.mul(instanceTransform, float4x4.Translate(point + instanceData.offset)));
                }
            }).Schedule(inputDeps);
            
            
            return inputDeps;
        }

        private void RenderStaticData()
        {
            //render staticData
            foreach (var data in _staticRenderDatas)
            {
                var sharedRenderDef = data.Key;
                var renderData      = data.Value;

                for (var i = 0; i < renderData.transformBlocks.Length; i++)
                {
                    Matrix4x4[] block = renderData.transformBlocks[i];
                    int         rc    = renderData.count - i * 1023;

                    if (rc < 0) rc = 0;
                    Graphics.DrawMeshInstanced(
                        sharedRenderDef.mesh,
                        0,
                        sharedRenderDef.material,
                        block,
                        Mathf.Min(rc, 1023),
                        sharedRenderDef.PropBlock);
                }
            }
        }
    }
}