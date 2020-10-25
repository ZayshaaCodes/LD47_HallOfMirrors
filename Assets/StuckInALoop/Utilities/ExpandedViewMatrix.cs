using StuckInALoop;
using Unity.Mathematics;
using UnityEngine;

public class ExpandedViewMatrix
{
    private readonly Camera _camera;
    private          float  offset = 5;

    private float4x4 projectionMatrix;


    //constructor
    public ExpandedViewMatrix(float offset)
    {
        _camera = Camera.main;
        Offset  = offset;
    }

    public float4x4 ViewMatrix => _camera.ViewMatrix(projectionMatrix);

    public float Offset
    {
        set
        {
            offset           = value;
            projectionMatrix = _camera.GetExpandedProjectionMatrix(offset);
        }
        get => offset;
    }

    public Bounds WorldBounds
    {
        get
        {
            var ps = CameraUtilities.GetUnitBoundsPoints(ViewMatrix);
            var b  = new Bounds(ps[0], Vector3.zero);
            foreach (var point in ps) b.Encapsulate(point);

            return b;
        }
    }

    public void DrawGizmo()
    {
        CameraUtilities.DrawMtxBounds(ViewMatrix, new Color(1f, .5f, .5f, .5f));
    }
}