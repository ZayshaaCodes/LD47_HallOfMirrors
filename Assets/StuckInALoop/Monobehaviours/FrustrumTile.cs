using UnityEngine;

namespace StuckInALoop
{
    [ExecuteAlways]
    public class FrustrumTile : MonoBehaviour
    {
        public  Grid3D grid;
        private Camera _cam;

        private void OnEnable()
        {
            _cam = GetComponent<Camera>();
        }

        private Matrix4x4 GetProjectionMatrix()
        {
            var _projectionMatrix = Matrix4x4.Perspective(
                _cam.fieldOfView,
                _cam.aspect,
                _cam.nearClipPlane,
                _cam.farClipPlane);

            _projectionMatrix.m02 *= -1;
            _projectionMatrix.m12 *= -1;
            _projectionMatrix.m22 *= -1;
            _projectionMatrix.m32 *= -1;
            return _projectionMatrix;
        }

        private static void DrawMtxBounds(Matrix4x4 mtx)
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

        private static Vector3[] GetUnitBoundsPoints(Matrix4x4 mtx)
        {
            var points = new Vector3[8];

            points[0] = mtx.MultiplyPoint(new Vector3(1,  1,  1));
            points[1] = mtx.MultiplyPoint(new Vector3(-1, 1,  1));
            points[2] = mtx.MultiplyPoint(new Vector3(-1, 1,  -1));
            points[3] = mtx.MultiplyPoint(new Vector3(1,  1,  -1));
            points[4] = mtx.MultiplyPoint(new Vector3(1,  -1, 1));
            points[5] = mtx.MultiplyPoint(new Vector3(-1, -1, 1));
            points[6] = mtx.MultiplyPoint(new Vector3(-1, -1, -1));
            points[7] = mtx.MultiplyPoint(new Vector3(1,  -1, z: -1));
            return points;
        }
    }
}