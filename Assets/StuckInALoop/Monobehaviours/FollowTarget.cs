using UnityEngine;

namespace StuckInALoop
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3   desiredOffset;

        // Update is called once per frame
        private void LateUpdate()
        {
            transform.position = target.transform.position + desiredOffset;
        }
    }
}