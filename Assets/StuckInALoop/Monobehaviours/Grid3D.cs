using System;
using StuckInALoop.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StuckInALoop
{
    [ExecuteAlways]
    public class Grid3D : MonoBehaviour, IConvertGameObjectToEntity
    {
        // public int3 gridCount;
        public float3      spacing = new float3(1, 1, 1);
        public int3        gridCount;
        public Vector3[,,] points = new Vector3[1, 1, 1];

        [Range(0, 1)] public float gizmoSize = .1f;

        private Bounds _worldSpaceBounds = new Bounds(Vector3.zero, Vector3.one);

        public Bounds WorldSpaceBounds
        {
            get => _worldSpaceBounds;
            set
            {
                _worldSpaceBounds = value;
                gridCount         = (int3) math.round(_worldSpaceBounds.size / spacing);
                points            = new Vector3[gridCount.x, gridCount.y, gridCount.z];
                UpdateGridPoints();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.98f, 0.95f, 1f, 0.20f);
            Gizmos.DrawWireCube(_worldSpaceBounds.center, _worldSpaceBounds.size);

            Gizmos.color = new Color(0.98f, 0.95f, 1f, .30f);
            Gizmos.DrawWireCube(Vector3.zero, spacing);
        }

        private void OnEnable() => UpdateGridPoints();

        public void UpdateGridPoints()
        {
            points = new Vector3[gridCount.x, gridCount.y, gridCount.z];
            var startPosition = (int3) math.ceil(_worldSpaceBounds.min / spacing) * spacing;

            for (var k = 0; k < gridCount.z; k++)
            for (var j = 0; j < gridCount.y; j++)
            for (var i = 0; i < gridCount.x; i++)
            {
                var ijk   = new float3(i, j, k);
                var point = startPosition + spacing * ijk;
                points[i, j, k] = point;
            }
        }

        public int GetPointsInViewFrustrum(float4x4 inverseViewMtx, ref NativeArray<float3> pointsInBounds)
        {
            var c = 0;

            var cullingBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 2f));

            foreach (var point in points)
            {
                var localPos = ((Matrix4x4) inverseViewMtx).MultiplyPoint(point);
                if (cullingBounds.Contains(localPos)) pointsInBounds[c++] = point;
            }

            return c;
        }

        public int GetPointsInViewFrustrum(float4x4 inverseViewMtx, ref float3[] pointsInBounds)
        {
            var c = 0;

            var cullingBounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 2f));

            foreach (var point in points)
            {
                var localPos = ((Matrix4x4) inverseViewMtx).MultiplyPoint(point);
                if (cullingBounds.Contains(localPos)) pointsInBounds[c++] = point;
            }

            return c;
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var wgd = new WorldGridData()
            {
                spacing = spacing
            };

            dstManager.AddComponentData(entity, wgd);
        }
    }
}