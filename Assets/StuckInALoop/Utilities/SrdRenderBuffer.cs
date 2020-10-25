using System;
using System.Collections.Generic;
using System.Linq;
using StuckInALoop;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SrdRenderBuffer
{
    private int _renderPointCount = 0;

    private NativeArray<float3> _renderPoints;

    private readonly Dictionary<SharedRenderDefinition, Matrix4x4[][]> _staticRenderData =
        new Dictionary<SharedRenderDefinition, Matrix4x4[][]>();

    private readonly Dictionary<SharedRenderDefinition, int> arrayIndexPositions =
        new Dictionary<SharedRenderDefinition, int>();

    public List<SimpleSprite2> StaticSprites { get; } = new List<SimpleSprite2>();

    public List<SimpleSprite2> DynamicSprites { get; } = new List<SimpleSprite2>();

    public void SetRenderPoints(float3[] points)
    {
        _renderPoints = new NativeArray<float3>(points.Length, Allocator.Persistent);
        _renderPoints.CopyFrom(points);
    }

    public int GetIndexPosition(SharedRenderDefinition key)
    {
        return arrayIndexPositions[key];
    }

    public void Render()
    {
        foreach (var groups in _staticRenderData)
        {
            var renderDef = groups.Key;
            if (!renderDef.enabled)
                continue;

            var pos = GetIndexPosition(renderDef);
            for (var i = 0; i < groups.Value.Length; i++)
            {
                var xformArray = groups.Value[i];
                if (pos > 0)
                    Graphics.DrawMeshInstanced(renderDef.mesh, 0, renderDef.material, xformArray,
                                               Mathf.Min(1023, pos), renderDef.PropBlock);

                pos -= 1023;
            }
        }
    }

    public void AddStatic(SharedRenderDefinition rDef, Matrix4x4 transform)
    {
        if (!_staticRenderData.ContainsKey(rDef))
        {
            arrayIndexPositions.Add(rDef, 0);

            var arrayGroups = new Matrix4x4[16][];
            _staticRenderData[rDef] = arrayGroups;

            for (var i = 0; i < arrayGroups.Length; i++) arrayGroups[i] = new Matrix4x4[1023];
        }

        var posIndex = arrayIndexPositions[rDef]++;

        var arrayIndex = posIndex / 1023;
        posIndex = posIndex % 1023;

        if (arrayIndex >= 16) throw new OverflowException();

        var curArray = _staticRenderData[rDef][arrayIndex];
        curArray[posIndex] = transform;
    }


    public void Clear()
    {
        var keys                                           = arrayIndexPositions.Keys.ToArray();
        foreach (var key in keys) arrayIndexPositions[key] = 0;
    }
}