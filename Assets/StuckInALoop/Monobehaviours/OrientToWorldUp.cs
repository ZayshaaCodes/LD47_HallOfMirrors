using UnityEngine;

namespace StuckInALoop
{
    public class OrientToWorldUp : MonoBehaviour, IDestroyOnClone
    {
        public float changeRate = 200; //degrees per second

        // Update is called once per frame
        private void LateUpdate()
        {
            var angle      = Vector2.SignedAngle(WorldData.instance.worldUp, transform.up);
            var sign       = Mathf.Sign(angle);
            var deltaAngle = sign * changeRate * Time.deltaTime;

            deltaAngle = sign * Mathf.Min(Mathf.Abs(angle), Mathf.Abs(deltaAngle));

            transform.RotateAround(transform.position, Vector3.forward, -deltaAngle);
        }
    }
}