using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StuckInALoop.EditorTools;
using Unity.Mathematics;
using UnityEngine;

namespace StuckInALoop
{
    [ExecuteAlways]
    [RequireComponent(typeof(Grid3D))]
    public class LoopingWorldRenderer2 : BaseBehavior
    {
        [AssertFieldNotNull] public new Camera camera;
        public                          Grid3D grid;
        public                          float  frustrumOffset;

        private readonly SrdRenderBuffer _insancedRenderBuffer = new SrdRenderBuffer();

        private readonly Dictionary<SharedRenderDefinition, List<Matrix4x4>> _localRenderTransforms =
            new Dictionary<SharedRenderDefinition, List<Matrix4x4>>();

        private List<SimpleSprite2> _dynamicSprites;
        private List<SimpleSprite2> _staticSprites;
        private ExpandedViewMatrix  _expandedViewMatrix;
        private float3[]            _pointsInBounds = new float3[16 * 1024];

        private void Start()
        {
            foreach (var sprite in _staticSprites) sprite.GetComponent<MeshRenderer>().enabled = false;
        }

        private void Update()
        {
            UpdateDynamicTransforms();
        }

        private void OnEnable()
        {
            grid = GetComponent<Grid3D>();

            if (_expandedViewMatrix == null)
                _expandedViewMatrix = new ExpandedViewMatrix(frustrumOffset);
            else
                _expandedViewMatrix.Offset = frustrumOffset;

            _localRenderTransforms.Clear();
            var allSprites = GetComponentsInChildren<SimpleSprite2>();

            _staticSprites  = allSprites.Where(s => s.isStatic).ToList();
            _dynamicSprites = allSprites.Where(s => !s.isStatic).ToList();

            foreach (var sprite in _staticSprites)
            {
                //sprite.GetComponent<MeshRenderer>().enabled = false;
                List<Matrix4x4> list;
                if (!_localRenderTransforms.ContainsKey(sprite.srd))
                {
                    list = new List<Matrix4x4>();
                    _localRenderTransforms.Add(sprite.srd, list);
                }
                else
                {
                    list = _localRenderTransforms[sprite.srd];
                }

                list.Add(sprite.transform.localToWorldMatrix);
            }

            UpdateRenderBuffer();
        }


        private void OnValidate()
        {
            OnEnable();
        }

        [Button]
        public void UpdateRenderBuffer()
        {
            grid.WorldSpaceBounds = _expandedViewMatrix.WorldBounds;

            var viewMtx        = _expandedViewMatrix.ViewMatrix;
            var inverseViewMtx = math.inverse(viewMtx);

            grid.GetPointsInViewFrustrum(inverseViewMtx, ref _pointsInBounds);

            _insancedRenderBuffer.SetRenderPoints(_pointsInBounds.ToArray());
            _insancedRenderBuffer.Clear();

            foreach (var renderDefListPair in _localRenderTransforms)
            foreach (var itemMatrix in renderDefListPair.Value)
            foreach (var point in _pointsInBounds)
                _insancedRenderBuffer.AddStatic(renderDefListPair.Key, Matrix4x4.Translate(point) * itemMatrix);
        }

        private void UpdateDynamicTransforms()
        {
            // foreach (var sprite in sprites)
            // {
            //     if (!sprite.isStatic)
            //     {
            //         for (var i = 0; i < pointsInBounds.Count; i++)
            //         {
            //             var point = pointsInBounds[i];
            //             insancedRenderBuffer.Set(sprite.srd, i, Matrix4x4.Translate(point) * sprite.transform.localToWorldMatrix);
            //         }
            //     }
            // }
            //
        }
    }
}