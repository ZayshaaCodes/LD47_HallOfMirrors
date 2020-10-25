using UnityEngine;

namespace StuckInALoop
{
    public class MoveOverTime : MonoBehaviour
    {
        public Vector3 direction = Vector3.zero;
        public Vector3 rotation  = Vector3.zero;

        private void LateUpdate()
        {
            transform.position += direction * (10 * Time.deltaTime);
            transform.Rotate(rotation * Time.deltaTime);
        }
    }
}