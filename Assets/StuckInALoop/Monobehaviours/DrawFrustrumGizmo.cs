using UnityEngine;

namespace StuckInALoop
{
    public class DrawFrustrumGizmo : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            var cam = GetComponent<Camera>();

            Gizmos.color  = new Color(0.54f, 0.27f, 1f, 0.25f);
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        }
    }
}