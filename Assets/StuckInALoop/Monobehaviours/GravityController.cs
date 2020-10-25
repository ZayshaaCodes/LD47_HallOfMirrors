using UnityEngine;
using UnityEngine.Events;

namespace StuckInALoop
{
    public class GravityController : MonoBehaviour, IToggle, IDestroyOnClone
    {
        public WorldButton triggerButton;
        public Vector2     activeGravity          = Vector2.up;
        public Vector2     revertGravity          = Vector2.down;
        public bool        revertGravityOnDepress = true;

        public UnityEvent gravityChangedEvent;

        [SerializeField] private bool changeWorldUp;

        private bool _state;

        private void Awake()
        {
            if (triggerButton == null) triggerButton = GetComponent<WorldButton>();

            if (triggerButton)
            {
                triggerButton.ButtonActivateEvent.AddListener(ToggleOn);
                triggerButton.ButtonDeactivateEvent.AddListener(ToggleOff);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3) activeGravity * .1f);
        }

        public void ToggleOn(int id)
        {
            if (revertGravityOnDepress) revertGravity = Physics2D.gravity;
            if (_state) return;
            _state = true;

            Physics2D.gravity = activeGravity;
            if (changeWorldUp) WorldData.instance.SetWorldUp(-activeGravity.normalized);

            gravityChangedEvent.Invoke();
        }

        public void ToggleOff(int id)
        {
            if (!_state) return;
            _state = false;

            if (revertGravityOnDepress)
            {
                Physics2D.gravity = revertGravity;
                if (changeWorldUp) WorldData.instance.SetWorldUp(-revertGravity.normalized);
                gravityChangedEvent.Invoke();
            }
        }

        public void Toggle()
        {
            _state = !_state;
            if (_state)
                ToggleOn(0);
            else
                ToggleOff(0);
        }
    }

    public interface IToggle
    {
        public void ToggleOn(int  id);
        public void ToggleOff(int id);
        public void Toggle();
    }
}