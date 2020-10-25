using UnityEngine;

namespace StuckInALoop
{
    public class InputFeedback : MonoBehaviour
    {
        private Camera       _cam;
        private SimpleSprite _indicatorSprite;

        private void Start()
        {
            _cam             = Camera.main;
            _indicatorSprite = GetComponent<SimpleSprite>();
        }

        private void Update()
        {
            var up = _cam.transform.up;
            var d  = Vector2.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) d    += Vector2.up;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) d  += Vector2.down;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) d  += Vector2.left;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) d += Vector2.right;

            if (d == Vector2.zero)
                _indicatorSprite.mr.enabled = false;
            else if (_indicatorSprite.mr.enabled == false)
                _indicatorSprite.mr.enabled = true;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, d)
                               * Quaternion.LookRotation(Vector3.forward, up);
        }
    }
}