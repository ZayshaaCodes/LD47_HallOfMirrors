using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif


namespace StuckInALoop
{
    [RequireComponent(typeof(Grid3D))] [ExecuteAlways]
    public class TiledWorldPreview : MonoBehaviour
    {
        public           bool     previewEnabled;
        private          Grid3D   _grid;
        private readonly float3[] _renderPoints = new float3[8];

        public Color gizmoColor = Color.red;

        private List<SimpleSprite2> sprites = new List<SimpleSprite2>();

        private void OnEnable()
        {
            _grid = GetComponent<Grid3D>();
            UpdateRenderPoints();

#if UNITY_EDITOR
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
#endif
        }

        private void UpdateRenderPoints()
        {
            var sp = _grid.spacing;

            _renderPoints[0] = new float3(-sp.x, sp.y,  0);
            _renderPoints[1] = new float3(0,     sp.y,  0);
            _renderPoints[2] = new float3(sp.x,  sp.y,  0);
            _renderPoints[3] = new float3(-sp.x, 0,     0);
            _renderPoints[4] = new float3(sp.x,  0,     0);
            _renderPoints[5] = new float3(-sp.x, -sp.y, 0);
            _renderPoints[6] = new float3(0,     -sp.y, 0);
            _renderPoints[7] = new float3(sp.x,  -sp.y, 0);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            if (previewEnabled)
            {
                Gizmos.color = gizmoColor;
                foreach (var sprite in sprites)
                {
                    if (sprite == null) continue;

                    foreach (var renderPoint in _renderPoints)
                    {
                        Gizmos.DrawWireMesh(sprite.srd.mesh,
                                            0,
                                            sprite.transform.position + (Vector3) renderPoint,
                                            sprite.transform.rotation,
                                            sprite.transform.localScale);
                    }
                }
            }
        }

        private void OnHierarchyChanged()
        {
            sprites.Clear();
            sprites = FindObjectsOfType<SimpleSprite2>().ToList();
        }
    }
}