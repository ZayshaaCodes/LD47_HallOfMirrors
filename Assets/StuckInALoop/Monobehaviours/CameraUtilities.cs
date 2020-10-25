using Unity.Mathematics;
using UnityEngine;

namespace StuckInALoop
{
    public static class CameraUtilities
    {
        public static Matrix4x4 GetProjectionMatrix(this Camera cam, float farclipShift = 0)
        {
            var projectionMatrix = Matrix4x4.Perspective(cam.fieldOfView,
                                                         cam.aspect,
                                                         cam.nearClipPlane,
                                                         cam.farClipPlane + farclipShift);

            projectionMatrix.m02 *= -1;
            projectionMatrix.m12 *= -1;
            projectionMatrix.m22 *= -1;
            projectionMatrix.m32 *= -1;
            return projectionMatrix;
        }


        public static Bounds GetCameraFrustrumBounds(Camera cam, float farclipShift = 0)
        {
            var mtx    = cam.transform.localToWorldMatrix * GetProjectionMatrix(cam, farclipShift).inverse;
            var data   = GetUnitBoundsPoints(mtx);
            var bounds = new Bounds(data[0], Vector3.zero);

            for (var i = 1; i < data.Length; i++)
                bounds.Encapsulate(data[i]);

            return bounds;
        }

        public static float4x4 ViewMatrix(this Camera camera, float4x4 projection)
        {
            var view = math.mul(float4x4.TRS(camera.transform.position, camera.transform.rotation, new float3(1)),
                                projection);
            // view.c2 *= -1;
            return view;
        }

        public static float4x4 GetExpandedProjectionMatrix(this Camera camera, float offset)
        {
            var extraDistance = offset / math.tan(math.radians(camera.fieldOfView / 2));
            var scaleMtx      = float4x4.Scale(1, 1, -1);
            var inversePerspectiveMtx =
                math.inverse(
                    float4x4.PerspectiveFov(math.radians(camera.fieldOfView),
                                            camera.aspect,
                                            camera.nearClipPlane + extraDistance,
                                            camera.farClipPlane + extraDistance));
            var translationMtx = float4x4.Translate(new float3(0, 0, extraDistance));
            // return math.mul(translationMtx, inversePerspectiveMtx);
            return math.mul(scaleMtx, math.mul(translationMtx, inversePerspectiveMtx));
        }

        public static void DrawMtxBounds(Matrix4x4 mtx)
        {
            var points = GetUnitBoundsPoints(mtx);

            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[0]);
            Gizmos.DrawLine(points[4], points[5]);
            Gizmos.DrawLine(points[5], points[6]);
            Gizmos.DrawLine(points[6], points[7]);
            Gizmos.DrawLine(points[7], points[4]);
            Gizmos.DrawLine(points[0], points[4]);
            Gizmos.DrawLine(points[1], points[5]);
            Gizmos.DrawLine(points[2], points[6]);
            Gizmos.DrawLine(points[3], points[7]);
        }

        public static Vector3[] GetUnitBoundsPoints(Matrix4x4 mtx)
        {
            var points = new Vector3[8];

            points[0] = mtx.MultiplyPoint(new Vector3(1,  1,  1));
            points[1] = mtx.MultiplyPoint(new Vector3(-1, 1,  1));
            points[2] = mtx.MultiplyPoint(new Vector3(-1, 1,  -1));
            points[3] = mtx.MultiplyPoint(new Vector3(1,  1,  -1));
            points[4] = mtx.MultiplyPoint(new Vector3(1,  -1, 1));
            points[5] = mtx.MultiplyPoint(new Vector3(-1, -1, 1));
            points[6] = mtx.MultiplyPoint(new Vector3(-1, -1, -1));
            points[7] = mtx.MultiplyPoint(new Vector3(1,  -1, -1));
            return points;
        }

        public static void DrawMtxBounds(Matrix4x4 mtx, Color color)
        {
            Gizmos.color = color;
            DrawMtxBounds(mtx);
        }
    }
}