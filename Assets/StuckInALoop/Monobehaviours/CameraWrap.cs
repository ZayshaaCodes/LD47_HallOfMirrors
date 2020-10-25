using UnityEngine;

namespace StuckInALoop
{
    public class CameraWrap : MonoBehaviour, ILoopBehaviour
    {
        private LoopingWorldRenderer2 _worldRenderer;

        private void Start()
        {
            _worldRenderer = FindObjectOfType<LoopingWorldRenderer2>();
        }

        public void Loop(Vector2 offset)
        {
            // worldRenderer.UpdateWorldTransforms();
        }
    }
}