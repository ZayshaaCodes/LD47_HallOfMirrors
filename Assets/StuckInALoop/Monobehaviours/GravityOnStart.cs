using UnityEngine;

namespace StuckInALoop
{
    public class GravityOnStart : MonoBehaviour
    {
        [SerializeField] private Vector2 startGravity;

        private void Start()
        {
            Physics2D.gravity = startGravity;
        }
    }
}